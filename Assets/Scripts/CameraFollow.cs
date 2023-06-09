using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] float rotationInterpolationFactor = 0.2f;
    [SerializeField] Transform player;

    [Header("Rotation")]
    [SerializeField] float sensitivity = 0.3f;
    [SerializeField] float maxAngle = 22f;
    [SerializeField] float minAngle = 30f;
    [SerializeField] float xRotation = 16f;
    [SerializeField] float yRotation = 0f;
    //[SerializeField] float yAngle = 22f;
    //[SerializeField] float rotateSpeed = 1f;
    //[SerializeField] int invertY = -1;
    [SerializeField] Vector3 rotationDelta;
    [SerializeField] Quaternion newRotation;
    [SerializeField] Menu menu;
    [SerializeField] PlayerMechanics mechs;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        menu = GameObject.FindWithTag("GameManager").GetComponent<Menu>();
    }

    // Grabs input and rotates camera
    // void OnLook(InputValue value) {
        
    // }
    void Update() {
        rotationDelta.y = mechs.actions.Gameplay.Look.ReadValue<Vector2>().x * menu.sensitivity;//value.Get<Vector2>().x * menu.sensitivity;
        rotationDelta.x = mechs.actions.Gameplay.Look.ReadValue<Vector2>().y * menu.sensitivity;//value.Get<Vector2>().y * menu.sensitivity;
        yRotation += rotationDelta.y;
        xRotation += rotationDelta.x;
        yRotation = Mathf.Repeat(yRotation, 360);

        if (!menu.invertY) {
            xRotation = Mathf.Clamp(xRotation, -maxAngle, minAngle);
        }
        else {
            xRotation = Mathf.Clamp(xRotation, -minAngle, maxAngle);
        }
        //xRotation = Mathf.Clamp(xRotation, -maxAngle, minAngle);

        newRotation = Quaternion.Euler((menu.invertY ? 1 : -1) * xRotation, yRotation, 0);
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = player.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationInterpolationFactor);
    }

    // public void FlipInvertY() {
    //     invertY *= -1;
    // }

    // public void SetSensitivity(float value) {
    //     sensitivity = value;
    // }
}
