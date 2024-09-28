// Script Name: ShowDataCompendium.cs
// Description:
//      * The script handles the display of animal data
//      * in a compendium within the house scene. It allows
//      * the user to navigate through different categories
//      * of animals (birds, amphibians, insects, mammals, plants)
//      * and view detailed information about each animal,
//      * including its scientific name, description, common
//      * name, and image.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 23/07/2024 
// **********************************************************************



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowDataCompendiums : MonoBehaviour
{
    /// <summary>
    /// List of animals used in the game.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Text component to display the name of the flora or fauna.
    /// </summary>
    public Text FloraFaunaNameText;

    /// <summary>
    /// Text component to display the description of the flora or fauna.
    /// </summary>
    public Text descriptionText;

    /// <summary>
    /// Image component to display the image of the flora or fauna.
    /// </summary>
    public Image ImageFloraFauna;

    /// <summary>
    /// Button component used for interactions.
    /// </summary>
    private Button button;

    /// <summary>
    /// Index representing the current list of items being 
    /// displayed (0-based index).
    /// </summary>
    private int currentList = 0;

    /// <summary>
    /// Index representing the current item in the list being 
    /// displayed (0-based index).
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// List of items that implement the IDataDisplayable interface,
    /// representing the current items being displayed.
    /// </summary>
    private List<IDataDisplayable> currentItems;

    /// <summary>
    /// Text component to display the common name of the flora or 
    /// fauna.
    /// </summary>
    public Text commonName;

    /// <summary>
    /// Index representing the active list (0-based index).
    /// </summary>
    private int listaactiva;

    /// <summary>
    /// Obtains the ID from the button and sets the current list and index.
    /// </summary>
    /// <param name="button">The button from which to obtain the ID.</param>
    public void ObtenerID(Button button)
    {
        currentList = button.GetComponent<ButtonInfo>().UniqueId;
        currentIndex = 0;
        SeleccionarCategoria(currentList);
    }

    /// <summary>
    /// Unity's Start method, called before the first frame update.
    /// Initializes the current index and displays the initial data.
    /// </summary>
    void Start()
    {
        currentIndex = 0;
        ShowInit();
    }


    /// <summary>
    /// Selects the category of animals or plants based on the provided category ID,
    /// converts the list to IDataDisplayable, sets the active list index, 
    /// and shows the data of the first item in the selected category.
    /// </summary>
    /// <param name="categoryId">The ID of the category to select.</param>
    public void SeleccionarCategoria(int categoryId)
    {
        switch (categoryId)
        {
            case 0:
                currentItems = animales.Pajaros.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 0; // Active list index for Pajaros (Birds)
                break;
            case 1:
                currentItems = animales.Anfibios.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 1; // Active list index for Anfibios (Amphibians)
                break;
            case 2:
                currentItems = animales.Insectos.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 2; // Active list index for Insectos (Insects)
                break;
            case 3:
                currentItems = animales.Mamiferos.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 3; // Active list index for Mamiferos (Mammals)
                break;
            case 4:
                currentItems = animales.Plantas.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 4; // Active list index for Plantas (Plants)
                break;
            default:
                Debug.LogWarning("Category not recognized."); // Warning for unrecognized category
                return;
        }
        currentIndex = 0; // Reset current index to 0
        ShowData(); // Show data of the first item in the selected category
    }


    /// <summary>
    /// Displays the data of the currently selected item in the compendium based on the active list and current index.
    /// </summary>
    private void ShowData()
    {
        // Check if the current list is valid and the current index is within bounds
        if (currentItems != null && currentItems.Count > 0 && currentIndex >= 0 && currentIndex < currentItems.Count)
        {
            // Check if the active list is Mammals (index 3)
            if (listaactiva == 3)
            {
                IDataDisplayable currentItem = currentItems[currentIndex];
                // Display data for Mammals with different formatting
                FloraFaunaNameText.text = $"Tipo de Huella: \r\n \r\n {currentItem.DisplayName}";
                descriptionText.text = "Descripción:\r\n \r\n " + currentItem.DisplayDescription;
                commonName.text = $" \r\n \r\n {currentItem.DisplayCommonName}";
                ImageFloraFauna.sprite = currentItem.DisplayImage;
            }
            else
            {
                IDataDisplayable currentItem = currentItems[currentIndex];
                // Display data for other categories with default formatting
                FloraFaunaNameText.text = $"Nombre Científico \r\n \r\n {currentItem.DisplayName}";
                descriptionText.text = "Descripción:\r\n \r\n " + currentItem.DisplayDescription;
                commonName.text = $"Nombre Común: \r\n \r\n {currentItem.DisplayCommonName}";
                ImageFloraFauna.sprite = currentItem.DisplayImage;
            }
        }
        else
        {
            // Log a warning if the current list or index is invalid
            Debug.LogWarning("Invalid index or list.");
        }
    }


    /// <summary>
    /// Initializes the display with the first item from the list of birds.
    /// </summary>
    public void ShowInit()
    {
        // Convert the list of birds to a list of IDataDisplayable and assign it to currentItems
        currentItems = animales.Pajaros.ConvertAll(x => (IDataDisplayable)x);

        // Get the first item from the currentItems list
        IDataDisplayable currentItem = currentItems[currentIndex];

        // Update the UI elements with the data of the current item
        FloraFaunaNameText.text = $"Nombre Científico \r\n \r\n {currentItem.DisplayName}";
        descriptionText.text = "Descripción\r\n \r\n: " + currentItem.DisplayDescription;
        commonName.text = $"Nombre Común: \r\n \r\n {currentItem.DisplayCommonName}";
        ImageFloraFauna.sprite = currentItem.DisplayImage;
    }


    /// <summary>
    /// Shows the next item in the current list of items.
    /// </summary>
    public void ShowNextData()
    {
        // Check if the current list of items is not null and contains elements
        if (currentItems != null && currentItems.Count > 0)
        {
            // Increment the current index and wrap around if necessary
            currentIndex = (currentIndex + 1) % currentItems.Count;
            // Display the data of the current item
            ShowData();
        }
    }

    /// <summary>
    /// Shows the previous item in the current list of items.
    /// </summary>
    public void ShowPreviousData()
    {
        // Check if the current list of items is not null and contains elements
        if (currentItems != null && currentItems.Count > 0)
        {
            // Decrement the current index and wrap around if necessary
            currentIndex = (currentIndex - 1 + currentItems.Count) % currentItems.Count;
            // Display the data of the current item
            ShowData();
        }
    }

}

