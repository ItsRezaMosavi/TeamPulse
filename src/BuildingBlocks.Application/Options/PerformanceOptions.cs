namespace BuildingBlocks.Application.Options;

/// <summary>
/// Represents configuration options for the <c>PerformanceBehavior</c>.
/// </summary>
/// <remarks>
/// These options control how request execution times are monitored and when
/// performance warnings are logged.
/// </remarks>
public class PerformanceOptions
{
    /// <summary>
    /// Gets or sets the execution time threshold, in milliseconds, above which
    /// a request is considered slow and a warning is logged.
    /// </summary>
    /// <value>
    /// The default value is <c>500</c> milliseconds.
    /// </value>
    public int ThresholdMilliseconds { get; set; } = 500;
}