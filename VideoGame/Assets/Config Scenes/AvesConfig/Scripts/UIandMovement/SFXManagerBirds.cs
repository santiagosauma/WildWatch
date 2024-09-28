// **********************************************************************
// Script Name: SFXManagerBirds.cs
// Description:
//      * This class provides methods to play various
//      * sound effects, including correct and incorrect
//      * action sounds, as well as bird sounds.
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

public class SFXManagerBirds : MonoBehaviour
{
    /// <summary>
    /// Array of audio clips containing bird sounds.
    /// </summary>
    public AudioClip[] birdAudios;

    /// <summary>
    /// Audio clip played when an action is correct.
    /// </summary>
    public AudioClip correct;

    /// <summary>
    /// Audio clip played when an action is incorrect.
    /// </summary>
    public AudioClip incorrect;

    /// <summary>
    /// Singleton instance of the bird sound effects manager.
    /// </summary>
    public static SFXManagerBirds instance;


    /// <summary>
    /// Unity's Awake method to initialize the singleton instance of the bird sound effects manager.
    /// Ensures that only one instance of SFXManagerBirds exists in the scene.
    /// </summary>
    void Awake()
    {
        // If an instance already exists and it is not this one, destroy this game object.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the instance.
        instance = this;
    }


    /// <summary>
    /// Plays the audio clip associated with an incorrect action at the main camera's position.
    /// </summary>
    public void playInCorrect()
    {
        AudioSource.PlayClipAtPoint(incorrect, Camera.main.transform.position, 0.5f);
    }

    /// <summary>
    /// Plays the audio clip associated with a correct action at the main camera's position.
    /// </summary>
    public void playCorrect()
    {
        AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);
    }

    /// <summary>
    /// Plays a bird sound based on the bird type index at the main camera's position.
    /// </summary>
    /// <param name="birdType">Index of the bird sound to play.</param>
    public void playBird(int birdType)
    {
        AudioSource.PlayClipAtPoint(birdAudios[birdType], Camera.main.transform.position, 0.5f);
    }

    public void playBj()
    {
        playBird(0);
    }

    public void playEag()
    {
        playBird(1);
    }

    public void playRB()
    {
        playBird(2);
    }

    public void playRob()
    {
        playBird(3);
    }

    public void playSpar()
    {
        playBird(4);
    }

}
