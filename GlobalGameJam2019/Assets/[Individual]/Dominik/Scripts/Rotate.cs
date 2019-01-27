using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.right * (Time.deltaTime * rotateSpeed));
    }
}
