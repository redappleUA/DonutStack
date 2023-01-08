using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StickMove : MonoBehaviour
{
    public enum MoveState { Choose, Moving, Stop }

    [SerializeField] LayerMask mask;

    private MoveState state;
    private InputManager inputManager;
    private Camera cameraMain;
    private DonutStick donutStick;
    public GameObject row { get; private set; }

    private void OnEnable() => inputManager.OnStartTouch += MoveToRow;
    private void OnDisable() => inputManager.OnStartTouch -= MoveToRow;

    void Awake()
    {
        state = MoveState.Choose;
        inputManager = FindObjectOfType<InputManager>();
        //inputManager = InputManager.Instance;
        cameraMain = Camera.main;
        donutStick = GetComponent<DonutStick>();
    }

    void Update()
    {
        if (state == MoveState.Moving )
        {
            Move();

            if (transform.position == row.transform.Find("SpawnPosition").position)
            {
                state = MoveState.Stop;
                donutStick.isSpawned = true;
            }         
        }

        CheckGravity();
    }

    private void MoveToRow(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new(screenPosition.x, screenPosition.y, 100);

        if (donutStick.isSpawned == false) 
        {
            if (Physics.Raycast(cameraMain.ScreenPointToRay(screenCoordinates), out var hit, 100, mask))
            {
                if (hit.collider.CompareTag("Row") && state != MoveState.Moving)
                {
                    row = hit.collider.gameObject;

                    var rowSticks = row.GetComponent<Row>();
                    rowSticks.donutStickCount++;

                    if (rowSticks.donutStickCount > 5)
                    {
                        state = MoveState.Choose;
                        rowSticks.donutStickCount = 5;
                    }
                    else
                        state = MoveState.Moving;
                }
            }
        }
    }

    void CheckGravity()
    {
        var kinem = GetComponent<Rigidbody>();
        var collider = GetComponent<Collider>();

        if (state == MoveState.Stop)
        {
            kinem.useGravity = true;
            collider.enabled = true;
        }
        else
        {
            kinem.useGravity = false;
            collider.enabled = false;
        }
    }

    void Move()
    {
        Vector3 directionToMove = row.transform.Find("SpawnPosition").position - transform.position;
        directionToMove = 3f * Time.deltaTime * directionToMove.normalized;
        float maxDistance = Vector3.Distance(transform.position, row.transform.Find("SpawnPosition").position);
        transform.position = transform.position + Vector3.ClampMagnitude(directionToMove, maxDistance);
    }
}
