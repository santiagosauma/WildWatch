using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using static UnityEditor.Progress;

public class DescripcionButtons : MonoBehaviour
{
    public Text DescriptionText;
    public List<Item> ItemInfo;
    private Item currentItem; // Item actualmente mostrado
    public GameObject CanvasDescription;
    // Cambia a detalles específicos del ítem

    public void EncontrarItem()
    {
        foreach (var item in ItemInfo)
        {
            if (item.description == DescriptionText.text)
            {
                currentItem = item;
            }
        }
    }
    public void SiguienteTexto()
    {
        
        EncontrarItem();

        if (currentItem != null && DescriptionText.text == currentItem.description)
        {
            switch (currentItem.id)
            {
                case 0:
                    DescriptionText.text = "Arrastra el icono de la camara hacia el" +
                        " mapa para colocarla. Recuerda que este objeto es util para todo" +
                        " tipo de animales.";
                    break;
                case 1:
                    DescriptionText.text = "Arrastra el icono de la trampa hacia el" +
                        " mapa para colocarla. Recuerda que este objeto es util para" +
                        " animales pequeños.";
                    break;
                case 2:
                    DescriptionText.text = "Arrastra el icono de la trampa hacia el" +
                        " mapa para colocarla. Recuerda que este objeto es util para todo" +
                        " tipo de animales, especialmente los grandes.";
                    break;
            }
        }
    }

    // Vuelve a la descripción original
    public void AnteriorText()
    {
        EncontrarItem();
        if (currentItem != null)
        {
            DescriptionText.text = currentItem.description;
        }
    }

    public void CerrarDescripcion(Button button)
    {
        // Activa el Canvas
        Time.timeScale = 1f;
        CanvasDescription.SetActive(false);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Arrastable");
        foreach (var buton in buttons)
        {
            buton.GetComponent<DragDrop>().enabled = true;
        }
        GameObject[] Desarmarador = GameObject.FindGameObjectsWithTag("Desarmador");
        foreach (var desarmador in Desarmarador)
        {
            desarmador.GetComponent<DragDesarmar>().enabled = true;
        }
    }
}


