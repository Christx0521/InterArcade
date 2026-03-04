using UnityEngine;

public class BK_DestroyOverTime : MonoBehaviour
{
    [SerializeField] float timer;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
