// Script Name: DataManager.cs
// Description:
//      * The script manages game-wide data,
//      * including whether the tutorial has been completed.
//      * It implements the singleton pattern to ensure
//      * only one instance exists.      
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 24/07/2024 
// **********************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the DataManager to ensure it follows the singleton pattern.
    /// </summary>
    public static DataManager Instance;

    /// <summary>
    /// Boolean to track if the tutorial has been completed by the player.
    /// </summary>
    public bool IsTutorialCompleted;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures that there is only one instance of DataManager and persists it across scenes.
    /// </summary>
    private void Awake()
    {
        // Check if the instance is null
        if (Instance == null)
        {
            // Assign this instance to the static Instance variable
            Instance = this;
            // Ensure that this object is not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy this object if another instance already exists
            Destroy(gameObject);
        }
    }
}
