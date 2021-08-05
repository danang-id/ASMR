//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 4:23 PM
//
// ReadUpdateDeleteRawMaterialRequestModel.cs
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
    public class UpdateBeanRequestModel
    {
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

    }
}
