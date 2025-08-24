namespace CrossCuttingMaster.Settings
{
    /// <summary>
    /// Settings for PerformanceBehavior. Configure in appsettings.json under "PerformanceBehavior".
    /// </summary>
    public class PerformanceBehaviorSettings
    {
        /// <summary>
        /// The threshold in milliseconds for logging slow requests.
        /// </summary>
        public int ThresholdMilliseconds { get; set; }
    }
}