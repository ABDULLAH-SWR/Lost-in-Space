using System;

/// <summary>
/// Serializable data container representing a modifier card's physical properties.
/// Used to alter the Rigidbody2D and PhysicsMaterial2D settings of the player.
/// </summary>
[Serializable]
public class CardData
{
    public string CardName;
    public float MassModifier;
    public float BouncinessModifier;
    public float GravityModifier;
}
