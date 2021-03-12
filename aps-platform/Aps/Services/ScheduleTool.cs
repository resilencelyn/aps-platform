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
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;
        public CpModel Model { get; private set; }
        public CpSolver Solver { get; private set; }
        public IEnumerable<ApsOrder> OrdersList { get; set; }

        public Dictionary<ApsProduct, int> ProductPrerequisite { get; private set; }

        public Dictionary<ApsSemiProduct, int> SemiProductPrerequisite { get; private set; }

        public List<ApsManufactureProcess> ManufactureProcesses { get; private set; }

        public IEnumerable<ApsAssemblyProcess> AssemblyProcesses { get; private set; }
        public IDictionary<ApsManufactureProcess, int> ManufactureProcessRequisite { get; private set; }


        public Dictionary<(ApsOrder, ProductInstance, SemiProductInstance, ApsManufactureProcess),
            ScheduleManufactureJob> ScheduleManufactureJobs
        { get; private set; }
            = new Dictionary<(ApsOrder, ProductInstance, SemiProductInstance, ApsManufactureProcess),
                ScheduleManufactureJob>();


        public Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; private set; } =
            new Dictionary<ApsOrder, List<ProductInstance>>();

        public Dictionary<(ApsOrder order, ProductInstance productInstance), List<SemiProductInstance>>
            SemiProductInstances
        { get; private set; } =
            new Dictionary<(ApsOrder apsOrder, ProductInstance productInstance), List<SemiProductInstance>>();


        public ScheduleTool(ApsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Model = new CpModel();
            Solver = new CpSolver();
        }

        public void SetPrerequisite(List<ApsOrder> orders)
        {
            OrdersList = orders;

            ProductPrerequisite = orders.GroupBy(o => o.Product,
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

                                if (manufactureProcess.ProductionMode == ProductionMode.Bp)
                                {
                                    ScheduleManufactureJobs.Add(
                                        (order, productInstance, semiProductInstance, manufactureProcess)
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
                                        });
                                }
                                else
                                {
                                    IntVar startVar = Model.NewIntVar(0, Ub, "start" + suffix);
                                    IntVar endVar = Model.NewIntVar(0, Ub, "end" + suffix);
                                    IntervalVar interval =
                                        Model.NewIntervalVar(startVar, (int)duration.TotalMinutes, endVar,
                                            "interval" + suffix);

                                    ScheduleManufactureJobs.Add(
                                        (order, productInstance, semiProductInstance, manufactureProcess)
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
                                            Vars = new JobVar(startVar, endVar, interval)
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
            var jobGroupByProcess = ScheduleManufactureJobs.Where(x =>
                    x.Key.Item4.ProductionMode == ProductionMode.Bp)
                .GroupBy(x => x.Key.Item4,
                    (process, jobPairs) => { return jobPairs.Select(x => x.Value).ToList(); })
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
                        Model.NewIntervalVar(startVar, (int)jobDuration.TotalMinutes, endVar,
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
                .Where(x => x.Key.Item4.ProductionMode == ProductionMode.Bp)
                .GroupBy(x => x.Value.Vars,
                    (interval, pairs) => pairs.First().Value).ToList();

            var resourceJobsFromSp = ScheduleManufactureJobs
                .Where(x => x.Key.Item4.ProductionMode == ProductionMode.Sp)
                .Select(x => x.Value);

            var resourceJobs = new List<ScheduleManufactureJob>(resourceJobsFromSp);
            resourceJobs.AddRange(resourceJobsFromBp);

            var resources = await _context.ApsResources
                .AsNoTracking()
                .Include(x => x.ResourceAttributes)
                .ThenInclude(x => x.ResourceClass)
                .ToListAsync();

            var jobCount = resourceJobs.Count;
            var resourcesCount = resources.Count;

            var resourceJobMatrix = new Dictionary<int, IntVar>[jobCount, resourcesCount];

            for (int i = 0; i < jobCount; i++)
            {
                for (int j = 0; j < resourcesCount; j++)
                {
                    resourceJobMatrix[i, j] = new Dictionary<int, IntVar>();
                    var resource = resources[j];
                    foreach (var resourceAttribute in resource.ResourceAttributes)
                    {
                        var intVar = Model.NewIntVar(0, 1,
                            $"Job:{resourceJobs[i].ApsManufactureProcess.PrevPart}_," +
                            $"Resource:{resource.Id}_," +
                            $"Class:{resourceAttribute.ResourceClass.ResourceClassName}");
                        resourceJobMatrix[i, j].Add(resourceAttribute.ResourceClass.Id, intVar);
                    }
                }
            }


            for (int i = 0; i < jobCount; i++)
            {
                var job = resourceJobs[i];
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
                            processResourcesVarFromClass.Add(resourceClassId, new List<IntVar> { performed });
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


            JobVar[,] resourceIntervals = new JobVar[jobCount, resourcesCount];

            for (int i = 0; i < jobCount; i++)
            {
                resourceJobs[i].Vars.Deconstruct(out IntVar startVar, out IntVar endVar, out IntervalVar intervalVar);
                var duration = (int)resourceJobs[i].Duration.TotalMinutes;

                for (int j = 0; j < resourcesCount; j++)
                {
                    var resourceIsPerformed = Model.NewIntVar(0, 1, $"Performed:[{i}, {j}]");

                    Model.Add(resourceIsPerformed == LinearExpr.Sum(resourceJobMatrix[i, j].Select(x => x.Value)));

                    var optionalIntervalVar = Model.NewOptionalIntervalVar(startVar, duration, endVar,
                        resourceIsPerformed, $"Resource:{i}, Job:{j}");

                    resourceIntervals[i, j] = new JobVar(startVar, endVar, optionalIntervalVar);
                }
            }

            for (int i = 0; i < resourcesCount; i++)
            {
                var resourceInterval = new List<IntervalVar>();
                for (int j = 0; j < jobCount; j++)
                {
                    resourceInterval.Add(resourceIntervals[j, i].Interval);
                }

                Model.AddNoOverlap(resourceInterval);
            }
        }

        public void SetPreJobConstraint()
        {
            foreach (var ((apsOrder, productInstance, semiProductInstance, apsManufactureProcess), job) in
                ScheduleManufactureJobs)
            {
                if (apsManufactureProcess.PrevPartId != null)
                {
                    ApsManufactureProcess preProcess =
                        ManufactureProcesses.FirstOrDefault(x => x.Id == apsManufactureProcess.PrevPartId);

                    var preJob = ScheduleManufactureJobs[(apsOrder, productInstance, semiProductInstance, preProcess)];
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

        public async Task<IEnumerable<JobDto>> Solve()
        {
            Solver.StringParameters = "max_time_in_seconds:30.0";

            CpSolverStatus cpSolverStatus = await Task.Run(() => Solver.Solve(Model));
            Console.WriteLine(cpSolverStatus);
            switch (cpSolverStatus)
            {
                case CpSolverStatus.Unknown:
                    throw new ApplicationException("未知模型");
                case CpSolverStatus.ModelInvalid:
                    throw new ApplicationException("模型错误");
                case CpSolverStatus.Feasible:
                    break;
                case CpSolverStatus.Infeasible:
                    break;
                case CpSolverStatus.Optimal:
                    break;
            }

            var solutionDictionary = new Dictionary<IntVar, int>();
            var objectiveValueString = $"Optimal Schedule Length: {Solver.ObjectiveValue}";

            var now = DateTime.Now;
            foreach (var ((apsOrder, productInstance, semiProductInstance, apsManufactureProcess), job) in
                ScheduleManufactureJobs)
            {
                job.Vars.Deconstruct(out var startVar, out var endVar, out _);

                var startValue = Solver.Value(startVar);
                job.Start = now.AddMinutes(solutionDictionary.ContainsKey(startVar)
                    ? solutionDictionary[startVar]
                    : startValue);

                var endValue = Solver.Value(endVar);
                job.End = now.AddMinutes(solutionDictionary.ContainsKey(endVar)
                    ? solutionDictionary[endVar]
                    : endValue);
            }

            return _mapper.Map<IEnumerable<ScheduleManufactureJob>, IEnumerable<JobDto>>(
                ScheduleManufactureJobs.Select(x => x.Value));
        }
    }
}