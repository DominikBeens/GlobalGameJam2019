using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private GameObject pickupParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Collect();
        }
    }

    private void Collect()
    {
        LevelManager.instance?.CollectPickup();
        Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
