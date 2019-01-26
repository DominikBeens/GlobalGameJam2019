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
        NotificationManager.instance?.NewNotification("Pickup Collected!");
        Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
