using UnityEngine;

public class Pickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bird")
        {
            Collect();
        }
    }

    private void Collect()
    {
        LevelManager.instance.CollectPickup();
        Destroy(gameObject);
    }
}
