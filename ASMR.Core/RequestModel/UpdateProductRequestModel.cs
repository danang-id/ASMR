//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 11:05 PM
//
// UpdateProductRequestModel.cs
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
    public class UpdateProductRequestModel
    {
        [Required(ErrorMessage = "Please fill in the product price.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please fill in the product weight per packaging.")]
        public decimal WeightPerPackaging { get; set; }
    }
}
