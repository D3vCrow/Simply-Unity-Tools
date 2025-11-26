using UnityEngine;

/// <summary>
/// Relays root motion from an Animator to a target Transform (e.g., a Rigidbody parent).
/// Useful when animations drive movement, but you want control over *where* that movement applies.
/// </summary>
[RequireComponent(typeof(Animator))]
public class RootMotionRelay : MonoBehaviour
{
    [Header("Target Root to Move")]
    [Tooltip("The transform that should receive root motion movement and rotation.")]
    [SerializeField] private Transform rootToMove;

    [Header("Animator Source")]
    [Tooltip("The Animator component providing root motion data.")]
    [SerializeField] private Animator animator;

    private Rigidbody targetRigidbody;

    private void Reset()
    {
        // Auto-assign components when added in the editor
        animator = GetComponent<Animator>();
        rootToMove = transform.root;
    }

    private void Awake()
    {
        // Optional: Get Rigidbody if the target uses physics
        if (rootToMove != null)
        {
            targetRigidbody = rootToMove.GetComponent<Rigidbody>();
        }
    }

    private void OnAnimatorMove()
    {
        // Safety checks
        if (!animator || !rootToMove || !animator.applyRootMotion)
            return;

        // Read root motion deltas from Animator
        Vector3 deltaPosition = animator.deltaPosition;
        Quaternion deltaRotation = animator.deltaRotation;

        // Optional: Zero out vertical movement if movement should stay on ground
        deltaPosition.y = 0f;

        // Apply root motion to target transform
        if (targetRigidbody != null)
        {
            // Physics-aware movement (if Rigidbody is present)
            targetRigidbody.MovePosition(targetRigidbody.position + deltaPosition);
            targetRigidbody.MoveRotation(targetRigidbody.rotation * deltaRotation);
        }
        else
        {
            // Direct transform manipulation (if no Rigidbody)
            rootToMove.position += deltaPosition;
            rootToMove.rotation *= deltaRotation;
        }
    }
}
