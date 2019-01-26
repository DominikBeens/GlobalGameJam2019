using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class LevelData : ScriptableObject
{

    public int level;
    public bool locked = true;
    public int pickupsCollected;

    public void AddPickup()
    {
        pickupsCollected++;
    }

    public void ResetPickups()
    {
        pickupsCollected = 0;
    }
}
