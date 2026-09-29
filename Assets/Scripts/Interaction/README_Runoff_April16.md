# 🔧 VR Ship Mechanics Runoff (April 16, 2026)

## ✅ Completed Fixes
1. **Throttle Clamping & Disappearing:** Fixed the throttle fighting physics. It no longer auto-slides forward (Harry Pottering) nor disappears to world zero due to incorrect `connectedAnchor` world-to-local space conversions.
2. **Reverse Thrust:** Fixed the double-negative thrust calculation in `ShipController.cs`. The ship will now properly thrust backward instead of continuing forward when the throttle is pulled back.
3. **VR Player Jump:** Implemented a new `VRPlayerJump.cs` script that properly lifts the player via `CharacterController` and allows multiple fallback XR input methods (direct Action, XR Node checking, Spacebar). Jump works!
4. **IK Feet Snapping:** Fixed `IKFootSolver.cs` so legs hang downwards correctly while falling/jumping instead of snapping to 0,0,0 when they lose ground detection.

## 📌 Current Issues to Fix Tomorrow

### 1. 🛑 VR Grabbing Broken When Boarding Ship
*   **The Bug:** The player successfully boards the ship, the `ShipBoardingArea` parents the `XROrigin` to the ship, and the `CharacterController` is disabled so the player can rotate upside down with the ship seamlessly. However, **the player can no longer grab the Throttle or the Yoke**.
*   **The Cause (Likely):** When the `XROrigin` is parented to a moving `Rigidbody` (the Ship), the XR Interaction Toolkit (XRIT) starts failing its physics overlap checks. 
    *   *Possibility A:* The Ship has a non-uniform scale (e.g., `(1.5, 1.0, 2.0)`), which shears the player's colliders, breaking trigger overlaps.
    *   *Possibility B:* The XR Interaction Manager expects world-space coordinates, but moving the player heavily confuses the Direct Interactors.
    *   *Possibility C:* The player's Hand Colliders are hitting the Ship's massive colliders, blocking the raycasts/overlap spheres to the smaller throttle objects inside.
*   **Next Fix Approach for Tomorrow:**
    1.  **Check Ship Scale:** Ensure the Ship root GameObject has exactly `(1, 1, 1)` scale. If the ship model needs scaling, do it on a child graphic object, NOT the root physics object.
    2.  **Interaction Layer Mask:** Ensure the Ship's colliders are on a "Ship" layer and that the VR hands/interactors are set to ignore the Ship layer but include an "Interactables" layer for the Throttle/Yoke.
    3.  **Locomotion System:** Instead of standard parenting `SetParent(transform, true)`, we might need to use a specialized XR vehicle script (like parenting a separate container under the XR Origin, or moving the XROrigin manually in `LateUpdate` instead of a hard parenting hierarchy).

---

## 📝 Next Session Action Items
1.  **Debug Grabbing:** We removed the `SetParent` line AND disabled the `ShipBoardingArea` `OnTriggerEnter` logic entirely to prevent boarding logic from interfering. If the player still cannot grab the throttle or yoke while standing in the ship naturally, the issue is **not the parenting** but likely something fundamental with the colliders, layers, or XROrigin's character controller blocking the rays.
2.  **Layer Matrices:** Check the layer masks of the VR Hand Interactors versus the Ship Hull. Currently the handles are on the `Default` layer, which might be getting blocked by the Ship's own colliders if they are also on the `Default` layer. We should put the Throttle and Yoke on an `Interactable` layer and ignore the Ship layer for hands.
3.  **Ship Scale:** Verify the ship's transform scale is uniform.
4.  **Confirm Colliders Are Hit:** Since we added BoxColliders directly to the `zeniththrottle` and `zenithsyolk` objects via MCP, we need to ensure the Direct/Ray interactors can actually see those colliders.
