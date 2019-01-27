using UnityEngine;

public class BirdCollisionParticleController : MonoBehaviour {

    [SerializeField] private GameObject birdCollisionParticle;
    AudioSource myAudio;

    void Awake() {
        myAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != 9) {
            myAudio.pitch = Random.Range(0.3f,0.8f);
            myAudio.Play();
            Instantiate(birdCollisionParticle, collision.bounds.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}