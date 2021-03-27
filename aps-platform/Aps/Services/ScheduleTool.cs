using Aps.Infrastructure;
using Aps.Shared.Entity;
using Aps.Shared.Extensions;
using AutoMapper;
using Google.OrTools.Sat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aps.Threading;
using MoreLinq;
using ValueUtils;

namespace Aps.Services
{
    public sealed class ManufactureJobNavigation : ValueObject<ManufactureJobNavigation>
    {
        public ApsOrder Order { get; set; }
        public ProductInstance ProductInstance { get; set; }
        public SemiProductInstance SemiProductInstance { get; set; }
        public ApsManufactureProcess ManufactureProcess { get; set; }

        public ManufactureJobNavigation(ApsOrder order, ProductInstance productInstance,
            SemiProductInstance semiProductInstance,
            ApsManufactureProcess manufactureProcess)
        {
            Order = order;
            ProductInstance = productInstance;
            SemiProductInstance = semiProductInstance;
            ManufactureProcess = manufactureProcess;
        }

        public override string ToString()
        {
            return
                $"{nameof(Order)}: {Order}, {nameof(ProductInstance)}: {ProductInstance}, {nameof(SemiProductInstance)}: {SemiProductInstance}, {nameof(ManufactureProcess)}: {ManufactureProcess}";
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

    public sealed class AssemblyJobNavigation : ValueObject<AssemblyJobNavigation>
    {
        public ApsOrder Order { get; set; }

        public ProductInstance ProductInstance { get; set; }

        public ApsAssemblyProcess AssemblyProcess { get; set; }

        public AssemblyJobNavigation(ApsOrder order, ProductInstance productInstance,
            ApsAssemblyProcess assemblyProcess)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            ProductInstance = productInstance ?? throw new ArgumentNullException(nameof(productInstance));
            AssemblyProcess = assemblyProcess ?? throw new ArgumentNullException(nameof(assemblyProcess));
        }

        public void Deconstruct(out ApsOrder order, out ProductInstance productInstance,
            out ApsAssemblyProcess assemblyProcess)
        {
            order = Order;
            productInstance = ProductInstance;
            assemblyProcess = AssemblyProcess;
        }

        public override string ToString()
        {
            return
                $"{nameof(Order)}: {Order}, {nameof(ProductInstance)}: {ProductInstance}, {nameof(AssemblyProcess)}: {AssemblyProcess}";
        }
    }

    public class ScheduleTool : IScheduleTool
    {
        private const int Ub = 1000000;
        private readonly ApsContext _context;
        private readonly IScheduleClass _scheduleClass;
        private readonly IMapper _mapper;

        private List<ApsJob> _jobs;
        private List<ApsResource> _resources;

        private Dictionary<Guid, List<ApsManufactureJob>> _batchJobDictionary;

        public CpModel Model { get; } = new CpModel();
        public CpSolver Solver { get; } = new CpSolver();
        public IEnumerable<ApsOrder> OrdersList { get; set; }

        public Dictionary<ApsProduct, int> ProductPrerequisite { get; private set; }

        public Dictionary<ApsSemiProduct, int> SemiProductPrerequisite { get; private set; }

        public List<ApsManufactureProcess> ManufactureProcesses { get; private set; }

        public IEnumerable<ApsAssemblyProcess> AssemblyProcesses { get; private set; }
        public IDictionary<ApsManufactureProcess, int> ManufactureProcessRequisite { get; private set; }


        public Dictionary<ManufactureJobNavigation, ApsManufactureJob> ScheduleManufactureJobs { get; set; }
        public Dictionary<AssemblyJobNavigation, ApsAssemblyJob> ScheduleAssemblyJobs { get; set; }

        public Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; } =
            new Dictionary<ApsOrder, List<ProductInstance>>();

        public Dictionary<(ApsOrder order, ProductInstance productInstance), List<SemiProductInstance>>
            SemiProductInstances { get; private set; } =
            new Dictionary<(ApsOrder apsOrder, ProductInstance productInstance), List<SemiProductInstance>>();

        private IntVar[,] ResourcePerformed { get; set; }

