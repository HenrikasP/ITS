using Claims.Contracts.Enums;

namespace Claims.Contracts.Requests;

public class ComputePremiumRequest
{
    /// <summary>
    /// Start date of premium
    /// </summary>
    /// <example>2024-01-01</example>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// End date of premium
    /// </summary>
    /// <example>2024-06-01</example>
    public DateOnly EndDate { get; set; }
    
    /// <summary>
    /// Start date of premium
    /// </summary>
    /// <example>BulkCarrier</example>
    public CoverType CoverType { get; set; }
}
