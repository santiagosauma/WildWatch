// **********************************************************************
// Script Name: AnfibiosController.cs
// Description:
//      * This script manages the functionality for an amphibian
//      * discovery minigame. It handles player interactions such
//      * as clicking on hidden amphibians and rocks, updates the
//      * player's score, and manages game state and transitions.
//      * It ensures that only one instance of the controller exists,
//      * initializes necessary variables, and updates the UI
//      * accordingly. The script also controls the appearance of
//      * animations and audio feedback related to player actions.
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
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnfibiosController : MonoBehaviour
{
    /// <summary>
    /// List of available amphibians that can be discovered.
    /// </summary>
    public List<AnfibioInfo> Disponibles = new List<AnfibioInfo>();

    /// <summary>
    /// List of amphibians that have already been discovered.
    /// </summary>
    private List<AnfibioInfo> Descubiertos = new List<AnfibioInfo>();

    /// <summary>
    /// Reference to the game object that controls the pause functionality.
    /// </summary>
    [SerializeField]
    private GameObject PauseControl;

    /// <summary>
    /// Static instance of the amphibians controller.
    /// </summary>
    public static AnfibiosController instance;

    /// <summary>
    /// Points accumulated for discovering amphibians.
    /// </summary>
    private int points = 0;

    /// <summary>
    /// Number of amphibians discovered.
    /// </summary>
    private int anfibiosNumber = 0;

    /// <summary>
    /// Indicates if it is the first discovery of an amphibian.
    /// </summary>
    private bool firstDiscover = false;

    /// <summary>
    /// Counter of amphibian discoveries.
    /// </summary>
    private int discover = 0;

    /// <summary>
    /// Audio clip for correct answers or actions.
    /// </summary>
    public AudioClip correct;

    /// <summary>
    /// Reference to the list of animals.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Reference to the data structure holding answers.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// Game object for displaying points animation.
    /// </summary>
    public GameObject puntosAnim;

    /// <summary>
    /// Counter for unique registrations.
    /// </summary>
    int uniqueRegister;

    /// <summary>
    /// Array to track already added amphibians.
    /// </summary>
    bool[] alreadyAdded = new bool[4];


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// This method ensures that only one instance of the AnfibiosController exists (singleton pattern).
    /// </summary>
    void Awake()
    {
        // Check if an instance of AnfibiosController already exists and is not this one
        if (instance != null && instance != this)
        {
            // Destroy this game object if another instance already exists
            Destroy(gameObject);
            return;
        }

        // Set this instance as the singleton instance
        instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// This method initializes the amphibian controller by resetting relevant variables and starting the UI timer.
    /// </summary>
    void Start()
    {
        // Initialize the alreadyAdded array to false
        for (int i = 0; i < 4; i++)
        {
            alreadyAdded[i] = false;
        }

        // Start the timer in the UI controller for amphibians
        UiControllerAnfibios.instance.starTimer();

        // Reset the number of discovered amphibians in PlayerPrefs
        PlayerPrefs.SetInt("anfibiosNumber", 0);

        // Reset the points
        points = 0;

        // Deactivate the points animation game object
        puntosAnim.SetActive(false);
    }


    /// <summary>
    /// Update is called once per frame.
    /// This method checks for mouse clicks and processes them if the game is not paused.
    /// </summary>
    void Update()
    {
        // Check if the left mouse button is clicked and the game is not paused
        if (Input.GetMouseButtonDown(0) && !PauseControl.GetComponent<ReturnHouse>().isPaused())
        {
            // Call method to check and handle mouse clicks
            CheckForClicks();
        }
    }

    /// <summary>
    /// Checks for mouse clicks and handles interactions with game objects.
    /// This method performs a raycast from the mouse position and triggers specific actions based on what is clicked.
    /// </summary>
    private void CheckForClicks()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast to detect what the ray hits
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        // Check if the raycast hit an object
        if (hit.collider != null)
        {
            // Handle the case where the hit object has the tag "Rock"
            if (hit.collider.CompareTag("Rock"))
            {
                RockClicked(hit);
            }
            // Handle the case where the hit object has the tag "Anfibio1"
            else if (hit.collider.CompareTag("Anfibio1"))
            {
                AnfibioClicked(hit, 0, 0);
            }
            // Handle the case where the hit object has the tag "Anfibio2"
            else if (hit.collider.CompareTag("Anfibio2"))
            {
                AnfibioClicked(hit, 1, 1);
            }
            // Handle the case where the hit object has the tag "Anfibio3"
            else if (hit.collider.CompareTag("Anfibio3"))
            {
                AnfibioClicked(hit, 2, 3);
            }
            // Handle the case where the hit object has the tag "Anfibio4"
            else if (hit.collider.CompareTag("Anfibio4"))
            {
                AnfibioClicked(hit, 3, 2);
            }

            // If fewer than 2 discoveries have been made and it's the first discovery
            if (discover < 2 && firstDiscover == true)
            {
                // Show the initial save data for amphibians
                ShowSaveDataAnfibios.instance.ShowInit();
            }
        }
    }

    /// <summary>
    /// Handles the interaction when a "Rock" is clicked.
    /// This method reveals a hidden amphibian at the position of the rock and then hides the rock itself.
    /// </summary>
    /// <param name="hit">The RaycastHit2D object containing information about the raycast hit.</param>
    void RockClicked(RaycastHit2D hit)
    {
        // Get the position of the clicked rock
        Vector2 hitPosition = hit.collider.transform.position;

        // Retrieve the hidden amphibian that is located at the position of the clicked rock
        GameObject hiddenAnfibio = AnfibiosSpawner.instance.GetHiddenAnfibioAtPosition(hitPosition);

        if (hiddenAnfibio != null)
        {
            // Enable the collider of the hidden amphibian, if it has one
            Collider2D anfibioCollider = hiddenAnfibio.GetComponent<Collider2D>();
            if (anfibioCollider != null)
            {
                anfibioCollider.enabled = true;
            }

            // Set the hidden amphibian to active, making it visible
            hiddenAnfibio.SetActive(true);
        }

        // Hide the rock that was clicked
        hit.collider.gameObject.SetActive(false);
    }




    /// <summary>
    /// Handles the interaction when an amphibian is clicked.
    /// This method updates the game state, points, and audio feedback based on the clicked amphibian.
    /// </summary>
    /// <param name="hit">The RaycastHit2D object containing information about the raycast hit.</param>
    /// <param name="anfibioIndex">The index of the clicked amphibian in the available list.</param>
    /// <param name="especieId">The species ID of the clicked amphibian.</param>
    void AnfibioClicked(RaycastHit2D hit, int anfibioIndex, int especieId)
    {
        // Handle the removal and scoring of the clicked amphibian
        HandleAmphibianRemoval(hit);

        // Update the number of discovered amphibians and the player's score
        UpdateAmphibianDiscovery(anfibioIndex, especieId);

        // Manage the discovery status
        ManageDiscoveryStatus();
    }

    /// <summary>
    /// Handles the removal of the clicked amphibian and updates the points.
    /// </summary>
    /// <param name="hit">The RaycastHit2D object containing information about the raycast hit.</param>
    private void HandleAmphibianRemoval(RaycastHit2D hit)
    {
        // Destroy the clicked amphibian game object
        Destroy(hit.collider.gameObject);

        // Increase the player's points
        IncreasePoints();

        // Update the number of amphibians discovered
        anfibiosNumber++;

        // Play the correct sound effect
        AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);

        // Save the updated number of discovered amphibians
        PlayerPrefs.SetInt("anfibiosNumber", anfibiosNumber);
    }

    /// <summary>
    /// Updates the discovery status and answers data based on the clicked amphibian.
    /// </summary>
    /// <param name="anfibioIndex">The index of the clicked amphibian in the available list.</param>
    /// <param name="especieId">The species ID of the clicked amphibian.</param>
    private void UpdateAmphibianDiscovery(int anfibioIndex, int especieId)
    {
        if (!Descubiertos.Contains(Disponibles[anfibioIndex]))
        {
            // Add the newly discovered amphibian to the list
            Descubiertos.Add(Disponibles[anfibioIndex]);
            animales.Anfibios.Add(Disponibles[anfibioIndex]);
            uniqueRegister++;

            // Update answers data for already discovered species
            if (alreadyAdded[anfibioIndex])
            {
                foreach (var answer in AnswersData.answers)
                {
                    if (answer.idEspecie == especieId)
                    {
                        answer.quantity += 1;
                    }
                }
            }

            // Add new answer for undiscovered species
            if (!alreadyAdded[anfibioIndex])
            {
                alreadyAdded[anfibioIndex] = true;
                Answer answer = new Answer
                {
                    imgId = especieId,
                    idEspecie = especieId,
                    minigameId = 1,
                    quantity = 1
                };
                AnswersData.answers.Add(answer);
            }
        }
    }

    /// <summary>
    /// Updates the discovery status flags based on the new discovery.
    /// </summary>
    private void ManageDiscoveryStatus()
    {
        discover++;
        if (!firstDiscover)
        {
            firstDiscover = true;
        }
    }


    /// <summary>
    /// Saves the final score and unique register count to PlayerPrefs 
    /// and loads the "HouseScene" scene. This method is used to transition
    /// back to the home scene, preserving the final score and register number.
    /// </summary>
    public void goHomeEnd()
    {
        // Save the final score and unique register count to PlayerPrefs
        PlayerPrefs.SetFloat("PuntuacionFinal", points);
        PlayerPrefs.SetInt("RegisterNum", uniqueRegister);

        // Load the "HouseScene" scene
        SceneManager.LoadScene("HouseScene");
    }

    /// <summary>
    /// Increases the player's score by 20 points and triggers a visual 
    /// points animation. This method updates the score displayed in the
    /// UI and starts a coroutine to show the points animation.
    /// </summary>
    public void IncreasePoints()
    {
        // Start the coroutine to show the points animation
        StartCoroutine(showPoints());

        // Increase the score by 20 points
        points += 20;

        // Update the score displayed in the UI
        UiControllerAnfibios.instance.UpdateScore(points);
    }

    /// <summary>
    /// Coroutine that activates and deactivates the points animation UI element.
    /// This method is used to show a visual indication of points being added to the score.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator showPoints()
    {
        // Activate the points animation UI element
        puntosAnim.SetActive(true);

        // Wait for 1 second to display the animation
        yield return new WaitForSeconds(1f);

        // Deactivate the points animation UI element
        puntosAnim.SetActive(false);
    }


    public List<AnfibioInfo> returnDescubiertos()
    {
        return Descubiertos;
    }

}
