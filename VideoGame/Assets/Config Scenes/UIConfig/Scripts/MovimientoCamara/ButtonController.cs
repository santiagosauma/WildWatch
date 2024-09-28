using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CamaraController cameraController;
    public string direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (direction)
        {
            case "Izquierda":
                cameraController.SetMoveDirection(Vector3.left);
                break;
            case "Derecha":
                cameraController.SetMoveDirection(Vector3.right);
                break;
            case "Arriba":
                cameraController.SetMoveDirection(Vector3.up);
                break;
            case "Abajo":
                cameraController.SetMoveDirection(Vector3.down);
                break;
            case "ZoomIn":
                cameraController.SetZoom(true);
                break;
            case "ZoomOut":
                cameraController.SetZoom(false);
                break;
        }
        cameraController.isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraController.isMoving = false;
    }
}
