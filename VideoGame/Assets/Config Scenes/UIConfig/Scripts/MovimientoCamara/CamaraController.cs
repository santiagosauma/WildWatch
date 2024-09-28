using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
// using static UnityEditor.Progress;

public class CamaraController : MonoBehaviour
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

        GameObject desarmador = GameObject.FindGameObjectWithTag("Desarmador");
        Collider2D CamaraCollider = mainCamera.GetComponent<Collider2D>();
        Collider2D DesarmadorCollider = desarmador.GetComponent<Collider2D>();

        if (CamaraCollider != null && DesarmadorCollider != null)
        {
            Physics2D.IgnoreCollision(CamaraCollider, DesarmadorCollider, true);

        }
    }
    void Update()
    {
        FindItems();
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

    private void FindItems()
    {
        // Encuentra todos los objetos con la etiqueta "PlaceableObject" (trampas)
        GameObject[] items = GameObject.FindGameObjectsWithTag("PlaceableObject");

        
        

        if (mainCamera != null)
        {
            // Ignora la colisión entre la camara y cada trampa del juego
            foreach (var item in items)
            {
                Collider2D CamaraCollider = mainCamera.GetComponent<Collider2D>();
                Collider2D ItemsCollider = item.GetComponent<Collider2D>();

                if (CamaraCollider != null && ItemsCollider != null)
                {
                    Physics2D.IgnoreCollision(CamaraCollider, ItemsCollider, true);
                }
            }
            

        }
    }    
}