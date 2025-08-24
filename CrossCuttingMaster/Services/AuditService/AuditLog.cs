namespace CrossCuttingMaster.Services.AuditService
{
    public class AuditLog
    {
        public string? RequestType { get; set; }
        public string? ResponseType { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
    }
}