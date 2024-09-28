// **********************************************************************
// Script Name: BirdBehaviour.cs
// Description: 
//      * The BirdBehavior script controls the movement of the
//      * bird by setting its direction and speed. It determines 
//      * the bird's trajectory based on its initial position and
//      * ensures that the bird remains within defined boundaries. 
//      * When the bird goes out of bounds, it handles the bird's
//      * destruction and updates the user interface and game state accordingly. 
//      * The script also manages interactions such as playing
//      * sound effects, showing UI elements, and handling camera switches 
//      * based on the game's round number. Additionally, the
//      * script logs errors related to mistakes in bird detection through 
//      * a POST request to a server, providing error records
//      * for tracking purposes.
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


using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BirdBehavior : MonoBehaviour
{
    /// <summary>
    /// The unique identifier for the bird.
    /// </summary>
    public int id;

    /// <summary>
    /// The speed at which the bird moves.
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// The direction in which the bird is moving.
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// The maximum x-coordinate of the bird's movement boundary.
    /// </summary>
    public float maxX;

    /// <summary>
    /// The maximum y-coordinate of the bird's movement boundary.
    /// </summary>
    public float maxY;

    /// <summary>
    /// The minimum y-coordinate of the bird's movement boundary.
    /// </summary>
    public float minY;

    /// <summary>
    /// The minimum x-coordinate of the bird's movement boundary.
    /// </summary>
    public float minX;

    /// <summary>
    /// Delegate used for the bird destroyed event.
    /// </summary>
    public delegate void BirdDestroyed(BirdBehavior bird);

    /// <summary>
    /// Event triggered when the bird is destroyed.
    /// </summary>
    public static event BirdDestroyed OnBirdDestroyed;

    /// <summary>
    /// Indicates whether the bird has been detected.
    /// </summary>
    private bool isDetected = false;


    /// <summary>
    /// Initializes the bird's direction when the game starts.
    /// </summary>
    /// <remarks>
    /// This method is called when the script is first run. It sets a random direction for the bird's movement
    /// by calling the SetRandomDirection method.
    /// </remarks>
    void Start()
    {
        // Set a random direction for the bird's movement
        SetRandomDirection();
    }

    /// <summary>
    /// Sets a random direction for the bird's movement based on its initial X position.
    /// </summary>
    /// <remarks>
    /// This method determines the bird's movement direction and visual orientation based on its initial
    /// position along the X-axis. It generates a random vertical angle to add variability to the bird's flight.
    /// The bird's direction is set to move either to the right or to the left depending on its starting position.
    /// Additionally, the bird's scale is adjusted to match its movement direction for visual consistency.
    /// </remarks>
    void SetRandomDirection()
    {
        // Define the range of vertical angles in degrees
        float grados1 = 10f, grados2 = 60f;

        // Convert angles from degrees to radians
        float radianes1 = grados1 * Mathf.Deg2Rad, radianes2 = grados2 * Mathf.Deg2Rad;

        // Generate a random vertical angle within the specified range
        float randomVerticalAngle = Random.Range(radianes1, radianes2);

        // Determine the initial movement direction based on the bird's X position
        if (transform.position.x >= -9f && transform.position.x <= 0f)
        {
            // If the bird starts between -9 and 0 on the X-axis, set direction to the right
            direction = new Vector2(1f, randomVerticalAngle).normalized;
            // Adjust the bird's scale to face the right
            transform.localScale = new Vector3(5f, transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x >= 20f && transform.position.x <= 30f)
        {
            // If the bird starts between 20 and 30 on the X-axis, set direction to the left
            direction = new Vector2(-1f, randomVerticalAngle).normalized;
            // Adjust the bird's scale to face the left
            transform.localScale = new Vector3(-5f, transform.localScale.y, transform.localScale.z);
        }
    }


    /// <summary>
    /// Updates the bird's movement and checks for boundary conditions every frame.
    /// </summary>
    /// <remarks>
    /// This method is called once per frame. It moves the bird according to its current direction and speed,
    /// and checks if the bird has gone out of bounds to handle its destruction and other related actions.
    /// </remarks>
    void Update()
    {
        // Move the bird based on its direction and speed
        MoveBird();

        // Check if the bird is outside the movement boundaries and handle its destruction if needed
        CheckBoundariesAndHandleDestruction();
    }


    /// <summary>
    /// Moves the bird based on its current direction and speed.
    /// </summary>
    /// <remarks>
    /// This method updates the bird's position by translating it in the direction specified by the `direction`
    /// vector, scaled by the `speed` and the elapsed time since the last frame (using `Time.deltaTime`).
    /// </remarks>
    private void MoveBird()
    {
        // Move the bird based on its direction and speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Checks if the bird is out of the defined movement boundaries and handles its destruction if needed.
    /// </summary>
    /// <remarks>
    /// This method verifies whether the bird has moved outside the allowed movement area by calling the
    /// IsOutOfBounds method. If the bird is out of bounds, it then calls HandleBirdDestruction to
    /// manage the bird's removal and any associated actions.
    /// </remarks>
    private void CheckBoundariesAndHandleDestruction()
    {
        // Check if the bird is out of the movement boundaries
        if (IsOutOfBounds())
        {
            // Handle the destruction of the bird if it is out of bounds
            HandleBirdDestruction();
        }
    }

    /// <summary>
    /// Determines if the bird is outside the defined movement boundaries.
    /// </summary>
    /// <returns>True if the bird's position is outside the boundaries; otherwise, false.</returns>
    /// <remarks>
    /// This method checks the bird's current position against the defined movement boundaries (maxX, minX, maxY, minY).
    /// It returns true if the bird's position is outside any of these boundaries, indicating that the bird is out of bounds.
    /// </remarks>
    private bool IsOutOfBounds()
    {
        // Determine if the bird is outside the defined movement boundaries
        return transform.position.x > maxX || transform.position.x < minX ||
               transform.position.y > maxY || transform.position.y < minY;
    }

    /// <summary>
    /// Handles the destruction of the bird and updates the UI and game state accordingly.
    /// </summary>
    /// <remarks>
    /// This method is called when the bird is out of bounds. It destroys the bird's game object,
    /// updates the user interface by showing relevant elements, and checks if the bird was detected.
    /// If the bird was not detected, it handles additional actions specific to undetected birds.
    /// It also manages the camera switch if applicable.
    /// </remarks>
    private void HandleBirdDestruction()
    {
        // Destroy the bird game object
        Destroy(gameObject);

        // Show the appropriate UI elements
        UIController.instance.showButton();

        // Check if the bird was detected
        if (!avesController.instance.getDetected())
        {
            // Handle additional actions for undetected birds
            HandleUndetectedBird();
        }

        // Handle camera switch if applicable
        HandleCameraSwitch();
    }

    /// <summary>
    /// Handles the actions related to an undetected bird.
    /// </summary>
    /// <remarks>
    /// This method updates the user interface and game state for a bird that was not detected.
    /// It shows additional UI elements specific to undetected birds, plays a sound effect indicating an error,
    /// and creates an error record for tracking purposes.
    /// </remarks>
    private void HandleUndetectedBird()
    {
        // Show additional UI elements for an undetected bird
        UIController.instance.showButton();
        UIController.instance.showAvesBird();

        // Play incorrect sound effect
        SFXManagerBirds.instance.playInCorrect();

        // Create an error record
        int matchID = PlayerPrefs.GetInt("MatchID");
        int avesMistakeID = 5;
        StartCoroutine(CreateError(matchID, avesMistakeID));
    }

    /// <summary>
    /// Handles the camera switch based on the current game round.
    /// </summary>
    /// <remarks>
    /// This method checks if the camera switch instance is available and, if so, determines
    /// if the current round is less than 3. If these conditions are met, it triggers the camera
    /// switch by calling ShowReadyC on the SwitchCamera instance. A debug message is logged to indicate
    /// the camera switch operation.
    /// </remarks>
    private void HandleCameraSwitch()
    {
        // Check if the SwitchCamera instance is available
        if (SwitchCamera.instance != null)
        {
            // Get the current round number
            int round = avesController.instance.getRound();

            // If the current round is less than 3, perform the camera switch
            if (round < 3)
            {
                Debug.Log("LLegue");
                SwitchCamera.instance.ShowReadyC();
            }
        }
    }

    /// <summary>
    /// Sends a POST request to log an error related to a specific match and mistake ID.
    /// </summary>
    /// <param name="matchID">The ID of the match in which the mistake occurred.</param>
    /// <param name="mistakeID">The ID of the specific mistake that needs to be logged.</param>
    /// <returns>An IEnumerator for coroutine handling in Unity.</returns>
    /// <remarks>
    /// This coroutine creates a JSON object with matchID and mistakeID, and sends it to a specified URL using
    /// UnityWebRequest. It handles the request asynchronously and logs an error message if the request fails.
    /// </remarks>
    public IEnumerator CreateError(int matchID, int mistakeID)
    {
        // URL of the API endpoint to post the error data
        string minigameURL = "https://10.22.156.99:7026/api/Videogame/Mistakes";

        // Create an anonymous object to hold the mistake data
        var mistakesData = new
        {
            matchID = matchID,
            mistakeID = mistakeID,
        };

        // Serialize the mistake data to JSON format
        string json = JsonConvert.SerializeObject(mistakesData);

        // Create a UnityWebRequest for sending the POST request
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(minigameURL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Allow all certificates (use with caution in production)
        webRequest.certificateHandler = new ForceAceptAll();

        // Send the request and wait for a response
        yield return webRequest.SendWebRequest();

        // Dispose of the certificate handler
        webRequest.certificateHandler?.Dispose();

        // Check if the request was successful
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            // Log an error message if the request failed
            Debug.Log("Error en el registro");
        }
    }

    public int getId()
    {
        return id;
    }

    public bool GetDetected()
    {
        return isDetected;
    }

    public void SetDetected()
    {
        isDetected = true;
    }
}