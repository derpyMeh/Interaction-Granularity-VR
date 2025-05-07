using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BookPhysicsLocker : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);

        // Only lock if not currently held
        if (!grab.isSelected)
            Invoke(nameof(LockPhysics), 0.2f);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        UnlockPhysics();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        LockPhysics();
    }

    private void LockPhysics()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
    }

    private void UnlockPhysics()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
    }
}
