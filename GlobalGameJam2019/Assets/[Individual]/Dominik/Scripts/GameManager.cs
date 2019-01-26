using System.Collections;
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

        bird = Instantiate(birdPrefab, transform.position, Quaternion.identity);
    }

    public void SpawnNewBird()
    {
        StartCoroutine(NewBird());
    }

    private IEnumerator NewBird()
    {
        if (bird)
        {
            bird.GetComponentInChildren<BirdMovement>().Freeze();
            NotificationManager.instance?.NewNotification("Level Failed!");

            yield return new WaitForSeconds(2);

            Destroy(bird);

            bird = Instantiate(birdPrefab, transform.position, Quaternion.identity);
            newHighscore.StartScore();
        }
    }
}
