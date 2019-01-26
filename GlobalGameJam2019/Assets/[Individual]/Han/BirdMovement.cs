﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour {
    Rigidbody2D myRidgidbody;
    BoxCollider2D myCheckbox;
    Animator myAnimator;
    [SerializeField] LayerMask chickenGround;
    [SerializeField] int wingSpeed;
    [SerializeField] int rotationAmount;
    [SerializeField] Transform myCamera;
    [SerializeField] float stabelizeSpeed;
    [SerializeField] float stabelizeAirSpeed;
    [SerializeField] float coolDown;
    float timer;

    void Awake() {
        myRidgidbody = GetComponent<Rigidbody2D>();
        myCheckbox = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update() {
        myCamera.position = new Vector3(transform.position.x, transform.position.y, -10);

        MovementUpdate();
    }

    Vector2Int newMovementAxis;

    void MovementUpdate() {
        timer -= Time.deltaTime;

        myAnimator.SetInteger("State", 0);
        if (MovementAxis() && timer < 0) {
            timer = coolDown;

            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, 0.4f, chickenGround)) {
                myRidgidbody.angularVelocity = 0;
                myRidgidbody.AddForce(Vector2.up * wingSpeed * 1.5f, ForceMode2D.Impulse);
                myAnimator.SetInteger("State", 3);
            }
            else {
                if (newMovementAxis.x == 1) {
                    if (newMovementAxis.y == 1) {
                        myAnimator.SetInteger("State", 3);
                    }
                    else {
                        myAnimator.SetInteger("State", 2);
                    }
                }
                else {
                    myAnimator.SetInteger("State", 1);
                }

                myRidgidbody.velocity = new Vector2(myRidgidbody.velocity.x, 0);
                myRidgidbody.AddTorque((newMovementAxis.x - newMovementAxis.y) * rotationAmount * Time.deltaTime);

                if (transform.up.y < 0) {
                    myRidgidbody.angularVelocity = 0;
                    myRidgidbody.AddForce(transform.up * (newMovementAxis.x + newMovementAxis.y) * wingSpeed / 2, ForceMode2D.Impulse);
                }
                else {
                    myRidgidbody.angularVelocity = 0;
                    myRidgidbody.AddForce(transform.up * (newMovementAxis.x + newMovementAxis.y) * wingSpeed, ForceMode2D.Impulse);
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
}