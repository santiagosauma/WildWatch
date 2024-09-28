// Script Name: DoorsController.cs
// Description:
//      * This script manages the behavior of a door in the game,
//      * including its opening and closing animations and
//      * interaction with the player. It also handles displaying
//      * game-related UI elements when the player is close to the door.        
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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorsController : MonoBehaviour
{
    /// <summary>
    /// List of animals used for various purposes within the script.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Reference to the Animator component for controlling animations.
    /// </summary>
    Animator animatorController;

    /// <summary>
    /// Reference to the player GameObject in the scene.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Distance within which the door will open when the player is close.
    /// </summary>
    public float openDistance = 5.0f;

    /// <summary>
    /// Indicates whether the door is currently open or closed.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Singleton instance of the DoorsController class.
    /// </summary>
    public static DoorsController instance;

    /// <summary>
    /// Data for handling answers, used for game logic or responses.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// Data for handling happy answers or responses, possibly used for specific conditions.
    /// </summary>
    public AnswersData Happy;

    /// <summary>
    /// Type of game or mode selected, constrained between 0 and 5.
    /// </summary>
    [Range(0, 5)] public int typeOfGame = 0;

    /// <summary>
    /// Temporary reference to a Button component, possibly used for UI interactions.
    /// </summary>
    public Button temp;

    /// <summary>
    /// Unity's Awake method to initialize the singleton instance of the DoorsController class.
    /// Ensures that only one instance of DoorsController exists in the scene.
    /// </summary>
    private void Awake()
    {
        // Check if the instance is null and assign this as the instance if true.
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Unity's Start method to initialize components and set up initial state.
    /// </summary>
    void Start()
    {
        // Get the Animator component attached to this GameObject.
        animatorController = GetComponent<Animator>();

        // Deactivate the temporary button.
        temp.gameObject.SetActive(false);
    }

    /// <summary>
    /// Prepares for entering the minigame by setting up game preferences, clearing data, 
    /// and loading the appropriate scene.
    /// </summary>
    public void enterMinigame()
    {
        // Retrieve the final option selected by the player and store it in 'typeOfGame'.
        typeOfGame = PlayerPrefs.GetInt("FinalOption");

        // Save the game type and ID to PlayerPrefs for later retrieval.
        PlayerPrefs.SetInt("TypeOfGame", typeOfGame);
        PlayerPrefs.SetInt("IDGAME", typeOfGame - 1);

        // Clear all animal categories from the 'animales' object.
        animales.Anfibios.Clear();
        animales.Pajaros.Clear();
        animales.Insectos.Clear();
        animales.Plantas.Clear();
        animales.Mamiferos.Clear();

        // Clear answers data for both general and happy responses.
        AnswersData.answers.Clear();
        Happy.answers.Clear();

        // Reset the final score to 0 in PlayerPrefs.
        PlayerPrefs.SetFloat("PuntuacionFinal", 0);

        // Load the scene named "LoadingMinigame" to transition into the minigame.
        SceneManager.LoadScene("LoadingMinigame");
    }

    /// <summary>
    /// Unity's Update method that checks the distance between the player and the door.
    /// Opens or closes the door and updates the UI based on the player's proximity to the door.
    /// </summary>
    void Update()
    {
        // Calculate the distance between the player and the door.
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Check if the player is within the specified distance to open the door.
        if (distance <= openDistance)
        {
            // If the door is not already open, perform actions to open it.
            if (!isOpen)
            {
                // Update the door's animation to the 'open' state.
                UpdateAnimation(DoorsAnimation.open);
                isOpen = true;

                // Activate the temporary UI element and update its text.
                temp.gameObject.SetActive(true);
                temp.GetComponentInChildren<Text>().text = "Jugar: \r\n \r\n" + GetMinigameName(typeOfGame);

                // Store the type of game being used for the door in PlayerPrefs.
                PlayerPrefs.SetInt("usedForDoor", typeOfGame);
            }
        }
        // If the player is outside the specified distance and the door is open.
        else if (isOpen)
        {
            // Update the door's animation to the 'closed' state.
            UpdateAnimation(DoorsAnimation.closed);
            isOpen = false;

            // Deactivate the temporary UI element.
            temp.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Returns the name of the minigame based on the provided game type.
    /// </summary>
    /// <param name="gameType">The type of the minigame, represented by an integer.</param>
    /// <returns>The name of the minigame as a string.</returns>
    string GetMinigameName(int gameType)
    {
        // Determine the minigame name based on the provided game type.
        switch (gameType)
        {
            case 1: return "Aves";        // Game type 1 corresponds to "Aves".
            case 2: return "Anfibios";    // Game type 2 corresponds to "Anfibios".
            case 3: return "Mamíferos";   // Game type 3 corresponds to "Mamíferos".
            case 4: return "Insectos";    // Game type 4 corresponds to "Insectos".
            case 5: return "Plantas";     // Game type 5 corresponds to "Plantas".
            default: return "Unknown";    // Return "Unknown" if the game type is not recognized.
        }
    }

    /// <summary>
    /// Enum representing the different states of door animations.
    /// </summary>
    public enum DoorsAnimation
    {
        /// <summary>
        /// Animation state for the door being open.
        /// </summary>
        open,

        /// <summary>
        /// Animation state for the door being closed.
        /// </summary>
        closed
    }

    /// <summary>
    /// Updates the door's animation state based on the provided DoorsAnimation value.
    /// </summary>
    /// <param name="animation">The desired animation state for the door.</param>
    void UpdateAnimation(DoorsAnimation animation)
    {
        // Update the Animator component's "isOpen" parameter based on the specified animation state.
        switch (animation)
        {
            case DoorsAnimation.open:
                // Set the "isOpen" parameter to true to trigger the open animation.
                animatorController.SetBool("isOpen", true);
                break;

            case DoorsAnimation.closed:
                // Set the "isOpen" parameter to false to trigger the closed animation.
                animatorController.SetBool("isOpen", false);
                break;
        }
    }


}
