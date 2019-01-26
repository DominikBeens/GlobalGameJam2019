using System.Collections.Generic;
using UnityEngine;
using DB.MenuPack;

public class LevelSelectButtonConnector : MonoBehaviour
{

    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform levelSelectButtonHolder;

    private void Start()
    {
        CreateConnection();
        SceneManager.OnLevelLoaded += SceneManager_OnLevelLoaded;
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

    private void ResetLine()
    {
        line.positionCount = 0;
    }

    private void SceneManager_OnLevelLoaded()
    {
        ResetLine();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            CreateConnection();
        }
    }

    private void OnDisable()
    {
        SceneManager.OnLevelLoaded -= SceneManager_OnLevelLoaded;
    }
}
