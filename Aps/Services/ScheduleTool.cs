using Aps.Infrastructure;
using Google.OrTools.Sat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aps.Shared.Entity;

namespace Aps.Services
{
    public class ScheduleAssemblyJob : ApsAssemblyJob
    {
        public IntVar StartVar { get; set; }
        public IntVar EndVar { get; set; }
        public IntervalVar Interval { get; set; }
    }

    public class ScheduleManufactureJob : ApsManufactureJob
    {
        public IntVar StartVar { get; set; }
        public IntVar EndVar { get; set; }
        public IntervalVar Interval { get; set; }
    }

    public class ScheduleTool
    {
        private readonly ApsContext _context;
        public CpModel Model { get; private set; }
        public IEnumerable<ApsOrder> OrdersList { get; set; }

        public IEnumerable<(ApsProduct, int)> ProductPrerequisite { get; private set; }

        public IEnumerable<(ApsSemiProduct, int)> SemiProductPrerequisite { get; private set; }
        public IEnumerable<ApsManufactureProcess> ManufactureProcesses { get; private set; }
        public IEnumerable<ApsAssemblyProcess> AssemblyProcesses { get; private set; }

        public Dictionary<(ApsOrder, ProductInstance, SemiProductInstance, ApsManufactureProcess),
            ScheduleManufactureJob> ScheduleManufactureJobs { get; private set; }
            = new Dictionary<(ApsOrder, ProductInstance, SemiProductInstance, ApsManufactureProcess),
                ScheduleManufactureJob>();

        public Dictionary<ApsOrder, List<ProductInstance>> ProductInstances { get; private set; } =
            new Dictionary<ApsOrder, List<ProductInstance>>();

        public Dictionary<(ApsOrder, ProductInstance), List<SemiProductInstance>> SemiProductInstances
        {
            get;
            private set;
        } = new Dictionary<(ApsOrder, ProductInstance), List<SemiProductInstance>>();


        public ScheduleTool(ApsContext context)
        {
            _context = context;
            Model = new CpModel();
        }

        public void SetPrerequisite(List<ApsOrder> orders)
        {
            OrdersList = orders;

            ProductPrerequisite = orders.GroupBy(o => o.Product,
                (product, groupOrders) => (product, groupOrders.Count()));

            SemiProductPrerequisite = ProductPrerequisite.SelectMany(p => p.Item1.AssembleBySemiProducts)
                .GroupBy(p => p.ApsSemiProduct,
                    (semiProduct, products) => (semiProduct, products.Sum(p => p.Amount)));
        }

        public void GenerateProcess()
        {
            ManufactureProcesses = SemiProductPrerequisite.SelectMany
                (x => x.Item1.ApsManufactureProcesses);

            AssemblyProcesses = ProductPrerequisite.Select(x => x.Item1.ApsAssemblyProcess);
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
                                string suffix = $"Order:{order.OrderId}_" +
                                                $"Product:{productInstance.ProductId}_" +
                                                $"SemiProduct:{semiProductInstance.SemiProductId}_" +
                                                $"Process:{manufactureProcess.PartId}_" +
                                                $"Duration:{duration}";

                                IntVar startVar = Model.NewIntVar(0, 1000, "start" + suffix);
                                IntVar endVar = Model.NewIntVar(0, 1000, "end" + suffix);
                                IntervalVar interval =
                                    Model.NewIntervalVar(startVar, duration, endVar, "interval" + suffix);

                                ScheduleManufactureJobs.Add(
                                    (order, productInstance, semiProductInstance, manufactureProcess)
                                    , new ScheduleManufactureJob
                                    {
                                        ApsOrder = order,
                                        ApsProduct = orderProduct,
                                        ProductInstance = productInstance,
                                        ApsSemiProduct = semiProduct,
                                        SemiProductInstance = semiProductInstance,
                                        ApsManufactureProcess = manufactureProcess,
                                        StartVar = startVar,
                                        EndVar = endVar,
                                        Interval = interval,
                                    });
                            }
                        }
                    }
                }
            }
        }

        public void DispatchResource()
        {

        }

    }
}