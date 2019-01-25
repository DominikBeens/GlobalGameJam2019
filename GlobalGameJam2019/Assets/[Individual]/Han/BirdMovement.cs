using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    Rigidbody2D myRidgidbody;
    [SerializeField] int wingSpeed;
    [SerializeField] int rotationAmount;
    [SerializeField] Transform myamera;

    void Awake(){
        myRidgidbody = GetComponent<Rigidbody2D>();
    }

    Vector2Int newMovementAxis;

    void Update()
    {
        myamera.position = new Vector3(transform.position.x,transform.position.y,-10);
        newMovementAxis = MovementAxis();

        myRidgidbody.AddTorque((newMovementAxis.x - newMovementAxis.y) * rotationAmount * Time.deltaTime);
        myRidgidbody.AddForce(transform.up * (newMovementAxis.x + newMovementAxis.y) * wingSpeed,ForceMode2D.Impulse);
    }

    Vector2Int MovementAxis(){
        if(Input.GetButtonDown("Right Wing")){
            if(Input.GetButtonDown("Left Wing")){
                return(new Vector2Int(1,1));
            }

            return (new Vector2Int(0,1));
        }
        else if(Input.GetButtonDown("Left Wing")){
            return (new Vector2Int(1,0));
        }

        return (new Vector2Int(0,0));
    }
}
