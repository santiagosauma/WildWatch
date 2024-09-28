using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public float speed = 2f; // Velocidad de movimiento de la luz
    private Vector3 moveDirection = Vector3.zero;
    public bool isMoving = false;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D[] allColliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D col in allColliders)
        {
            if (col.CompareTag("Insecto"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }
}
