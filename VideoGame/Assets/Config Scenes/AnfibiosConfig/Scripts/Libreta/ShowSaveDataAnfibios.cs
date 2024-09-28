// **********************************************************************
// Script Name: ShowSaveDataAnfibios.cs
// Description:
//      * Manages the display of amphibian data in the UI.
//      * This script handles showing detailed information
//      * about discovered amphibians. It includes functionality
//      * for navigating through the list of amphibians,
//      * displaying the current amphibian's scientific name,
//      * common name, description, and image.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 20/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowSaveDataAnfibios : MonoBehaviour
{

    /// <summary>
    /// UI Text component for displaying the name of the amphibian.
    /// </summary>
    public Text AmphibianNameText;

    /// <summary>
    /// UI Text component for displaying the description of the amphibian.
    /// </summary>
    public Text descriptionText;

    /// <summary>
    /// UI Text component for displaying the common name of the amphibian.
    /// </summary>
    public Text commonName;

    /// <summary>
    /// UI Image component for displaying the image of the amphibian.
    /// </summary>
    public Image imageAmphibian;

    /// <summary>
    /// Index used to keep track of the current amphibian being displayed or interacted with.
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// Singleton instance of the ShowSaveDataAnfibios class.
    /// Ensures that only one instance of this class exists and provides a global point of access to it.
    /// </summary>
    public static ShowSaveDataAnfibios instance;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the singleton instance of this class. If another instance of the class already exists, it destroys the current game object to ensure that only one instance remains.
    /// </summary>
    void Awake()
    {
        if (instance != null && instance != this)
        {
            // Destroy this instance if another instance already exists
            Destroy(gameObject);
            return;
        }
        // Set the singleton instance to this instance
        instance = this;
    }
    

    /// <summary>
    /// Shows the amphibian data based on the specified index adjustment.
    /// This method updates the UI with the amphibian information from the list.
    /// </summary>
    /// <param name="indexAdjustment">The amount to adjust the current index. Positive for next, negative for previous, and zero for initial.</param>
    private void ShowData(int indexAdjustment)
    {
        // Get the list of discovered amphibians
        List<AnfibioInfo> InfoAnfibiosLista = AnfibiosController.instance.returnDescubiertos();

        // Check if the list is valid and contains elements
        if (InfoAnfibiosLista != null && InfoAnfibiosLista.Count > 0)
        {
            // Update the index based on the adjustment value
            currentIndex = (currentIndex + indexAdjustment + InfoAnfibiosLista.Count) % InfoAnfibiosLista.Count;

            // Get the current amphibian data
            AnfibioInfo informacion = InfoAnfibiosLista[currentIndex];

            // Display the data in the UI
            AmphibianNameText.text = $"Nombre Científico: \r\n \r\n {informacion.Name}";
            descriptionText.text = $"Descripción: \r\n \r\n {informacion.Description}";
            commonName.text = $"Nombre Común: \r\n \r\n {informacion.nombreComun}";
            imageAmphibian.gameObject.GetComponent<Image>().sprite = informacion.Image;
        }
        else
        {
            Debug.LogWarning("No selected tile data available.");
        }
    }

    /// <summary>
    /// Shows the next amphibian data in the list by calling the general function with an index adjustment of 1.
    /// </summary>
    public void ShowNextData()
    {
        ShowData(1); // Call the general function with direction 1 for next
    }

    /// <summary>
    /// Shows the previous amphibian data in the list by calling the general function with an index adjustment of -1.
    /// </summary>
    public void ShowPreviousData()
    {
        ShowData(-1); // Call the general function with direction -1 for previous
    }

    /// <summary>
    /// Initializes the display to show the first amphibian data in the list by calling the general function with an index adjustment of 0.
    /// </summary>
    public void ShowInit()
    {
        ShowData(0); // Call the general function with direction 0 for initial
    }
    
    
}


