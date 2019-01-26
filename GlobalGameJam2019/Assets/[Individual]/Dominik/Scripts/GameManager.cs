using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private GameObject bird;

    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private NewHighscore newHighscore;

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
        if (bird)
        {
            Destroy(bird);
            newHighscore.StartScore();
        }

        bird = Instantiate(birdPrefab, transform.position, Quaternion.identity);
    }
}
