using UnityEngine;

public class Breakable : MonoBehaviour
{

    private Collider2D col;

    [SerializeField] private float requiredBreakForce;

    private void Awake()
    {
        col = GetComponentInChildren<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == 9)
        {
            if (Mathf.Abs(collision.relativeVelocity.x * collision.relativeVelocity.y) > requiredBreakForce)
            {
                col.enabled = false;
            }
        }
    }
}
