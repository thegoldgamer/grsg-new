using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Engine status. Phase 2 only actively uses Online/Offline/Destroyed; the rest
/// are reserved stubs for later phases (discharge lever, Overrider, thermal runaway).
/// </summary>
public enum EngineStatus
{
    Online,
    Offline,
    Destroyed,
    COffline,     // discharge lever (future)
    Overridden,   // Overrider device (future)
    Overheating,  // future
    ThermalRunaway, // future
    Fatal         // future
}

/// <summary>
/// Engine.cs — placed on the engine GameObject. Takes a normalized thrust request
/// from ShipController and converts it to an actual force value, scaled by engine
/// health and ship fuel, then pushes a visual power level out to each Nacelle.
/// Phase 2 implements the basics only.
/// </summary>
public class Engine : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The root Ship. Auto-found from parent if left empty.")]
    public Ship shipReference;
    [Tooltip("Nacelles driven by this engine. Drag each Nacelle GameObject here (default 2 for the test ship).")]
    public List<Nacelle> nacelles = new List<Nacelle>();

    [Header("State")]
    [Tooltip("Current engine status. Phase 2 uses Online/Offline/Destroyed.")]
    public EngineStatus status = EngineStatus.Online;
    [Range(0f, 100f)]
    [Tooltip("Engine health. Degrades from E-brake abuse/damage. At 0 the engine is destroyed.")]
    public float engineHealth = 100f;

    [Header("Tuning")]
    [Tooltip("Health lost per second of E-brake usage.")]
    public float eBrakeDamageAmount = 0.5f;
    [Tooltip("Base force produced at full throttle and full health (before drive-mode multiplier).")]
    public float baseThrustPower = 5000f;
    [Tooltip("Fuel units consumed per second at full power.")]
    public float fuelBurnRate = 1f;

    /// <summary>True when the engine is actively running (Online).</summary>
    public bool IsRunning => status == EngineStatus.Online;

    private void Awake()
    {
        if (shipReference == null)
            shipReference = GetComponentInParent<Ship>();
    }

    /// <summary>Called by a Nacelle to register itself if not already wired in the Inspector.</summary>
    public void RegisterNacelle(Nacelle nacelle)
    {
        if (nacelle != null && !nacelles.Contains(nacelle))
            nacelles.Add(nacelle);
    }

    /// <summary>
    /// Converts a normalized thrust request (0..1) and drive mode into an actual
    /// signed force value. Negative = reverse. Also updates nacelle visuals and burns fuel.
    /// Returns 0 when the engine can't produce force.
    /// </summary>
    public float ProcessThrust(float requestedThrust, DriveMode mode)
    {
        // Hard-off states produce no force and kill visuals.
        if (status == EngineStatus.Offline ||
            status == EngineStatus.Destroyed ||
            status == EngineStatus.COffline)
        {
            UpdateNacelles(0f);
            return 0f;
        }

        // Overrider (future): forces max forward, uncontrollable.
        if (status == EngineStatus.Overridden)
        {
            requestedThrust = 1f;
            mode = DriveMode.DRIVE;
        }

        requestedThrust = Mathf.Clamp01(Mathf.Abs(requestedThrust));

        // Drive-mode multiplier.
        float multiplier;
        switch (mode)
        {
            case DriveMode.DRIVE:   multiplier = 1f; break;
            case DriveMode.REVERSE: multiplier = -0.5f; break;  // reverse is half speed
            default:                multiplier = 0f; break;     // NEUTRAL / PARK = no force
        }

        float healthFactor = engineHealth / 100f;
        float actualThrust = requestedThrust * baseThrustPower * multiplier * healthFactor;

        // Normalized visual power (0..1-ish) for nacelle particles/audio.
        float visualPower = requestedThrust * Mathf.Abs(multiplier) * healthFactor;
        if (status == EngineStatus.Overridden)
            visualPower = 2f; // extreme visuals (future)

        UpdateNacelles(visualPower);

        // Burn fuel proportional to power actually used.
        if (shipReference != null && visualPower > 0f)
        {
            shipReference.ConsumeFuel(visualPower * fuelBurnRate * Time.fixedDeltaTime);
            if (!shipReference.HasFuel)
                status = EngineStatus.Offline;
        }

        return actualThrust;
    }

    /// <summary>Degrades engine health while the E-brake is held.</summary>
    public void TriggerEBrakeDamage()
    {
        if (status == EngineStatus.Offline || status == EngineStatus.Destroyed)
            return;

        engineHealth -= eBrakeDamageAmount * Time.deltaTime;
        if (engineHealth <= 0f)
        {
            engineHealth = 0f;
            status = EngineStatus.Destroyed;
            UpdateNacelles(0f);
        }
    }

    /// <summary>Pushes a normalized power level out to every registered nacelle.</summary>
    private void UpdateNacelles(float powerLevel)
    {
        for (int i = 0; i < nacelles.Count; i++)
        {
            if (nacelles[i] != null)
                nacelles[i].SetPowerLevel(powerLevel);
        }
    }
}
