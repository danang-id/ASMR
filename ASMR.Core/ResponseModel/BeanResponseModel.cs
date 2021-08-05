//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 4:15 PM
//
// BeanResponseModel.cs
//
using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel
{
    public class BeanResponseModel : DefaultResponseModel<Bean>
    {
        public BeanResponseModel() 
        {
        }

        public BeanResponseModel(Bean bean) : base(bean)
        {
        }

        public BeanResponseModel(ResponseError error) : base(error)
        {
        }

        public BeanResponseModel(IEnumerable<ResponseError> errors) : base(errors)
        {
        }
    }
}