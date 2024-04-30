using Claims.Contracts.Enums;

namespace Claims.Contracts.Requests;

public class CreateCoverRequest
{
    /// <summary>
    /// Start date of cover
    /// </summary>
    /// <example>2024-06-01</example>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// End date of cover
    /// </summary>
    /// <example>2025-01-01</example>
    public DateOnly EndDate { get; set; }
    
    /// <summary>
    /// Date of claim
    /// </summary>
    /// <example>PassengerShip</example>
    public CoverType Type { get; set; }
}