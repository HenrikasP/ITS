using Claims.Contracts.Enums;

namespace Claims.Contracts.Requests;

public class CreateClaimRequest
{
    /// <summary>
    /// Claim Id which will be covered
    /// </summary>
    /// <example>a5e18aa1-d1c7-4d0f-8022-2fc44858c708</example>
    public Guid CoverId { get; set; }
    
    /// <summary>
    /// Date of claim
    /// </summary>
    /// <example>2024-06-01</example>
    public DateOnly Created { get; set; }
    
    /// <summary>
    /// Name of claim
    /// </summary>
    /// <example>Example claim name</example>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Type of claim
    /// </summary>
    /// <example>BadWeather</example>
    public ClaimType Type { get; set; }
    
    /// <summary>
    /// Cost of damage
    /// </summary>
    /// <example>1000</example>
    public decimal DamageCost { get; set; }
}