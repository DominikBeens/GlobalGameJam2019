using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private GameObject bird;

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

        SpawnNewBird();
    }

    public void SpawnNewBird()
    {
        Destroy(bird);
        bird = Instantiate(birdPrefab, transform.position, Quaternion.identity);
    }
}
