namespace Claims.Contracts.Enums;

public enum ClaimType
{
    /// <summary>
    /// Claim type of collision
    /// </summary>
    Collision = 0,
    
    /// <summary>
    /// Claim type of grounding
    /// </summary>
    Grounding = 1,
    
    /// <summary>
    /// Claim type of bad weather. Includes but not limited to : storms, hails, hurricanes 
    /// </summary>
    BadWeather = 2,
    
    /// <summary>
    /// Claim type of fire. Doesn't include cases of negligence
    /// </summary>
    Fire = 3
}