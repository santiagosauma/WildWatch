// Script Name: ReturnHouse.cs
// Description:
//      * The script manages functionalities related
//      * to pausing and resuming the game, handling
//      * the notebook and configuration panels, and
//      * returning to the house or main menu.
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
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnHouse : MonoBehaviour
{
    /// <summary>
    /// The Button component associated with pausing or 
    /// resuming the game.
    /// </summary>
    public Button button;

    /// <summary>
    /// The sprite image used for the pause button.
    /// </summary>
    public Sprite ImgPausa;

    /// <summary>
    /// The sprite image used for the resume button.
    /// </summary>
    public Sprite ImgResume;

    /// <summary>
    /// The Canvas component that is shown when the
    /// game is paused.
    /// </summary>
    public Canvas Paused;

    /// <summary>
    /// A boolean flag indicating whether the game is 
    /// currently paused.
    /// </summary>
    private bool paused = false;

    /// <summary>
    /// A boolean flag indicating whether the Libreta (notebook)
    /// canvas is currently open.
    /// </summary>
    private bool openedLibreta = false;

    /// <summary>
    /// A boolean flag indicating whether the Config (settings)
    /// canvas is currently open.
    /// </summary>
    private bool openedConfig = false;

    /// <summary>
    /// The Canvas component representing the notebook UI.
    /// </summary>
    public Canvas LibretaCanvas;

    /// <summary>
    /// The Canvas component representing the settings UI.
    /// </summary>
    public Canvas ConfigCanvas;

    /// <summary>
    /// An array of Button components used for various UI
    /// interactions.
    /// </summary>
    public Button[] buttons;

    /// <summary>
    /// An instance of `AnswersData` used to store and manage
    /// answers data.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// An instance of `AnswersData` used to manage additional
    /// answer data.
    /// </summary>
    public AnswersData Happy;

    /// <summary>
    /// An instance of `ListaAnimales` used to manage lists of
    /// animal data.
    /// </summary>
    public ListaAnimales animales;

    /// <summary>
    /// Resets game data and transitions the player back to the
    /// house scene.
    /// </summary>
    public void returnHouse()
    {
        // Clear animal lists to reset collected data
        animales.Anfibios.Clear();
        animales.Pajaros.Clear();
        animales.Insectos.Clear();
        animales.Plantas.Clear();
        animales.Mamiferos.Clear();

        // Reset PlayerPrefs to initial values
        PlayerPrefs.SetInt("RegisterNum", 0);
        PlayerPrefs.SetFloat("PuntuacionFinal", 0);

        // Clear answer data
        Happy.answers.Clear();
        AnswersData.answers.Clear();

        // Load the house scene
        SceneManager.LoadScene("HouseScene");

        // Ensure game time scale is set to normal
        Time.timeScale = 1;

        // Mark the game as not paused
        paused = false;
    }

    /// <summary>
    /// Transitions the player to the main menu scene and resets game state.
    /// </summary>
    public void returnMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");

        // Ensure game time scale is set to normal
        Time.timeScale = 1;

        // Mark the game as not paused
        paused = false;
    }

    /// <summary>
    /// Toggles between pausing and resuming the game. 
    /// When the game is paused, the button sprite is set to the resume image, 
    /// the pause menu is activated, and game time is stopped. 
    /// Additionally, movement and other buttons are disabled. 
    /// When the game is resumed, the button sprite is set to the pause image, 
    /// the pause menu is deactivated, and game time is restored. 
    /// Movement and other buttons are re-enabled accordingly.
    /// </summary>
    public void PauseGameAndResume()
    {
        if (paused)
        {
            button.GetComponent<Image>().sprite = ImgPausa;
            Paused.gameObject.SetActive(false);
            paused = false;
            if (openedConfig)
            {
                // No specific action taken if the config is open when resuming
            }
            else
            {
                Time.timeScale = 1;
                RestoreMovement();
                EnableOtherButtons();
            }

        }
        else
        {
            button.GetComponent<Image>().sprite = ImgResume;
            Paused.gameObject.SetActive(true);
            paused = true;
            Time.timeScale = 0;
            DisableMovement();
            DisableOtherButtons();
        }
    }


    /// <summary>
    /// Toggles the visibility of the LibretaCanvas. 
    /// When the LibretaCanvas is visible, the time scale is
    /// set to 0 to pause the game. When it is hidden, the 
    /// time scale is restored to its previous state (0 if the game was paused, 1 otherwise). 
    /// The function also updates the state of the `openedLibreta` 
    /// boolean to reflect whether the LibretaCanvas is currently
    /// open or closed.
    /// </summary>
    public void ShowLibreta()
    {
        if (openedLibreta)
        {
            LibretaCanvas.gameObject.SetActive(false);
            openedLibreta = false;
            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        else
        {
            LibretaCanvas.gameObject.SetActive(true);
            openedLibreta = true;
            Time.timeScale = 0;
        }
    }


    /// <summary>
    /// Closes the LibretaCanvas by setting its GameObject to inactive.
    /// If the LibretaCanvas was open (`openedLibreta` is true), 
    /// it updates the state to closed (`openedLibreta` to false) 
    /// and restores the game's time scale to 1, resuming the game
    /// if it was paused.
    /// </summary>
    public void closelibreta()
    {
        LibretaCanvas.gameObject.SetActive(false);
        if (openedLibreta)
        {
            openedLibreta = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Toggles the visibility of the ConfigCanvas. 
    /// If the configuration menu is currently open (`openedConfig` is true),
    /// it closes the ConfigCanvas by setting its GameObject to inactive,
    /// updates the state to closed (`openedConfig` to false), and resumes 
    /// the game by restoring the time scale and re-enabling movement and buttons, 
    /// but only if the game was not paused.
    ///
    /// If the configuration menu is currently closed, it opens the 
    /// ConfigCanvas by setting its GameObject to active,
    /// pauses the game by setting the time scale to 0, updates the
    /// state to open (`openedConfig` to true), and disables movement
    /// and other buttons to prevent interaction while the config 
    /// menu is open.
    /// </summary>
    public void ShowConfig()
    {
        if (openedConfig)
        {
            ConfigCanvas.gameObject.SetActive(false);
            openedConfig = false;

            if (paused)
            {
                // If paused, do nothing
            }
            else
            {
                Time.timeScale = 1;
                RestoreMovement();
                EnableOtherButtons();
            }

        }
        else
        {
            ConfigCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            openedConfig = true;
            DisableMovement();
            DisableOtherButtons();
        }
    }


    /// <summary>
    /// Disables the movement-related components on all buttons
    /// in the `buttons` array. It iterates through each button
    /// and attempts to disable the following components if they
    /// are present:
    /// - ButtonControllerAnfibios
    /// - ButtonControllerHouse
    /// - ButtonControllerAves
    /// - ButtonControllerInsectos
    /// </summary>
    public void DisableMovement()
    {
        foreach (Button button in buttons)
        {
            TrySetComponentEnabled<ButtonControllerAnfibios>(button, false);
            TrySetComponentEnabled<ButtonControllerHouse>(button, false);
            TrySetComponentEnabled<ButtonControllerAves>(button, false);
            TrySetComponentEnabled<ButtonControllerInsectos>(button, false);
        }
    }

    /// <summary>
    /// Restores the movement-related components on all buttons 
    /// in the `buttons` array. It iterates through each button
    /// and attempts to enable the following components if they
    /// are present:
    /// - ButtonControllerAnfibios
    /// - ButtonControllerHouse
    /// - ButtonControllerAves
    /// - ButtonControllerInsectos
    /// </summary>
    public void RestoreMovement()
    {
        foreach (Button button in buttons)
        {
            TrySetComponentEnabled<ButtonControllerAnfibios>(button, true);
            TrySetComponentEnabled<ButtonControllerHouse>(button, true);
            TrySetComponentEnabled<ButtonControllerAves>(button, true);
            TrySetComponentEnabled<ButtonControllerInsectos>(button, true);
        }
    }

    /// <summary>
    /// Disables UI-related components and interactability on all 
    /// buttons in the `buttons` array. It iterates through each 
    /// button and attempts to disable the following components if
    /// they are present:
    /// - UIController
    /// - DragDrop
    /// - DragDesarmar
    /// Additionally, it sets the `interactable` property of each
    /// button to false.
    /// </summary>
    public void DisableOtherButtons()
    {
        foreach (Button button in buttons)
        {
            TrySetComponentEnabled<UIController>(button, false);
            TrySetComponentEnabled<DragDrop>(button, false);
            TrySetComponentEnabled<DragDesarmar>(button, false);
            button.interactable = false;
        }
    }

    /// <summary>
    /// Enables UI-related components and interactability on all 
    /// buttons in the `buttons` array. It iterates through each 
    /// button and attempts to enable the following components if
    /// they are present:
    /// - UIController
    /// - DragDrop
    /// - DragDesarmar
    /// Additionally, it sets the `interactable` property of each 
    /// button to true.
    /// </summary>
    public void EnableOtherButtons()
    {
        foreach (Button button in buttons)
        {
            TrySetComponentEnabled<UIController>(button, true);
            TrySetComponentEnabled<DragDrop>(button, true);
            TrySetComponentEnabled<DragDesarmar>(button, true);
            button.interactable = true;
        }
    }

    /// <summary>
    /// Tries to set the `enabled` property of a component of type
    /// `T` on the given button. If the button has a component of 
    /// type `T`, it sets the `enabled` property to the specified
    /// value.
    /// </summary>
    /// <typeparam name="T">The type of the component to enable 
    /// or disable.</typeparam>
    /// <param name="button">The button on which to check and modify
    /// the component.</param>
    /// <param name="enabled">Whether to enable or disable the
    /// component.</param>
    private void TrySetComponentEnabled<T>(Button button, bool enabled) where T : Behaviour
    {
        if (button.TryGetComponent(out T component))
        {
            component.enabled = enabled;
        }
    }

    public bool isPaused()
    {
        return paused;
    }
}
