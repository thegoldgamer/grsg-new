using UnityEngine;

/// <summary>
/// DEPRECATED — superseded by ArtificialGravity.cs.
///
/// The old boarding logic reparented the XR Origin to the ship, which broke XRI
/// grabbing under non-uniform parent scale. Player carrying + artificial gravity
/// is now handled by ArtificialGravity.cs (no reparenting). This stub remains only
/// so any lingering scene reference doesn't throw a missing-script error; it does
/// nothing. Safe to remove the component (and this file) once the scene is clean.
/// </summary>
[System.Obsolete("Use ArtificialGravity instead. This component is a no-op.")]
public class ShipBoardingArea : MonoBehaviour
{
    private void Awake()
    {
        // Intentionally does nothing. Disable so it has zero runtime effect.
        enabled = false;
    }
}
