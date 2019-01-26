using System.Collections.Generic;
using UnityEngine;

public class ChirpController : MonoBehaviour
{

    private AudioSource source;
    private Camera mainCam;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private PitchMode pitchMode;

    [Space]

    [SerializeField] private List<AudioClip> chirps;

    public enum PitchMode { Random, MousePos };

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        mainCam = Camera.main;
    }

    [KeyCommand(KeyCode.C, PressType.KeyPressType.Down)]
    private void Chirp()
    {
        int randomChirp = Random.Range(0, chirps.Count);
        source.clip = chirps[randomChirp];

        float pitch = 1;
        switch (pitchMode)
        {
            case PitchMode.Random:
                pitch = Random.Range(minPitch, maxPitch);
                break;

            case PitchMode.MousePos:
                Vector3 mousePos = Input.mousePosition;
                pitch = Remap(mousePos.x, 0, Screen.width, minPitch, maxPitch);
                break;
        }
        source.pitch = pitch;

        source.Play();
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
