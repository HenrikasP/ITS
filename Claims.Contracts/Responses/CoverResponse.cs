using Claims.Contracts.Enums;

namespace Claims.Contracts.Responses;

public class CoverResponse
{
    /// <summary>
    /// Id of cover
    /// </summary>
    /// <example>afd1592e-7e50-404e-bf16-6d421e51e21b</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Start date of cover
    /// </summary>
    /// <example>2024-03-01</example>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// End date of cover
    /// </summary>
    /// <example>2024-09-01</example>
    public DateOnly EndDate { get; set; }
    
    /// <summary>
    /// Type of cover
    /// </summary>
    /// <example>BulkCarrier</example>
    public CoverType Type { get; set; }
    
    /// <summary>
    /// Premium of cover
    /// </summary>
    /// <example>201375</example>
    public decimal Premium { get; set; }
}