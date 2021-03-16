using Aps.Infrastructure;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;
using Aps.Shared.Model;
using AutoMapper;
using Google.OrTools.Sat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aps.Infrastructure.Repositories;
using Aps.Shared.Enum;
using Aps.Threading;
using Microsoft.Extensions.Hosting;
using ValueUtils;

namespace Aps.Services
{
    public class JobVar
    {
        public IntVar StartVar { get; set; }
        public IntVar EndVar { get; set; }
        public IntervalVar Interval { get; set; }

        public void Deconstruct(out IntVar startVar, out IntVar endVar, out IntervalVar interval)
        {
            startVar = StartVar;
            endVar = EndVar;
            interval = Interval;
        }

        public JobVar(IntVar startVar, IntVar endVar, IntervalVar interval)
        {
            StartVar = startVar ?? throw new ArgumentNullException(nameof(startVar));
            EndVar = endVar ?? throw new ArgumentNullException(nameof(endVar));
            Interval = interval ?? throw new ArgumentNullException(nameof(interval));
        }
    }

    public sealed class JobNavigation : ValueObject<JobNavigation>
    {
        public ApsOrder Order { get; set; }
        public ProductInstance ProductInstance { get; set; }
        public SemiProductInstance SemiProductInstance { get; set; }
        public ApsManufactureProcess ManufactureProcess { get; set; }

        public JobNavigation(ApsOrder order, ProductInstance productInstance, SemiProductInstance semiProductInstance,
            ApsManufactureProcess manufactureProcess)
        {
            Order = order;
            ProductInstance = productInstance;
            SemiProductInstance = semiProductInstance;
            ManufactureProcess = manufactureProcess;
        }

        public void Deconstruct(out ApsOrder order, out ProductInstance productInstance,
            out SemiProductInstance semiProductInstance, out ApsManufactureProcess manufactureProcess)
        {
            order = Order;
            productInstance = ProductInstance;
            semiProductInstance = SemiProductInstance;
            manufactureProcess = ManufactureProcess;
        }
    }

    public class ScheduleAssemblyJob : ApsAssemblyJob
    {
        public JobVar Vars { get; set; }
    }

    public class ScheduleManufactureJob : ApsManufactureJob
    {
        public JobVar Vars { get; set; }
    }

    public class ScheduleTool : IScheduleTool
    {
        private const int Ub = 1000000;
        private readonly ApsContext _context;
        private readonly IScheduleClass _scheduleClass;
        private readonly IRepository<ScheduleRecord, Guid> _scheduleRecordRepository;
        private List<ScheduleManufactureJob> _jobs;
        private List<ApsResource> _resources;


        public CpModel Model { get; } = new CpModel();
        public CpSolver Solver { get; } = new CpSolver();
        public ICollection<ApsOrder> OrdersList { get; set; }

        public Dictionary<ApsProduct, int> ProductPrerequisite { get; private set; }

        public Dictionary<ApsSemiProduct, int> SemiProductPrerequisite { get; private set; }

        public List<ApsManufactureProcess> ManufactureProcesses { get; private set; }

        public IEnumerable<ApsAssemblyProcess> AssemblyProcesses { get; private set; }
        public IDictionary<ApsManufactureProcess, int> ManufactureProcessRequisite { get; private set; }


        public Dictionary<JobNavigation, ScheduleManufactureJob> ScheduleManufactureJobs { get; set; }
            = new Dictionary<JobNavigation, ScheduleManufactureJob>();


        public Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; } =
            new Dictionary<ApsOrder, List<ProductInstance>>();

        public Dictionary<(ApsOrder order, ProductInstance productInstance), List<SemiProductInstance>>
            SemiProductInstances { get; private set; } =
            new Dictionary<(ApsOrder apsOrder, ProductInstance productInstance), List<SemiProductInstance>>();

        private IntVar[,] ResourcePerformed { get; set; }

        public ScheduleTool(ApsContext context,
            IScheduleClass scheduleClass,
            IRepository<ScheduleRecord, Guid> scheduleRecordRepository)
        {
            _context = context;
            _scheduleClass = scheduleClass ?? throw new ArgumentNullException(nameof(scheduleClass));
            _scheduleRecordRepository = scheduleRecordRepository ??
                                        throw new ArgumentNullException(nameof(scheduleRecordRepository));
        }

