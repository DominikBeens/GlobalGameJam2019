﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{

    private int sceneToLoad;
    private Button button;

    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject lockedOverlay;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        sceneToLoad = transform.GetSiblingIndex() + 1;
    }

    private void Start()
    {
        button.onClick.AddListener(() => LevelManager.instance.SelectLevelToPlay(sceneToLoad));
    }

    public void UpdateLock(LevelData data)
    {
        ToggleLock(data.locked);
    }

    private void OnValidate()
    {
        sceneToLoad = transform.GetSiblingIndex() + 1;

        if (buttonText)
        {
            buttonText.text = $"Level: {sceneToLoad}";
        }

        transform.name = $"LevelSelectButton L{sceneToLoad}";
    }

    private void ToggleLock(bool b)
    {
        lockedOverlay.SetActive(b);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(() => LevelManager.instance.SelectLevelToPlay(sceneToLoad));
    }
}
