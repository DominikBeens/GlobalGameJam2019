using UnityEngine;

public class ParticleCleanup : MonoBehaviour
{

    [SerializeField] private float cleanupAfterSeconds;

    private void Awake()
    {
        Destroy(gameObject, cleanupAfterSeconds);
    }
}
