using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour {
    Rigidbody2D myRidgidbody;
    BoxCollider2D myCheckbox;
    Animator birdAnimator;
    [SerializeField] LayerMask chickenGround;
    [SerializeField] int wingSpeed;
    [SerializeField] int rotationAmount;
    [SerializeField] Transform myCamera;
    [SerializeField] float stabelizeSpeed;
    [SerializeField] float stabelizeAirSpeed;
    [SerializeField] float coolDown;
    [SerializeField] int zoomSpeed;
    [SerializeField] int minZoom, maxZoom;
    float timerRight;
    float timerLeft;
    bool frozen;

    void Awake() {
        myRidgidbody = GetComponent<Rigidbody2D>();
        myCheckbox = GetComponent<BoxCollider2D>();
        birdAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (!frozen) {
            myCamera.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(myCamera.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed, minZoom, maxZoom));
            MovementUpdate();
        }

    }

    Vector2Int newMovementAxis;
    bool axisMovement;
    Vector2 newVelocity;

    void MovementUpdate() {
        timerRight -= Time.deltaTime;
        timerRight -= Time.deltaTime;
        axisMovement = MovementAxis();

        if (axisMovement && timerRight < 0 || timerLeft < 0 && axisMovement) {
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, 0.4f, chickenGround)) {
                timerLeft = coolDown;
                timerRight = coolDown;
                myRidgidbody.angularVelocity = 0;
                myRidgidbody.AddForce(Vector2.up * wingSpeed * 1.5f, ForceMode2D.Impulse);
                birdAnimator.SetTrigger("FlapR");
                birdAnimator.SetTrigger("FlapL");
            }
            else {
                if (newMovementAxis.x == 1) {
                    if (newMovementAxis.y == 1) {
                        birdAnimator.SetTrigger("FlapR");
                        birdAnimator.SetTrigger("FlapL");
                        timerLeft = coolDown;
                        timerRight = coolDown;
                    }
                    else {
                        birdAnimator.SetTrigger("FlapR");
                        timerRight = coolDown;
                    }
                }
                else {
                    birdAnimator.SetTrigger("FlapL");
                    timerLeft = coolDown;
                }

                myRidgidbody.velocity = new Vector2(myRidgidbody.velocity.x, 0);
                myRidgidbody.AddTorque((newMovementAxis.x - newMovementAxis.y) * rotationAmount * Time.deltaTime);

                newVelocity = transform.up * (newMovementAxis.x + newMovementAxis.y) * wingSpeed;

                if (transform.up.y < 0) {
                    //myRidgidbody.angularVelocity = 0;
                    myRidgidbody.AddForce(new Vector2(newVelocity.x / 4, newVelocity.y), ForceMode2D.Impulse);
                }
                else {
                    //myRidgidbody.angularVelocity = 0;
                    myRidgidbody.AddForce(new Vector2(newVelocity.x / 4, newVelocity.y), ForceMode2D.Impulse);
                }
            }
        }
        else if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, 0.4f, chickenGround)) {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.localEulerAngles.z, 0, stabelizeAirSpeed * Time.deltaTime));
        }
        else if (myRidgidbody.velocity.x < 0.4 && myRidgidbody.velocity.x > -0.4 && myRidgidbody.velocity.y < 0.4 && myRidgidbody.velocity.y > -0.4) {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.localEulerAngles.z, 0, stabelizeSpeed * Time.deltaTime));
        }
    }

    bool MovementAxis() {
        if (Input.GetButtonDown("Right Wing")) {
            if (Input.GetButtonDown("Left Wing")) {
                newMovementAxis = new Vector2Int(1, 1);
            }

            newMovementAxis = new Vector2Int(0, 1);
            return true;

        }
        else if (Input.GetButtonDown("Left Wing")) {
            newMovementAxis = new Vector2Int(1, 0);
            return true;
        }

        newMovementAxis = new Vector2Int(0, 0);
        return false;

    }

    public void Freeze() {
        frozen = true;
    }
}