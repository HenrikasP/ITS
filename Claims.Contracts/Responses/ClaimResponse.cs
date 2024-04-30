using System.ComponentModel.DataAnnotations;
using Claims.Contracts.Enums;

namespace Claims.Contracts.Responses;

public class ClaimResponse
{
    /// <summary>
    /// Id of claim
    /// </summary>
    /// <example>54f43bc7-86fe-4908-a7dc-cd2765bb6a25</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Id of cover
    /// </summary>
    /// <example>dad1a2e3-3f22-4830-a26e-b0ab2183aed0</example>
    public Guid? CoverId { get; set; }
    
    /// <summary>
    /// Creation date of claim
    /// </summary>
    /// <example>2024-02-01</example>
    public DateOnly Created { get; set; }
    
    /// <summary>
    /// Name of claim
    /// </summary>
    /// <example>Claim name</example>
    [Required]
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Type of claim
    /// </summary>
    /// <example>Grounding</example>
    public ClaimType Type { get; set; }
    
    /// <summary>
    /// Damage cost of claim
    /// </summary>
    /// <example>6000</example>
    [Required]
    public decimal DamageCost { get; set; }
}