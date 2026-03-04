using UnityEngine;

[RequireComponent(typeof(PAC_Ghost))]
public abstract class PAC_GhostBehavior : MonoBehaviour
{
    public PAC_Ghost ghost { get; private set; }
    public float duration;

    private void Awake()
    {
        ghost = GetComponent<PAC_Ghost>();
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float duration)
    {
        enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        enabled = false;
        CancelInvoke();
    }
}