        public ScheduleTool(ApsContext context,
            IScheduleClass scheduleClass,
            IMapper mapper)
        {
            _context = context;
            _scheduleClass = scheduleClass ?? throw new ArgumentNullException(nameof(scheduleClass));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void SetProductPrerequisite(IEnumerable<ApsOrder> orders)
        {
            OrdersList = orders.ToList();

            ProductPrerequisite = OrdersList.GroupBy(
                o => o.Product,
                (product, groupOrders) =>
                    new
                    {
                        product,
                        amount = groupOrders.Sum(x => x.Amount)
                    }
            ).ToDictionary(x => x.product, x => x.amount);

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

            foreach (var (semiProduct, value) in SemiProductPrerequisite)
            {
                var apsManufactureProcesses = semiProduct.ApsManufactureProcesses;
                foreach (var p in apsManufactureProcesses)
                {
                    if (ManufactureProcessRequisite.ContainsKey(p))
                    {
                        ManufactureProcessRequisite[p] = ManufactureProcessRequisite[p] + value;
                    }
                    else
                    {
                        ManufactureProcessRequisite.Add(p, value);
                    }
                }
            }
        }

        public void GenerateJob()
        {
            ScheduleManufactureJobs = new Dictionary<ManufactureJobNavigation, ApsManufactureJob>();
            ScheduleAssemblyJobs = new Dictionary<AssemblyJobNavigation, ApsAssemblyJob>();

            foreach (var order in OrdersList)
            {
                ProductInstances.Add(order, new List<ProductInstance>());

                var orderProduct = order.Product;
                for (int i = 0; i < order.Amount; i++)
                {
                    var productInstance = new ProductInstance(Guid.NewGuid(), orderProduct, order);
                    ProductInstances[order].Add(productInstance);

                    // var assemblyProcess = order.Product.ApsAssemblyProcess;
                    // var assemblyJobNavigation = new AssemblyJobNavigation(order, productInstance, assemblyProcess);
                    //
                    // ScheduleAssemblyJobs.Add(assemblyJobNavigation, new ApsAssemblyJob
                    // {
                    //     Id = Guid.NewGuid(),
                    //     ApsOrder = order,
                    //     ApsProduct = orderProduct,
                    //     ProductInstance = productInstance,
                    //     Duration = assemblyProcess.ProductionTime,
                    //     Workspace = Workspace.加工,
                    // });

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
                                var jobNavigation = new ManufactureJobNavigation(order, productInstance,
                                    semiProductInstance,
                                    manufactureProcess);


                                if (manufactureProcess.ProductionMode == ProductionMode.Bp)
                                {
                                    ScheduleManufactureJobs.Add(
                                        jobNavigation
                                        , new ApsManufactureJob()
                                        {
                                            Id = Guid.NewGuid(),
                                            ApsOrder = order,
                                            ApsProduct = orderProduct,
                                            ProductInstance = productInstance,
                                            ApsSemiProduct = semiProduct,
                                            SemiProductInstance = semiProductInstance,
                                            ApsManufactureProcess = manufactureProcess,
                                            Duration = manufactureProcess.ProductionTime,
                                            Workspace = Workspace.加工,
                                        });
                                }
                                else
                                {
                                    ScheduleManufactureJobs.Add(
                                        jobNavigation
                                        , new ApsManufactureJob()
                                        {
                                            Id = Guid.NewGuid(),
                                            ApsOrder = order,
                                            ApsProduct = orderProduct,
                                            ProductInstance = productInstance,
                                            ApsSemiProduct = semiProduct,
                                            SemiProductInstance = semiProductInstance,
                                            ApsManufactureProcess = manufactureProcess,
                                            Duration = manufactureProcess.ProductionTime,
                                            Workspace = Workspace.加工,
                                        });
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetStreamJob()
        {
            foreach (var scheduleManufactureJob in ScheduleManufactureJobs.Values.Where(x =>
                x.ApsManufactureProcess.ProductionMode == ProductionMode.Sp))
            {
                var jobDuration = scheduleManufactureJob.ApsManufactureProcess.ProductionTime;

                string suffix = $"Order:{scheduleManufactureJob.ApsOrder.Id}_" +
                                $"Product:{scheduleManufactureJob.ProductInstance.Id}_" +
                                $"SemiProduct:{scheduleManufactureJob.SemiProductInstance.Id}_" +
                                $"Process:{scheduleManufactureJob.ApsManufactureProcess.Id}_" +
                                $"Duration:{jobDuration}";


                IntVar startVar = Model.NewIntVar(0, Ub, "start" + suffix);
                IntVar endVar = Model.NewIntVar(0, Ub, "end" + suffix);
                IntervalVar interval =
                    Model.NewIntervalVar(startVar, (int) jobDuration.TotalMinutes, endVar, "interval" + suffix);


                scheduleManufactureJob.Vars = new JobVar(startVar, endVar, interval);
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
                .ToDictionary(x => x.First().ApsManufactureProcess);


            foreach (var (manufactureProcess, requisite) in ManufactureProcessRequisite.Where(x =>
                x.Key.ProductionMode == ProductionMode.Bp))
            {
                var batch = manufactureProcess.MaximumProductionQuantity.GetValueOrDefault(1);

                var jobProductTimes = requisite % manufactureProcess.MaximumProductionQuantity == 0
                    ? requisite / batch
                    : requisite / batch + 1;

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

                    var batchId = Guid.NewGuid();

                    var jobVar = new JobVar(startVar, endVar, interval);

                    for (int j = 0; j < productedOnce; j++)
                    {
                        jobsInBatch[producted].Vars = jobVar;
                        jobsInBatch[producted].BatchId = batchId;
                        producted++;
                        remainder--;
                    }
                }
            }
        }

        public void SetExternalManufactureJob(IEnumerable<ApsManufactureJob> manufactureJobs)
        {
            foreach (var manufactureJob in manufactureJobs)
            {
                var jobNavigation = new ManufactureJobNavigation(manufactureJob.ApsOrder,
                    manufactureJob.ProductInstance,
                    manufactureJob.SemiProductInstance, manufactureJob.ApsManufactureProcess);

                var scheduleManufactureJob = _mapper.Map<ApsManufactureJob, ApsManufactureJob>(manufactureJob);

                var suffix = jobNavigation.ToString();

                var startVar = Model.NewIntVar(0, Ub, "start" + suffix);
                var endVar = Model.NewIntVar(0, Ub, "end" + suffix);
                var interval = Model.NewIntervalVar(startVar, (int) manufactureJob.Duration.TotalMinutes, endVar,
                    "interval" + suffix);

                scheduleManufactureJob.Vars = new JobVar(startVar, endVar, interval);

                ScheduleManufactureJobs.Add(jobNavigation, scheduleManufactureJob);
            }
        }

        public void AssignResource(IEnumerable<ApsResource> resources)
        {
            // var resourceJobsFromBpDistinct = ScheduleManufactureJobs
            //     .Where(x => x.Key.ManufactureProcess.ProductionMode == ProductionMode.Bp)
            //     .GroupBy(x => x.Value.Vars,
            //         (interval, pairs) => pairs.First().Value).ToList();

            var resourceJobsFromBpDistinct = ScheduleManufactureJobs.Values
                .DistinctBy(x => x.BatchId).ToList();

            _batchJobDictionary = new Dictionary<Guid, List<ApsManufactureJob>>();

            foreach (var batchJob in ScheduleManufactureJobs.Values.Where(x =>
                x.ApsManufactureProcess.ProductionMode == ProductionMode.Bp))
            {
                if (_batchJobDictionary.ContainsKey(batchJob.BatchId))
                {
                    _batchJobDictionary[batchJob.BatchId].Add(batchJob);
                }
                else
                {
                    _batchJobDictionary.Add(batchJob.BatchId, new List<ApsManufactureJob>());
                    _batchJobDictionary[batchJob.BatchId].Add(batchJob);
                }
            }

            var resourceJobsFromSp = ScheduleManufactureJobs
                .Where(x => x.Key.ManufactureProcess.ProductionMode == ProductionMode.Sp)
                .Select(x => x.Value);

            _jobs = new List<ApsJob>(resourceJobsFromSp);
            _jobs.AddRange(resourceJobsFromBpDistinct);

            _resources = resources.ToList();

            var jobCount = _jobs.Count;
            var resourcesCount = _resources.Count;

            var resourceJobMatrix = new Dictionary<int, IntVar>[jobCount, resourcesCount];

            for (var i = 0; i < jobCount; i++)
            {
                for (var j = 0; j < resourcesCount; j++)
                {
                    resourceJobMatrix[i, j] = new Dictionary<int, IntVar>();
                    var resource = _resources[j];
                    foreach (var resourceAttribute in resource.ResourceAttributes)
                    {
                        var intVar = Model.NewIntVar(0, 1,
                            $"Job:{_jobs[i].Id}_," +
                            $"Resource:{resource.Id}_," +
                            $"Class:{resourceAttribute.ResourceClass.ResourceClassName}");
                        resourceJobMatrix[i, j].Add(resourceAttribute.ResourceClass.Id, intVar);
                    }
                }
            }


            for (var i = 0; i < jobCount; i++)
            {
                var job = _jobs[i];
                var processResourcesNeed =
                    (job as ApsManufactureJob)?.ApsManufactureProcess.ApsResources;

                if (processResourcesNeed == null)
                {
                    continue;
                }

                var processResourcesVarFromClass = new Dictionary<int, List<IntVar>>();

                for (var j = 0; j < resourcesCount; j++)
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


            var resourceIntervals = new IntervalVar[jobCount, resourcesCount];
            ResourcePerformed = new IntVar[jobCount, resourcesCount];

            for (int i = 0; i < jobCount; i++)
            {
                _jobs[i].Vars.Deconstruct(out var startVar, out var endVar, out _);
                var duration = (int) _jobs[i].Duration.TotalMinutes;

                for (int j = 0; j < resourcesCount; j++)
                {
                    var resourceIsPerformed = Model.NewIntVar(0, 1, $"Performed:[{i}, {j}]");

                    Model.Add(resourceIsPerformed == LinearExpr.Sum(
                        resourceJobMatrix[i, j].Select(x => x.Value)));
                    
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
                if (manufactureProcess.PrevPartId == null) continue;

                var preProcess =
                    ManufactureProcesses.FirstOrDefault(x => x.Id == manufactureProcess.PrevPartId);

                var preJob =
                    ScheduleManufactureJobs
                        [new ManufactureJobNavigation(order, productInstance, semiProductInstance, preProcess)];

                if (preJob == null) continue;
                Model.Add(preJob.Vars.EndVar <= job.Vars.StartVar);

                job.PreJob = preJob;
                job.PreJobId = preJob.Id;
            }
        }

        public void SetResourceAvailableTime(IDictionary<ApsResource, int> resourceAvailableTime)
        {
            foreach (var (resource, availableTime) in resourceAvailableTime)
            {
                var indexOfResource = _resources.IndexOf(resource);

                if (indexOfResource == -1)
                {
                    throw new ArgumentException(
                        $"{nameof(resourceAvailableTime)}不在资源中");
                }

                for (var i = 0; i < _jobs.Count; i++)
                {
                    var job = _jobs[i];
                    Model.Add(job.Vars.StartVar >= availableTime)
                        .OnlyEnforceIf(ResourcePerformed[i, indexOfResource]);
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
                Id = Guid.NewGuid(),
                Orders = new List<ApsOrder>(OrdersList),
                RecordState = RecordState.UnKnow,
                ScheduleFinishTime = null,
                Jobs = new List<ApsManufactureJob>(ScheduleManufactureJobs.Values),
            };


            var entityEntry = await _context.AddAsync(scheduleRecord);
            await _context.SaveChangesAsync();

            var scheduleModel = new ScheduleModel(Model, entityEntry.Entity, ScheduleManufactureJobs, _jobs, _resources,
                ResourcePerformed, _batchJobDictionary);

            _scheduleClass.ScheduleModel = scheduleModel;

            var schedule = new Thread(_scheduleClass.ExecuteAsync);

            schedule.Start();
            schedule.IsBackground = true;

            return scheduleRecord;
        }
    }
}