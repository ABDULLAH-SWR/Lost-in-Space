using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the registry of modifier cards and applies their physics adjustments
/// to the player's Rigidbody2D and PhysicsMaterial2D at runtime.
/// </summary>
public class CardManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the player's Rigidbody2D component.")]
    public Rigidbody2D playerRb;

    [Header("Card Configuration")]
    [Tooltip("List of available card modifiers in the game.")]
    public List<CardData> availableCards = new List<CardData>();

    // 1. Add this static instance variable right at the top
    public static CardManager instance;

    // Your existing variables (like an array/list of CardData) should be here
    // public CardData[] availableCards; 

    // 2. Add the Awake method to set the instance
    void Awake()
    {
        // This tells Unity: "I am the one and only CardManager in the scene!"
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates if you reload the scene
        }
    }


    /// <summary>
    /// Grabs a card from the registry by index, and applies its mass, gravity scale,
    /// and bounciness modifications to the player.
    /// </summary>
    /// <param name="cardIndex">Index of the card in availableCards.</param>
    public void ApplyCard(int cardIndex)
    {
        if(playerRb == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerRb = playerObj.GetComponent<Rigidbody2D>();
            }
        }

        if (playerRb == null)
        {
            Debug.LogError("[CardManager] Could not find the Player in the scene!");
            return;
        }

        if (availableCards == null || cardIndex < 0 || cardIndex >= availableCards.Count)
        {
            Debug.LogError($"[CardManager] Card index {cardIndex} is out of bounds of the availableCards list.");
            return;
        }

        if (playerRb == null)
        {
            Debug.LogError("[CardManager] Player Rigidbody2D reference is null. Cannot apply card.");
            return;
        }

        CardData card = availableCards[cardIndex];

        // Update player's physical properties
        playerRb.mass = card.MassModifier;
        playerRb.gravityScale = card.GravityModifier;

        // Create and assign a new PhysicsMaterial2D for the bounciness modification
        PhysicsMaterial2D newMaterial = new PhysicsMaterial2D();
        newMaterial.name = $"{card.CardName}_PhysicsMaterial";
        newMaterial.bounciness = card.BouncinessModifier;
        newMaterial.friction = 0.4f; // Default standard friction

        playerRb.sharedMaterial = newMaterial;

        Debug.Log($"[CardManager] Applied card '{card.CardName}' -> Mass: {playerRb.mass}, Gravity: {playerRb.gravityScale}, Bounciness: {newMaterial.bounciness}");
    }
}
