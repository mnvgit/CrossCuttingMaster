namespace CrossCuttingMaster.MediatRPipeline.Handlers.Settings
{
    public class CreateOrderOptions
    {
        /// <summary>
        /// UserId that triggers a special error scenario in CreateOrderHandler.
        /// </summary>
        public int SpecialUserId { get; set; }
    }
}