// Script Name: OpenCompendio.cs
// Description:
//      * This script checks the distance between the player
//      * and the object to determine whether the compendium
//      * should be opened or closed. The compendium is
//      * activated when the player is within a certain
//      * distance from the object and deactivated when the
//      * player moves away.
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
using Unity.VisualScripting;
using UnityEngine;
using static DoorsController;

public class OpenCompendio : MonoBehaviour
{
    /// <summary>
    /// The GameObject representing the player, used to determine the distance to the object.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// The distance threshold within which the compendium will be opened.
    /// </summary>
    public float openDistance = 0.01f;

    /// <summary>
    /// A boolean flag indicating whether the compendium is currently open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// The GameObject representing the compendium UI to be activated or deactivated.
    /// </summary>
    public GameObject compendio;

    /// <summary>
    /// Continuously checks the distance between the player and the object to determine whether to open or close the compendium.
    /// </summary>
    void Update()
    {
        // Calculate the distance between the player and the object.
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Open the compendium if the distance is within the threshold and it is not already open.
        if (distance <= openDistance && !isOpen)
        {
            compendio.SetActive(true);
            isOpen = true;
        }

        // Close the compendium if the distance is greater than the threshold.
        if (distance > openDistance)
        {
            isOpen = false;
        }
    }
}
