using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowSavedData : MonoBehaviour
{
    public Text plantNameText;
    public Text descriptionText;

    public Text commonName;

    public Image ImagePlant;
    private int currentIndex = 0;

    public static ShowSavedData instance;

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
        List<TileData> selectedTileDataList = FindObjectOfType<MapManager>().GetSelectedTileDataList();
        if (selectedTileDataList != null && selectedTileDataList.Count > 0)
        {
            // Incrementar el índice para avanzar en la lista
            currentIndex = (currentIndex + 1) % selectedTileDataList.Count;

            // Obtener los datos de la planta actual
            TileData currentTileData = selectedTileDataList[currentIndex];

            // Mostrar los datos en los Text UI
            plantNameText.text = $"Nombre Científico: \r\n \r\n {currentTileData.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + currentTileData.Description;
            commonName.text = $"Nombre Común: \r\n \r\n {currentTileData.nombreComun}";
            ImagePlant.gameObject.GetComponent<Image>().sprite = currentTileData.Image;
        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }

    public void ShowPreviousData()
    {
        List<TileData> selectedTileDataList = FindObjectOfType<MapManager>().GetSelectedTileDataList();
        if (selectedTileDataList != null && selectedTileDataList.Count > 0)
        {
            // Decrementar el índice para retroceder en la lista
            currentIndex = (currentIndex - 1 + selectedTileDataList.Count) % selectedTileDataList.Count;

            // Obtener los datos de la planta actual
            TileData currentTileData = selectedTileDataList[currentIndex];

            // Mostrar los datos en los Text UI
            plantNameText.text = $"Nombre Científico: \r\n \r\n {currentTileData.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + currentTileData.Description;
            commonName.text = $"Nombre Común: \r\n \r\n {currentTileData.nombreComun}";

            ImagePlant.gameObject.GetComponent<Image>().sprite = currentTileData.Image;
        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }

    public void ShowInit()
    {
        // Debug.Log("LLegueZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
        List<TileData> InfoTileList = MapManager.Instance.GetSelectedTileDataList();
        // print(InfoTileList.Count);
        // print(InfoTileList.IsUnityNull());

        if (InfoTileList != null && InfoTileList.Count > 0)
        {
            // Incrementar el índice para avanzar en la lista
            // currentIndex = (currentIndex + 1) % InfoAnfibiosLista.Count;


            // Obtener los datos de la planta actual
            TileData currentTileData = InfoTileList[0];

            // Mostrar los datos en los Text UI
            plantNameText.text = $"Nombre Científico: \r\n \r\n {currentTileData.Name}";
            descriptionText.text = "Descripción:\r\n \r\n " + currentTileData.Description;
            commonName.text = $"Nombre Común: \r\n \r\n {currentTileData.nombreComun}";

            ImagePlant.gameObject.GetComponent<Image>().sprite = currentTileData.Image;

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