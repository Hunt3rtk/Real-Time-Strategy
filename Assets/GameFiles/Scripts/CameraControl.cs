using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour {

    //Singleton
    public static CameraControl Instance;

    private PlayerInput cameraInput;
    private InputAction movement;
    private Transform cameraTransform;

    //horizontal motion
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;

    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    //vertical motion - zooming
    [SerializeField]
    private float stepSize = 5f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 10f;
    [SerializeField]
    private float maxHeight = 30f;
    [SerializeField]
    private float zoomSpeed = 2f;

    //screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.005f;
    [SerializeField]
    private bool useScreenEdge = true;

    //value set in various functions
    //used to update the position of the camera base object
    private Vector3 targetPostion;

    private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //tracks where the dragging action started
    Vector3 startDrag;

    private Camera cam;
    
    private void Awake() {

        //Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        cameraInput = new PlayerInput();
        cam = this.GetComponentInChildren<Camera>();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
        cameraInput.Camera.Enable();
    }

    private void OnEnable() {
        zoomHeight = cam.orthographicSize;
        //zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);
        lastPosition = this.transform.position;
        cameraInput.Camera.Enable();
        movement = cameraInput.Camera.Movement;
        cameraInput.Camera.ZoomCamera.performed += ZoomCamera;
        //cameraInput.Camera.RotateCamera.performed += RotateCamera;
    }

    private void OnDisable()
    {
        cameraInput.Camera.ZoomCamera.performed -= ZoomCamera;
        //cameraInput.Camera.RotateCamera.performed -= RotateCamera;
        cameraInput.Disable();
    }

    public void Disable()
    {
        OnDisable();
    }

    public void Enable()
    {
        OnEnable();
    }


    private void Update()
    {
        if ( Time.timeScale == 0f) { return; }
        GetKeyboardMovement();
        if (useScreenEdge) { CheckMouseAtScreenEdge(); }
        DragCamera();
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePostion();
    }

    private void UpdateVelocity() {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement() {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();
        inputValue = inputValue.normalized;
        if(inputValue.sqrMagnitude > 0.1f) {
            targetPostion += inputValue;
        }
    }

    private Vector3 GetCameraRight() {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

        private Vector3 GetCameraForward() {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePostion() {
        if(targetPostion.sqrMagnitude > 0.1f) {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPostion * speed * Time.deltaTime;
        } else {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            if (horizontalVelocity == new Vector3(float.NaN, 0, float.NaN)) horizontalVelocity = Vector3.zero;
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPostion = Vector3.zero;
    }

    
    // private void RotateCamera(InputAction.CallbackContext ctx) {
    //     if(!Mouse.current.rightButton.isPressed) return;
    //     float valueX = ctx.ReadValue<Vector2>().x;
    //     transform.rotation = Quaternion.Euler(0f, valueX * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);

    // }

    
    private void ZoomCamera(InputAction.CallbackContext ctx) {
        float value = -ctx.ReadValue<Vector2>().y;
        if(Mathf.Abs(value) > 0.1f) {
            zoomHeight = cam.orthographicSize + value * stepSize;
            //zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            zoomHeight = zoomHeight % stepSize >= 2.5f ? zoomHeight = (zoomHeight - zoomHeight % stepSize) + stepSize :  zoomHeight = zoomHeight - zoomHeight % stepSize;
            
            if (zoomHeight < minHeight) {
                zoomHeight = minHeight;
            } else if (zoomHeight > maxHeight) {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UpdateCameraPosition() {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * (Vector3.forward/2);

        //cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cam.orthographicSize = zoomHeight;
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge() {
        Vector2 mousePostion = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePostion.x < edgeTolerance * Screen.width) {
            moveDirection -= GetCameraRight();
        } else if (mousePostion.x > (1f - edgeTolerance) * Screen.width){
            moveDirection += GetCameraRight();
        }

        if (mousePostion.y < edgeTolerance * Screen.height) {
            moveDirection -= GetCameraForward();
        } else if (mousePostion.y > (1f - edgeTolerance) * Screen.height){
            moveDirection += GetCameraForward();
        }

        targetPostion += moveDirection;
    }

    private void DragCamera() {
        if (!Mouse.current.middleButton.isPressed) { return; }
        Plane plane  = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out float distance)) {
            if (Mouse.current.middleButton.wasPressedThisFrame) {
                startDrag = ray.GetPoint(distance);
            } else {
                targetPostion += startDrag - ray.GetPoint(distance);
            }
        }
    }
}
