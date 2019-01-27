using UnityEngine;

public class MarkOfDeath : MonoBehaviour
{

    private GameObject activeMark;
    private float markDuration;
    private Animator anim;

    [SerializeField] private GameObject markPrefab;
    [SerializeField] private Transform markSpawn;
    [SerializeField] private float triggerDuration = 3f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (activeMark)
        {
            markDuration += Time.deltaTime;

            if (markDuration >= triggerDuration)
            {
                TriggerMark();
                activeMark = null;
            }
        }
    }

    public void AddMark()
    {
        if (activeMark)
        {
            DestroyMark();
        }

        activeMark = Instantiate(markPrefab, markSpawn.position, Quaternion.identity, markSpawn);
    }

    private void TriggerMark()
    {
        anim.SetTrigger("Trigger");
        DestroyMark();
    }

    public void DestroyMark()
    {
        Destroy(activeMark);
        activeMark = null;
        markDuration = 0;
    }

    public void AnimationEventKillBurb()
    {
        GameManager.instance.SpawnNewBird();
    }
}
