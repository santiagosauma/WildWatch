// **********************************************************************
// Script Name: UIController.cs
// Description:
//      * This script manages the user interface and game
//      * logic for a minigame involving birds. It includes
//      * functionality for updating and displaying scores,
//      * rounds, and bird information. It also handles
//      * showing and hiding various UI elements and managing
//      * the game's state.
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 21/07/2024 
// **********************************************************************



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// Text component to display the current points.
    /// </summary>
    public Text points;

    /// <summary>
    /// Text component to display the current round.
    /// </summary>
    public Text round;

    /// <summary>
    /// Tracks the elapsed time.
    /// </summary>
    private int time = 0;

    /// <summary>
    /// GameObject representing a secondary UI.
    /// </summary>
    public GameObject UI2;

    /// <summary>
    /// Button component for user interaction.
    /// </summary>
    public Button button;

    /// <summary>
    /// Image component to display warning messages.
    /// </summary>
    public Image warning;

    /// <summary>
    /// GameObject representing an unseen (hidden) element.
    /// </summary>
    public GameObject novisto;

    /// <summary>
    /// Tracks the game's current points.
    /// </summary>
    int pointsGame = 0;

    /// <summary>
    /// Tracks the game's current round.
    /// </summary>
    int roundGame = 0;

    /// <summary>
    /// Tracks the current selection index.
    /// </summary>
    int selection = 0;

    /// <summary>
    /// Singleton instance of the UIController class.
    /// </summary>
    public static UIController instance;

    /// <summary>
    /// GameObject representing a UI text element.
    /// </summary>
    public GameObject uiText;

    /// <summary>
    /// GameObject representing the end screen UI.
    /// </summary>
    public GameObject uiEnd;

    /// <summary>
    /// Text component to display the final score on the end screen.
    /// </summary>
    public Text PuntuacionEnd;

    /// <summary>
    /// Text component to display the final count of animals on the end screen.
    /// </summary>
    public Text animalesEnd;


    /// <summary>
    /// Ends the minigame, displaying the final score and caught animals count, 
    /// and saves the elapsed time to PlayerPrefs. Updates the end screen UI accordingly.
    /// </summary>
    void showEndMinigame()
    {
        // Stop the coroutine that counts the time.
        StopCoroutine(contarTiempo());

        // Display the final score.
        PuntuacionEnd.text = $"Puntuación Obtenida: {pointsGame}";

        // Display the count of animals caught, retrieving the value from PlayerPrefs.
        animalesEnd.text = $"Animales Atrapados: {PlayerPrefs.GetInt("avesNumber")}/3";

        // Save the elapsed time to PlayerPrefs.
        PlayerPrefs.SetInt("TiempoJuego", time);

        // Show the end screen UI.
        uiEnd.SetActive(true);

        // Hide the in-game UI.
        uiText.SetActive(false);
    }

    /// <summary>
    /// Unity's Awake method to initialize the singleton instance of the UIController class.
    /// Ensures that only one instance of UIController exists in the scene.
    /// </summary>
    void Awake()
    {
        // If no instance exists, set this as the instance.
        if (instance == null)
        {
            instance = this;
        }
        // If an instance already exists and it is not this one, destroy this game object.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unity's Start method to initialize game settings and UI elements at the beginning of the game.
    /// </summary>
    void Start()
    {
        // Initialize the game round to 1.
        roundGame = 1;

        // Reset the number of birds caught to 0 in PlayerPrefs.
        PlayerPrefs.SetInt("avesNumber", 0);

        // Initialize the game points to 0.
        pointsGame = 0;

        // Hide the warning image.
        warning.gameObject.SetActive(false);

        // Hide the secondary UI.
        UI2.SetActive(false);

        // Hide the button.
        button.gameObject.SetActive(false);

        // Hide the end screen UI.
        uiEnd.SetActive(false);

        // Hide the unseen (hidden) element.
        novisto.SetActive(false);
    }


    /// <summary>
    /// Handles the logic for processing the user's selection. 
    /// Updates UI elements, plays sounds, and manages game state based on the user's selection and the current round.
    /// </summary>
    public void send()
    {
        // Check if a valid selection has been made (selection is not 0).
        if (selection != 0)
        {
            // Hide the button and secondary UI elements.
            hideButton();
            UI2.SetActive(false);
            warning.gameObject.SetActive(false);

            // Show the continue screen if the current round is less than 3.
            if (roundGame < 3)
            {
                SwitchCamera.instance.showContinue();
            }

            // Check if the selected bird matches the correct bird type.
            if (selection - 1 == PlayerPrefs.GetInt("birdType"))
            {
                // Increase the points and display the points animation.
                avesController.instance.increasePoints();
                Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
                avesController.instance.ShowPointsAnimation(cameraCenter, false);

                // Play the correct action sound.
                SFXManagerBirds.instance.playCorrect();
            }

            // Log the current round for debugging purposes.
            Debug.Log(roundGame);

            // Reset the selection.
            selection = 0;

            // If the current round is 3, show the end of minigame screen.
            if (roundGame == 3)
            {
                showEndMinigame();
            }
        }
        else
        {
            // Show the warning image if no valid selection was made.
            warning.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the game score and the corresponding UI text.
    /// </summary>
    /// <param name="newScore">The new score to be displayed.</param>
    public void UpdateScore(int newScore)
    {
        // Update the internal score value.
        pointsGame = newScore;

        // Update the points text UI element with the new score.
        points.text = "Puntuación: " + newScore.ToString();
    }

    /// <summary>
    /// Updates the current round and the corresponding UI text.
    /// </summary>
    /// <param name="newRound">The new round number to be displayed.</param>
    public void UpdateRound(int newRound)
    {
        // Update the internal round value.
        roundGame = newRound;

        // Update the round text UI element with the new round number.
        round.text = "Ronda: " + newRound.ToString() + "/3";
    }

    /// <summary>
    /// Activates the secondary UI and hides the continue button.
    /// </summary>
    public void OpenUi()
    {
        // Show the secondary UI.
        UI2.SetActive(true);

        // Hide the button.
        button.gameObject.SetActive(false);

        // Hide the continue screen in the camera switch.
        SwitchCamera.instance.hideContinue();
    }

    /// <summary>
    /// Starts a coroutine to display the bird information.
    /// </summary>
    public void showAvesBird()
    {
        // Begin the coroutine to show bird information.
        StartCoroutine(showBird());
    }

    /// <summary>
    /// Coroutine to display the hidden element for a short duration.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator showBird()
    {
        // Show the hidden element.
        novisto.SetActive(true);

        // Wait for 3 seconds.
        yield return new WaitForSeconds(3);

        // Hide the hidden element.
        novisto.SetActive(false);
    }

    /// <summary>
    /// Deactivates the secondary UI, shows the button, hides the warning image, 
    /// and displays the continue screen in the camera switch.
    /// </summary>
    public void CloseUi()
    {
        // Hide the secondary UI.
        UI2.SetActive(false);

        // Show the button.
        button.gameObject.SetActive(true);

        // Hide the warning image.
        warning.gameObject.SetActive(false);

        // Show the continue screen in the camera switch.
        SwitchCamera.instance.showContinue();
    }


    public void showButton()
    {
        button.gameObject.SetActive(true);
    }

    public void hideButton()
    {
        button.gameObject.SetActive(false);
    }

    void Update()
    {
        StartCoroutine(contarTiempo());
    }
    public void bj()
    {
        selection = 1;
    }

    public void eagle()
    {
        selection = 2;
    }

    public void bRB()
    {
        selection = 3;
    }

    public void robin()
    {
        selection = 4;
    }

    public void sparr()
    {
        selection = 5;
    }

    

    private IEnumerator contarTiempo()
    {
        yield return new WaitForSeconds(1f);
        time++;
    }    

    

}