        public void SetProductPrerequisite(List<ApsOrder> orders)
        {
            OrdersList = orders;

            ProductPrerequisite = orders.GroupBy(
                o => o.Product,
                (product, groupOrders) =>
                {
                    return new
                    {
                        product,
                        amount = groupOrders.Sum(x => x.Amount)
                    };
                }).ToDictionary(x => x.product, x => x.amount);

            SemiProductPrerequisite = new Dictionary<ApsSemiProduct, int>();

            foreach (var (product, amount) in ProductPrerequisite)
            {
                foreach (var productAssembleBySemiProduct in product.AssembleBySemiProducts)
                {
                    var semiProduct = productAssembleBySemiProduct.ApsSemiProduct;
                    if (SemiProductPrerequisite.ContainsKey(semiProduct))
                    {
                        SemiProductPrerequisite[semiProduct] += productAssembleBySemiProduct.Amount * amount;
                    }
                    else
                    {
                        SemiProductPrerequisite.Add(semiProduct, productAssembleBySemiProduct.Amount * amount);
                    }
                }
            }
        }

        public void GenerateProcess()
        {
            ManufactureProcesses = new List<ApsManufactureProcess>();
            ManufactureProcesses.AddRange(SemiProductPrerequisite.SelectMany
                (x => x.Key.ApsManufactureProcesses));


            ManufactureProcessRequisite = new Dictionary<ApsManufactureProcess, int>();

            foreach (var x in SemiProductPrerequisite)
            {
                var apsManufactureProcesses = x.Key.ApsManufactureProcesses;
                foreach (var p in apsManufactureProcesses)
                {
                    if (ManufactureProcessRequisite.ContainsKey(p))
                    {
                        ManufactureProcessRequisite[p] = ManufactureProcessRequisite[p] + x.Value;
                    }
                    else
                    {
                        ManufactureProcessRequisite.Add(p, x.Value);
                    }
                }
            }
        }

