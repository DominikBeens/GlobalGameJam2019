using UnityEngine;

public class Test : MonoBehaviour
{

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    [ConsoleCommand("setcolor")]
    private void ChangeColor(string color)
    {
        if (ColorUtility.TryParseHtmlString(color, out Color c))
        {
            rend.material.color = c;
        }
        else
        {
            Debug.LogError("Invalid Color");
        }
    }
}
