using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private GameObject pickupParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bird")
        {
            Collect();
        }
    }

    private void Collect()
    {
        LevelManager.instance.CollectPickup();
        Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
