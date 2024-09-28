// **********************************************************************
// Script Name: TabletInfoDisplay.cs
// Description:
//      * This script manages the display and interaction of the tablet UI
//      * in the game. It handles user inputs, displays information about
//      * various species, manages the registration process, and communicates
//      * with the server to report errors and send game data. 
//      * 
//      * The script includes functionalities for:
//      * - Handling navigation between different species and their images.
//      * - Validating user inputs against stored data.
//      * - Managing the registration of species and updating the game state
//      *   based on the user's performance.
//      * - Sending error reports to a server and handling server responses.
//      * - Resetting UI elements and states for new user inputs.
//      *
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 28/09/2024
// **********************************************************************

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TabletInfoDisplay : MonoBehaviour
{
    /// <summary>
    /// A list of names for different muestreos (sampling events or categories).
    /// </summary>
    List<string> muestreos = new List<string>();

    /// <summary>
    /// A nested list containing information about different species.
    /// Each inner list corresponds to a different species group.
    /// </summary>
    public List<List<string>> infoEspecies = new List<List<string>>();

    /// <summary>
    /// A nested list containing sprites (images) associated with different species.
    /// Each inner list corresponds to images of a specific species group.
    /// </summary>
    public List<List<Sprite>> imagenEspecies = new List<List<Sprite>>();

    /// <summary>
    /// Information specific to insect species.
    /// </summary>
    public List<string> infoInsectos = new List<string>();

    /// <summary>
    /// Information specific to bird species.
    /// </summary>
    public List<string> infoAves = new List<string>();

    /// <summary>
    /// Information specific to amphibian species.
    /// </summary>
    public List<string> infoAnfibios = new List<string>();

    /// <summary>
    /// Information specific to plant species.
    /// </summary>
    public List<string> infoPlantas = new List<string>();

    /// <summary>
    /// Information specific to mammal species.
    /// </summary>
    public List<string> infoMamiferos = new List<string>();

    /// <summary>
    /// Sprites (images) associated with insect species.
    /// </summary>
    public List<Sprite> imgInsectos = new List<Sprite>();

    /// <summary>
    /// Sprites (images) associated with bird species.
    /// </summary>
    public List<Sprite> imgAves = new List<Sprite>();

    /// <summary>
    /// Sprites (images) associated with amphibian species.
    /// </summary>
    public List<Sprite> imgAnfibios = new List<Sprite>();

    /// <summary>
    /// Sprites (images) associated with plant species.
    /// </summary>
    public List<Sprite> imgPlantas = new List<Sprite>();

    /// <summary>
    /// Sprites (images) associated with mammal species.
    /// </summary>
    public List<Sprite> imgMamiferos = new List<Sprite>();

    /// <summary>
    /// A UI text element that displays the quantity of a certain item or species.
    /// </summary>
    public Text quantity;

    /// <summary>
    /// An integer representation of the quantity. Used internally.
    /// </summary>
    private int quantityInt = 0;

    /// <summary>
    /// A UI text element that displays the type of muestreo (sampling event or category).
    /// </summary>
    public Text muestreoType;

    /// <summary>
    /// An integer identifier for the muestreo type. Used internally.
    /// </summary>
    private int muestreoID = -1;

    /// <summary>
    /// A UI text element that displays the name of an item or species.
    /// </summary>
    public Text nameText;

    /// <summary>
    /// An integer identifier for the name. Used internally.
    /// </summary>
    private int NameId = -1;

    /// <summary>
    /// A UI image element that displays a specific image.
    /// </summary>
    public Image Image;

    /// <summary>
    /// An integer identifier for the image. Used internally.
    /// </summary>
    private int ImageId = -1;

    /// <summary>
    /// A list of UI image elements representing the status of certain items.
    /// </summary>
    public List<Image> status = new List<Image>();

    /// <summary>
    /// A sprite (image) used to indicate a correct status.
    /// </summary>
    public Sprite correct;

    /// <summary>
    /// A sprite (image) used to indicate an incorrect status.
    /// </summary>
    public Sprite incorrect;

    /// <summary>
    /// Booleans used internally to track various states.
    /// </summary>
    bool m = false, n = false, im = false, q = false;

    /// <summary>
    /// A UI button used to send data or trigger an action.
    /// </summary>
    public Button send;

    /// <summary>
    /// A UI image element that displays a warning message.
    /// </summary>
    public Image warning;

    /// <summary>
    /// A UI GameObject that displays the results.
    /// </summary>
    public GameObject results;

    /// <summary>
    /// A UI GameObject that displays preliminary results.
    /// </summary>
    public GameObject preResults;

    /// <summary>
    /// A UI GameObject that contains form elements.
    /// </summary>
    public GameObject forms;

    /// <summary>
    /// A circular progress bar used to indicate progress.
    /// </summary>
    public CircularProgressBar progressBar;

    /// <summary>
    /// Data structure that holds the possible answers for the quiz.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// Number of available attempts a player has.
    /// </summary>
    int availableTries;

    /// <summary>
    /// A UI text element that displays the number of attempts remaining.
    /// </summary>
    public Text registerTries;

    /// <summary>
    /// The final score calculated for the player.
    /// </summary>
    float finalScore = 0;

    /// <summary>
    /// The maximum score possible in the quiz or game.
    /// </summary>
    int maxScore;

    /// <summary>
    /// Index of the best response or highest score achieved.
    /// </summary>
    int indexBest;

    /// <summary>
    /// An array of booleans tracking the correctness of the responses.
    /// </summary>
    bool[] responses = new bool[4];

    /// <summary>
    /// A list of UI image elements representing marks that are not final.
    /// </summary>
    public List<Image> notFinalMarks = new List<Image>();

    /// <summary>
    /// A UI text element that displays the player's score.
    /// </summary>
    public Text puntuacion;

    /// <summary>
    /// A UI text element that displays the status of the current level.
    /// </summary>
    public Text estatusNivel;

    /// <summary>
    /// A UI image element that serves as a background.
    /// </summary>
    public Image fondo;

    /// <summary>
    /// Another AnswersData structure, possibly for a different quiz or category.
    /// </summary>
    public AnswersData Happy;

    /// <summary>
    /// A UI button used to close a panel or window.
    /// </summary>
    public Button cerrar;

    /// <summary>
    /// A UI GameObject representing the iPad panel.
    /// </summary>
    public GameObject ipadPanel;

    /// <summary>
    /// The maximum possible score in a given context.
    /// </summary>
    int maxPossible = 0;

    /// <summary>
    /// Handles data communication, sending and receiving information from the database.
    /// </summary>
    public LecturaBaseDatos EnviarRecibirDatos;


    void Start()
    {
        maxPossible = Happy.answers.Count;

       
        availableTries = AnswersData.answers.Count;
        Debug.Log("available tries:" + availableTries);
        maxScore = availableTries * 4;
        send.interactable = false;
        warning.gameObject.SetActive(false);

        muestreos.Add("Aves");
        muestreos.Add("Anfibios");
        muestreos.Add("Mam�feros");
        muestreos.Add("Insectos");
        muestreos.Add("Plantas");

        infoEspecies.Add(infoAves);
        infoEspecies.Add(infoAnfibios);
        infoEspecies.Add(infoMamiferos);
        infoEspecies.Add(infoInsectos);
        infoEspecies.Add(infoPlantas);

        imagenEspecies.Add(imgAves);
        imagenEspecies.Add(imgAnfibios);
        imagenEspecies.Add(imgMamiferos);
        imagenEspecies.Add(imgInsectos);
        imagenEspecies.Add(imgPlantas);
    }

    /// <summary>
    /// Advances to the next "muestreo" (sampling type) in the list. 
    /// Updates the UI to reflect the current "muestreo" type, resets the species image index, 
    /// and hides any warning messages.
    /// </summary>
    public void nextMuestreo()
    {
        // Increment the muestreoID and loop back to the start if the end of the list is reached.
        muestreoID = (muestreoID + 1) % muestreos.Count;

        // Update the muestreoType text in the UI to display the current muestreo.
        muestreoType.text = muestreos[muestreoID];

        // Reset the index for species images to the beginning.
        ResetEspecieImagenIndex();

        // Hide any warning messages that might be active.
        warning.gameObject.SetActive(false);
    }

    /// <summary>
    /// Goes back to the previous "muestreo" (sampling type) in the list.
    /// Updates the UI to reflect the current "muestreo" type, resets the species image index,
    /// and hides any warning messages.
    /// </summary>
    public void backMuestreo()
    {
        // Decrement the muestreoID, and loop to the end of the list if it goes below 0.
        muestreoID = (muestreoID - 1 + muestreos.Count) % muestreos.Count;

        // Update the muestreoType text in the UI to display the current muestreo.
        muestreoType.text = muestreos[muestreoID];

        // Reset the index for species images to the beginning.
        ResetEspecieImagenIndex();

        // Hide any warning messages that might be active.
        warning.gameObject.SetActive(false);
    }


    /// <summary>
    /// Resets the indices for the species name and image to their initial values.
    /// This prepares the system to display the first species in the list.
    /// </summary>
    private void ResetEspecieImagenIndex()
    {
        // Reset the NameId to 0, pointing to the first species name.
        NameId = 0;

        // Reset the ImageId to 0, pointing to the first species image.
        ImageId = 0;

        // Update the UI text and image to reflect the reset indices.
        UpdateTextAndImage();
    }


    /// <summary>
    /// Advances to the next image in the list of species images for the current muestreo.
    /// If no muestreo is selected, a warning is shown.
    /// </summary>
    public void nextImg()
    {
        // Check if a muestreo (sampling type) is selected; if not, show a warning and exit.
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }

        // Increment the ImageId to point to the next image in the list, looping back to the start if necessary.
        ImageId = (ImageId + 1) % imagenEspecies[muestreoID].Count;

        // Update the displayed image in the UI to reflect the new ImageId.
        UpdateImage();
    }


    /// <summary>
    /// Goes back to the previous image in the list of species images for the current muestreo.
    /// If no muestreo is selected, a warning is shown.
    /// </summary>
    public void backImg()
    {
        // Check if a muestreo (sampling type) is selected; if not, show a warning and exit.
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }

        // Decrement the ImageId to point to the previous image in the list,
        // looping to the end of the list if it goes below 0.
        ImageId = (ImageId - 1 + imagenEspecies[muestreoID].Count) % imagenEspecies[muestreoID].Count;

        // Update the displayed image in the UI to reflect the new ImageId.
        UpdateImage();
    }


    /// <summary>
    /// Advances to the next species in the list for the current muestreo.
    /// Updates the UI to reflect the current species name and image.
    /// If no muestreo is selected, a warning is shown.
    /// </summary>
    public void nextEspecie()
    {
        // Check if a muestreo (sampling type) is selected; if not, show a warning and exit.
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }

        // Increment the NameId to point to the next species name in the list,
        // looping back to the start if necessary.
        NameId = (NameId + 1) % infoEspecies[muestreoID].Count;

        // Update the UI text and image to reflect the new species.
        UpdateTextAndImage();
    }


    /// <summary>
    /// Goes back to the previous species in the list for the current muestreo.
    /// Updates the UI to reflect the current species name and image.
    /// If no muestreo is selected, a warning is shown.
    /// </summary>
    public void BackEspecie()
    {
        // Check if a muestreo (sampling type) is selected; if not, show a warning and exit.
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }

        // Decrement the NameId to point to the previous species name in the list,
        // looping to the end of the list if it goes below 0.
        NameId = (NameId - 1 + infoEspecies[muestreoID].Count) % infoEspecies[muestreoID].Count;

        // Update the UI text and image to reflect the new species.
        UpdateTextAndImage();
    }


    /// <summary>
    /// Updates the UI text and image to reflect the current species selection.
    /// Also updates the status indicators to show the "correct" status.
    /// </summary>
    private void UpdateTextAndImage()
    {
        // Update the name text in the UI to display the current species name.
        nameText.text = infoEspecies[muestreoID][NameId];

        // Check if there are any images available for the current species;
        // if so, update the UI image to display the current species image.
        if (imagenEspecies[muestreoID].Count > 0)
        {
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }

        // Set the status indicators to show the "correct" status.
        status[0].sprite = correct;
        status[1].sprite = correct;
        status[2].sprite = correct;

        // Set internal flags to true indicating that the name, image, 
        // and status have been successfully updated.
        m = true;
        n = true;
        im = true;
    }


    /// <summary>
    /// Updates the UI image to reflect the current species image
    /// based on the selected muestreo and ImageId.
    /// </summary>
    private void UpdateImage()
    {
        // Check if there are any images available for the current muestreo;
        // if so, update the UI image to display the current species image.
        if (imagenEspecies[muestreoID].Count > 0)
        {
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }
    }

    /// <summary>
    /// Increases the quantity count by one, updates the UI to display the new quantity,
    /// and sets the status indicator to "correct" for the quantity.
    /// </summary>
    public void plusCantidad()
    {
        // Increment the quantity integer value by one.
        quantityInt++;

        // Update the UI text element to reflect the new quantity value.
        quantity.text = quantityInt.ToString();

        // Set the status indicator for the quantity to show the "correct" status.
        status[3].sprite = correct;

        // Set the internal flag 'q' to true, indicating that the quantity has been updated.
        q = true;
    }


    /// <summary>
    /// Decreases the quantity count by one, but only if the current quantity is greater than zero.
    /// Updates the UI to display the new quantity.
    /// </summary>
    public void minusCantidad()
    {
        // Check if the current quantity is greater than zero before decrementing.
        if (quantityInt > 0)
            quantityInt--;

        // Update the UI text element to reflect the new quantity value.
        quantity.text = quantityInt.ToString();
    }


    /// <summary>
    /// Checks if all necessary conditions are met to enable the send button.
    /// </summary>
    private void CheckSendButtonStatus()
    {
        if (m && n && im && q)
        {
            send.interactable = true;
        }
    }

    /// <summary>
    /// Unity's Update method, called once per frame. 
    /// It checks and updates the status of the send button.
    /// </summary>
    void Update()
    {
        // Call the function to check if the send button should be enabled.
        CheckSendButtonStatus();
    }


    /// <summary>
    /// Handles the process of submitting a registration attempt. 
    /// Reduces the available tries, processes the registration form, 
    /// and updates the game state depending on whether the player 
    /// has remaining tries or not.
    /// </summary>
    public void sendRegister()
    {
        // Reduce the number of available registration attempts.
        ReduceAvailableTries();

        // Deactivate the registration form and validate the current state.
        forms.SetActive(false);
        validar();

        // If no attempts are remaining, finalize the score and update the game state.
        if (availableTries == 0)
        {
            // Calculate the final score based on player performance.
            CalculateFinalScore();

            // Update the game state and UI elements to reflect the player's final status.
            UpdateGameStateAndUI();

            // Send the minigame information to the database.
            SendMinigameInfo();

            // Display the results panel to the player.
            results.SetActive(true);
        }
        else
        {
            // If attempts are still available, update the preliminary results.
            UpdatePreliminaryResults();
        }
    }


    /// <summary>
    /// Reduces the number of available tries and updates the stored value.
    /// </summary>
    private void ReduceAvailableTries()
    {
        availableTries--;
        int uniqueAvailable = PlayerPrefs.GetInt("RegisterNum", 0);
        uniqueAvailable--;
        PlayerPrefs.SetInt("RegisterNum", uniqueAvailable);
    }

    /// <summary>
    /// Calculates the final score based on the user's performance and stored data.
    /// </summary>
    private void CalculateFinalScore()
    {
        finalScore = finalScore * 100 / maxScore;
        finalScore = (finalScore * 60) / 100;

        float finalPuntuacion = PlayerPrefs.GetFloat("PuntuacionFinal", 0);
        finalPuntuacion = (finalPuntuacion * 40) / 100;

        float num = finalScore + finalPuntuacion;
        progressBar.setFill((int)num / 100f);
        puntuacion.text = $"{num}/100";
    }

    /// <summary>
    /// Updates the game state and UI elements based on the final score.
    /// </summary>
    private void UpdateGameStateAndUI()
    {
        if (finalScore >= 80)
        {
            estatusNivel.text = "Completado";
            fondo.color = new Color(147f / 255f, 255f / 255f, 97f / 255f, 255f / 255f);
        }
        else
        {
            int matchID = PlayerPrefs.GetInt("MatchID");
            StartCoroutine(SendError(matchID, 2));
        }

        cerrar.interactable = false;
    }

    /// <summary>
    /// Sends minigame information to the database.
    /// </summary>
    private void SendMinigameInfo()
    {
        int matchID = PlayerPrefs.GetInt("MatchID");
        int userID = PlayerPrefs.GetInt("UserID");
        int minigameIDVideojuego = PlayerPrefs.GetInt("FinalOption");
        int minigameIDBaseDatos = ObtenerIDMinijuego(minigameIDVideojuego);
        int tiempominijuego = PlayerPrefs.GetInt("TiempoJuego");

        StartCoroutine(EnviarRecibirDatos.UpdateMinigameInfo(
            matchID, userID, minigameIDBaseDatos, (int)finalScore, tiempominijuego));
        StartCoroutine(EnviarRecibirDatos.GetPoints());
    }

    /// <summary>
    /// Updates preliminary results and sets the corresponding marks in the UI.
    /// </summary>
    private void UpdatePreliminaryResults()
    {
        registerTries.text = $"Registros Restantes: {availableTries}";
        preResults.SetActive(true);

        if (indexBest == -3)
        {
            for (int i = 0; i < 4; i++)
            {
                notFinalMarks[i].sprite = correct;
            }
        }
        else
        {
            UpdateResultMarks(AnswersData.answers[indexBest]);
        }
    }

    /// <summary>
    /// Updates the result marks in the UI based on the stored answer.
    /// </summary>
    private void UpdateResultMarks(Answer storedAnswer)
    {
        notFinalMarks[0].sprite = storedAnswer.minigameId == muestreoID ? correct : incorrect;
        notFinalMarks[1].sprite = storedAnswer.idEspecie == NameId ? correct : incorrect;
        notFinalMarks[2].sprite = storedAnswer.imgId == ImageId ? correct : incorrect;
        notFinalMarks[3].sprite = storedAnswer.quantity == quantityInt ? correct : incorrect;
    }


    /// <summary>
    /// Prepares the system for another registration attempt by resetting relevant UI elements
    /// and states, then reactivating the registration form.
    /// </summary>
    public void anotherRegister()
    {
        // Hide the previous results UI panel to allow a new registration.
        preResults.SetActive(false);

        // Reset validation states and related UI elements for a new registration.
        ResetUIAndStates();

        // Reset any other necessary states for a fresh registration.
        ResetFormStates();

        // Reactivate the registration form to enable a new registration attempt.
        forms.SetActive(true);
    }


    /// <summary>
    /// Resets the UI elements and validation states to their initial conditions
    /// to prepare for a new registration attempt.
    /// </summary>
    private void ResetUIAndStates()
    {
        // Reset each status image to a default or "incorrect" sprite,
        // indicating that no validation has been completed yet.
        foreach (var image in status)
        {
            image.sprite = incorrect; // Alternatively, assign an "incomplete" image if available.
        }

        // Reset progress or validation indicators to their initial (false) state.
        m = n = im = q = false;

        // Disable the send button to prevent submission until all conditions are met.
        send.interactable = false;

        // Hide any warning messages if they are currently visible.
        warning.gameObject.SetActive(false);

        // Reset the quantity value and update the UI text to reflect this reset.
        quantityInt = 0;
        quantity.text = "0";

        // Reset the best index tracker to its initial value.
        indexBest = 0;
    }


    /// <summary>
    /// Resets the form fields and any associated data to their initial states.
    /// </summary>
    private void ResetFormStates()
    {
        // Reset the selected indices to their initial values,
        // indicating that no valid selection has been made.
        muestreoID = -1;
        NameId = -1;
        ImageId = -1;

        // Update the UI text elements to show that no selection has been assigned.
        muestreoType.text = "Sin Asignar";
        nameText.text = "Sin Asignar";

        // Clear the image displayed in the UI, as no valid image is selected.
        Image.sprite = null;
    }


    void validar()
    {
        bool found = false;
        int indexToRemove = -1;
        int maxCurrentAns = 0;

        // Itera sobre cada respuesta almacenada para buscar coincidencias con la respuesta del usuario
        for (int i = 0; i < AnswersData.answers.Count; i++)
        {
            Answer storedAnswer = AnswersData.answers[i];

            // Verifica si la respuesta actual coincide completamente con la almacenada
            if (CheckForExactMatch(storedAnswer))
            {
                found = true;
                indexToRemove = i;
                indexBest = -3;
                break;
            }
            else
            {
                // Evalúa coincidencias parciales y actualiza la puntuación temporal
                maxCurrentAns = EvaluatePartialMatch(storedAnswer, maxCurrentAns, i);
            }
        }

        // Si se encuentra una coincidencia exacta, maneja la respuesta correcta
        if (found)
        {
            HandleCorrectAnswer(indexToRemove);
        }
        else
        {
            // Si no se encuentra una coincidencia exacta, actualiza la puntuación final
            UpdateFinalScore(maxCurrentAns);
        }
    }

    /// <summary>
    /// Verifica si la respuesta almacenada coincide exactamente con la respuesta del usuario.
    /// </summary>
    /// <param name="storedAnswer">La respuesta almacenada a comparar.</param>
    /// <returns>True si hay una coincidencia exacta; de lo contrario, false.</returns>
    private bool CheckForExactMatch(Answer storedAnswer)
    {
        return storedAnswer.minigameId == muestreoID &&
               storedAnswer.idEspecie == NameId &&
               storedAnswer.imgId == ImageId &&
               storedAnswer.quantity == quantityInt;
    }

    /// <summary>
    /// Evalúa las coincidencias parciales entre la respuesta almacenada y la respuesta del usuario,
    /// y actualiza la puntuación temporal y el índice de la mejor coincidencia.
    /// </summary>
    /// <param name="storedAnswer">La respuesta almacenada a comparar.</param>
    /// <param name="currentMaxScore">La puntuación máxima actual basada en coincidencias parciales.</param>
    /// <param name="answerIndex">El índice de la respuesta almacenada actual.</param>
    /// <returns>La puntuación máxima actualizada basada en coincidencias parciales.</returns>
    private int EvaluatePartialMatch(Answer storedAnswer, int currentMaxScore, int answerIndex)
    {
        int tempScore = 0;

        // Enviar un informe de error si no hay coincidencia exacta
        int matchid = PlayerPrefs.GetInt("MatchID");
        StartCoroutine(SendError(matchid, 1));

        // Verifica cada campo individualmente y asigna puntos por aciertos parciales
        if (storedAnswer.minigameId == muestreoID)
        {
            tempScore += 1;  // Acierto parcial en minigameId
        }
        if (storedAnswer.idEspecie == NameId)
        {
            tempScore += 1;  // Acierto parcial en idEspecie
        }
        if (storedAnswer.imgId == ImageId)
        {
            tempScore += 1;  // Acierto parcial en imgId
        }
        if (storedAnswer.quantity == quantityInt)
        {
            tempScore += 1;  // Acierto parcial en quantity
        }

        // Actualiza el índice de la mejor coincidencia si la puntuación temporal es mayor
        if (tempScore > currentMaxScore)
        {
            indexBest = answerIndex;
            currentMaxScore = tempScore;
        }

        return currentMaxScore;
    }

    /// <summary>
    /// Maneja la lógica cuando se encuentra una respuesta correcta exacta.
    /// </summary>
    /// <param name="indexToRemove">El índice de la respuesta correcta en la lista de respuestas almacenadas.</param>
    private void HandleCorrectAnswer(int indexToRemove)
    {
        // Incrementa la puntuación final si la respuesta es correcta
        finalScore += 4; // Asumiendo que cada respuesta correcta vale 4 puntos

        // Elimina la respuesta correcta de la lista para evitar validarla de nuevo
        if (indexToRemove != -1)
        {
            AnswersData.answers.RemoveAt(indexToRemove);
        }

        // Opcional: Muestra un feedback al usuario
        Debug.Log("¡Respuesta correcta! Puntuación: " + finalScore);
    }

    /// <summary>
    /// Actualiza la puntuación final basada en la puntuación parcial máxima alcanzada.
    /// </summary>
    /// <param name="maxCurrentAns">La puntuación parcial máxima alcanzada.</param>
    private void UpdateFinalScore(int maxCurrentAns)
    {
        finalScore += maxCurrentAns;

        // Opcional: Maneja respuestas incorrectas
        Debug.Log("Respuesta incorrecta. Puntuación parcial: " + maxCurrentAns);
    }


    /// <summary>
    /// Handles the finalization process by deactivating the iPad panel and enabling the close button.
    /// </summary>
    public void finished()
    {
        // Deactivate the iPad panel to hide it from view.
        ipadPanel.SetActive(false);

        // Enable the close button to allow the user to close the current interface or panel.
        cerrar.interactable = true;
    }


    /// <summary>
    /// Converts the minigame ID from the video game to the corresponding database ID.
    /// </summary>
    /// <param name="minijuegoIDVideojuego">The minigame ID used in the video game.</param>
    /// <returns>The corresponding minigame ID used in the database.</returns>
    private int ObtenerIDMinijuego(int minijuegoIDVideojuego)
    {
        /*
           * Mapping between the minigame IDs:
           * minigameID in the Database:
           * 1 - Birds (Aves)
           * 2 - Mammals (Mamiferos)
           * 3 - Amphibians (Anfibios)
           * 4 - Insects (Insectos)
           * 5 - Flora
           *
           * minigameID in the Video Game:
           * 1 - Birds (Aves)
           * 3 - Mammals (Mamiferos)
           * 2 - Amphibians (Anfibios)
           * 4 - Insects (Insectos)
           * 5 - Flora
        */

        int minijuegoIDBaseDatos = 0;

        // Map the video game minigame ID to the corresponding database ID.
        if (minijuegoIDVideojuego == 1)
            minijuegoIDBaseDatos = 1; // Aves
        else if (minijuegoIDVideojuego == 2)
            minijuegoIDBaseDatos = 3; // Anfibios
        else if (minijuegoIDVideojuego == 3)
            minijuegoIDBaseDatos = 2; // Mamiferos
        else if (minijuegoIDVideojuego == 4)
            minijuegoIDBaseDatos = 4; // Insectos
        else if (minijuegoIDVideojuego == 5)
            minijuegoIDBaseDatos = 5; // Flora

        return minijuegoIDBaseDatos;
    }


    public IEnumerator SendError(int MatchID, int MistakeID)
    {
        // Serializa los datos de error a formato JSON
        string json = SerializeErrorData(MatchID, MistakeID);

        // Configura la solicitud web con la URL del minijuego y el JSON
        UnityWebRequest webRequest = SetupWebRequest("https://10.22.156.99:7026/api/Videogame/Mistakes", json);

        // Envía la solicitud y maneja la respuesta
        yield return HandleWebRequest(webRequest);
    }

    /// <summary>
    /// Serializes the error data into a JSON string using Newtonsoft.Json.
    /// </summary>
    /// <param name="MatchID">The match ID associated with the error.</param>
    /// <param name="MistakeID">The mistake ID to report.</param>
    /// <returns>A JSON string representing the error data.</returns>
    private string SerializeErrorData(int MatchID, int MistakeID)
    {
        // Create an anonymous object with the error data
        var errorData = new
        {
            matchID = MatchID,
            mistakeID = MistakeID
        };

        // Serialize the object to JSON format
        string json = JsonConvert.SerializeObject(errorData);

        // Debugging: Print the JSON string to the Unity Console
        Debug.Log("Sending JSON: " + json);

        return json;
    }

    /// <summary>
    /// Sets up the UnityWebRequest to send the JSON data to the specified URL.
    /// </summary>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="json">The JSON data to send.</param>
    /// <returns>A configured UnityWebRequest object ready to be sent.</returns>
    private UnityWebRequest SetupWebRequest(string url, string json)
    {
        // Create a new UnityWebRequest for a POST request
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, "POST");

        // Convert the JSON string to a byte array
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        // Set up the upload handler with the JSON data
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);

        // Set up the download handler to handle the response
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Set the content type to application/json
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Optionally set up a custom certificate handler if needed
        webRequest.certificateHandler = new ForceAceptAll();

        return webRequest;
    }

    /// <summary>
    /// Sends the UnityWebRequest and handles the response.
    /// </summary>
    /// <param name="webRequest">The UnityWebRequest object to send.</param>
    /// <returns>An IEnumerator to handle the coroutine in Unity.</returns>
    private IEnumerator HandleWebRequest(UnityWebRequest webRequest)
    {
        // Send the request and wait for a response
        yield return webRequest.SendWebRequest();

        // Dispose of the certificate handler if one was used
        webRequest.certificateHandler?.Dispose();

        // Check if the request was successful
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error en el registro");
        }
        else
        {
            Debug.Log("Registro Exitoso");
        }
    }


}
