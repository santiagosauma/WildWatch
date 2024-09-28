using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonControllerHouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Animator animatorController;
    public CamaraControllerHouse cameraController;
    public string direction;
    private Camera mainCamera;
    public void Start()
    {
        mainCamera = Camera.main;
        animatorController = mainCamera.GetComponentInChildren<Animator>();
        GameObject Jugador = GameObject.FindGameObjectWithTag("Jugador");
        Collider2D CamaraCollider = mainCamera.GetComponent<Collider2D>();
        Collider2D JugadorCollider = Jugador.GetComponent<Collider2D>();

        if (CamaraCollider != null && JugadorCollider != null)
        {
            Physics2D.IgnoreCollision(CamaraCollider, JugadorCollider, true);
        }
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (direction)
        {
            case "Izquierda":
                cameraController.SetMoveDirection(Vector3.left);
                UpdateAnimation(CharacterAnimation.WalkLeft);
                break;
            case "Derecha":
                cameraController.SetMoveDirection(Vector3.right);
                UpdateAnimation(CharacterAnimation.WalkRight);
                break;
            case "Arriba":
                cameraController.SetMoveDirection(Vector3.up);
                UpdateAnimation(CharacterAnimation.WalkUp);
                break;
            case "Abajo":
                cameraController.SetMoveDirection(Vector3.down);
                UpdateAnimation(CharacterAnimation.WalkDown);
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
    public enum CharacterAnimation
    {
        WalkUp, WalkDown, WalkLeft, WalkRight
    }

    void UpdateAnimation(CharacterAnimation animation)
    {
        switch (animation)
        {
            case CharacterAnimation.WalkUp:
                animatorController.SetBool("isUpward", true);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", false);
                break;
            case CharacterAnimation.WalkDown:
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", true);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", false);
                break;
            case CharacterAnimation.WalkRight:
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", true);
                animatorController.SetBool("isLeft", false);
                break;
            case CharacterAnimation.WalkLeft:
                animatorController.SetBool("isUpward", false);
                animatorController.SetBool("isDownward", false);
                animatorController.SetBool("isRight", false);
                animatorController.SetBool("isLeft", true);
                break;

        }
    }
}
