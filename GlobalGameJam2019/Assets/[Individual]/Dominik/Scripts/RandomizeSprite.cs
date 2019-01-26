using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{

    [SerializeField] private float minSize = 0.8f;
    [SerializeField] private float maxSize = 1.2f;
    [SerializeField] private float oddScaleRandomizeAmount = 0.05f;

    [Space]

    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    private void Awake()
    {
        Vector3 randomSize = Vector3.one * Random.Range(minSize, maxSize);
        randomSize.x += Random.Range(-oddScaleRandomizeAmount, oddScaleRandomizeAmount);
        randomSize.y += Random.Range(-oddScaleRandomizeAmount, oddScaleRandomizeAmount);
        transform.localScale = randomSize;

        if (Random.Range(0, 2) == 0)
        {
            Vector3 invertedScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            transform.localScale = invertedScale;
        }

        Vector3 position = transform.position;
        position.z = Random.Range(minZ, maxZ);
        transform.position = position;
    }
}
