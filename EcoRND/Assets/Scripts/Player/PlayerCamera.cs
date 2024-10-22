using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Enums;

public class PlayerCamera : MonoBehaviour
{
    public MenuSettings menu;
    [SerializeField] public float mouseSensitivity = 3f;
    [SerializeField] float movementSpeed = 50f;
    [SerializeField] float mass = 1f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] Transform cameraTransform;

    CharacterController Controller;
    internal Vector2 look;
    internal Vector3 velocity;
    bool hasFiredThisFrame;

    public SwapCamera CameraSwapper;

    bool isPossessing;
    public bool canMove;
    public SpawnMode spawnMode;
    public bool isInSpawnMode;
    public bool creatureDeleteMode;
    public CreatureSpawner creatureSpawner;
    public GameObject cursorOutline;

    public LayerMask objectSpawnLayer;

    (Vector3, Quaternion) initialPositionAndRotation;

    public event Action OnBeforeMove;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction flyAction;
    InputAction fireAction;
    InputAction exitAction;
    InputAction swapCamAction;
    InputAction menuAction;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];
        flyAction = playerInput.actions["fly"];
        fireAction = playerInput.actions["fire"];
        exitAction = playerInput.actions["exit"];
        swapCamAction = playerInput.actions["swapcam"];
        menuAction = playerInput.actions["openMenu"];

    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialPositionAndRotation = (transform.position, transform.rotation);
        CameraSwapper.SwitchCamera(0);
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (menuAction.WasPerformedThisFrame())
        {
            if (!menu.menuEnabled)
            {
                menu.EnableMenu();
            }
            else
            {
                menu.DisableMenu();
            }
        }
        if (canMove)
        {

            if (isPossessing)
            {
                if (exitAction.WasPerformedThisFrame())
                {
                    isPossessing = false;
                    transform.parent = null;
                }
            }

            float value = swapCamAction.ReadValue<float>();
            if (value != 0f)
            {
                CameraSwapper.SwapCameraInList(value);
            }

            if (!isPossessing)
            {
                UpdateLook();
                UpdateMovement();
                UpdateFly();
            }
            if (hasFiredThisFrame)
            {
                cursorOutline.SetActive(true);
            }else if (!hasFiredThisFrame)
            {
                cursorOutline.SetActive(false);
            }

            if (fireAction.WasPressedThisFrame() && !hasFiredThisFrame)
            {
                hasFiredThisFrame = true;
                Fire();
            }
        }

    }
    void LateUpdate()
    {
        hasFiredThisFrame = false; // Ensure reset happens at the end of each frame
    }

    void UpdateMovement()
    {
        OnBeforeMove?.Invoke();

        var moveInput = moveAction.ReadValue<Vector2>();

        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= movementSpeed;

        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);


        Controller.Move(velocity * Time.deltaTime);
    }

    void UpdateFly()
    {
        var flyInput = flyAction.ReadValue<Vector2>();
        var input = new Vector3();
        input += transform.up * flyInput.y;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= movementSpeed;

        var factor = acceleration * Time.deltaTime;
        velocity.y = Mathf.Lerp(velocity.y, input.y, factor);
    }

    void UpdateLook()
    {
        var lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensitivity;
        look.y += lookInput.y * mouseSensitivity;


        look.y = Mathf.Clamp(look.y, -89f, 89f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void Fire()
    {
        if (isInSpawnMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out hit, Mathf.Infinity, objectSpawnLayer))
            {
                if (spawnMode == SpawnMode.Creature)
                    creatureSpawner.SpawnCreatureAtLocation(hit.point);
                if (spawnMode == SpawnMode.Food)
                    creatureSpawner.SpawnFoodAtLocation(hit.point);
            }
        }
        else if (creatureDeleteMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out hit))
            {
                if (hit.transform.tag == "Creature")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out hit))
            {
                if (hit.transform.tag == "Creature")
                {
                    hit.transform.gameObject.GetComponent<CreatureController>().DisplayStats();
                }
            }
        }
    }

    void Possess(GameObject target)
    {
        isPossessing = true;
        transform.parent = target.transform;
        transform.position = target.transform.position;
        Camera[] targetCameralist = target.GetComponentsInChildren<Camera>();
        CameraSwapper.cameras = targetCameralist;
    }
}
