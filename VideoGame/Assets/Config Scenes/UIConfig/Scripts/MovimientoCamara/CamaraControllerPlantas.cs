using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControllerPlantas : MonoBehaviour
{
    public bool isMoving = false;
    public Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 5.0f;
    private Camera mainCamera;
    public float zoomSpeed = 1.0f;
    public float minZoom = 1f;
    public float maxZoom = 10f;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (isMoving)
        {
            MoveCamera(moveDirection);
        }

    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    public void SetZoom(bool zoomIn)
    {
        float zoomChange = zoomIn ? -zoomSpeed : zoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize + zoomChange, minZoom, maxZoom);
    }

    private void MoveCamera(Vector3 direction)
    {
        Vector3 newPosition = transform.position + (direction * moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
