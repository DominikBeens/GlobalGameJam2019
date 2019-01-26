using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{

    public static NotificationManager instance;

    [SerializeField] private Transform notificationSpawn;
    [SerializeField] private GameObject notificationPrefab;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void NewNotification(string message)
    {
        TextMeshProUGUI text = Instantiate(notificationPrefab, notificationSpawn.position, Quaternion.identity, notificationSpawn).GetComponentInChildren<TextMeshProUGUI>();
        text.text = message;
    }
}
