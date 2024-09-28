// Script Name: IpadController.cs
// Description:
//      * Manages the functionality of the iPad UI in the
//      * game, including displaying information about
//      * animal registration, showing or hiding panels,
//      * and handling user interactions with buttons.                    
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 22/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IpadController : MonoBehaviour
{
    /// <summary>
    /// GameObject representing a message displayed when no animals are available for registration.
    /// </summary>
    public GameObject noAvailable;

    /// <summary>
    /// GameObject representing the iPad panel UI that is shown when the iPad is opened.
    /// </summary>
    public GameObject ipadPanel;

    /// <summary>
    /// Button component associated with opening the iPad panel (not used in the current script).
    /// </summary>
    public Button buttonIpad;

    /// <summary>
    /// Button used for transitioning levels (not utilized in this script).
    /// </summary>
    public Button temp;

    /// <summary>
    /// Text component showing the number of species remaining to be registered.
    /// </summary>
    public Text textRemaining;

    /// <summary>
    /// Text component used for displaying a message when no animals are available for registration.
    /// </summary>
    public Text labelRemaining;

    /// <summary>
    /// Integer storing the number of available registrations (loaded from PlayerPrefs).
    /// </summary>
    int available;

    /// <summary>
    /// Static reference to the singleton instance of the IpadController class.
    /// </summary>
    public static IpadController instance;

    /// <summary>
    /// Coroutine that displays a message indicating no animals are available for registration,
    /// then hides the message after 3 seconds.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator MatchTime()
    {
        labelRemaining.text = "No hay animales para registrar";
        noAvailable.SetActive(true);
        yield return new WaitForSeconds(3);
        noAvailable.SetActive(false);
    }

    /// <summary>
    /// Opens the iPad panel if there are animals to register; 
    /// otherwise, shows a message indicating no animals are available.
    /// </summary>
    public void openIpad()
    {
        if (PlayerPrefs.GetInt("RegisterNum", 0) < 1)
        {
            textRemaining.text = string.Empty;
            StartCoroutine(MatchTime());
        }
        else
        {
            ipadPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Initializes the script by hiding the `noAvailable` and `ipadPanel` GameObjects,
    /// and retrieves the number of available registrations from PlayerPrefs.
    /// </summary>
    void Start()
    {
        noAvailable.SetActive(false);
        ipadPanel.SetActive(false);
        available = PlayerPrefs.GetInt("RegisterNum", 0);
    }

    /// <summary>
    /// Updates the `textRemaining` UI component with the number of species left to register,
    /// or clears the text if no animals are available.
    /// </summary>
    void Update()
    {
        if (PlayerPrefs.GetInt("RegisterNum", 0) < 1)
        {
            textRemaining.text = string.Empty;
        }
        else
        {
            textRemaining.text = $"Especies a registrar: {PlayerPrefs.GetInt("RegisterNum", 0)}";
        }
    }
}
