// Script Name: ShowDataNotebook.cs
// Description:
//      This script manages the display of information
//      about various categories of flora and fauna in
//      a user interface, such as a compendium or
//      information panel. It interacts with a collection
//      of IDataDisplayable objects, which represent
//      different items in the categories. The script
//      is responsible for displaying details of these
//      items, including their scientific name, description,
//      common name, and image. The script allows users
//      to navigate through the items by showing the
//      next or previous entry in the list.     
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
using UnityEngine;
using UnityEngine.UI;

public class ShowDataNotebook : MonoBehaviour
{
    /// <summary>
    /// ID of the current minigame.
    /// </summary>
    int idMinigame;

    /// <summary>
    /// Reference to the scriptable object containing lists
    /// of different animal categories.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Text component to display the scientific name or
    /// type of the selected animal.
    /// </summary>
    public Text FloraFaunaNameText;

    /// <summary>
    /// Text component to display the description of the
    /// selected animal.
    /// </summary>
    public Text descriptionText;

    /// <summary>
    /// Text component to display the common name of the
    /// selected animal.
    /// </summary>
    public Text commonName;

    /// <summary>
    /// Image component to display the image of the
    /// selected animal.
    /// </summary>
    public Image ImageFloraFauna;

    /// <summary>
    /// The current index of the selected item within
    /// the current list.
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// The current list of animals being displayed, 
    /// converted to IDataDisplayable interface.
    /// </summary>
    private List<IDataDisplayable> currentItems;

    /// <summary>
    /// The index of the currently active list (category).
    /// </summary>
    private int listaactiva;


    /// <summary>
    /// Initializes the script by setting the current index to 0 and 
    /// retrieving the minigame ID from PlayerPrefs.
    /// It then selects the category based on the retrieved minigame ID.
    /// </summary>
    private void Start()
    {
        // Set the initial index for the current items list to 0.
        currentIndex = 0;

        // Retrieve the ID of the current minigame from PlayerPrefs.
        idMinigame = PlayerPrefs.GetInt("IDGAME");

        // Select the category based on the retrieved minigame ID.
        SeleccionarCategoria(idMinigame);
    }


    /// <summary>
    /// Selects the category of items to display based on the given
    /// category ID. It sets the currentItems list to the appropriate
    /// category and updates the active list indicator.
    /// </summary>
    /// <param name="categoryId">The ID of the category to select.
    /// Corresponds to different animal categories.</param>
    public void SeleccionarCategoria(int categoryId)
    {
        // Determine which category to load based on the provided category ID.
        switch (categoryId)
        {
            case 0:
                // Load bird items and set the active list indicator for birds.
                currentItems = animales.Pajaros.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 0;
                break;
            case 1:
                // Load amphibian items and set the active list indicator for amphibians.
                currentItems = animales.Anfibios.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 1;
                break;
            case 2:
                // Load mammal items and set the active list indicator for mammals.
                currentItems = animales.Mamiferos.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 2;
                break;
            case 3:
                // Load insect items and set the active list indicator for insects.
                currentItems = animales.Insectos.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 3;
                break;
            case 4:
                // Load plant items and set the active list indicator for plants.
                currentItems = animales.Plantas.ConvertAll(x => (IDataDisplayable)x);
                listaactiva = 4;
                break;
            default:
                // Log a warning if the provided category ID does not match any known categories.
                Debug.LogWarning("Category not recognized.");
                return;
        }

        // Reset the index to 0 to start from the beginning of the selected category.
        currentIndex = 0;

        // Update the displayed data with the newly selected category.
        ShowData();
    }


    /// <summary>
    /// Displays the data of the current item based on the 
    /// current index and active category. Updates the UI
    /// elements with the item's scientific name, description,
    /// common name, and image.
    /// </summary>
    private void ShowData()
    {
        // Check if the currentItems list is valid and if the currentIndex is within range.
        if (currentItems != null && currentItems.Count > 0 
        && currentIndex >= 0 && currentIndex < currentItems.Count)
        {
            IDataDisplayable currentItem = currentItems[currentIndex];

            if (listaactiva == 3)
            {
                // Special handling for the insect category (list ID 3)
                FloraFaunaNameText.text = $"Tipo de Huella: \r\n \r\n {currentItem.DisplayName}";
                descriptionText.text = "Descripción:\r\n \r\n " + currentItem.DisplayDescription;
                commonName.text = $" \r\n \r\n {currentItem.DisplayCommonName}";
                ImageFloraFauna.sprite = currentItem.DisplayImage;
            }
            else
            {
                // General handling for all other categories
                FloraFaunaNameText.text = $"Nombre Científico \r\n \r\n {currentItem.DisplayName}";
                descriptionText.text = "Descripción:\r\n \r\n " + currentItem.DisplayDescription;
                commonName.text = $"Nombre Común: \r\n \r\n {currentItem.DisplayCommonName}";
                ImageFloraFauna.sprite = currentItem.DisplayImage;
            }
        }
        else
        {
            // Log a warning if the index or list is invalid.
            Debug.LogWarning("Invalid index or list.");
        }
    }

    /// <summary>
    /// Advances to the next item in the currentItems list
    /// and updates the displayed data. Wraps around to the
    /// beginning of the list if the end is reached.
    /// </summary>
    public void ShowNextData()
    {
        // Check if the currentItems list is valid and not empty.
        if (currentItems != null && currentItems.Count > 0)
        {
            // Move to the next item and wrap around if necessary.
            currentIndex = (currentIndex + 1) % currentItems.Count;
            ShowData();
        }
    }

    /// <summary>
    /// Moves to the previous item in the currentItems
    /// list and updates the displayed data. Wraps around
    /// to the end of the list if the beginning is reached.
    /// </summary>
    public void ShowPreviousData()
    {
        // Check if the currentItems list is valid and not empty.
        if (currentItems != null && currentItems.Count > 0)
        {
            // Move to the previous item and wrap around if necessary.
            currentIndex = (currentIndex - 1 + currentItems.Count) % currentItems.Count;
            ShowData();
        }
    }

}
