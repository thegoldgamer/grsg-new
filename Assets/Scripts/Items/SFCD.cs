using UnityEngine;

/// <summary>
/// SFCD.cs — wearable identity device worn on the non-dominant arm.
/// Stores the owner's username and captain flag; used by the voice modulator
/// security system to identify authorized crew, and (later) by keycard scanners
/// and the orbital station tractor beam call-up. Foundation for future tools.
/// Phase 2: identity storage only.
/// </summary>
public class SFCD : MonoBehaviour
{
    [Header("Identification")]
    [Tooltip("Owner username this SFCD is registered to.")]
    public string username = "Player1";
    [Tooltip("Whether this SFCD's owner is the ship captain.")]
    public bool isCaptain = false;

    /// <summary>Whether the owner is authorized for ship systems. (Always true in Phase 2.)</summary>
    public bool IsAuthorized() => true;

    public bool IsCaptain() => isCaptain;

    public string GetUsername() => username;
}
