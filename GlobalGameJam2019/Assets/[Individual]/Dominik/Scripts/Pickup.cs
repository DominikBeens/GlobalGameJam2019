using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private GameObject pickupParticle;
    [SerializeField] private int pickupScore = 15;

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
        NewHighscore.instance?.Pickup(pickupScore);
        NotificationManager.instance?.NewNotification($"Pickup collected! +{pickupScore} score!");
        Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
