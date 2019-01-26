using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private GameObject birdPrefab;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SpawnBird();
    }

    public void SpawnBird()
    {
        Instantiate(birdPrefab, transform.position, Quaternion.identity);
    }
}
