using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class LevelData : ScriptableObject
{

    public int level;
    public bool locked = true;
    public int pickupsCollected;
}
