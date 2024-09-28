using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class ClickOnInsecto : MonoBehaviour
{
    public LampOn lampara;
    public Button[] buttonsInsectosUI;
    public GeneradorInsectos generador;
    public List<InsectoInfo> Disponibles = new List<InsectoInfo>();
    private List<InsectoInfo> Descubiertos = new List<InsectoInfo>();

    private int timecomparar = 0;

    private int? idSeleccionadoUI = null;
    private Button botonSeleccionadoUI = null;
    private Button currentlySelectedButton = null;
    // private int? idSeleccionadoJuego = null;
    // private Button botonSeleccionadoJuego = null;
    public Text textoPuntuacion; // Asigna este objeto en el Inspector.
    private float puntuacion = 0;
    private int Equivocaciones = 0;

    public AudioClip correct;
    public AudioClip incorrect;

    public static ClickOnInsecto Instance;

    public ListaAnimales animales;
    public AnswersData answers;
    public AnswersData Happy;

    bool first = false;

    int uniqueRegisters;

    public float returnPuntuacion()
    {
        return puntuacion;
    }

    public int returnEquivocaciones()
    {
        return Equivocaciones;
    }

    // M�todo para restablecer el color del bot�n a normal
    private void SetButtonAsSelected(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.gray; // Indica visualmente que este bot�n est� seleccionado
        button.colors = colors;
    }

    // M�todo para restablecer el color del bot�n a normal
    private void ResetButtonColor(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // Restablece el color normal del bot�n
        button.colors = colors;
    }

    private void Awake()
    {
        // Si la instancia no existe, la establece a este script.
        if (Instance == null)
        {
            Instance = this;
            // Opcional: si quieres que el spawner persista entre escenas, descomenta la siguiente l�nea.
            // DontDestroyOnLoad(gameObject);
        }
        // Si la instancia existe y no es este script, destruye este objeto.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicialmente, desactiva los botones para que no puedan ser clickeados.
        SetButtonsInteractable(false);

        // ShowSavedDataInsects.instance.ShowInit();
        // Debug.Log("AfterInit");

        // Inicia la coroutine para aumentar el tiempo y luego activar los botones.
        StartCoroutine(AumentaTiempoYActivaBotones());

        foreach (var button in buttonsInsectosUI)
        {
            // Aseg�rate de que este m�todo es llamado cuando el bot�n es clickeado
            button.onClick.AddListener(() => ButtonClicked(button));
        }

        foreach (var Insectobutton in generador.getInsectosGenerados())
        {
            // Aseg�rate de que este m�todo es llamado cuando el bot�n es clickeado
            Button ButonInsecto = Insectobutton.GetComponent<Button>();
            ButonInsecto.onClick.AddListener(() => InsectoClicked(ButonInsecto, Insectobutton));
        }
    }

    IEnumerator AumentaTiempoYActivaBotones()
    {
        // Espera hasta que timecomparar sea igual a lampara.timeencender
        while (timecomparar <= lampara.timeencender)
        {
            yield return new WaitForSeconds(1); // Espera un segundo
            timecomparar++; // Aumenta el contador de tiempo
            print(timecomparar);
        }

        // Una vez que es el tiempo adecuado, haz que los botones sean clickeables.
        SetButtonsInteractable(true);
    }

    // Este m�todo es llamado cuando uno de los botones es clickeado
    private void ButtonClicked(Button buttonClicked)
    {
        if (currentlySelectedButton != null && currentlySelectedButton != buttonClicked)
        {
            ResetButtonColor(currentlySelectedButton);
            ResetSeleccion();
        }

        // Guarda el bot�n actualmente seleccionado y cambia su color.
        currentlySelectedButton = buttonClicked;
        SetButtonAsSelected(currentlySelectedButton);


        int idUI = buttonClicked.gameObject.GetComponent<ButtonUI>().UniqueID;
        //Debug.Log("Bot�n UI clickeado: " + buttonClicked.name + " Con ID " + idUI);

        idSeleccionadoUI = idUI;
        botonSeleccionadoUI = buttonClicked;

        // Restablece el color del bot�n previamente seleccionado, si lo hay.
        /*
        // Verificar si ya se ha seleccionado un insecto del juego y comparar IDs
        if (idSeleccionadoJuego.HasValue)
        {
            if (idSeleccionadoJuego.Value == idUI)
            {
                //Debug.Log("Coincidencia encontrada");
                puntuacion++;
                textoPuntuacion.text = "Puntuaci�n: " + puntuacion;

                AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);

                // Desactivar la interacci�n del bot�n UI, pero mantenerlo visible
                botonSeleccionadoUI.interactable = false;
                // Desactivar el GameObject del insecto en el juego para que "desaparezca"
                if (botonSeleccionadoJuego != null)
                {
                    botonSeleccionadoJuego.gameObject.SetActive(false);
                }
                //Debug.Log("Intentando a�adir a Descubiertos, ID: " + idUI);
                foreach (InsectoInfo insectoinfo in Disponibles)
                {
                    if (insectoinfo.UniqueId == idUI)
                    {
                        //Debug.Log("Encontrado en Disponibles: " + insectoinfo.Name);
                        if (!Descubiertos.Contains(insectoinfo))
                        {
                            Descubiertos.Add(insectoinfo);
                            //Debug.Log("A�adido a Descubiertos: " + insectoinfo.Name);
                        }
                    }
                }
                // ResetSeleccion();
                
            }
            else
            {
                Equivocaciones++;
                AudioSource.PlayClipAtPoint(incorrect, Camera.main.transform.position, 0.5f);
            }
        }
        */
        // Si no hay insecto del juego seleccionado previamente, esperar a la siguiente selecci�n
    }


    private void InsectoClicked(Button buttonClicked, Insecto insecto)
    {
        int id = insecto.GetID();

        // Verificar si ya se ha seleccionado un bot�n UI y comparar IDs
        if (idSeleccionadoUI.HasValue && idSeleccionadoUI.Value == id)
        {
            puntuacion+=12.5f;
            textoPuntuacion.text = "Puntuaci�n: " + (int)puntuacion;
            AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position);

            botonSeleccionadoUI.interactable = false;
            buttonClicked.gameObject.SetActive(false);

            InsectoInfo insectoInfo = Disponibles.FirstOrDefault(i => i.UniqueId == id);
            if (insectoInfo != null && !Descubiertos.Contains(insectoInfo))
            {
                Descubiertos.Add(insectoInfo);
                animales.Insectos.Add(insectoInfo);

                // Aqu� manejamos la l�gica de Answer
                Answer existingAnswer2 = answers.answers.FirstOrDefault(a => a.idEspecie == id);
                if (existingAnswer2 == null)
                {
                    answers.answers.Add(new Answer
                    {
                        imgId = id,
                        idEspecie = id,
                        minigameId = 3, // Asumiendo un ID de minijuego espec�fico para insectos
                        quantity = 0
                    });
                    uniqueRegisters++;
                }
            }

            // Aqu� manejamos la l�gica de Answer
            Answer existingAnswer = answers.answers.FirstOrDefault(a => a.idEspecie == id);
            if (existingAnswer != null)
            {
                existingAnswer.quantity++;
            }

            if (!first)
            {
                first = true;
                // Asumiendo que hay una funci�n para mostrar datos guardados o alguna interfaz inicial
                // ShowSavedDataInsects.instance.ShowInit();
            }

            if (puntuacion >= 100) // Si alcanzas un cierto umbral de puntuaci�n, finaliza el minijuego
            {
                ShowEndMinigameIfPossible();
            }
        }
        else
        {
            Equivocaciones++;
            // Agregar funcion error
            int MatchID = PlayerPrefs.GetInt("MatchID");
            StartCoroutine(SendError(MatchID, 4));
            AudioSource.PlayClipAtPoint(incorrect, Camera.main.transform.position);
        }
    }

    public IEnumerator SendError(int MatchID, int MistakeID)
    {
        string minigameURL = "https://10.22.156.99:7026/api/Videogame/Mistakes";

        // Using Newtonsoft.Json to serialize the data
        var errorData = new
        {
            matchID = MatchID,
            mistakeID = MistakeID
        };
        string json = JsonConvert.SerializeObject(errorData);
        Debug.Log("Sending JSON: " + json);  // This will print the JSON string to the Unity Console


        // Setting up the UnityWebRequest
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(minigameURL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Custom certificate handler if needed
        webRequest.certificateHandler = new ForceAceptAll();
        yield return webRequest.SendWebRequest();
        webRequest.certificateHandler?.Dispose();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error en el registro");
        }
        else
        {
            Debug.Log("Registro Exitoso");
        }
    }



    public void ShowEndMinigameIfPossible()
    {
        if (GeneradorInsectos.instance != null)
        {
            puntuacion = puntuacion - (Equivocaciones * 2);
            Happy.answers = answers.answers;
            PlayerPrefs.SetInt("RegisterNum", uniqueRegisters);
            PlayerPrefs.SetFloat("PuntuacionFinal", puntuacion);
            GeneradorInsectos.instance.showEndMinigame();
        }
        else
        {
            Debug.LogError("La instancia de GeneradorInsectos es nula.");
        }
    }


    // M�todo para reiniciar las selecciones
    private void ResetSeleccion()
    {
        botonSeleccionadoUI.interactable = true;
        idSeleccionadoUI = null;
        botonSeleccionadoUI = null;
        //idSeleccionadoJuego = null;
        //botonSeleccionadoJuego = null;
    }


    public List<InsectoInfo> RegresarInsectosIdentificados()
    {
        return Descubiertos;
    }

    // M�todo para activar o desactivar la interactividad de todos los botones
    private void SetButtonsInteractable(bool interactable)
    {
        foreach (var button in buttonsInsectosUI)
        {
            button.interactable = interactable;
        }
        foreach (Insecto insecto in generador.getInsectosGenerados())
        {
            insecto.GetComponent<Button>().interactable = interactable;
        }
    }


}
