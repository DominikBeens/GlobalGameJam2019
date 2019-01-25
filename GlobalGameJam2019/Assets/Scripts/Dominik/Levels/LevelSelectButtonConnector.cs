using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonConnector : MonoBehaviour
{

    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform levelSelectButtonHolder;

    private void Start()
    {
        CreateConnection();
    }

    private void OnValidate()
    {
        CreateConnection();
    }
    
    [ContextMenu("CreateConnection")]
    private void CreateConnection()
    {
        if (!line || !levelSelectButtonHolder)
        {
            return;
        }

        Camera mainCam = Camera.main;
        List<Vector3> buttonWorldPos = new List<Vector3>();

        foreach (Transform child in levelSelectButtonHolder)
        {
            Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(child.transform.position.x, child.transform.position.y, mainCam.nearClipPlane));
            buttonWorldPos.Add(worldPos);
        }

        line.positionCount = buttonWorldPos.Count;
        line.SetPositions(buttonWorldPos.ToArray());
    }
}
