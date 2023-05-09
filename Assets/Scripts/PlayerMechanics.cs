using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float hMove;
    [SerializeField] float vMove;
    [SerializeField] Vector2 move;
    [SerializeField] Vector3 dir;
    [SerializeField] float speed = 23f;
    [SerializeField] float currentDrag = 0.5f;
    [SerializeField] float maxDrag = 1f;
    [SerializeField] Quaternion viewRotation;

    [Header("Jumping")]
    [SerializeField] float jumpSpeed = 200f;
    [SerializeField] float jumpAccl = 5f;
    [SerializeField] float fallingSpeed = 15f;
    [SerializeField] float jumpTimer = 0f;
    [SerializeField] float jumpAngle = 75;
    
    [Header("Swinging")]
    [SerializeField] float distanceFromPoint;
    [SerializeField] float maxSwingDistance = 30f;
    [SerializeField] float minSwingJointDistanceMod = 0.25f;
    [SerializeField] float maxSwingJointDistanceMod = 0.8f;
    [SerializeField] float spring = 4.5f;
    [SerializeField] float damper = 7f;
    [SerializeField] float massScale = 4.5f;
    [SerializeField] Vector3 currentGrapplePosition;
    [SerializeField] Vector3 swingPoint;
    [SerializeField] SpringJoint joint;

    [Header("Booleans")]
    [SerializeField] bool isWalking;
    [SerializeField] bool isJumping;
    [SerializeField] bool isSwinging;
    [SerializeField] bool isGrounded;

    [Header("Components")]
    [SerializeField] Transform cam;
    [SerializeField] Collider groundCollider;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask swingable;
    [SerializeField] LineRenderer lr;
    [SerializeField] PlayerActions actions;
    [SerializeField] ParticleSystem speedLines;

    // [Header("Input Actions")]
    // [SerializeField] InputAction moveAction;
    // [SerializeField] InputAction jumpAction;
    // [SerializeField] InputAction swingAction;
    [SerializeField] Menu menuScript;

    void Awake() {
        menuScript = GameObject.FindWithTag("GameManager").GetComponent<Menu>();
        actions = new PlayerActions();
        actions.Gameplay.Jump.performed += OnJump;
        actions.Gameplay.Swing.started += OnSwingPress;
        actions.Gameplay.Swing.canceled += OnSwingRelease;
        actions.Gameplay.Pause.started += OnPause;
        actions.UI.Resume.started += OnResume;
        //moveAction = actions.FindActionMap("Gameplay").FindAction("Move");
        // actions.FindActionMap("Gameplay").FindAction("Jump").performed += OnJump;
        // actions.FindActionMap("Gameplay").FindAction("Swing").performed += OnSwingPress;
        // actions.FindActionMap("Gameplay").FindAction("Swing").canceled += OnSwingRelease;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GameObject.FindWithTag("MainCamera").transform;
        groundCollider = GetComponent<Collider>();
        groundMask = LayerMask.GetMask("Ground");
        //rb = transform.Find("Armature/Root").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    // void OnMove(InputValue value) {
        
    // }

    // Only jump if the angle of the surface is not steeper than the required angle
    void OnJump(InputAction.CallbackContext context) {
        RaycastHit hit;
        bool normalRay = Physics.Raycast(transform.position, Vector3.down, out hit, 5f);

        if (Vector3.Angle(hit.normal, Vector3.up) <= jumpAngle) {
            AnimationCurve jumpCurve = new AnimationCurve(new Keyframe[] {new Keyframe(0f, 1f), 
            new Keyframe(0.3f, 0.2f)}); // Use keyframes to slow jump at max height

            rb.AddForce(Vector3.up * jumpSpeed * jumpAccl * jumpCurve.Evaluate(jumpTimer), ForceMode.Acceleration);
        }
        else {
            jumpTimer = 0f;
        }
    }

    void OnSwingPress(InputAction.CallbackContext context) {
        //Debug.Log("Swing Press");

        // RaycastHit hitSwing;
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out hitSwing, maxSwingDistance)) {
        //     swingPoint = hitSwing.point;
        //     joint = gameObject.AddComponent<SpringJoint>();
        //     joint.autoConfigureConnectedAnchor = false;
        //     joint.connectedAnchor = swingPoint;

        //     distanceFromPoint = Vector3.Distance(transform.position, swingPoint);
        //     joint.maxDistance = distanceFromPoint * maxSwingJointDistanceMod;
        //     joint.minDistance = distanceFromPoint * minSwingJointDistanceMod;

        //     joint.spring = spring;
        //     joint.damper = damper;
        //     joint.massScale = massScale;

        //     lr.positionCount = 2;
        //     currentGrapplePosition = transform.position;
        // }

        //BETA SWING
        RaycastHit hitSwing;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + (maxSwingDistance / 2), new Vector3(maxSwingDistance / 2,
        maxSwingDistance / 2, maxSwingDistance / 2), transform.forward, transform.rotation);

        if (hits.length != 0) {
            swingPoint = hits[0].point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            distanceFromPoint = Vector3.Distance(transform.position, swingPoint);
            joint.maxDistance = distanceFromPoint * maxSwingJointDistanceMod;
            joint.minDistance = distanceFromPoint * minSwingJointDistanceMod;

            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            lr.positionCount = 2;
            currentGrapplePosition = transform.position;
        }
        
    }

    void OnSwingPerformed() {
        //Debug.Log("Swing Performed");
    }

    void OnSwingRelease(InputAction.CallbackContext context) {
        //Debug.Log("Swing Release");
        lr.positionCount = 0;
        Destroy(joint);
    }

    void OnPause(InputAction.CallbackContext context) {
        Debug.Log("Gotit");
        menuScript.PauseGame();
    }

    void OnResume(InputAction.CallbackContext context) {
        Debug.Log("yippee");
        PlayerPrefs.Save();
        menuScript.ResumeGame();
    }

    void FixedUpdate()
    {
        move = actions.Gameplay.Move.ReadValue<Vector2>();
        viewRotation = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up);
        dir = viewRotation * new Vector3(Mathf.Clamp(move.x * 2, -1, 1), 0, Mathf.Clamp(move.y * 2, -1, 1));
        rb.AddForce(dir * speed, ForceMode.Acceleration);

        // if (move.x == 0 && move.y == 0 && currentDrag < maxDrag) {
        //     currentDrag += Time.fixedDeltaTime;
        //     rb.drag = currentDrag;
        // }

        // if (currentDrag >= maxDrag) {
        //     rb.velocity = Vector3.zero;
        //     rb.drag = 0.5f;
        // }
        if (rb.velocity.y < 0) {
            rb.AddForce(Vector3.down * fallingSpeed, ForceMode.Acceleration);
        }

        if (rb.velocity.x > 15f || rb.velocity.z > 15) {
            speedLines.Play();
        }
        else {
            speedLines.Stop();
        }

        CheckIsGrounded();

        if (isGrounded) {
            jumpTimer = 0f;
        }
        else {
            jumpTimer += Time.fixedDeltaTime;
        }
    }

    void LateUpdate() {
        //Debug.Log("update");
        DrawRope();
    }

    void CheckIsGrounded() {
        float distanceGround = GetComponent<Collider>().bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceGround + 0.1f);
    }

    void DrawRope() {
        //Debug.Log("Checl");
        if (!joint) {
            //Debug.Log("Return");
            return;
        }
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.fixedDeltaTime * 8f);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Kill") {
            SceneManager.LoadScene(0);
        }
    }

    public void OnEnable() {
        // actions.FindActionMap("Gameplay").Enable();
        actions.Gameplay.Enable();
        actions.UI.Disable();
    }

    public void OnDisable() {
        // actions.FindActionMap("Gameplay").Disable();
        actions.Gameplay.Disable();
        actions.UI.Enable();
    }

    
}
