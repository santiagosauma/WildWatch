// **********************************************************************
// Script Name: avesController.cs
// Description:
//      * Controls the behavior of bird detection,
//      * score updating, and scene transitions.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class avesController : MonoBehaviour
{
    /// <summary>
    /// Text UI element to display obtained animation points.
    /// </summary>
    public Text obtainAnim;

    /// <summary>
    /// Main camera used to check if birds are in view.
    /// </summary>
    public Camera Camera;

    /// <summary>
    /// Top-left corner of the viewing area.
    /// </summary>
    public Transform topLeft;

    /// <summary>
    /// Top-right corner of the viewing area.
    /// </summary>
    public Transform topRight;

    /// <summary>
    /// Bottom-left corner of the viewing area.
    /// </summary>
    public Transform bottomLeft;

    /// <summary>
    /// Bottom-right corner of the viewing area.
    /// </summary>
    public Transform bottomRight;

    /// <summary>
    /// UI Controller for updating the score display.
    /// </summary>
    public UIController uiController;

    /// <summary>
    /// Singleton instance of the avesController class.
    /// </summary>
    public static avesController instance;

    /// <summary>
    /// Indicates whether a bird has been detected.
    /// </summary>
    private bool detected;

    /// <summary>
    /// List of available bird information.
    /// </summary>
    public List<BirdInfo> Disponibles = new List<BirdInfo>();

    /// <summary>
    /// List of discovered bird information.
    /// </summary>
    private List<BirdInfo> Descubiertos = new List<BirdInfo>();

    /// <summary>
    /// The number of birds detected by the player.
    /// </summary>
    int avesDetected = 0;

    /// <summary>
    /// The player's current score points.
    /// </summary>
    int points = 0;

    /// <summary>
    /// The current round number in the game.
    /// </summary>
    int round = 1;

    /// <summary>
    /// Tracks the number of unique bird detections.
    /// </summary>
    int uniqueRegisters;

    /// <summary>
    /// Array indicating whether each bird has already been added.
    /// </summary>
    /// <remarks>
    /// The array size should match the number of different bird IDs.
    /// </remarks>
    bool[] alreadyAdded = new bool[5];

    /// <summary>
    /// Reference to the ListaAnimales class containing animal data.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Reference to the AnswersData class that manages answer records.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// Text UI element to display the points gained from game animations.
    /// </summary>
    public Text gameAnimationPoints;

    /// <summary>
    /// Text UI element to display a message when no birds are detected.
    /// </summary>
    public Text noBird;

    public int getRound()
    {
        return round;
    }

    public int getPoints()
    {
        return points;
    }

    public void setDetected()
    {
        detected = false;
    }

    public bool getDetected()
    {
        return detected;
    }

    public List<BirdInfo> returnDescubiertos()
    {
        return Descubiertos;
    }
    public void increaseRound()
    {
        round++;
    }

    /// <summary>
    /// Unity method called when the script instance is being loaded.
    /// </summary>
    /// <remarks>
    /// Sets up the singleton pattern for this class and subscribes to the
    /// BirdBehavior.OnBirdDestroyed event to handle bird destruction events.
    /// </remarks>
    void Awake()
    {
        // Subscribe to the OnBirdDestroyed event
        BirdBehavior.OnBirdDestroyed += HandleBirdDestruction;

        // Check if this is the only instance of the class
        if (instance == null)
        {
            instance = this; // Set this instance as the singleton instance
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it is not the singleton
        }
    }

    /// <summary>
    /// Unity method called before the first frame update.
    /// </summary>
    /// <remarks>
    /// Initializes the state of UI elements by hiding the obtainAnim and noBird text elements.
    /// </remarks>
    void Start()
    {
        // Hide the obtainAnim text element
        obtainAnim.gameObject.SetActive(false);

        // Hide the noBird text element
        noBird.gameObject.SetActive(false);
    }

    /// <summary>
    /// Unity method called once per frame.
    /// </summary>
    /// <remarks>
    /// Searches for all game objects with the tag "Bird" and processes each bird
    /// to check for detection using the HandleBirdDetection method.
    /// </remarks>
    private void Update()
    {
        // Find all game objects with the tag "Bird"
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");

        // Process each bird to check for detection
        foreach (GameObject bird in birds)
        {
            HandleBirdDetection(bird);
        }
    }

    /// <summary>
    /// Handles the detection of a bird by processing its position and updating the game state.
    /// </summary>
    /// <param name="bird">The bird GameObject to be processed.</param>
    /// <remarks>
    /// Checks if the bird is within the viewing area and has not been previously detected.
    /// If the bird meets the criteria, updates the detection status, score, and relevant game data.
    /// Additionally, it triggers UI updates and sound effects.
    /// </remarks>
    private void HandleBirdDetection(GameObject bird)
    {
        // Check if the bird is in view and has not been detected yet
        if (IsBirdInView(bird.transform.position) && !detected)
        {
            detected = true; // Mark the bird as detected
            UpdateScore();   // Update the player's score
            UpdateBirdDetection(bird); // Handle specific detection logic for the bird
            ShowSaveDataBirds.instance.ShowInit(); // Update UI to reflect the save data
            SFXManagerBirds.instance.playCorrect(); // Play sound effect for correct detection
        }
    }

    /// <summary>
    /// Updates the player's score and the number of detected birds.
    /// </summary>
    /// <remarks>
    /// Increases the player's points by 24, clamps the score to a maximum of 100,
    /// increments the count of detected birds, and updates the saved score and UI display.
    /// </remarks>
    private void UpdateScore()
    {
        // Increase the player's points by 24
        points += 24;

        // Clamp the points to a maximum value of 100
        if (points > 100) points = 100;

        // Increment the count of detected birds
        avesDetected++;

        // Save the number of detected birds to player preferences
        PlayerPrefs.SetInt("avesNumber", avesDetected);

        // Update the UI with the new score
        uiController.UpdateScore(points);
    }

    /// <summary>
    /// Processes bird detection by updating the detection status for unique and non-unique birds.
    /// </summary>
    /// <param name="bird">The bird GameObject to be processed.</param>
    /// <remarks>
    /// Displays an animation for the detected bird and determines whether it is a new or previously detected bird.
    /// Calls the appropriate methods to handle the detection, updating the list of discovered birds or
    /// incrementing the count for previously detected birds.
    /// </remarks>
    private void UpdateBirdDetection(GameObject bird)
    {
        // Display an animation showing points for the detected bird
        ShowPointsAnimation(bird.transform.position, true);

        // Get the unique ID of the bird
        int birdID = BirdSpawner.Instance.getIdBird();

        // Check if the bird is a new discovery
        if (!Descubiertos.Contains(Disponibles[birdID]))
        {
            // Handle detection of a new bird
            AddNewBirdDetection(birdID);
        }
        else
        {
            // Handle detection of a previously discovered bird
            IncrementBirdDetection(birdID);
        }
    }

    /// <summary>
    /// Handles the detection of a new bird by updating relevant lists and records.
    /// </summary>
    /// <param name="birdID">The unique ID of the detected bird.</param>
    /// <remarks>
    /// Adds the detected bird to the list of discovered birds and the list of animals.
    /// Increments the count of unique bird detections and updates the answer records if the bird
    /// has not been previously added. Sets the bird as added and creates a new answer record for it.
    /// </remarks>
    private void AddNewBirdDetection(int birdID)
    {
        // Add the new bird to the list of discovered birds
        Descubiertos.Add(Disponibles[birdID]);

        // Add the new bird to the list of animals
        animales.Pajaros.Add(Disponibles[birdID]);

        // Increment the count of unique bird detections
        uniqueRegisters++;

        // Check if the bird has not been previously added
        if (!alreadyAdded[birdID])
        {
            // Mark the bird as added
            alreadyAdded[birdID] = true;

            // Create a new answer record for the bird
            Answer answer = new Answer
            {
                imgId = birdID,
                idEspecie = birdID,
                minigameId = 0,
                quantity = 1
            };

            // Add the new answer record to the answers data
            AnswersData.answers.Add(answer);
        }
    }

    /// <summary>
    /// Increments the detection count for a previously discovered bird.
    /// </summary>
    /// <param name="birdID">The unique ID of the detected bird.</param>
    /// <remarks>
    /// Searches through the list of answer records to find the record associated with the given bird ID.
    /// Increases the quantity for that record by 1 to reflect the additional detection of the bird.
    /// </remarks>
    private void IncrementBirdDetection(int birdID)
    {
        // Iterate through the list of answer records
        for (int i = 0; i < AnswersData.answers.Count; i++)
        {
            // Check if the current record corresponds to the detected bird
            if (AnswersData.answers[i].idEspecie == birdID)
            {
                // Increment the quantity for the detected bird
                AnswersData.answers[i].quantity += 1;
            }
        }
    }

    /// <summary>
    /// Unity method called when the MonoBehaviour is being destroyed.
    /// </summary>
    /// <remarks>
    /// Unsubscribes from the BirdBehavior.OnBirdDestroyed event to prevent memory leaks
    /// and ensure that no callbacks are made to a destroyed instance.
    /// </remarks>
    void OnDestroy()
    {
        // Unsubscribe from the OnBirdDestroyed event
        BirdBehavior.OnBirdDestroyed -= HandleBirdDestruction;
    }

    /// <summary>
    /// Handles the event when a bird is destroyed.
    /// </summary>
    /// <param name="bird">The BirdBehavior instance representing the destroyed bird.</param>
    /// <remarks>
    /// Checks if the destroyed bird was not detected. If the bird was not detected,
    /// initiates a coroutine to show a "no bird" message to the player.
    /// </remarks>
    private void HandleBirdDestruction(BirdBehavior bird)
    {
        // Check if the destroyed bird was not detected
        if (!bird.GetDetected())
        {
            // Start a coroutine to display a "no bird" message
            StartCoroutine(ShowNoBirdMessage());
        }
    }

    /// <summary>
    /// Coroutine to display a "no bird" message for a short duration.
    /// </summary>
    /// <remarks>
    /// Activates the noBird text UI element to show the message, waits for 3 seconds,
    /// and then deactivates the noBird text UI element to hide the message.
    /// </remarks>
    IEnumerator ShowNoBirdMessage()
    {
        // Show the "no bird" message
        noBird.gameObject.SetActive(true);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3);

        // Hide the "no bird" message
        noBird.gameObject.SetActive(false);
    }


    /// <summary>
    /// Initiates the animation for additional points based on whether the bird was seen or not.
    /// </summary>
    /// <param name="birdPosition">The position of the bird on the screen where the animation will be shown.</param>
    /// <param name="isSeen">Indicates whether the bird was seen (true) or not (false).</param>
    /// <remarks>
    /// Sets the text of the gameAnimationPoints UI element to display the appropriate points value
    /// based on whether the bird was seen or not. Starts a coroutine to handle the animation of the points.
    /// </remarks>
    public void ShowPointsAnimation(Vector2 birdPosition, bool isSeen)
    {
        // Set the points text based on whether the bird was seen or not
        if (isSeen)
            gameAnimationPoints.text = "+24";
        else
            gameAnimationPoints.text = "+10";

        // Start the coroutine to animate the points at the given bird position
        StartCoroutine(PointsAnimation(birdPosition));
    }

    /// <summary>
    /// Coroutine to animate the display of additional points at the given bird position.
    /// </summary>
    /// <param name="birdPosition">The position of the bird on the screen where the animation will be shown.</param>
    /// <remarks>
    /// Activates the obtainAnim text UI element to display the points animation,
    /// waits for 1 second to allow the animation to be seen, and then deactivates
    /// the obtainAnim text UI element to hide it.
    /// </remarks>
    IEnumerator PointsAnimation(Vector3 birdPosition)
    {
        // Activate the text UI element to show the points animation
        obtainAnim.gameObject.SetActive(true);

        // Wait for 1 second to allow the animation to be visible
        yield return new WaitForSeconds(1f);

        // Deactivate the text UI element to hide the points animation
        obtainAnim.gameObject.SetActive(false);
    }

    /// <summary>
    /// Increases the player's score by 10 points and updates the UI display.
    /// </summary>
    /// <remarks>
    /// Adds 10 points to the current score and calls the UpdateScore method on the uiController
    /// to refresh the score display in the user interface.
    /// </remarks>
    public void increasePoints()
    {
        // Increase the player's score by 10
        points += 10;

        // Update the UI with the new score
        uiController.UpdateScore(points);
    }

    /// <summary>
    /// Determines if a bird is within the viewable area defined by screen corners.
    /// </summary>
    /// <param name="birdPosition">The position of the bird to be checked.</param>
    /// <returns>True if the bird's position is within the viewable area; otherwise, false.</returns>
    /// <remarks>
    /// Computes the minimum and maximum x and y coordinates of the viewable area based on
    /// the positions of the screen corners. Checks if the bird's position falls within these bounds.
    /// </remarks>
    bool IsBirdInView(Vector2 birdPosition)
    {
        // Calculate the minimum and maximum x coordinates of the viewable area
        float minX = Mathf.Min(topLeft.position.x, bottomLeft.position.x);
        float maxX = Mathf.Max(topRight.position.x, bottomRight.position.x);

        // Calculate the minimum and maximum y coordinates of the viewable area
        float minY = Mathf.Min(bottomLeft.position.y, bottomRight.position.y);
        float maxY = Mathf.Max(topLeft.position.y, topRight.position.y);

        // Check if the bird's position is within the viewable area
        return birdPosition.x >= minX && birdPosition.x <= maxX && birdPosition.y >= minY && birdPosition.y <= maxY;
    }


    /// <summary>
    /// Saves the final score and unique register count, then loads the home scene.
    /// </summary>
    /// <remarks>
    /// Stores the current score and the number of unique bird detections in player preferences,
    /// and then switches to the "HouseScene" to return to the home screen.
    /// </remarks>
    public void goHomeEnd()
    {
        // Save the final score to player preferences
        PlayerPrefs.SetFloat("PuntuacionFinal", points);

        // Save the number of unique bird detections to player preferences
        PlayerPrefs.SetInt("RegisterNum", uniqueRegisters);

        // Load the home scene
        SceneManager.LoadScene("HouseScene");
    }


}