        public void GenerateJobsFromOrders()
        {
            foreach (var order in OrdersList)
            {
                ProductInstances.Add(order, new List<ProductInstance>());

                var orderProduct = order.Product;
                for (int i = 0; i < order.Amount; i++)
                {
                    var productInstance = new ProductInstance(Guid.NewGuid(), orderProduct, order);
                    ProductInstances[order].Add(productInstance);

                    SemiProductInstances.Add((order, productInstance), new List<SemiProductInstance>());

                    foreach (var productAssembleBySemiProduct in orderProduct.AssembleBySemiProducts)
                    {
                        var semiProduct = productAssembleBySemiProduct.ApsSemiProduct;
                        var processes = semiProduct.ApsManufactureProcesses;


                        for (int j = 0; j < productAssembleBySemiProduct.Amount; j++)
                        {
                            var semiProductInstance =
                                new SemiProductInstance(Guid.NewGuid(), semiProduct, productInstance);
                            SemiProductInstances[(order, productInstance)].Add(semiProductInstance);
                            foreach (var manufactureProcess in processes)
                            {
                                var duration = manufactureProcess.ProductionTime;
                                string suffix = $"Order:{order.Id}_" +
                                                $"Product:{productInstance.Id}_" +
                                                $"SemiProduct:{semiProductInstance.Id}_" +
                                                $"Process:{manufactureProcess.Id}_" +
                                                $"Duration:{duration}";


                                var jobNavigation = new JobNavigation(order, productInstance, semiProductInstance,
                                    manufactureProcess);


                                if (manufactureProcess.ProductionMode == ProductionMode.Bp)
                                {
                                    ScheduleManufactureJobs.Add(
                                        jobNavigation
                                        , new ScheduleManufactureJob
                                        {
                                            Id = Guid.NewGuid(),
                                            ApsOrder = order,
                                            ApsProduct = orderProduct,
                                            ProductInstance = productInstance,
                                            ApsSemiProduct = semiProduct,
                                            SemiProductInstance = semiProductInstance,
                                            ApsManufactureProcess = manufactureProcess,
                                            Duration = manufactureProcess.ProductionTime,
                                            Workspace = Workspace.装配
                                        });
                                }
                                else
                                {
                                    IntVar startVar = Model.NewIntVar(0, Ub, "start" + suffix);
                                    IntVar endVar = Model.NewIntVar(0, Ub, "end" + suffix);
                                    IntervalVar interval =
                                        Model.NewIntervalVar(startVar, (int) duration.TotalMinutes, endVar,
                                            "interval" + suffix);

                                    ScheduleManufactureJobs.Add(
                                        jobNavigation
                                        , new ScheduleManufactureJob
                                        {
                                            Id = Guid.NewGuid(),
                                            ApsOrder = order,
                                            ApsProduct = orderProduct,
                                            ProductInstance = productInstance,
                                            ApsSemiProduct = semiProduct,
                                            SemiProductInstance = semiProductInstance,
                                            ApsManufactureProcess = manufactureProcess,
                                            Duration = manufactureProcess.ProductionTime,
                                            Vars = new JobVar(startVar, endVar, interval),
                                            Workspace = Workspace.装配,
                                        });
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetBatchJob()
        {
            var jobGroupByProcess = ScheduleManufactureJobs
                .Where(x =>
                    x.Key.ManufactureProcess.ProductionMode == ProductionMode.Bp)
                .GroupBy(x => x.Key.ManufactureProcess,
                    (process, jobPairs) =>
                        jobPairs.Select(x => x.Value).ToList())
                .ToDictionary(x => x[0].ApsManufactureProcess);


            foreach (var (manufactureProcess, requisite) in ManufactureProcessRequisite.Where(x =>
                x.Key.ProductionMode == ProductionMode.Bp))
            {
                int jobProductTimes;
                var batch = manufactureProcess.MaximumProductionQuantity.GetValueOrDefault(1);
                if (requisite % manufactureProcess.MaximumProductionQuantity == 0)
                {
                    jobProductTimes = requisite / batch;
                }
                else
                {
                    jobProductTimes = requisite / batch + 1;
                }

                int remainder = requisite;
                int producted = 0;

                var jobsInBatch = jobGroupByProcess[manufactureProcess];

                var job = jobsInBatch.First();

                if (jobsInBatch.Count != requisite)
                {
                    throw new ApplicationException(nameof(manufactureProcess) + "的作业数量与总需求量不同");
                }

                for (int i = 0; i < jobProductTimes; i++)
                {
                    var productedOnce = remainder >= batch ? batch : remainder;

                    var jobDuration = job.Duration;

                    string suffix = $"Order:{job.ApsOrder.Id}_" +
                                    $"Product:{job.ProductInstance.Id}_" +
                                    $"SemiProduct:{job.SemiProductInstance.Id}_" +
                                    $"Process:{job.ApsManufactureProcess.Id}_" +
                                    $"Duration:{jobDuration}";


                    IntVar startVar = Model.NewIntVar(0, Ub, "start" + suffix);
                    IntVar endVar = Model.NewIntVar(0, Ub, "end" + suffix);
                    IntervalVar interval =
                        Model.NewIntervalVar(startVar, (int) jobDuration.TotalMinutes, endVar,
                            "interval" + suffix);


                    var jobVar = new JobVar(startVar, endVar, interval);

                    for (int j = 0; j < productedOnce; j++)
                    {
                        jobsInBatch[producted].Vars = jobVar;

                        producted++;
                        remainder--;
                    }
                }
            }
        }

        public async Task AssignResource()
        {
            var resourceJobsFromBp = ScheduleManufactureJobs
                .Where(x => x.Key.ManufactureProcess.ProductionMode == ProductionMode.Bp)
                .GroupBy(x => x.Value.Vars,
                    (interval, pairs) => pairs.First().Value).ToList();

            var resourceJobsFromSp = ScheduleManufactureJobs
                .Where(x => x.Key.ManufactureProcess.ProductionMode == ProductionMode.Sp)
                .Select(x => x.Value);

            _jobs = new List<ScheduleManufactureJob>(resourceJobsFromSp);
            _jobs.AddRange(resourceJobsFromBp);

            _resources = await _context.ApsResources
                .Include(x => x.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass)
                .Where(x => x.Type != ResourceType.人员)
                .ToListAsync();

            var jobCount = _jobs.Count;
            var resourcesCount = _resources.Count;

            var resourceJobMatrix = new Dictionary<int, IntVar>[jobCount, resourcesCount];

            for (int i = 0; i < jobCount; i++)
            {
                for (int j = 0; j < resourcesCount; j++)
                {
                    resourceJobMatrix[i, j] = new Dictionary<int, IntVar>();
                    var resource = _resources[j];
                    foreach (var resourceAttribute in resource.ResourceAttributes)
                    {
                        var intVar = Model.NewIntVar(0, 1,
                            $"Job:{_jobs[i].ApsManufactureProcess.PrevPart}_," +
                            $"Resource:{resource.Id}_," +
                            $"Class:{resourceAttribute.ResourceClass.ResourceClassName}");
                        resourceJobMatrix[i, j].Add(resourceAttribute.ResourceClass.Id, intVar);
                    }
                }
            }


            for (int i = 0; i < jobCount; i++)
            {
                var job = _jobs[i];
                var processResourcesNeed = job.ApsManufactureProcess.ApsResources;


                var processResourcesVarFromClass = new Dictionary<int, List<IntVar>>();

                for (int j = 0; j < resourcesCount; j++)
                {
                    var resourceAsClass = new List<IntVar>();
                    foreach (var (resourceClassId, performed) in resourceJobMatrix[i, j])
                    {
                        resourceAsClass.Add(performed);

                        if (processResourcesVarFromClass.ContainsKey(resourceClassId))
                        {
                            processResourcesVarFromClass[resourceClassId].Add(performed);
                        }
                        else
                        {
                            processResourcesVarFromClass.Add(resourceClassId, new List<IntVar> {performed});
                        }
                    }

                    Model.Add(LinearExpr.Sum(resourceAsClass) <= 1);
                }

                foreach (var (resourceClassId, performedResources) in processResourcesVarFromClass)
                {
                    int performedResourcesAmount;
                    if (processResourcesNeed.Any(x => x.ResourceClass.Id == resourceClassId))
                    {
                        performedResourcesAmount =
                            processResourcesNeed.Single(x => x.ResourceClass.Id == resourceClassId).Amount;

                        Model.Add(LinearExpr.Sum(performedResources) == performedResourcesAmount);
                    }
                    else
                    {
                        performedResourcesAmount = 0;
                        Model.Add(LinearExpr.Sum(performedResources) == performedResourcesAmount);
                    }
                }
            }


            IntervalVar[,] resourceIntervals = new IntervalVar[jobCount, resourcesCount];

            ResourcePerformed = new IntVar[jobCount, resourcesCount];

            for (int i = 0; i < jobCount; i++)
            {
                _jobs[i].Vars.Deconstruct(out IntVar startVar, out IntVar endVar, out IntervalVar intervalVar);
                var duration = (int) _jobs[i].Duration.TotalMinutes;

                for (int j = 0; j < resourcesCount; j++)
                {
                    var resourceIsPerformed = Model.NewIntVar(0, 1, $"Performed:[{i}, {j}]");

                    Model.Add(resourceIsPerformed == LinearExpr.Sum(resourceJobMatrix[i, j].Select(x => x.Value)));
                    ResourcePerformed[i, j] = resourceIsPerformed;

                    var optionalIntervalVar = Model.NewOptionalIntervalVar(startVar, duration, endVar,
                        resourceIsPerformed, $"Resource:{i}, Job:{j}");

                    resourceIntervals[i, j] = optionalIntervalVar;
                }
            }

            for (int i = 0; i < resourcesCount; i++)
            {
                var resourceInterval = new List<IntervalVar>();
                for (int j = 0; j < jobCount; j++)
                {
                    resourceInterval.Add(resourceIntervals[j, i]);
                }

                Model.AddNoOverlap(resourceInterval);
            }
        }

        public void SetPreJobConstraint()
        {
            foreach (var ((order, productInstance, semiProductInstance, manufactureProcess), job) in
                ScheduleManufactureJobs)
            {
                if (manufactureProcess.PrevPartId != null)
                {
                    ApsManufactureProcess preProcess =
                        ManufactureProcesses.FirstOrDefault(x => x.Id == manufactureProcess.PrevPartId);

                    var preJob =
                        ScheduleManufactureJobs[
                            new JobNavigation(order, productInstance, semiProductInstance, preProcess)];
                    Model.Add(preJob.Vars.EndVar < job.Vars.StartVar);
                }
            }
        }

        public void SetObjective()
        {
            var objective = Model.NewIntVar(0, Ub, "Objective");

            Model.AddMaxEquality(objective, ScheduleManufactureJobs
                .Select(x => x.Value.Vars.EndVar));

            Model.Minimize(objective);
        }

        public async Task<ScheduleRecord> Solve()
        {
            var scheduleRecord = new ScheduleRecord
            {
                Jobs = _jobs.AsEnumerable() as ICollection<ApsJob>,
                Id = Guid.NewGuid(),
                Orders = OrdersList,
                RecordState = RecordState.UnKnow,
                ScheduleFinishTime = null,
            };

            await _scheduleRecordRepository.InsertAsync(scheduleRecord);

            var scheduleModel = new ScheduleModel
            {
                ScheduleManufactureJobs = ScheduleManufactureJobs,
                Jobs = _jobs,
                ScheduleRecord = scheduleRecord,
                Model = Model,
                ResourcePerformed = ResourcePerformed,
                Resources = _resources
            };

            _scheduleClass.ScheduleModel = scheduleModel;

            var schedule = new Thread(new ThreadStart(_scheduleClass.ExecuteAsync));
            schedule.Start();
            schedule.IsBackground = true;

            // _scheduleHostService.ScheduleModel = scheduleModel;
            // await _scheduleHostService.StartAsync(new CancellationToken());

            return scheduleRecord;
        }
    }
}