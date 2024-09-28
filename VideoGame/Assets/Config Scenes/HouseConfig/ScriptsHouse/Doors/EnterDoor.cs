// Script Name: EnterDoor.cs
// Description:
//      * This script handles interactions when entering
//      * a door, including displaying messages and
//      * transitioning to a minigame based on certain conditions.           
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
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class EnterDoor : MonoBehaviour
{
    /// <summary>
    /// Text component used to display messages to the player.
    /// </summary>
    public Text descripcion;

    /// <summary>
    /// GameObject used as a visual indicator to highlight the message display.
    /// </summary>
    public GameObject square;

    /// <summary>
    /// Coroutine that activates the visual indicator (square) for 3 seconds before deactivating it.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator MatchTime()
    {
        // Show the visual indicator.
        square.SetActive(true);

        // Wait for 3 seconds.
        yield return new WaitForSeconds(3);

        // Hide the visual indicator.
        square.SetActive(false);
    }

    /// <summary>
    /// Handles the entry action when the player interacts with the door.
    /// Checks if required player actions have been completed, and either displays a warning message
    /// or transitions to the minigame.
    /// </summary>
    public void enter()
    {
        // Check if the player has completed the required registrations.
        if (PlayerPrefs.GetInt("RegisterNum", 0) > 0)
        {
            // Display a warning message if the registrations are not completed.
            descripcion.text = "Debes llenar los registros primero";
            StartCoroutine(MatchTime());
        }
        else
        {
            // Set the final option for the minigame and transition to it.
            PlayerPrefs.SetInt("FinalOption", PlayerPrefs.GetInt("usedForDoor"));
            DoorsController.instance.enterMinigame();
        }
    }
}
