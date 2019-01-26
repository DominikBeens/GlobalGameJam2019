using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject birdPrefab;

    private void Awake()
    {
        Instantiate(birdPrefab, Vector3.zero, Quaternion.identity);
    }
}
