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
    [SerializeField] float currentDrag = 0.05f;
    [SerializeField] float maxDrag = 10f;
    [SerializeField] float particleAppearSpeed = 40f;
    [SerializeField] Quaternion viewRotation;

    [Header("Jumping")]
    [SerializeField] float jumpSpeed = 200f;
    [SerializeField] float jumpAccl = 5f;
    [SerializeField] float fallingSpeed = 15f;
    [SerializeField] float jumpTimer = 0f;
    [SerializeField] float jumpAngle = 75;
    [SerializeField] float inAirSpeed = 10f;
    
    [Header("Swinging")]
    [SerializeField] float distanceFromPoint;
    [SerializeField] float maxSwingDistance = 25f;
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
    public PlayerActions actions;
    [SerializeField] ParticleSystem speedLines;
    [SerializeField] Menu menu;
    [SerializeField] CheckpointManager checkMgr;

    void Awake() {
        menu = GameObject.FindWithTag("GameManager").GetComponent<Menu>();
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
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GameObject.FindWithTag("MainCamera").transform;
        groundCollider = GetComponent<Collider>();
        groundMask = LayerMask.GetMask("Ground");
        //rb = transform.Find("Armature/Root").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        speedLines = GameObject.Find("SpeedLines").GetComponent<ParticleSystem>();
    }

    // Only jump if the angle of the surface is not steeper than the required angle
    void OnJump(InputAction.CallbackContext context) {
        RaycastHit hit;
        bool normalRay = Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f);

        if ((Vector3.Angle(hit.normal, Vector3.up) <= jumpAngle && isGrounded) || isSwinging) {
            ResetDrag();

            AnimationCurve jumpCurve = new AnimationCurve(new Keyframe[] {new Keyframe(0f, 1f), 
            new Keyframe(0.3f, 0.2f)}); // Use keyframes to slow jump at max height

            rb.AddForce(Vector3.up * jumpSpeed * jumpAccl * jumpCurve.Evaluate(jumpTimer), ForceMode.Acceleration);
        }
        else {
            jumpTimer = 0f;
        }
    }

    void OnSwingPress(InputAction.CallbackContext context) { // TODO: Pick hangables first, before grtound
        //Debug.Log("Swing Press");

        if (menu.freeAim) {
            RaycastHit hitSwing;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitSwing, maxSwingDistance, swingable)) {
                swingPoint = hitSwing.point;
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
        
        else {
            //BETA SWING
            RaycastHit hitSwing;

            Vector3 newForward = cam.forward;
            newForward.y = 0;
            newForward.Normalize();

            Quaternion newRotation = Quaternion.AngleAxis(cam.eulerAngles.y, Vector3.up);
            Collider[] cols = Physics.OverlapBox(transform.position + (newForward * (maxSwingDistance / 2)) + (Vector3.up * (maxSwingDistance / 4)), 
            new Vector3(maxSwingDistance / 4, maxSwingDistance / 4, maxSwingDistance / 2), newRotation, swingable);

            if (cols.Length != 0) {
                // Debug.Log("Here");
                // foreach (Collider col in cols) {
                //     Debug.Log(col.transform.position);
                //     Debug.Log(col.gameObject.name);
                // }
                // Debug.DrawRay(transform.position, cols[0].transform.position - transform.position, Color.red, maxSwingDistance);
                Physics.Raycast(transform.position, cols[0].transform.position - transform.position, out hitSwing, maxSwingDistance, swingable);
                
                swingPoint = hitSwing.point;
                joint = gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = swingPoint;

                distanceFromPoint = Vector3.Distance(transform.position, swingPoint);
                joint.maxDistance = distanceFromPoint * maxSwingJointDistanceMod;
                joint.minDistance = distanceFromPoint * minSwingJointDistanceMod;

                joint.spring = spring;
                joint.damper = damper;
                joint.massScale = massScale;
                joint.connectedMassScale = massScale;

                lr.positionCount = 2;
                currentGrapplePosition = transform.position;

                isSwinging = true;
            }
        }
        
    }

    void OnSwingPerformed() {
        //Debug.Log("Swing Performed");
    }

    void OnSwingRelease(InputAction.CallbackContext context) {
        //Debug.Log("Swing Release");
        lr.positionCount = 0;
        Destroy(joint);
        isSwinging = false;
    }

    void OnPause(InputAction.CallbackContext context) {
        menu.PauseMenu();
    }

    void OnResume(InputAction.CallbackContext context) {
        menu.PauseMenu();
    }

    void Update() {
        move = actions.Gameplay.Move.ReadValue<Vector2>();
        viewRotation = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up);
        dir = viewRotation * new Vector3(Mathf.Clamp(move.x * 2, -1, 1), 0, Mathf.Clamp(move.y * 2, -1, 1));
    }
    
    void FixedUpdate() {
        if (rb.velocity.y == 0) {
            rb.AddForce(dir * speed, ForceMode.Acceleration);
        }
        else {
            rb.AddForce(dir * inAirSpeed, ForceMode.Acceleration);
        }
        

        if (move.x == 0 && move.y == 0 && currentDrag < maxDrag) {
            currentDrag += Time.deltaTime;
            rb.angularDrag = currentDrag;
        }

        if (currentDrag >= maxDrag) {
            //rb.velocity = Vector3.zero;
            rb.angularDrag = maxDrag;
        }

        if (move.x > 0 || move.y > 0) {
            ResetDrag();
        }

        if (rb.velocity.y < 0 && !isSwinging) {
            //rb.AddForce(Vector3.down * fallingSpeed, ForceMode.Acceleration);
        }

        if (Mathf.Abs(rb.velocity.x) > particleAppearSpeed || Mathf.Abs(rb.velocity.y) > particleAppearSpeed
        || Mathf.Abs(rb.velocity.z) > particleAppearSpeed) {
            speedLines.Play();
        }
        else {
            speedLines.Stop();
        }

        CheckIsGrounded();

        if (isGrounded || isSwinging) {
            jumpTimer = 0f;
        }
        else {
            jumpTimer += Time.deltaTime;
        }
    }

    void LateUpdate() {
        DrawRope();
    }

    void CheckIsGrounded() {
        float distanceGround = groundCollider.bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceGround + 0.1f);
    }

    void ResetDrag() {
        currentDrag = 0.05f;
        rb.angularDrag = 0.05f;
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
        Debug.Log("here");
        if (other.tag == "Kill") {
            menu.BlkScreenFadeInOut(0.5f);
            //DEBUG
            SceneManager.LoadScene(1);
            //SceneManager.LoadScene(LevelManager.current.sceneID);
        }
        else if (other.tag == "Checkpoint") {
            //checkMgr.UpdateCheckpoint(other.gameObject);
        }
        else if(other.gameObject.name == "JumpCol") {
            Debug.Log("test");
            menu.ControlFadeIn("Jump");
        }
        else if(other.gameObject.name == "SwingCol") {
            menu.ControlFadeIn("Swing");
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.name == "JumpCol") {
            menu.ControlFadeOut("Jump");
        }
        else if(other.gameObject.name == "SwingCol") {
            menu.ControlFadeOut("Swing");
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 newForward = cam.forward; //cam.InverseTransformDirection(cam.forward);
        newForward.y = 0;
        newForward.Normalize();

        Quaternion newRotation = Quaternion.Euler(0, cam.eulerAngles.y, 0);
        var oldMat = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + (newForward * maxSwingDistance / 2) + (Vector3.up * (maxSwingDistance / 4)), 
        Quaternion.AngleAxis(cam.eulerAngles.y, Vector3.up), new Vector3(maxSwingDistance / 2, maxSwingDistance / 2, maxSwingDistance));
        // Gizmos.DrawWireCube(transform.position + (newForward * maxSwingDistance / 2) + (Vector3.up * (maxSwingDistance / 4)), 
        // new Vector3(maxSwingDistance / 2, maxSwingDistance / 2, maxSwingDistance));
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMat;
    }
}
