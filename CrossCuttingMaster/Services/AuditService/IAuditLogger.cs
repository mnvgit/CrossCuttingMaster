namespace CrossCuttingMaster.Services.AuditService
{
    public interface IAuditLogger
    {
        public Task LogAsync(AuditLog log);
    }
}