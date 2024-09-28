// **********************************************************************
// Script Name: viewBirdsController.cs
// Description:
//      * This script allows the player to move an object within defined boundaries
//      * by capturing horizontal and vertical input.It also handles zooming in and
//      * out, adjusting the field of view smoothly between two predefined values. 
//      * The binoculars overlay is shown or hidden based on the zoom state.
//      * Sensitivity settings are applied to the movement, and the object's position
//      * is clamped to stay within the specified minimum and maximum X and Y boundaries.
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

//NOT USED IN FINAL GAME VERSION

using UnityEngine;
using UnityEngine.UI; 

public class viewBirdsController : MonoBehaviour
{
    /// <summary>
    /// Sensitivity of the object movement based on player input.
    /// </summary>
    public float sensitivity;

    /// <summary>
    /// Field of view value when zoomed in.
    /// </summary>
    public int zoomedIn = 25;

    /// <summary>
    /// Field of view value when zoomed out.
    /// </summary>
    public int zoomedOut = 60;

    /// <summary>
    /// Indicates whether the view is currently zoomed in.
    /// </summary>
    public bool isZoomed;

    /// <summary>
    /// Smooth transition speed for changing the field of view.
    /// </summary>
    public float smoothView = 4.5f;

    /// <summary>
    /// Reference to the binoculars overlay image object.
    /// </summary>
    public GameObject binocularsOverlay;

    /// <summary>
    /// Minimum X boundary for the object's movement.
    /// </summary>
    public float minX = -5.0f;

    /// <summary>
    /// Maximum X boundary for the object's movement.
    /// </summary>
    public float maxX = 5.0f;

    /// <summary>
    /// Minimum Y boundary for the object's movement.
    /// </summary>
    public float minY = -3.0f;

    /// <summary>
    /// Maximum Y boundary for the object's movement.
    /// </summary>
    public float maxY = 3.0f;


    /// <summary>
    /// Handles the movement of the object based on player input in the FixedUpdate loop.
    /// </summary>
    /// <remarks>
    /// This method captures horizontal and vertical input, calculates the new position of the object based on 
    /// sensitivity and deltaTime, and then moves the object accordingly. It ensures that the object's position 
    /// stays within defined boundaries using Mathf.Clamp.
    /// </remarks>    
    void FixedUpdate()
    {
        // Get horizontal movement input and apply sensitivity and deltaTime
        float moveHorizontal = Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;

        // Get vertical movement input and apply sensitivity and deltaTime
        float moveVertical = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;

        // Calculate the new position based on the current position and movement input
        Vector3 newPosition = transform.position + new Vector3(moveHorizontal, moveVertical, 0f);

        // Move the object based on the input values
        transform.Translate(moveHorizontal, moveVertical, 0f);

        // Clamp the new position to ensure it stays within the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Update the object's position
        transform.position = newPosition;


    }

}
