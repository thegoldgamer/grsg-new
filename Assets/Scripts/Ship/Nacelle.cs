using UnityEngine;

/// <summary>
/// Nacelle status. Phase 2 uses Online/Offline/Destroyed; the rest are reserved
/// stubs for later phases (overheating, thermal runaway, integrity loss, break-off).
/// </summary>
public enum NacelleStatus
{
    Online,
    Offline,
    Destroyed,
    Overheating,    // future
    ThermalRunaway, // future
    ICompromised,   // future
    Gone            // future (physically broken off)
}

/// <summary>
/// Nacelle.cs — placed on each individual booster/thruster GameObject. Receives a
/// normalized power level (0..1) from its Engine and responds with particle + audio
/// visuals, and an optional physical force contribution at its thrustPoint.
/// Each nacelle is independent — one can fail without taking the others down.
/// </summary>
public class Nacelle : MonoBehaviour
{
    [Header("State")]
    [Tooltip("Current nacelle status. Phase 2 uses Online/Offline/Destroyed.")]
    public NacelleStatus status = NacelleStatus.Online;

    [Header("Visuals & Audio")]
    [Tooltip("Exhaust particle system. Emission scales with power.")]
    public ParticleSystem exhaustParticles;
    [Tooltip("Looping engine audio. Pitch/volume scale with power.")]
    public AudioSource engineAudio;
    [Tooltip("Point this nacelle pushes from (optional). Used for per-nacelle force.")]
    public Transform thrustPoint;

    [Header("Force Contribution (optional)")]
    [Tooltip("If assigned, this nacelle adds force to the ship Rigidbody at thrustPoint. " +
             "Leave shipBody empty to let ShipController handle all force centrally instead.")]
    public Rigidbody shipBody;
    [Tooltip("Force this nacelle contributes at full power (only used if shipBody is set).")]
    public float nacelleForce = 0f;

    [Header("Particle Tuning")]
    public float maxEmissionRate = 100f;

    private ParticleSystem.EmissionModule emissionModule;
    private float defaultAudioVolume = 1f;
    private float currentPower = 0f;

    private void Awake()
    {
        // Auto-register with parent engine if present.
        Engine parentEngine = GetComponentInParent<Engine>();
        if (parentEngine != null)
            parentEngine.RegisterNacelle(this);

        if (exhaustParticles != null)
            emissionModule = exhaustParticles.emission;

        if (engineAudio != null)
            defaultAudioVolume = engineAudio.volume;
    }

    /// <summary>
    /// Sets this nacelle's power level (0..1). Failed nacelles clamp to 0 regardless.
    /// </summary>
    public void SetPowerLevel(float normalizedPower)
    {
        if (status == NacelleStatus.Offline ||
            status == NacelleStatus.Destroyed ||
            status == NacelleStatus.Gone)
        {
            normalizedPower = 0f;
        }

        currentPower = Mathf.Max(0f, normalizedPower);

        UpdateParticles(currentPower);
        UpdateAudio(currentPower);
    }

    private void FixedUpdate()
    {
        // Optional per-nacelle force. Only used when explicitly wired (shipBody + force).
        if (shipBody != null && nacelleForce != 0f && currentPower > 0f && thrustPoint != null)
        {
            Vector3 force = thrustPoint.forward * (nacelleForce * currentPower);
            shipBody.AddForceAtPosition(force, thrustPoint.position, ForceMode.Force);
        }
    }

    private void UpdateParticles(float power)
    {
        if (exhaustParticles == null) return;

        if (power > 0.001f)
        {
            if (!exhaustParticles.isPlaying)
                exhaustParticles.Play();
            emissionModule.rateOverTime = maxEmissionRate * power;
        }
        else if (exhaustParticles.isPlaying)
        {
            exhaustParticles.Stop();
        }
    }

    private void UpdateAudio(float power)
    {
        if (engineAudio == null) return;

        if (power > 0.001f)
        {
            if (!engineAudio.isPlaying)
                engineAudio.Play();
            engineAudio.pitch = Mathf.Lerp(0.5f, 2.0f, power);
            engineAudio.volume = Mathf.Lerp(0f, defaultAudioVolume, power);
        }
        else if (engineAudio.isPlaying)
        {
            engineAudio.Stop();
        }
    }
}
