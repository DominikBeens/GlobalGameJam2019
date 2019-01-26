using UnityEngine;

public class ObjectCleanup : MonoBehaviour
{

    [SerializeField] private float cleanupAfterSeconds;

    private void Awake()
    {
        Destroy(gameObject, cleanupAfterSeconds);
    }
}
