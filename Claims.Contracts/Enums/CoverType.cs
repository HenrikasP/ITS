namespace Claims.Contracts.Enums;

public enum CoverType
{
    
    /// <summary>
    /// Yacht up to 1 ton
    /// </summary>
    Yacht = 0,
    
    /// <summary>
    /// Passenger up to 150 000 tons and up to 3000 passengers
    /// </summary>
    PassengerShip = 1,
    
    /// <summary>
    /// Doesn't include vessels  in high risk waters
    /// </summary>
    ContainerShip = 2,
    
    /// <summary>
    /// Doesn't include vessels  in high risk waters
    /// </summary>
    BulkCarrier = 3,
    
    /// <summary>
    /// Doesn't include vessels  in high risk waters
    /// </summary>
    Tanker = 4
}