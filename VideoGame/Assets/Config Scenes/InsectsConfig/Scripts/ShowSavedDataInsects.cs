using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowSavedDataInsects : MonoBehaviour
{
    public Text InsectNameText;
    public Text descriptionText;

    public Text commonName;

    public Image ImageInsect;
    private int currentIndex = 0;
    public ClickOnInsecto clickOnInsecto;

    public static ShowSavedDataInsects instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destruye este objeto ya que solo debe haber una instancia.
            return;
        }
        instance = this;

    }

    public void ShowNextData()
    {
        List<InsectoInfo> InfoInsectosLista = ClickOnInsecto.Instance.RegresarInsectosIdentificados();
        print(InfoInsectosLista.Count);
        print(InfoInsectosLista.IsUnityNull());

        if (InfoInsectosLista != null && InfoInsectosLista.Count > 0)
        {
            // Incrementar el índice para avanzar en la lista
            currentIndex = (currentIndex + 1) % InfoInsectosLista.Count;

            // Obtener los datos de la planta actual
            InsectoInfo informacion = InfoInsectosLista[currentIndex];

            // Mostrar los datos en los Text UI
            InsectNameText.text = $"Nombre Científico: \r\n \r\n {informacion.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + informacion.Description;
            ImageInsect.gameObject.GetComponent<Image>().sprite = informacion.Image;
            commonName.text = $"Nombre Común: \r\n \r\n {informacion.nombreComun}";

        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }

    public void ShowPreviousData()
    {
        List<InsectoInfo> InfoInsectosLista = ClickOnInsecto.Instance.RegresarInsectosIdentificados();
        if (InfoInsectosLista != null && InfoInsectosLista.Count > 0)
        {
            // Decrementar el índice para retroceder en la lista
            currentIndex = (currentIndex - 1 + InfoInsectosLista.Count) % InfoInsectosLista.Count;

            // Obtener los datos de la planta actual
            InsectoInfo informacion = InfoInsectosLista[currentIndex];

            // Mostrar los datos en los Text UI
            InsectNameText.text = $"Nombre Científico: \r\n \r\n {informacion.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + informacion.Description;
            ImageInsect.gameObject.GetComponent<Image>().sprite = informacion.Image;
            commonName.text = $"Nombre Común: \r\n \r\n {informacion.nombreComun}";

        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }

    public void ShowInit()
    {
        // Debug.Log("LLegueZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
        List<InsectoInfo> InfoInsectosList = ClickOnInsecto.Instance.RegresarInsectosIdentificados();//avesController.instance.returnDescubiertos();
        // print(InfoInsectosList.Count);
        // print(InfoInsectosList.IsUnityNull());

        if (InfoInsectosList != null && InfoInsectosList.Count > 0)
        {
            // Incrementar el índice para avanzar en la lista
            // currentIndex = (currentIndex + 1) % InfoAnfibiosLista.Count;


            // Obtener los datos de la planta actual
            InsectoInfo informacion = InfoInsectosList[0];

            // Mostrar los datos en los Text UI
            InsectNameText.text = $"Nombre Científico: \r\n \r\n {informacion.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + informacion.Description;
            ImageInsect.gameObject.GetComponent<Image>().sprite = informacion.Image;
            commonName.text = $"Nombre Común: \r\n \r\n {informacion.nombreComun}";


            /*
            InsectNameText.text = "hola";
            descriptionText.text = "hola";
            ImageInsect.gameObject.GetComponent<Image>().sprite = null;
            */
        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }
}
