using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathSaveFile
{
    public List<float> deathLocationsX = new List<float>();
    public List<float> deathLocationsY = new List<float>();
    public List<float> deathLocationsZ = new List<float>();
}
