// Script Name: IntroTutorial.cs
// Description:
//      * The script orchestrates a step-by-step
//      * tutorial experience, guiding users through
//      * instructional content with interactive
//      * elements such as an iPad and a notebook.
//      * It manages tutorial state transitions,
//      * handles visual effects for transitions,
//      * and updates UI components based on user
//      * progress. This ensures that users receive
//      * clear instructions and interact with various
//      * elements as they advance through the tutorial.
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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroTutorial : MonoBehaviour
{
    /// <summary>
    /// The current index in the list of tutorial instructions.
    /// </summary>
    int index = 0;

    /// <summary>
    /// List of tutorial instructions to be displayed to the user.
    /// </summary>
    public List<string> instructions = new List<string>();

    /// <summary>
    /// Flag indicating whether this is the user's first time running the tutorial.
    /// </summary>
    bool firstTime;

    /// <summary>
    /// Flag to determine if it is time to show the iPad tutorial.
    /// </summary>
    bool timeforipad;

    /// <summary>
    /// Flag to determine if it is time to show the notebook tutorial.
    /// </summary>
    bool timeforbook;

    /// <summary>
    /// GameObject containing long descriptions or additional tutorial information.
    /// </summary>
    public GameObject descripcionesLargas;

    /// <summary>
    /// GameObject representing the iPad in the tutorial.
    /// </summary>
    public GameObject ipad;

    /// <summary>
    /// GameObject representing the notebook in the tutorial.
    /// </summary>
    public GameObject libreta;

    /// <summary>
    /// Text component for displaying titles in the tutorial.
    /// </summary>
    public Text titulosText;

    /// <summary>
    /// Text component for displaying descriptions in the tutorial.
    /// </summary>
    public Text descripcionesText;

    /// <summary>
    /// Button to terminate or close the iPad tutorial section.
    /// </summary>
    public Button terminarIpad;

    /// <summary>
    /// GameObject representing the arrow pointing to the notebook.
    /// </summary>
    public GameObject arrowNotebook;

    /// <summary>
    /// GameObject representing the arrow pointing to the iPad.
    /// </summary>
    public GameObject arrowIpad;

    /// <summary>
    /// Button to end or proceed to the next section of the tutorial.
    /// </summary>
    public Button endButton;

    /// <summary>
    /// GameObject representing a black overlay or background in the tutorial.
    /// </summary>
    public GameObject black;

    /// <summary>
    /// SpriteRenderer component for controlling the appearance of the black overlay.
    /// </summary>
    public SpriteRenderer blackRenderer;

    /// <summary>
    /// Button to navigate to the next instruction or step in the tutorial.
    /// </summary>
    public Button next;

    void Update()
    {
        // Update the font size based on the current index
        UpdateFontSize();

        // Handle special cases based on the current index
        HandleSpecialCases();
    }

    /// <summary>
    /// Updates the font size of the description text based on the current index.
    /// </summary>
    private void UpdateFontSize()
    {
        // If index is between 1 and 4 (inclusive), set font size to 25
        if (index >= 1 && index <= 4)
        {
            descripcionesText.fontSize = 25;
        }
        // Otherwise, set font size to 35
        else
        {
            descripcionesText.fontSize = 35;
        }
    }

    /// <summary>
    /// Handles special cases based on the current index value.
    /// </summary>
    private void HandleSpecialCases()
    {
        // Handle the case where index is 3
        if (index == 3)
        {
            HandleNotebook();
        }
        // Handle the case where index is 6
        else if (index == 6)
        {
            HandleIpad();
        }
        // Handle the case where index is 9
        else if (index == 9)
        {
            HandleEnd();
        }
    }

    /// <summary>
    /// Handles the specific case when the index is 3.
    /// This involves updating UI elements for the notebook tutorial step.
    /// </summary>
    private void HandleNotebook()
    {
        timeforbook = true;
        next.gameObject.SetActive(false); // Hide the "next" button
        descripcionesLargas.SetActive(false); // Hide the long descriptions
        arrowNotebook.SetActive(true); // Show the arrow pointing to the notebook
        index++; // Move to the next index
    }

    /// <summary>
    /// Handles the specific case when the index is 6.
    /// This involves updating UI elements for the iPad tutorial step.
    /// </summary>
    private void HandleIpad()
    {
        next.gameObject.SetActive(false); // Hide the "next" button
        descripcionesLargas.SetActive(false); // Hide the long descriptions
        timeforipad = true;
        arrowIpad.SetActive(true); // Show the arrow pointing to the iPad
        index++; // Move to the next index
    }

    /// <summary>
    /// Handles the specific case when the index is 9.
    /// This involves showing the end button.
    /// </summary>
    private void HandleEnd()
    {
        next.gameObject.SetActive(false); // Hide the "next" button
        endButton.gameObject.SetActive(true); // Show the end button
    }

    /// <summary>
    /// Coroutine to manage the timing and UI transitions for the match.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    public IEnumerator MatchTime()
    {
        // Fade in the screen over a period of 3 seconds
        yield return StartCoroutine(FadeIn(3));

        // Wait for 2 seconds after the fade-in is complete
        yield return new WaitForSeconds(2);

        // Fade out the screen over a period of 3 seconds
        yield return StartCoroutine(FadeOut(3));

        // After fading out, make the long descriptions visible
        descripcionesLargas.SetActive(true);

        // Show the "next" button
        next.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fades in the screen by gradually increasing the alpha value of the blackRenderer's color.
    /// </summary>
    /// <param name="time">The duration of the fade-in effect in seconds.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    private IEnumerator FadeIn(float time)
    {
        // Get the current alpha value of the blackRenderer's color
        float alpha = blackRenderer.color.a;

        // Gradually increase the alpha value to 1
        while (alpha < 1)
        {
            alpha += Time.deltaTime / time;  // Increment alpha based on the elapsed time
            blackRenderer.color = new Color(blackRenderer.color.r,
                                            blackRenderer.color.g,
                                            blackRenderer.color.b,
                                            alpha);  // Update color with the new alpha value
            yield return null;  // Wait for the next frame
        }
    }

    /// <summary>
    /// Fades out the screen by gradually decreasing the alpha value of the blackRenderer's color.
    /// </summary>
    /// <param name="time">The duration of the fade-out effect in seconds.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    private IEnumerator FadeOut(float time)
    {
        // Get the current alpha value of the blackRenderer's color
        float alpha = blackRenderer.color.a;

        // Gradually decrease the alpha value to 0
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / time;  // Decrement alpha based on the elapsed time
            blackRenderer.color = new Color(blackRenderer.color.r,
                                            blackRenderer.color.g,
                                            blackRenderer.color.b,
                                            alpha);  // Update color with the new alpha value
            yield return null;  // Wait for the next frame
        }
    }


    /// <summary>
    /// Advances to the next instruction in the tutorial and updates the displayed text.
    /// </summary>
    public void change()
    {
        index += 1;  // Increment the index to show the next instruction
        descripcionesText.text = instructions[index];  // Update the description text with the new instruction
    }

    /// <summary>
    /// Initializes the tutorial by setting the index to the first set of instructions.
    /// </summary>
    public void startTutorial()
    {
        index = 0;  // Reset the index to start from the first instruction
    }

    /// <summary>
    /// Opens the notebook if the current tutorial step indicates it's time for the notebook.
    /// </summary>
    public void abrirLibreta()
    {
        if (timeforbook)
        {
            libreta.SetActive(true);  // Make the notebook visible
            arrowNotebook.SetActive(false);  // Hide the notebook arrow indicator
        }
    }

    /// <summary>
    /// Closes the notebook and shows the long descriptions and the next button.
    /// </summary>
    public void cerrar()
    {
        libreta.SetActive(false);  // Hide the notebook
        descripcionesLargas.SetActive(true);  // Make the long descriptions visible
        next.gameObject.SetActive(true);  // Show the "next" button
        descripcionesText.text = instructions[index];  // Update the description text to the current instruction
    }

    /// <summary>
    /// Opens the iPad if the current tutorial step indicates it's time for the iPad.
    /// </summary>
    public void openIpad()
    {
        if (timeforipad)
        {
            ipad.SetActive(true);  // Make the iPad visible
            arrowIpad.SetActive(false);  // Hide the iPad arrow indicator
        }
    }

    /// <summary>
    /// Closes the iPad and shows the long descriptions and the next button.
    /// </summary>
    public void cerrarIpad()
    {
        ipad.SetActive(false);  // Hide the iPad
        descripcionesLargas.SetActive(true);  // Make the long descriptions visible
        next.gameObject.SetActive(true);  // Show the "next" button
        descripcionesText.text = instructions[index];  // Update the description text to the current instruction
    }


    //Cambiar de escena/Salir del tutorial
    public void salir()
    {
        SceneManager.LoadScene("HouseScene");
    }

}
