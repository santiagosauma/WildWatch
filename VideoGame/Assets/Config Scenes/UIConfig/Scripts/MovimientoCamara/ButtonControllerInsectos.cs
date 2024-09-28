using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControllerInsectos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LightMovement lightMovement; // Referencia al controlador de movimiento de la luz
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
        }
        lightMovement.SetMoveDirection(dir);
        lightMovement.isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lightMovement.isMoving = false;
    }
}
