using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IniciarJuego : MonoBehaviour
{
    public Text DescriptionJuego;
    public Text TiempoText;
    public Canvas CanvasInicio;
    private int tiempo = 61;
    private bool juegoIniciado = false;
    public List<GameObject> Buttons;
    public PointsController pointsController;
    public GameObject Desarmador;
    public bool canceltiempo = false;

    void Start()
    {
        DesactivarDragAndDrop();
        // Configurar las instrucciones del juego
        DescriptionJuego.text = "Instrucciones:\n" +
            "1. Tendrás un minuto para encontrar todas las huellas.\n" +
            "2. Coloca trampas en el mapa para sumar puntos.\n" +
            "3. Arrastra las trampas desde el inventario al mapa.\n" +
            "4. Usa los botones para navegar por el mapa y hacer zoom.";
        

    }

    void DesactivarDragAndDrop()
    {
        foreach (GameObject item in Buttons)
        {
            item.GetComponent<DragDrop>().enabled = false;
        }
    }

    void ActivarDragAndDrop()
    {
        foreach (GameObject item in Buttons)
        {
            item.GetComponent<DragDrop>().enabled = true;
        }
    }

    public void IniciarContador()
    {
        if (!juegoIniciado)
        {
            juegoIniciado = true;
            CanvasInicio.enabled = false;
            StartCoroutine(TiempoDeEspera());
            ActivarDragAndDrop();
        }
        
    }

    IEnumerator TiempoDeEspera()
    {
        while (tiempo > 0)
        {
            yield return new WaitForSeconds(1);
            tiempo--;
            TiempoText.text = "Tiempo: " + tiempo;
            PlayerPrefs.SetInt("TiempoJuego", 60-tiempo);
            if (canceltiempo)
            {
                break;
            }
        }

        DesactivarDragAndDrop();
        Desarmador.GetComponent<DragDesarmar>().enabled = false;    
        pointsController.EmpezarACalcularPuntos();
        // Aquí puedes agregar lógica para el final del juego cuando el tiempo se acabe
        Debug.Log("Tiempo agotado. ¡Fin del juego!");
    }
    
    public void Canceltiempo()
    {
        canceltiempo = true;
    }
}
