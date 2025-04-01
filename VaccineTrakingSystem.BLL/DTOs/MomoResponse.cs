using Newtonsoft.Json;

namespace Client.Controllers.V1.ThirdParty
{
    public class MomoResponse
    {
    }

    public class MomoExecuteResponseModel
    {
        public string ErrorCode { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string FullName { get; set; }
        public string OrderInfo { get; set; } 
        public string TransactionId { get; set; }
        
        public string ExtraData { get; set; }
    }

    public class MomoCreatePaymentResponseModel
    {
        public string RequestId { get; set; }
        public int ErrorCode { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
        public string LocalMessage { get; set; }
        public string RequestType { get; set; }
        public string PayUrl { get; set; }
        public string Signature { get; set; }
        public string QrCodeUrl { get; set; }
        public string Deeplink { get; set; }
        public string DeeplinkWebInApp { get; set; }
    }

    public class MomoOptionModel
    {
        public string MomoApiUrl { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string ReturnUrl { get; set; }
        public string ReturnUrlOrder { get; set; }
        public string NotifyUrl { get; set; }
        public string PartnerCode { get; set; }
        public string RequestType { get; set; }
    }

public class MomoIPNModel
    {
        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("partnerCode")]
        public string PartnerCode { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("extraData")]
        public string ExtraData { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("transId")]
        public long TransId { get; set; }

        [JsonProperty("responseTime")]
        public long ResponseTime { get; set; }

        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("payType")]
        public string PayType { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("orderInfo")]
        public string OrderInfo { get; set; }
    }
    public class MomoRefundRequest
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }     
        public string RequestId { get; set; }   
        public long Amount { get; set; }
        public long TransId { get; set; }      
        public string Lang { get; set; }        
        public string Description { get; set; }
        public string Signature { get; set; }
    }

    public class MomoRefundResponse
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public string ExtraData { get; set; }
        public long Amount { get; set; }
        public long TransId { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public long ResponseTime { get; set; }
    }


}
