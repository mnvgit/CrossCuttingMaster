namespace CrossCuttingMaster.MediatRPipeline.Behaviors.CustomExceptions
{
    public class MissingIdempotencyKey(string msg) : Exception(msg)
    {
    }
}
