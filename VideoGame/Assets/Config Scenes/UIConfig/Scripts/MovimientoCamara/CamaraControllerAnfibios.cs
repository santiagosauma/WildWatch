using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControllerAnfibios : MonoBehaviour
{
    public bool isMoving = false;
    public Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 5.0f;
    private Camera mainCamera;
    public float zoomSpeed = 1.0f;
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float minX, maxX, minY, maxY;

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
        Vector3 newPosition = transform.position + (moveDirection * moveSpeed);

        // Limita la posición nueva con los valores mínimos y máximos
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}
