// Script Name: IpadTempTut.cs
// Description:
//      * The script provides a interface for handling
//      * a tutorial application related to species
//      * identification. It manages user inputs, updates
//      * the UI based on interactions, and validates
//      * user responses to provide feedback and scores.
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
using UnityEngine.UI;

public class IpadTempTut : MonoBehaviour
{
    /// <summary>
    /// Lista de muestreos realizados.
    /// </summary>
    List<string> muestreos = new List<string>();

    /// <summary>
    /// Lista de listas que contiene información sobre diferentes especies.
    /// </summary>
    public List<List<string>> infoEspecies = new List<List<string>>();

    /// <summary>
    /// Lista de listas que contiene imágenes de diferentes especies.
    /// </summary>
    public List<List<Sprite>> imagenEspecies = new List<List<Sprite>>();

    /// <summary>
    /// Información específica de insectos.
    /// </summary>
    public List<string> infoInsectos = new List<string>();

    /// <summary>
    /// Información específica de aves.
    /// </summary>
    public List<string> infoAves = new List<string>();

    /// <summary>
    /// Información específica de anfibios.
    /// </summary>
    public List<string> infoAnfibios = new List<string>();

    /// <summary>
    /// Información específica de plantas.
    /// </summary>
    public List<string> infoPlantas = new List<string>();

    /// <summary>
    /// Información específica de mamíferos.
    /// </summary>
    public List<string> infoMamiferos = new List<string>();

    /// <summary>
    /// Imágenes específicas de insectos.
    /// </summary>
    public List<Sprite> imgInsectos = new List<Sprite>();

    /// <summary>
    /// Imágenes específicas de aves.
    /// </summary>
    public List<Sprite> imgAves = new List<Sprite>();

    /// <summary>
    /// Imágenes específicas de anfibios.
    /// </summary>
    public List<Sprite> imgAnfibios = new List<Sprite>();

    /// <summary>
    /// Imágenes específicas de plantas.
    /// </summary>
    public List<Sprite> imgPlantas = new List<Sprite>();

    /// <summary>
    /// Imágenes específicas de mamíferos.
    /// </summary>
    public List<Sprite> imgMamiferos = new List<Sprite>();

    /// <summary>
    /// Texto que muestra la cantidad.
    /// </summary>
    public Text quantity;

    /// <summary>
    /// Valor entero que representa la cantidad.
    /// </summary>
    private int quantityInt = 0;

    /// <summary>
    /// Texto que muestra el tipo de muestreo.
    /// </summary>
    public Text muestreoType;

    /// <summary>
    /// Identificador del muestreo.
    /// </summary>
    private int muestreoID = -1;

    /// <summary>
    /// Texto que muestra el nombre.
    /// </summary>
    public Text nameText;

    /// <summary>
    /// Identificador del nombre.
    /// </summary>
    private int NameId = -1;

    /// <summary>
    /// Imagen que muestra la especie.
    /// </summary>
    public Image Image;

    /// <summary>
    /// Identificador de la imagen.
    /// </summary>
    private int ImageId = -1;

    /// <summary>
    /// Lista de imágenes que representan el estado.
    /// </summary>
    public List<Image> status = new List<Image>();

    /// <summary>
    /// Sprite que representa una respuesta correcta.
    /// </summary>
    public Sprite correct;

    /// <summary>
    /// Sprite que representa una respuesta incorrecta.
    /// </summary>
    public Sprite incorrect;

    /// <summary>
    /// Banderas para controlar los estados de muestreo, nombre, imagen y cantidad.
    /// </summary>
    bool m = false, n = false, im = false, q = false;

    /// <summary>
    /// Botón para enviar la información.
    /// </summary>
    public Button send;

    /// <summary>
    /// Imagen que muestra una advertencia.
    /// </summary>
    public Image warning;

    /// <summary>
    /// GameObject que muestra los resultados.
    /// </summary>
    public GameObject results;

    /// <summary>
    /// GameObject que muestra los resultados preliminares.
    /// </summary>
    public GameObject preResults;

    /// <summary>
    /// GameObject que muestra los formularios.
    /// </summary>
    public GameObject forms;

    /// <summary>
    /// Barra de progreso circular.
    /// </summary>
    public CircularProgressBar progressBar;

    /// <summary>
    /// Datos de las respuestas.
    /// </summary>
    public AnswersData AnswersData;

    /// <summary>
    /// Número de intentos disponibles.
    /// </summary>
    int availableTries;

    /// <summary>
    /// Texto que muestra el número de intentos de registro.
    /// </summary>
    public Text registerTries;

    /// <summary>
    /// Puntuación final.
    /// </summary>
    float finalScore = 0;

