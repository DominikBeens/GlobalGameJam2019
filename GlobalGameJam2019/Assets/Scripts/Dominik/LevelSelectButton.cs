using DB.MenuPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{

    private int sceneToLoad;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        sceneToLoad = transform.GetSiblingIndex() + 1;
        button.onClick.AddListener(() => LoadLevel());
    }

    private void OnValidate()
    {
        sceneToLoad = transform.GetSiblingIndex() + 1;
        GetComponentInChildren<TextMeshProUGUI>().text = $"Level: {sceneToLoad}";
        transform.name = $"LevelSelectButton L{sceneToLoad}";
    }

    private void LoadLevel()
    {
        SceneManager.instance.LoadScene(sceneToLoad, false);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(() => LoadLevel());
    }
}
