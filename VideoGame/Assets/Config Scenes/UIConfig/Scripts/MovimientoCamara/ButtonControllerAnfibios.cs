using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControllerAnfibios : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CamaraControllerAnfibios cameraController;
    public string direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 dir = Vector3.zero;
        switch (direction)
        {
            case "Izquierda":
                dir = Vector3.left;
                break;
            case "Derecha":
                dir = Vector3.right;
                break;
            case "Arriba":
                dir = Vector3.up;
                break;
            case "Abajo":
                dir = Vector3.down;
                break;
            case "ZoomIn":
                cameraController.SetZoom(true);
                return;
            case "ZoomOut":
                cameraController.SetZoom(false);
                return;
        }
        cameraController.SetMoveDirection(dir);
        cameraController.isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraController.isMoving = false;
    }
}