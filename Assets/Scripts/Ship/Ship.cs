using UnityEngine;

/// <summary>
/// Ship presets. Selecting anything other than Generic auto-configures stats.
/// Generic leaves all fields manually editable.
/// </summary>
public enum ShipPreset
{
    Zenith,
    ToppatAirship,
    OrbitalStation,
    Generic
}

/// <summary>
/// Ship.cs — the root "brain" placed on the root ship GameObject.
/// Single source of truth for ship-wide stats (power, fuel) and which built-in
/// features the ship supports. Every other module (Engine, ShipController, etc.)
/// references this. Phase 2 keeps this lightweight; more stats get added later.
/// </summary>
[DisallowMultipleComponent]
public class Ship : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Select a preset to auto-configure stats, or Generic for manual configuration.")]
    public ShipPreset currentPreset = ShipPreset.Generic;

    [Header("Stats")]
    [Tooltip("Maximum power the ship can output. Drives engine thrust ceiling.")]
    public float maxPower = 1000f;
    [Tooltip("Total fuel the ship can hold.")]
    public float fuelCapacity = 100f;
    [Tooltip("Current fuel remaining. Set to capacity on Start.")]
    public float currentFuel = 100f;

    [Header("Built-in Features")]
    public bool hasInvisiTech = false;
    public bool hasHyperdrive = false;
    public bool hasGravityGenerator = false;
    public bool hasLifeSupport = false;
    public bool hasCollisionCourseDetection = false;
    public bool hasEmergencySeparation = false;
    public bool hasBattleArtillery = false;

    /// <summary>True when the ship has any fuel left.</summary>
    public bool HasFuel => currentFuel > 0f;

    private void OnValidate()
    {
        ApplyPreset();
    }

    private void Start()
    {
        // Always start full.
        currentFuel = fuelCapacity;
    }

    /// <summary>
    /// Auto-configures stats/features from the chosen preset. Generic is a no-op
    /// so the designer's manual values survive.
    /// </summary>
    private void ApplyPreset()
    {
        switch (currentPreset)
        {
            case ShipPreset.Zenith:
                maxPower = 2500f;
                fuelCapacity = 500f;
                hasInvisiTech = true;
                hasHyperdrive = true;
                hasGravityGenerator = true;
                hasLifeSupport = true;
                hasCollisionCourseDetection = true;
                hasEmergencySeparation = true;
                hasBattleArtillery = true;
                break;

            case ShipPreset.ToppatAirship:
                maxPower = 1200f;
                fuelCapacity = 300f;
                hasInvisiTech = false;
                hasHyperdrive = false;
                hasGravityGenerator = false;
                hasLifeSupport = false;
                hasCollisionCourseDetection = false;
                hasEmergencySeparation = false;
                hasBattleArtillery = false; // Uses drill pods, handled separately.
                break;

            case ShipPreset.OrbitalStation:
                maxPower = 99999f;   // Effectively infinite energy core.
                fuelCapacity = 99999f;
                hasInvisiTech = false; // Item based, not built in.
                hasHyperdrive = false;
                hasGravityGenerator = true;
                hasLifeSupport = true;
                hasCollisionCourseDetection = false;
                hasEmergencySeparation = false;
                hasBattleArtillery = true;
                break;

            case ShipPreset.Generic:
                // Do not override user values.
                break;
        }

        // Keep current fuel sane if capacity shrank.
        if (currentFuel > fuelCapacity)
            currentFuel = fuelCapacity;
    }

    /// <summary>Drains fuel, clamped at zero.</summary>
    public void ConsumeFuel(float amount)
    {
        currentFuel = Mathf.Max(0f, currentFuel - amount);
    }
}