    /// <summary>
    /// Puntuación máxima.
    /// </summary>
    int maxScore;

    /// <summary>
    /// Índice de la mejor puntuación.
    /// </summary>
    int indexBest;

    /// <summary>
    /// Array de booleanos que representan las respuestas.
    /// </summary>
    bool[] responses = new bool[4];

    /// <summary>
    /// Lista de imágenes que no muestran la marca final.
    /// </summary>
    public List<Image> notFinalMarks = new List<Image>();

    /// <summary>
    /// Texto que muestra la puntuación.
    /// </summary>
    public Text puntuacion;

    /// <summary>
    /// Texto que muestra el estado del nivel.
    /// </summary>
    public Text estatusNivel;

    /// <summary>
    /// Imagen de fondo.
    /// </summary>
    public Image fondo;

    /// <summary>
    /// Datos de las respuestas felices.
    /// </summary>
    public AnswersData Happy;

    /// <summary>
    /// Botón para cerrar.
    /// </summary>
    public Button cerrar;

    /// <summary>
    /// Panel del iPad.
    /// </summary>
    public GameObject ipadPanel;

    /// <summary>
    /// Máximo posible.
    /// </summary>
    int maxPossible = 0;

    /// <summary>
    /// Initializes the game state and sets up initial configurations for the tutorial.
    /// </summary>
    void Start()
    {
        // Sets the maximum possible score based on the number of happy answers.
        maxPossible = Happy.answers.Count;

        // Sets the available tries and calculates the maximum score.
        availableTries = 1;
        maxScore = availableTries * 4;

        // Initially disables the send button and hides the warning.
        send.interactable = false;
        warning.gameObject.SetActive(false);

        // Adds "Aves" (Birds) to the list of samplings and their corresponding info and image lists.
        muestreos.Add("Aves");
        infoEspecies.Add(infoAves);
        imagenEspecies.Add(imgAves);
    }

    /// <summary>
    /// Advances to the next sampling type and updates the UI accordingly.
    /// </summary>
    public void nextMuestreo()
    {
        // Increment the muestreoID and wrap around if necessary.
        muestreoID = (muestreoID + 1) % muestreos.Count;

        // Update the text field to display the current sampling type.
        muestreoType.text = muestreos[muestreoID];

        // Reset the species and image index for the new sampling type.
        ResetEspecieImagenIndex();

        // Hide the warning message.
        warning.gameObject.SetActive(false);
    }

