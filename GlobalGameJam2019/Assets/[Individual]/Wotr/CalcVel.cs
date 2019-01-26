using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcVel : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator myanim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myanim.SetFloat("DownwardVel", -rb.velocity.y);
    }
}
