using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    private bool cooldown;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (!cooldown)
            {
                DeathPlacement.instance?.AddDeath(collision.transform.position);
                cooldown = true;
                StartCoroutine(CD());
            }

            print(gameObject);
            GameManager.instance.SpawnNewBird();
        }
    }

    IEnumerator CD()
    {
        yield return new WaitForSeconds(1);
        cooldown = false;
    }
}
