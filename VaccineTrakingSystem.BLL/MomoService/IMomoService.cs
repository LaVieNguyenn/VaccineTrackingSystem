using Client.Controllers.V1.ThirdParty;
using Microsoft.AspNetCore.Http;

namespace Client.Logics.Commons.MomoLogics
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(MomoExecuteResponseModel model);
        

        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
        
        Task<MomoRefundResponse> CreateRefundAsync(MomoRefundRequest model);

        string ComputeHmacSha256(string message, string secretKey);
    }
}
