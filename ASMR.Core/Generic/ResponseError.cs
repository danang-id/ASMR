//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 1:31 PM
//
// ErrorModel.cs
//
namespace ASMR.Core.Generic
{
    public class ResponseError
    {
        public ResponseError(int code, string reason)
        {
            Code = code;
            Reason = reason;
            SupportId = null;
        }

        public ResponseError(int code, string reason, string supportId)
        {
            Code = code;
            Reason = reason;
            SupportId = supportId;
        }

        public int Code { get; set; }

        public string Reason { get; set; }

        public string SupportId { get; set; }
    }
}
