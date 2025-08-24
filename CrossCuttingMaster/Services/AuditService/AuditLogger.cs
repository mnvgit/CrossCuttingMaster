namespace CrossCuttingMaster.Services.AuditService
{
    public class AuditLogger : IAuditLogger
    {
        public async Task LogAsync(AuditLog log)
        {
            // Simulate async logging operation
            await Task.Delay(1);
        }
    }
}
