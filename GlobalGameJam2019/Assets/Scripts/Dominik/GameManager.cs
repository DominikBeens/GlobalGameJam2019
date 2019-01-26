using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject birdPrefab;

    private void Awake()
    {
        Instantiate(birdPrefab, transform.position, Quaternion.identity);
    }
}
