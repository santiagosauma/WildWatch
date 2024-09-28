// Script Name: CollisionLibro.cs
// Description:
//      * This script handles the activation of a Canvas
//      * when a collision with a specific tagged object occurs.
//      * It includes functionality for: Detecting collisions
//      * with objects tagged as "LibroCasa", enabling a
//      * specified Canvas when such a collision is detected.   
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

public class CollisionLibro : MonoBehaviour
{
    /// <summary>
    /// The Canvas to activate upon collision.
    /// </summary>
    public Canvas objectToActivate;

    /// <summary>
    /// Unity's OnCollisionEnter method to handle collision events.
    /// Enables the specified Canvas if the colliding object has the tag "LibroCasa".
    /// </summary>
    /// <param name="collision">The collision data associated with this collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "LibroCasa".
        if (collision.gameObject.CompareTag("LibroCasa"))
        {
            // If the Canvas is assigned, enable it.
            if (objectToActivate != null)
            {
                objectToActivate.enabled = true;
            }
            else
            {
                // No action is taken if the Canvas is not assigned.
                // This can be used for debugging or future extensions.
            }
        }
    }
}
