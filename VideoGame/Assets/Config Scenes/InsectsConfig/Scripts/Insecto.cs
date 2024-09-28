using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insecto : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad de movimiento del insecto
    public float changeDirectionTime = 1f; // Tiempo entre cambios de dirección

    private Vector3 targetPosition;
    private float timer;
    private bool isMoving = true; // Variable para controlar si el insecto está en movimiento o no
    private Sprite imagenOriginal;
    private int UniqueId;

    public int GetID() 
    { 
        return UniqueId; 
    }
    public void SetID(int uniqueId) 
    {
        UniqueId = uniqueId; 
    }

    public void SetImagenOriginal(Sprite img)
    {
        imagenOriginal = img;
    }
       
    public string GetImagenOriginal()
    {
        return imagenOriginal.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = changeDirectionTime;
        SetNewRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) // Solo mueve el insecto si está en movimiento
        {
            // Mueve gradualmente el insecto hacia la nueva posición
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Actualiza el temporizador
            timer -= Time.deltaTime;

            // Cuando el temporizador llega a cero, establece una nueva posición aleatoria
            if (timer <= 0f)
            {
                SetNewRandomPosition();
                timer = changeDirectionTime;
            }
        }
    }

    void SetNewRandomPosition()
    {
        // Genera una nueva posición aleatoria dentro de un área alrededor del insecto
        targetPosition = transform.position + Random.insideUnitSphere * 2f;
        targetPosition.z = transform.position.z; // Mantiene la misma coordenada Z
    }

    public void StopMovement()
    {
        isMoving = false; // Detiene el movimiento del insecto
    }
}
