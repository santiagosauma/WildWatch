// Script Name: LecturaBaseDatos.cs
// Description:
//      * The script manages the retrieval and
//      * updating of minigame scores and user
//      * information, processes responses from
//      * an external API, and updates the game's
//      * UI to reflect current performance metrics.
//      * It handles both the fetching of score
//      * data and the submission of updated scores,
//      * ensuring the user interface remains
//      * accurate and responsive to changes in the
//      * user's minigame performance.
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


using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LecturaBaseDatos : MonoBehaviour
{
    /// <summary>
    /// GameObject representing the UI element that
    /// displays the score for bird-related games.
    /// </summary>
    public GameObject PuntuacionAves;

    /// <summary>
    /// GameObject representing the UI element that
    /// displays the score for amphibian-related games.
    /// </summary>
    public GameObject PuntuacionAnfibios;

    /// <summary>
    /// GameObject representing the UI element that
    /// displays the score for insect-related games.
    /// </summary>
    public GameObject PuntuacionInsectos;

    /// <summary>
    /// GameObject representing the UI element that
    /// displays the score for mammal-related games.
    /// </summary>
    public GameObject PuntuacionMamiferos;

    /// <summary>
    /// GameObject representing the UI element that
    /// displays the score for plant-related games.
    /// </summary>
    public GameObject PuntuacionPlantas;

    /// <summary>
    /// Array of sprites used to indicate different 
    /// star ratings for game completion.
    /// </summary>
    public Sprite[] Estrellas;

    /// <summary>
    /// GameObject that is activated when the training
    /// or tutorial is completed.
    /// </summary>
    public GameObject FinalizacionCapacitacion;

    /// <summary>
    /// Integer variable to track the number of games
    /// or levels that have been completed.
    /// </summary>
    private int juegoscompletados = 0;


    private void Awake()
    {
        StartCoroutine(GetPoints());
    }

    /// <summary>
    /// Retrieves the points for each minigame from the API and processes the results.
    /// Initializes the number of completed games and sends a request to get the scores.
    /// </summary>
    /// <returns>An IEnumerator for coroutine support.</returns>
    public IEnumerator GetPoints()
    {
        // Initialize the number of completed games to zero
        juegoscompletados = 0;

        // Retrieve the user ID from PlayerPrefs
        int userID = PlayerPrefs.GetInt("UserID");
        // Construct the URL for the API request
        string JSONurl = "https://10.22.156.99:7026/api/Videogame/MinigameScores/" + userID;

        // Send the request to the API and process the response
        yield return StartCoroutine(SendRequest(JSONurl));
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URL and handles the response.
    /// </summary>
    /// <param name="url">The URL for the API request.</param>
    /// <returns>An IEnumerator for coroutine support.</returns>
    private IEnumerator SendRequest(string url)
    {
        // Create a UnityWebRequest to send an HTTP GET request
        UnityWebRequest web = UnityWebRequest.Get(url);
        web.useHttpContinue = true;

        // Create a custom certificate handler to accept all certificates
        var cert = new ForceAceptAll();
        web.certificateHandler = cert;
        cert?.Dispose();

        // Send the web request and wait for the response
        yield return web.SendWebRequest();

        // Check if the request was successful
        if (web.result != UnityWebRequest.Result.Success)
        {
            // Log any errors from the request
            UnityEngine.Debug.Log("Error API: " + web.error);
        }
        else
        {
            // Process the successful response
            ProcessResponse(web.downloadHandler.text);
        }
    }

    /// <summary>
    /// Processes the JSON response from the API and updates the UI with the scores.
    /// </summary>
    /// <param name="jsonResponse">The JSON response string from the API.</param>
    private void ProcessResponse(string jsonResponse)
    {
        // Deserialize the JSON response into a list of Points objects
        List<Points> pointsList = JsonConvert.DeserializeObject<List<Points>>(jsonResponse);

        // Iterate through each Points object and update the UI
        foreach (Points minigamePoints in pointsList)
        {
            // Update the UI based on the minigame and score
            UpdateUI(minigamePoints);
            // Increment the completed games count if the score is 80 or above
            if (minigamePoints.score >= 80)
            {
                juegoscompletados += 1;
            }
        }

        // Check if the training is completed based on the number of games completed
        CheckCompletion();
    }

    /// <summary>
    /// Updates the UI elements based on the minigame points and score.
    /// </summary>
    /// <param name="minigamePoints">The Points object containing minigame information and score.</param>
    private void UpdateUI(Points minigamePoints)
    {
        // Retrieve the minigame name
        string minijuego = minigamePoints.minigame;

        // Update the corresponding UI element based on the minigame name
        switch (minijuego)
        {
            case "Aves":
                ImagePuntuacion(minigamePoints.score, PuntuacionAves);
                break;
            case "Anfibios":
                ImagePuntuacion(minigamePoints.score, PuntuacionAnfibios);
                break;
            case "Insectos":
                ImagePuntuacion(minigamePoints.score, PuntuacionInsectos);
                break;
            case "Flora":
                ImagePuntuacion(minigamePoints.score, PuntuacionPlantas);
                break;
            case "Mamíferos":
                ImagePuntuacion(minigamePoints.score, PuntuacionMamiferos);
                break;
        }

        // Log the points and minigame name for debugging purposes
        Debug.Log("puntos " + minigamePoints.score + " minijuego " + minijuego);
    }

    /// <summary>
    /// Checks if the training is completed based on the number of games with a score of 80 or above.
    /// If completed, activates the finalization training UI element.
    /// </summary>
    private void CheckCompletion()
    {
        // If 5 or more games are completed with a score of 80 or above, activate the finalization UI element
        if (juegoscompletados >= 5)
        {
            FinalizacionCapacitacion.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the sprite of the given GameObject based on the score provided.
    /// The sprite represents the rating or performance level.
    /// </summary>
    /// <param name="score">The score to determine the rating level.</param>
    /// <param name="Puntuacion">The GameObject whose sprite will be updated based on the score.</param>
    private void ImagePuntuacion(int score, GameObject Puntuacion)
    {
        // Log the score for debugging purposes
        Debug.Log(score);

        // Determine the appropriate sprite based on the score
        // Update the sprite of the Puntuacion GameObject based on score thresholds
        if (score >= 90)
        {
            // Assign the sprite for the highest rating (5 stars) if score is 90 or above
            Puntuacion.GetComponent<SpriteRenderer>().sprite = Estrellas[4];
        }
        else if (score >= 75)
        {
            // Assign the sprite for the 4-star rating if score is between 75 and 89
            Puntuacion.GetComponent<SpriteRenderer>().sprite = Estrellas[3];
        }
        else if (score >= 50)
        {
            // Assign the sprite for the 3-star rating if score is between 50 and 74
            Puntuacion.GetComponent<SpriteRenderer>().sprite = Estrellas[2];
        }
        else if (score >= 25)
        {
            // Assign the sprite for the 2-star rating if score is between 25 and 49
            Puntuacion.GetComponent<SpriteRenderer>().sprite = Estrellas[1];
        }
        else
        {
            // Assign the sprite for the lowest rating (1 star) if score is below 25
            Puntuacion.GetComponent<SpriteRenderer>().sprite = Estrellas[0];
        }
    }

    /// <summary>
    /// Updates the information for a specific minigame record in the database.
    /// This includes the user ID, minigame ID, points, and time taken.
    /// </summary>
    /// <param name="matchid">The unique identifier for the minigame record.</param>
    /// <param name="userid">The unique identifier for the user who played the minigame.</param>
    /// <param name="minigameid">The unique identifier for the minigame.</param>
    /// <param name="puntos">The points scored by the user in the minigame.</param>
    /// <param name="tiempo">The time taken by the user to complete the minigame, in seconds.</param>
    /// <returns>An IEnumerator for coroutine support.</returns>
    public IEnumerator UpdateMinigameInfo(int matchid, int userid, int minigameid, int puntos, int tiempo)
    {
        // Construct the URL for the API endpoint using the match ID
        string minigameURL = "https://10.22.156.99:7026/api/Videogame/Minigame/" + matchid.ToString();

        // Create an anonymous object with the data to be sent
        var minigameData = new
        {
            userID = userid,
            minigameID = minigameid,
            points = puntos,
            time = tiempo
        };

        // Serialize the data to a JSON string using Newtonsoft.Json
        string json = JsonConvert.SerializeObject(minigameData);
        Debug.Log("Sending " + json + " to: " + minigameURL);

        // Create a UnityWebRequest for the PUT operation
        UnityWebRequest webRequest = UnityWebRequest.Put(minigameURL, json);
        // Set the content type header to JSON
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Use a custom certificate handler if needed
        webRequest.certificateHandler = new ForceAceptAll();

        // Send the web request and wait for it to complete
        yield return webRequest.SendWebRequest();

        // Dispose of the certificate handler if it was created
        webRequest.certificateHandler?.Dispose();

        // Check if the request was successful
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            // Log any errors encountered during the request
            Debug.Log("Error en el registro: " + webRequest.error);
        }
    }
}