    /// <summary>
    /// Moves to the previous sampling type and updates the UI accordingly.
    /// </summary>
    public void backMuestreo()
    {
        // Decrement the muestreoID and wrap around if necessary.
        muestreoID = (muestreoID - 1 + muestreos.Count) % muestreos.Count;

        // Update the text field to display the current sampling type.
        muestreoType.text = muestreos[muestreoID];

        // Reset the species and image index for the new sampling type.
        ResetEspecieImagenIndex();

        // Hide the warning message.
        warning.gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the indices for species name and image, and updates the displayed text and image.
    /// </summary>
    private void ResetEspecieImagenIndex()
    {
        // Reset the name index to the first element.
        NameId = 0;

        // Reset the image index to the first element.
        ImageId = 0;

        // Update the text and image to reflect the reset indices.
        UpdateTextAndImage();
    }

    /// <summary>
    /// Moves to the next image in the list for the current species and updates the displayed image.
    /// </summary>
    public void nextImg()
    {
        if (muestreoID == -1)
        {
            // Show a warning if no species type is selected.
            warning.gameObject.SetActive(true);
            return;
        }
        // Increment the image index and wrap around if it exceeds the count.
        ImageId = (ImageId + 1) % imagenEspecies[muestreoID].Count;
        // Update the displayed image.
        UpdateImage();
    }

    /// <summary>
    /// Moves to the previous image in the list for the current species and updates the displayed image.
    /// </summary>
    public void backImg()
    {
        if (muestreoID == -1)
        {
            // Show a warning if no species type is selected.
            warning.gameObject.SetActive(true);
            return;
        }
        // Decrement the image index and wrap around if it goes below zero.
        ImageId = (ImageId - 1 + imagenEspecies[muestreoID].Count) % imagenEspecies[muestreoID].Count;
        // Update the displayed image.
        UpdateImage();
    }

    /// <summary>
    /// Moves to the next species in the list and updates the displayed text and image.
    /// </summary>
    public void nextEspecie()
    {
        if (muestreoID == -1)
        {
            // Show a warning if no species type is selected.
            warning.gameObject.SetActive(true);
            return;
        }
        // Increment the species index and wrap around if it exceeds the count.
        NameId = (NameId + 1) % infoEspecies[muestreoID].Count;
        // Update the displayed text and image.
        UpdateTextAndImage();
    }

    /// <summary>
    /// Moves to the previous species in the list and updates the displayed text and image.
    /// </summary>
    public void BackEspecie()
    {
        if (muestreoID == -1)
        {
            // Show a warning if no species type is selected.
            warning.gameObject.SetActive(true);
            return;
        }
        // Decrement the species index and wrap around if it goes below zero.
        NameId = (NameId - 1 + infoEspecies[muestreoID].Count) % infoEspecies[muestreoID].Count;
        // Update the displayed text and image.
        UpdateTextAndImage();
    }

    /// <summary>
    /// Updates the displayed text and image based on the current species and image indices.
    /// Sets the status indicators to correct sprites and updates boolean flags.
    /// </summary>
    private void UpdateTextAndImage()
    {
        // Set the name text to the current species name.
        nameText.text = infoEspecies[muestreoID][NameId];

        // Check if there are any images for the current species.
        if (imagenEspecies[muestreoID].Count > 0)
        {
            // Set the image sprite to the current image.
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }

        // Set all status indicators to the correct sprite.
        status[0].sprite = correct;
        status[1].sprite = correct;
        status[2].sprite = correct;

        // Update the boolean flags to indicate correct status.
        m = true;
        n = true;
        im = true;
    }

    /// <summary>
    /// Updates the displayed image based on the current image index for the selected species.
    /// </summary>
    private void UpdateImage()
    {
        // Check if there are any images for the current species.
        if (imagenEspecies[muestreoID].Count > 0)
        {
            // Set the image sprite to the current image.
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }
    }

    /// <summary>
    /// Increases the quantity by one, updates the displayed quantity, 
    /// sets the status indicator to the correct sprite, and updates the boolean flag.
    /// </summary>
    public void plusCantidad()
    {
        // Increment the quantity.
        quantityInt++;
        // Update the displayed quantity.
        quantity.text = quantityInt.ToString();
        // Set the status indicator to the correct sprite.
        status[3].sprite = correct;
        // Update the boolean flag to indicate correct status.
        q = true;
    }

    /// <summary>
    /// Decreases the quantity by one if it is greater than zero, 
    /// and updates the displayed quantity.
    /// </summary>
    public void minusCantidad()
    {
        // Check if the quantity is greater than zero.
        if (quantityInt > 0)
        {
            // Decrement the quantity.
            quantityInt--;
        }
        // Update the displayed quantity.
        quantity.text = quantityInt.ToString();
    }

    /// <summary>
    /// Unity's Update method called once per frame. Checks if all conditions are met 
    /// to make the send button interactable.
    /// </summary>
    void Update()
    {
        // If all conditions (m, n, im, q) are true, make the send button interactable.
        if (m && n && im && q)
        {
            send.interactable = true;
        }
    }

    /// <summary>
    /// Handles the registration process when the user submits their data.
    /// Updates the number of available tries, adjusts scores, and updates UI elements.
    /// </summary>
    public void sendRegister()
    {
        // Decrement available tries and registration count
        UpdateTriesAndRegistration();

        // Hide the forms and perform validation
        forms.SetActive(false);
        validar();

        // Check if all tries are used up and update results
        if (availableTries == 0)
        {
            UpdateFinalScore();
            UpdateUI();
            ShowResults();
        }
    }

    /// <summary>
    /// Decrements the number of available tries and updates the registration count in PlayerPrefs.
    /// </summary>
    private void UpdateTriesAndRegistration()
    {
        availableTries--;
        int uniqueAvailable = PlayerPrefs.GetInt("RegisterNum", 0);
        uniqueAvailable--;
        PlayerPrefs.SetInt("RegisterNum", uniqueAvailable);
    }

    /// <summary>
    /// Calculates and updates the final score based on performance.
    /// </summary>
    private void UpdateFinalScore()
    {
        finalScore = finalScore * 100 / maxScore;  // Convert to percentage
        finalScore = (finalScore * 60) / 100;      // Apply 60% weight

        float finalPuntuacion = PlayerPrefs.GetFloat("PuntuacionFinal", 0);
        finalPuntuacion = (finalPuntuacion * 40) / 100; // Apply 40% weight
    }

    /// <summary>
    /// Updates the UI elements to reflect the final score and progress.
    /// </summary>
    private void UpdateUI()
    {
        float num = finalScore + 40;
        progressBar.setFill((int)num / 100f);
        puntuacion.text = $"{num}/100";

        if (num > 60)
        {
            estatusNivel.text = "Completed";
            fondo.color = new Color(147f / 255f, 255f / 255f, 97f / 255f, 255f / 255f);
        }
    }

    /// <summary>
    /// Displays the results UI to the user.
    /// </summary>
    private void ShowResults()
    {
        results.SetActive(true);
    }

    /// <summary>
    /// Prepares the UI for a new registration by resetting previous 
    /// results, validation states, and form states.
    /// </summary>
    public void anotherRegister()
    {
        // Hide the previous results UI to allow for a new registration
        preResults.SetActive(false);

        // Reset validation states and related UI elements
        ResetUIAndStates();

        // Reset any other necessary states for a new registration
        ResetFormStates();

        // Reactivate the registration form for new input
        forms.SetActive(true);
    }

    /// <summary>
    /// Resets the user interface and validation states to prepare
    /// for a new registration.
    /// </summary>
    private void ResetUIAndStates()
    {
        // Reset status images to a default or empty state, such as 'incorrect' or a placeholder image
        foreach (var image in status)
        {
            image.sprite = incorrect; // Assign 'incorrect' image to indicate no validation has occurred
        }

        // Reset validation indicators by setting boolean flags to false
        m = n = im = q = false;

        // Disable the 'send' button to prevent submission until all required fields are filled
        send.interactable = false;

        // Hide any warning messages or UI elements related to previous validation issues
        warning.gameObject.SetActive(false);

        // Reset the displayed quantity text and internal quantity value to zero
        quantityInt = 0;
        quantity.text = "0";

        // Reset the index for tracking the best score or other relevant state
        indexBest = 0;
    }

    /// <summary>
    /// Resets form fields and any associated data to their initial states.
    /// </summary>
    private void ResetFormStates()
    {
        // Reset selected indices to their initial values, which indicates no selection or default state
        muestreoID = -1; // Indicates no current sampling type selected
        NameId = -1;     // Indicates no current species name selected
        ImageId = -1;    // Indicates no current image selected

        // Update UI elements to reflect the reset state
        muestreoType.text = "Sin Asignar"; // Set the sampling type text to indicate no assignment
        nameText.text = "Sin Asignar";     // Set the species name text to indicate no assignment
        Image.sprite = null;               // Clear the image display to indicate no image selected
    }

    /// <summary>
    /// Validates the user's current responses against stored answers, calculating the final score based on matches.
    /// </summary>
    void validar()
    {
        bool found = false;       // Indicates if an exact match was found
        int indexToRemove = -1;   // Index of the stored answer to remove (not used here)
        int maxCurrentAns = 0;    // Tracks the maximum score for partial matches

        // Loop through stored answers to find a match or the best partial match
        for (int i = 0; i < 1; i++)
        {
            // Create a placeholder for a stored answer
            Answer storedAnswer = new Answer();
            storedAnswer.quantity = 1;
            storedAnswer.imgId = 0;
            storedAnswer.minigameId = 0;
            storedAnswer.idEspecie = 0;

            // Check if the current user's response exactly matches the stored answer
            if (storedAnswer.minigameId == muestreoID &&
                storedAnswer.idEspecie == NameId &&
                storedAnswer.imgId == ImageId &&
                storedAnswer.quantity == quantityInt)
            {
                found = true;             // Exact match found
                indexToRemove = i;        // Set index to remove (not used)
                indexBest = -3;           // Set indexBest to a specific value indicating exact match
                break;                    // Exit the loop as we found an exact match
            }
            else
            {
                int tempScore = 0;       // Initialize a temporary score for partial matches

                // Check each field for partial matches and increment score accordingly
                if (storedAnswer.minigameId == muestreoID)
                {
                    tempScore += 1;      // Partial match for minigameId
                }
                if (storedAnswer.idEspecie == NameId)
                {
                    tempScore += 1;      // Partial match for idEspecie
                }
                if (storedAnswer.imgId == ImageId)
                {
                    tempScore += 1;      // Partial match for imgId
                }
                if (storedAnswer.quantity == quantityInt)
                {
                    tempScore += 1;      // Partial match for quantity
                }

                // Update the best partial match index and score
                if (tempScore > maxCurrentAns)
                {
                    indexBest = i;             // Update indexBest with the current index
                    maxCurrentAns = tempScore; // Update the maximum partial score
                }
            }
        }

        // Update the final score based on whether an exact match was found
        if (found)
        {
            finalScore += 4;            // Award full points for an exact match
        }
        else
        {
            finalScore += maxCurrentAns; // Award points for the best partial match
        }
    }

    /// <summary>
    /// Handles the completion of the tutorial or task by updating UI elements.
    /// </summary>
    public void finished()
    {
        // Deactivates the iPad panel, indicating that the tutorial or task is complete
        ipadPanel.SetActive(false);

        // Makes the 'cerrar' button interactable, allowing the user to proceed or close
        cerrar.interactable = true;
    }



}
