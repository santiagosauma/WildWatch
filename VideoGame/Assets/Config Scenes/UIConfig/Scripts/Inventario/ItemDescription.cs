using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using static UnityEditor.Progress;

public class ItemDescription : MonoBehaviour
{
    public Text TextoDescription;
    public Text TextoQuantity;
    public Button ItemObject;
    public Item ItemInfo;
    public GameObject CanvasDescription; // Referencia al Canvas
    void Start()
    {
        // Asegúrate de que el Canvas empiece desactivado
        ItemInfo.quantity = 3;
        CanvasDescription.SetActive(false);
        ActualizarCantidad();
    }

    public void MostrarDescripcion(Button button)
    {
        // Activa el Canvas
        Time.timeScale = 0f;
        CanvasDescription.SetActive(true);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Arrastable");
        foreach(var buton in buttons)
        {
            buton.GetComponent<DragDrop>().enabled = false;
        }
        GameObject[] Desarmarador = GameObject.FindGameObjectsWithTag("Desarmador");
        foreach (var desarmador in Desarmarador)
        {
            desarmador.GetComponent<DragDesarmar>().enabled = false;
        }
        // Actualiza el texto de la descripción y la cantidad
        TextoDescription.text = ItemInfo.description;
    }

    public void ActualizarCantidad()
    {
        TextoQuantity.text = ItemInfo.quantity.ToString();
    }

}
