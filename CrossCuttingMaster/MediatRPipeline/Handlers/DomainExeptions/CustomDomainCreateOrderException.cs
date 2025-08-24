namespace CrossCuttingMaster.MediatRPipeline.Handlers.DomainExeptions
{
    public enum CreateOrderErrorCode
    {
        InvalidProductId,
        PaymentFailed
    }

    public class CustomDomainCreateOrderException : Exception
    {
        public CreateOrderErrorCode Code { get; }

        public CustomDomainCreateOrderException(string message, CreateOrderErrorCode code) : base(message)
        {
            Code = code;
        }
    }
}
