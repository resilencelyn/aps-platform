﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Aps.Shared.Entity
{
    public class SemiProductInstance
    {
        [Key]
        public Guid SemiProductId { get; set; }
        public ApsSemiProduct ApsSemiProduct { get; set; }
        public ProductInstance ProductAssemblyTo { get; set; }
        public SemiProductInstance(Guid semiProductId, ApsSemiProduct apsSemiProduct, ProductInstance productAssemblyTo)
        {
            SemiProductId = semiProductId;
            ApsSemiProduct = apsSemiProduct ?? throw new ArgumentNullException(nameof(apsSemiProduct));
            ProductAssemblyTo = productAssemblyTo ?? throw new ArgumentNullException(nameof(productAssemblyTo));
        }
        public SemiProductInstance()
        {
        }
    }
}