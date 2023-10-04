using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class sp_Rotate : MonoBehaviour
{
    public float rotationSpeed = 90f; // Degrees per second
    public float movementSpeed = 1f; // Speed of movement
    public float movementAmplitude = 1f; // Amplitude of movement

    private Vector3 startPosition;

    private void Start(){
        startPosition = transform.position;
    }

    private void Update(){
        RotateContinuously();
        MoveUpDownContinuously();
    }

    private void RotateContinuously(){
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void MoveUpDownContinuously(){
        float newYPosition = startPosition.y + Mathf.Sin(Time.time * movementSpeed) * movementAmplitude;
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
}