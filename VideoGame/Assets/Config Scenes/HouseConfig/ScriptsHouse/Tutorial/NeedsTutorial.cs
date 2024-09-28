// Script Name: NeedsTutorial.cs
// Description:
//      * The script checks whether a player has
//      * completed a tutorial when the game starts.
//      * If the tutorial is not completed, it loads
//      * the tutorial scene; otherwise, it allows
//      * the player to proceed normally.
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
using Newtonsoft.Json;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class NeedsTutorial : MonoBehaviour
{
    /// <summary>
    /// Called when the script instance is being loaded.
    /// Retrieves the player ID and starts the coroutine to check if the tutorial is completed.
    /// </summary>
    void Start()
    {
        // Retrieves the player ID stored in PlayerPrefs when the scene is loaded
        int userId = PlayerPrefs.GetInt("UserID");
        // Starts the coroutine to check if the tutorial has been completed for this user
        StartCoroutine(CheckTutorialCompleted(userId));
    }

    /// <summary>
    /// Determines whether the tutorial has been completed and loads the tutorial scene if not.
    /// </summary>
    void TutorialJugado()
    {
        // Checks if the tutorial has been marked as completed in the DataManager
        bool tutorialCompleted = DataManager.Instance.IsTutorialCompleted;
        Debug.Log("El tutorial está completado: " + tutorialCompleted);

        // Loads the tutorial scene if it hasn't been completed
        if (!tutorialCompleted) SceneManager.LoadScene("TutorialScene");
    }

    /// <summary>
    /// Coroutine that sends a web request to check if the tutorial is completed for the given user ID.
    /// </summary>
    /// <param name="userId">The player ID to check the tutorial completion status for.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator CheckTutorialCompleted(int userId)
    {
        // Constructs the URL for the web request
        string url = $"https://10.22.156.99:7026/api/Videogame/isTutorialCompleted/{userId}";

        // Creates a new web request to the specified URL
        UnityWebRequest request = UnityWebRequest.Get(url);
        // Assigns a custom certificate handler to accept all certificates
        request.certificateHandler = new ForceAceptAll();

        // Sends the web request and waits for a response
        yield return request.SendWebRequest();

        // Checks if the web request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parses the response from the server
            var response = JsonConvert.DeserializeObject<Dictionary<string, bool>>(request.downloadHandler.text);
            // Updates the DataManager with the tutorial completion status
            DataManager.Instance.IsTutorialCompleted = response["isTutorialCompleted"];
            // Calls the method to decide whether to load the tutorial scene
            TutorialJugado();
        }
        else
        {
            // Logs an error if the web request failed
            Debug.LogError("Error al obtener la información del tutorial: " + request.error);
            // Calls the method to decide whether to load the tutorial scene even if there was an error
            TutorialJugado();
        }
    }

}
