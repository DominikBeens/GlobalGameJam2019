using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntedArea : MonoBehaviour
{
    [Header("doesnt have to be filled")]
    [SerializeField] private Canvas turtorial;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            if (collision.transform.root.GetComponentInChildren<MarkOfDeath>() != null)
            {
                collision.transform.root.GetComponentInChildren<MarkOfDeath>().AddMark();
            }

            if(turtorial != null)
            {
                turtorial.transform.parent = collision.transform.root.GetComponentInChildren<Camera>().gameObject.transform;
                turtorial.transform.position = collision.transform.position;
                turtorial.enabled = true;
                turtorial.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.transform.root.GetComponentInChildren<MarkOfDeath>() != null)
            {
                collision.transform.root.GetComponentInChildren<MarkOfDeath>().DestroyMark();
            }
            if (turtorial != null)
            {
                turtorial.transform.parent = null;
                turtorial.enabled = false;
            }
        }
    }
}
