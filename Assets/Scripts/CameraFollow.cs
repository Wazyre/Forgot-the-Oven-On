using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] float movementInterpolationSpeed = 0.05f;
    [SerializeField] float rotationInterpolationFactor = 0.2f;
    [SerializeField] Transform player;

    [Header("Rotation")]
    [SerializeField] float sensitivity = 0.3f;
    [SerializeField] float maxAngle = 22f;
    [SerializeField] float minAngle = 30f;
    [SerializeField] float xRotation = 16f;
    [SerializeField] float yRotation = 0f;
    [SerializeField] float yAngle = 22f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] int invertY = -1;
    [SerializeField] Vector3 rotationDelta;
    [SerializeField] Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        //offset = player.position - transform.position;
    }

    // Grabs input and rotates camera
    void OnLook(InputValue value) {
        rotationDelta.y = value.Get<Vector2>().x * sensitivity;
        rotationDelta.x = value.Get<Vector2>().y * sensitivity;
        yRotation += rotationDelta.y;
        xRotation += rotationDelta.x;
        yRotation = Mathf.Repeat(yRotation, 360);
        xRotation = Mathf.Clamp(xRotation, -maxAngle, minAngle);

        newRotation = Quaternion.Euler(-xRotation, yRotation, 0);
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = player.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationInterpolationFactor);
    }

    public void SetSensitivity(float value) {
        sensitivity = value;
    }
}
