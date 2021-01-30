using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Sat;

namespace 高级计划与排产.algorithm
{
    public class Schedule
    {
        public static void Main()
        {
            CpModel model = new CpModel();

            var productsData =
                new List<(int machine, int duration, List<(int semiProduct, int amount)> semiProductList)>
                {
                    (0, 3, new List<(int semiProduct, int amount)> {(0, 1), (0, 1)}),
                    (1, 4, new List<(int semiProduct, int amount)> {(0, 1), (1, 2), (2, 1)}),
                    (2, 6, new List<(int semiProduct, int amount)> {(0, 2), (2, 1)}),
                };


            var semiProductsData =
                new[]
                {
                    new List<(int machine, int duration, int[] preNeed)>
                    {
                        (0, 3, new int[] { }), (1, 2, new[] {0}), (2, 2, new[] {0}), (2, 1, new[] {2}),
                        (1, 4, new int[] { }), (2, 3, new int[] {0, 1, 2, 3, 4})
                    },
                    new List<(int machine, int duration, int[] preNeed)>
                    {
                        (0, 2, new int[] { }), (2, 1, new int[] { }), (1, 4, new int[] {0}), (0, 3, new int[] { }),
                        (1, 2, new[] {0, 1, 2, 3}),
                    },
                    new List<(int machine, int duration, int[] preNeed)>
                    {
                        (1, 4, new int[] { }), (2, 3, new int[] { }), (2, 1, new[] {0, 1}), (0, 3, new int[] { }),
                        (1, 2, new[] {0, 1, 2, 3}),
                    }
                };

            var allSemiProducts = Enumerable.Range(0, semiProductsData.Length);
            var allSemiProductDemands = new List<List<int>>();
            var allSemiProductTimes = new List<List<IntVar>>();
            var semiProductCount = semiProductsData.Length;

            var machineCount = 1 + semiProductsData.Max(job =>
                job.Max(task => task.machine)
            );
            var allMachines = Enumerable.Range(0, machineCount).ToArray();

            var allTasks = new Dictionary<(int jobId, int taskId),
                (IntVar start, IntVar end, IntervalVar interval)>();
            var machineToIntervals = new List<List<IntervalVar>>();

            for (int i = 0; i < machineCount; i++)
            {
                machineToIntervals.Add(new List<IntervalVar>());
            }

            for (int i = 0; i < semiProductCount; i++)
            {
                allSemiProductDemands.Add(new List<int>());
            }

            for (int semiProductId = 0; semiProductId < semiProductsData.Length; semiProductId++)
            {
                var semiProduct = semiProductsData[semiProductId];
                for (int taskId = 0; taskId < semiProduct.Count; taskId++)
                {
                    var task = semiProduct[taskId];
                    var machine = task.machine;
                    var duration = task.duration;

                    string suffix = $"_{machine}_{duration}";
                    IntVar startVar = model.NewIntVar(0, 10000, "start" + suffix);
                    IntVar endVar = model.NewIntVar(0, 10000, "end" + suffix);
                    // model.AddReservoirConstraint()

                    IntervalVar intervalVar = model.NewIntervalVar(
                        startVar, duration, endVar, "interval" + suffix);


                    allTasks.Add((semiProductId, taskId), (startVar, endVar, intervalVar));
                    machineToIntervals[machine].Add(intervalVar);
                }
            }

            //添加资源约束
            foreach (var machine in allMachines)
            {
                model.AddNoOverlap(machineToIntervals[machine]);
            }

            //半成品数量约束
            foreach (var semiProduct in allSemiProducts)
            {
                // model.AddReservoirConstraint();
            }

            for (int semiProductId = 0; semiProductId < semiProductsData.Length; semiProductId++)
            {
                var semiProduct = semiProductsData[semiProductId];
                for (int taskId = 0; taskId < semiProduct.Count - 1; taskId++)
                {
                    var preNeedTasks = model.NewIntVar(0, 10000, "preNeedMax");
                    var preNeed = semiProductsData[semiProductId][taskId + 1].preNeed;
                    if (preNeed.Length == 0)
                    {
                        continue;
                    }

                    var id = semiProductId;
                    var preNeedVars = preNeed.Select(preTask => allTasks[(id, preTask)].end);
                    model.AddMaxEquality(preNeedTasks, preNeedVars);
                    model.Add(
                        allTasks[(semiProductId, taskId + 1)].start >=
                        preNeedTasks);
                }
            }

            var objVar = model.NewIntVar(0, 10000, "makeSpan");

            // var lastCompleted = new HashSet<IntVar>();
            // for (int jobId = 0; jobId < jobsData.Length; jobId++)
            // {
            //     var job = jobsData[jobId];
            //     lastCompleted.Add(allTasks[(jobId, job.Count - 1)].end);
            // }

            model.AddMaxEquality(objVar, allTasks.Select(x => x.Value.end));
            model.Minimize(objVar);

            CpSolver solver = new CpSolver();
            var status = solver.Solve(model);

            var assignedJobs = new List<List<(long start, int job, int index, int duration)>>();
            for (int i = 0; i < machineCount; i++)
            {
                assignedJobs.Add(new List<(long start, int job, int index, int duration)>());
            }

            for (int jobId = 0; jobId < semiProductsData.Length; jobId++)
            {
                var job = semiProductsData[jobId];
                for (int taskId = 0; taskId < job.Count; taskId++)
                {
                    var task = job[taskId];
                    var machine = task.machine;

                    assignedJobs[machine].Add((
                        solver.Value(allTasks[(jobId, taskId)].start),
                        jobId,
                        taskId,
                        task.duration
                    ));
                }
            }

            string output = "";
            foreach (var machine in allMachines)
            {
                assignedJobs[machine] = assignedJobs[machine].OrderBy(x => x.start).ToList();
                var solLineTasks = "Machine" + machine + ":";
                var solLine = "         ";

                foreach (var assignedTask in assignedJobs[machine])
                {
                    string name = $"job_{assignedTask.job}_{assignedTask.index}";

                    solLineTasks += $"{name,-10}";
                    var start = assignedTask.start;
                    var duration = assignedTask.duration;
                    var solTmp = $"[{start}, {start + duration}]";
                    solLine += $"{solTmp,-10}";
                }

                solLine += '\n';
                solLineTasks += '\n';
                output += solLineTasks;
                output += solLine;
            }

            Console.WriteLine($"Optimal Schedule Length: {solver.ObjectiveValue}");

            Console.WriteLine(output);
        }
    }
}