using UnityEngine;

public class MarkOfDeath : MonoBehaviour
{

    private GameObject activeMark;
    private float markDuration;

    [SerializeField] private GameObject markPrefab;
    [SerializeField] private Transform markSpawn;
    [SerializeField] private float triggerDuration = 3f;

    [Space]

    [SerializeField] private GameObject markTriggerCanvas;

    private void Awake()
    {
        markTriggerCanvas.SetActive(false);
    }

    private void Update()
    {
        if (activeMark)
        {
            markDuration += Time.deltaTime;

            if (markDuration >= triggerDuration)
            {
                TriggerMark();
            }
        }
    }

    [KeyCommand(KeyCode.G, PressType.KeyPressType.Down)]
    public void AddMark()
    {
        if (activeMark)
        {
            DestroyMark();
        }

        activeMark = Instantiate(markPrefab, markSpawn.position, Quaternion.identity);
    }

    private void TriggerMark()
    {
        markTriggerCanvas.SetActive(true);
    }

    public void DestroyMark()
    {
        Destroy(activeMark);
        activeMark = null;
    }

    public void AnimationEventKillBurb()
    {
        GameManager.instance.SpawnNewBird();
    }
}
