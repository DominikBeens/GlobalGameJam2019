using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 1f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotateSpeed));
    }
}
