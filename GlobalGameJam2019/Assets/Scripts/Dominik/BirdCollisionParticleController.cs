using UnityEngine;

public class BirdCollisionParticleController : MonoBehaviour
{

    [SerializeField] private GameObject birdCollisionParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9)
        {
            Instantiate(birdCollisionParticle, collision.bounds.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
