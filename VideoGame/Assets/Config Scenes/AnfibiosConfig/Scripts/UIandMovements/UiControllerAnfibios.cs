// **********************************************************************
// Script Name: UIControllerAnfibios.cs
// Description:
//      * This script manages the user interface (UI) for a
//      * mini-game where the player captures animals within a
//      * time limit. It handles the display of the timer,
//      * score, and the end-of-game UI.
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
using UnityEngine.UI;

public class UiControllerAnfibios : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the UiControllerAnfibios class.
    /// Used to access UI control methods from other scripts.
    /// </summary>
    public static UiControllerAnfibios instance;

    /// <summary>
    /// UI Text element to display the current score or points in the game.
    /// </summary>
    public Text points;

    /// <summary>
    /// UI Text element to display the remaining time for the game.
    /// </summary>
    public Text timer;

    /// <summary>
    /// The initial amount of time (in seconds) allocated for the game.
    /// </summary>
    public int time = 30;

    /// <summary>
    /// The current score or points accumulated during the game.
    /// Initialized to zero and updated as the game progresses.
    /// </summary>
    int pointsGame = 0;

    /// <summary>
    /// UI GameObject that represents the end-of-game screen or panel.
    /// This panel is displayed when the game ends.
    /// </summary>
    public GameObject uiEnd;

    /// <summary>
    /// UI Text element to display the final score at the end of the game.
    /// </summary>
    public Text PuntuacionEnd;

    /// <summary>
    /// UI Text element to display the number of animals collected or encountered at the end of the game.
    /// </summary>
    public Text animalesEnd;


    /// <summary>
    /// Initializes the singleton instance of the UiControllerAnfibios class.
    /// Ensures that only one instance of this class exists in the scene.
    /// If another instance already exists, the new instance will be destroyed.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Updates the timer text to display the remaining time.
    /// Sets the timer text to show the current value of the `time` variable.
    /// </summary>
    public void ActiveText()
    {
        timer.text = "Tiempo: " + time;
    }

    /// <summary>
    /// Manages the countdown of the game timer and checks for game end conditions.
    /// Decreases the `time` variable every second and updates the timer text.
    /// If the `time` reaches zero or the `pointsGame` variable reaches 100, it triggers the end of the game.
    /// Otherwise, it recursively calls itself to continue the countdown.
    /// </summary>
    /// <returns>An enumerator that allows the coroutine to yield and wait for a specified duration.</returns>
    IEnumerator MatchTime()
    {
        yield return new WaitForSeconds(1);
        time -= 1;
        ActiveText();

        if (time == 0 || pointsGame == 100)
        {
            showEndMinigame();
        }
        else
        {
            StartCoroutine(MatchTime());
        }
    }

    /// <summary>
    /// Starts the game timer by initiating the countdown coroutine.
    /// This method begins the `MatchTime` coroutine, which manages the countdown and checks for end-of-game conditions.
    /// </summary>
    public void starTimer()
    {
        StartCoroutine(MatchTime());
    }

    /// <summary>
    /// Updates the displayed score on the UI.
    /// This method sets the score to the specified value and updates the `points` Text UI element to reflect the new score.
    /// </summary>
    /// <param name="newScore">The new score to display.</param>
    public void UpdateScore(int newScore)
    {
        pointsGame = newScore;
        points.text = "Puntuación: " + newScore.ToString();
    }

    /// <summary>
    /// Initializes the UI at the start of the game.
    /// This method hides the end game UI by setting its active state to false when the game starts.
    /// </summary>
    void Start()
    {
        uiEnd.SetActive(false);
    }

    /// <summary>
    /// Displays the end of game UI and updates it with the final game data.
    /// This method sets the end game UI active, updates the score and the number of captured animals,
    /// and saves the remaining time in the player preferences.
    /// </summary>
    void showEndMinigame()
    {
        PuntuacionEnd.text = $"Puntuación Obtenida: {pointsGame}";
        animalesEnd.text = $"Animales Atrapados: {PlayerPrefs.GetInt("anfibiosNumber")}/5";
        PlayerPrefs.SetInt("TiempoJuego", time);
        uiEnd.SetActive(true);
    }

}
