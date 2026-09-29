using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

/// <summary>
/// ArtificialGravity.cs — placed on the root ship GameObject (needs a trigger Collider
/// describing the "deck" volume riders must be inside to be affected).
///
/// Solves two problems from the old build:
///  1. "Ship flew off without me" — riders are CARRIED with the ship (moving-platform
///     technique, no reparenting) so they travel with it.
///  2. Artificial gravity — while the ship is moving, riders are pulled toward the ship's
///     local "down" and slerped upright relative to the ship, so flipping the ship makes
///     the WORLD appear to spin, not the player.
///
/// While the ship is still, riders get normal world gravity and stay upright (environment
/// behaves normally). Passive zero-g generation is a later phase.
///
/// This component replaces the old ShipBoardingArea behaviour.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ArtificialGravity : MonoBehaviour
{
    [Header("Movement Detection")]
    [Tooltip("Linear speed (units/s) above which the ship counts as 'moving'.")]
    public float moveLinearThreshold = 0.05f;
    [Tooltip("Angular speed (deg/s) above which the ship counts as 'moving'.")]
    public float moveAngularThreshold = 1f;

    [Header("Gravity")]
    [Tooltip("Artificial gravity strength pulling riders toward ship-local down (m/s^2).")]
    public float gravityStrength = 9.81f;
    [Tooltip("How quickly a rider re-orients to match the ship's up axis.")]
    public float reorientSpeed = 4f;

    [Header("Debug")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private int riderCount = 0;

    private Rigidbody rb;

    // Per-rider tracked state.
    private class Rider
    {
        public Transform root;            // the XR Origin transform we move
        public XROrigin origin;
        public CharacterController cc;
        public Rigidbody body;
        public bool ccWasEnabled;
        public bool gravityApplied;       // are we currently overriding this rider?
        public float verticalVelocity;    // accumulated fall speed under artificial gravity
    }

    private readonly Dictionary<Transform, Rider> riders = new Dictionary<Transform, Rider>();

    // Ship transform from the previous physics step (for the carry delta).
    private Vector3 prevPos;
    private Quaternion prevRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        XROrigin origin = other.GetComponentInParent<XROrigin>();
        if (origin == null) return;

        Transform root = origin.transform;
        if (riders.ContainsKey(root)) return;

        riders[root] = new Rider
        {
            root = root,
            origin = origin,
            cc = origin.GetComponent<CharacterController>(),
            body = origin.GetComponent<Rigidbody>(),
            ccWasEnabled = origin.GetComponent<CharacterController>() != null &&
                           origin.GetComponent<CharacterController>().enabled
        };
        riderCount = riders.Count;
    }

    private void OnTriggerExit(Collider other)
    {
        XROrigin origin = other.GetComponentInParent<XROrigin>();
        if (origin == null) return;

        Transform root = origin.transform;
        if (riders.TryGetValue(root, out Rider r))
        {
            RestoreRider(r);
            riders.Remove(root);
        }
        riderCount = riders.Count;
    }

    private void FixedUpdate()
    {
        isMoving = IsShipMoving();

        // Carry delta from last step (where the ship moved/rotated this physics frame).
        Vector3 deltaPos = transform.position - prevPos;
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(prevRot);

        foreach (var kv in riders)
        {
            Rider r = kv.Value;
            if (r.root == null) continue;

            if (isMoving)
                ApplyMoving(r, deltaPos, deltaRot);
            else
                RestoreRider(r);
        }

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    private bool IsShipMoving()
    {
        float lin = rb.linearVelocity.magnitude;
        float ang = rb.angularVelocity.magnitude * Mathf.Rad2Deg;
        return lin > moveLinearThreshold || ang > moveAngularThreshold;
    }

    private void ApplyMoving(Rider r, Vector3 deltaPos, Quaternion deltaRot)
    {
        // Take over from the rig's own locomotion/gravity while carried.
        if (!r.gravityApplied)
        {
            r.gravityApplied = true;
            if (r.cc != null)
            {
                r.ccWasEnabled = r.cc.enabled;
                r.cc.enabled = false;
            }
            if (r.body != null)
                r.body.isKinematic = true;
            r.verticalVelocity = 0f;
        }

        Transform root = r.root;

        // 1) CARRY: rotate the rider about the ship's pivot and translate by the same delta,
        //    so the rider rides the platform rigidly (no parenting).
        Vector3 offset = root.position - prevPos;
        Vector3 carriedPos = prevPos + deltaPos + (deltaRot * offset);
        root.position = carriedPos;
        root.rotation = deltaRot * root.rotation;

        // 2) REORIENT: slerp the rig's up toward the ship's up so the deck feels "down".
        Vector3 shipUp = transform.up;
        Quaternion targetUp = Quaternion.FromToRotation(root.up, shipUp) * root.rotation;
        root.rotation = Quaternion.Slerp(root.rotation, targetUp, reorientSpeed * Time.fixedDeltaTime);

        // 3) GRAVITY: pull the rider toward ship-local down, with simple ground stick.
        Vector3 down = -shipUp;
        r.verticalVelocity += gravityStrength * Time.fixedDeltaTime;

        // Probe for the deck beneath the rider along ship-down.
        bool grounded = Physics.Raycast(root.position, down, out RaycastHit hit, 0.3f);
        if (grounded)
        {
            r.verticalVelocity = 0f;
        }
        else
        {
            root.position += down * (r.verticalVelocity * Time.fixedDeltaTime);
        }
    }

    private void RestoreRider(Rider r)
    {
        if (!r.gravityApplied) return;
        r.gravityApplied = false;

        // Hand control back to the rig's own systems (normal world gravity / locomotion).
        if (r.cc != null)
            r.cc.enabled = r.ccWasEnabled;
        if (r.body != null)
            r.body.isKinematic = false;

        // Stand the rig back upright in world space (keep its heading).
        if (r.root != null)
        {
            Vector3 e = r.root.eulerAngles;
            r.root.rotation = Quaternion.Euler(0f, e.y, 0f);
        }
        r.verticalVelocity = 0f;
    }

    private void OnDisable()
    {
        // Make sure we never leave a rider stuck in the overridden state.
        foreach (var kv in riders)
            RestoreRider(kv.Value);
    }
}
