using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    Rigidbody2D myRidgidbody;
    [SerializeField] int wingSpeed;
    [SerializeField] int rotationAmount;
    [SerializeField] Transform myamera;
    [SerializeField] float stabelizeSpeed;

    void Awake(){
        myRidgidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        myamera.position = new Vector3(transform.position.x,transform.position.y,-10);

        MovementUpdate();
    }

    Vector2Int newMovementAxis;

    void MovementUpdate(){
        if(MovementAxis()){
            myRidgidbody.AddTorque((newMovementAxis.x - newMovementAxis.y) * rotationAmount * Time.deltaTime);
            myRidgidbody.AddForce(transform.up * (newMovementAxis.x + newMovementAxis.y) * wingSpeed,ForceMode2D.Impulse);
        }
        else if(myRidgidbody.velocity.x < 0.1 && myRidgidbody.velocity.x > -0.1 && myRidgidbody.velocity.y < 0.1 && myRidgidbody.velocity.y > -0.1){
            transform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(transform.localEulerAngles.z,0,stabelizeSpeed * Time.deltaTime));
        }
    }

    bool MovementAxis(){
        if(Input.GetButtonDown("Right Wing")){
            if(Input.GetButtonDown("Left Wing")){
                newMovementAxis = new Vector2Int(1,1);
            }

             newMovementAxis = new Vector2Int(0,1);
                return true;

        }
        else if(Input.GetButtonDown("Left Wing")){
             newMovementAxis = new Vector2Int(1,0);
                return true;
        }

         newMovementAxis = new Vector2Int(0,0);
                return false;

    }
}
