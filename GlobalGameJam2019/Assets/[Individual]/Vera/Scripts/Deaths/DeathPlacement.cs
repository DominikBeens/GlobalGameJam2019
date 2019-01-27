using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlacement : MonoBehaviour
{
    [SerializeField] private List<GameObject> deathObjects = new List<GameObject>();
    [SerializeField] private int lvl;
    [SerializeField] private GameObject deathMarker;
    public static DeathPlacement instance;
    
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        lvl = GetComponentInParent<NewHighscore>().level - 1;
        if(HighscoreManager.instance.deaths[lvl].deathLocationsX.Count != 0)
        {
            for (int i = 0; i < HighscoreManager.instance.deaths[lvl].deathLocationsX.Count; i++)
            {
               StartCoroutine(RandomStart(new Vector3(HighscoreManager.instance.deaths[lvl].deathLocationsX[i], HighscoreManager.instance.deaths[lvl].deathLocationsY[i], HighscoreManager.instance.deaths[lvl].deathLocationsZ[i])));
            }
        }
    }

    public void AddDeath(Vector3 location)
    {
        HighscoreManager.instance.deaths[lvl].deathLocationsX.Add(location.x);
        HighscoreManager.instance.deaths[lvl].deathLocationsY.Add(location.y);
        HighscoreManager.instance.deaths[lvl].deathLocationsZ.Add(location.z);
        if (HighscoreManager.instance.deaths[lvl].deathLocationsX.Count >= 51)
        {
            HighscoreManager.instance.deaths[lvl].deathLocationsX.RemoveAt(0);
            HighscoreManager.instance.deaths[lvl].deathLocationsY.RemoveAt(0);
            HighscoreManager.instance.deaths[lvl].deathLocationsZ.RemoveAt(0);
            RemoveDeath(0);
        }
        PlaceDeath(location);
    }

    private void RemoveDeath(int index)
    {
        Destroy(deathObjects[index]);
        deathObjects.RemoveAt(0);
    }

    IEnumerator RandomStart(Vector3 location)
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        PlaceDeath(location);
    }

    private void PlaceDeath(Vector3 location)
    {
        deathObjects.Add(Instantiate(deathMarker, location, Quaternion.identity));
    }
}
