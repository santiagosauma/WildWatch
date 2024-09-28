using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TabletInfoDisplay : MonoBehaviour
{
    List<string> muestreos = new List<string>();

    public List<List<string>> infoEspecies = new List<List<string>>();
    public List<List<Sprite>> imagenEspecies = new List<List<Sprite>>();

    public List<string> infoInsectos = new List<string>();
    public List<string> infoAves = new List<string>();
    public List<string> infoAnfibios = new List<string>();
    public List<string> infoPlantas = new List<string>();
    public List<string> infoMamiferos = new List<string>();

    public List<Sprite> imgInsectos = new List<Sprite>();
    public List<Sprite> imgAves = new List<Sprite>();
    public List<Sprite> imgAnfibios = new List<Sprite>();
    public List<Sprite> imgPlantas = new List<Sprite>();
    public List<Sprite> imgMamiferos = new List<Sprite>();

    public Text quantity;
    private int quantityInt = 0;

    public Text muestreoType;
    private int muestreoID = -1;

    public Text nameText;
    private int NameId = -1;

    public Image Image;
    private int ImageId = -1; // Variable para manejar el �ndice de la imagen actual

    public List<Image> status = new List<Image>();
    public Sprite correct;
    public Sprite incorrect;
    bool m = false, n = false, im = false, q = false;

    public Button send;

    public Image warning;

    public GameObject results;
    public GameObject preResults;
    public GameObject forms;
    public CircularProgressBar progressBar;

    public AnswersData AnswersData;
    int availableTries;
    public Text registerTries;

    float finalScore = 0;

    int maxScore;
    int indexBest;
    bool[] responses = new bool[4];

    public List<Image> notFinalMarks = new List<Image>();

    public Text puntuacion;
    public Text estatusNivel;
    public Image fondo;

    public AnswersData Happy;

    public Button cerrar;
    public GameObject ipadPanel;

    int maxPossible = 0;

    public LecturaBaseDatos EnviarRecibirDatos;
    /*
    void Awake()
    {
        if (AnswersData == null)
        {
            Debug.LogError("AnswersData no est� asignado en el inspector!");
        }
        else
        {
            Debug.Log("AnswersData cargado correctamente con " + AnswersData.answers.Count + " respuestas.");
        }
    }
    */

    void prueba()
    {
        Answer answer = new Answer();
        answer.minigameId = 0;
        answer.idEspecie = 2;
        answer.imgId = 2;
        answer.quantity = 2;
        AnswersData.answers.Add(answer);
        answer.minigameId = 0;
        answer.idEspecie = 3;
        answer.imgId = 3;
        answer.quantity = 1;
        AnswersData.answers.Add(answer);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxPossible = Happy.answers.Count;

        //prueba();
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

    public void nextMuestreo()
    {
        muestreoID = (muestreoID + 1) % muestreos.Count;
        muestreoType.text = muestreos[muestreoID];
        ResetEspecieImagenIndex();
        warning.gameObject.SetActive(false);

    }

    public void backMuestreo()
    {
        muestreoID = (muestreoID - 1 + muestreos.Count) % muestreos.Count;
        muestreoType.text = muestreos[muestreoID];
        ResetEspecieImagenIndex();
        warning.gameObject.SetActive(false);

    }

    private void ResetEspecieImagenIndex()
    {
        NameId = 0;
        ImageId = 0;
        UpdateTextAndImage();
    }

    public void nextImg()
    {
        if(muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }
        ImageId = (ImageId + 1) % imagenEspecies[muestreoID].Count;
        UpdateImage();
    }

    public void backImg()
    {
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }
        ImageId = (ImageId - 1 + imagenEspecies[muestreoID].Count) % imagenEspecies[muestreoID].Count;
        UpdateImage();
    }

    public void nextEspecie()
    {
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }
        NameId = (NameId + 1) % infoEspecies[muestreoID].Count;
        UpdateTextAndImage();
    }

    public void BackEspecie()
    {
        if (muestreoID == -1)
        {
            warning.gameObject.SetActive(true);
            return;
        }
        NameId = (NameId - 1 + infoEspecies[muestreoID].Count) % infoEspecies[muestreoID].Count;
        UpdateTextAndImage();
    }

    private void UpdateTextAndImage()
    {
        nameText.text = infoEspecies[muestreoID][NameId];
        if (imagenEspecies[muestreoID].Count > 0)
        {
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }

        status[0].sprite = correct;
        status[1].sprite = correct;
        status[2].sprite = correct;
        m = true;
        n = true;
        im = true;

    }

    private void UpdateImage()
    {
        if (imagenEspecies[muestreoID].Count > 0)
        {
            Image.sprite = imagenEspecies[muestreoID][ImageId];
        }
    }

    public void plusCantidad()
    {
        quantityInt++;
        quantity.text = quantityInt.ToString();
        status[3].sprite = correct;
        q = true;

    }

    public void minusCantidad()
    {
        if (quantityInt > 0)
            quantityInt--;
        quantity.text = quantityInt.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(m && n &&  im && q)
        {
            send.interactable = true;
        }
    }

    public void sendRegister()
    {
        availableTries--;
        int uniqueAvailable = PlayerPrefs.GetInt("RegisterNum", 0);
        uniqueAvailable--;
        PlayerPrefs.SetInt("RegisterNum", uniqueAvailable);
        forms.SetActive(false);
        validar();

        if(availableTries == 0)
        {
            //float num = 50/100f;
            finalScore = finalScore * 100 / maxScore;

            finalScore = (finalScore * 60) / 100;

            float finalPuntuacion = PlayerPrefs.GetFloat("PuntuacionFinal", 0);
            finalPuntuacion = (finalPuntuacion * 40) / 100;
            
            cerrar.interactable = false;

            float num = finalScore + finalPuntuacion;
            //Debug.Log(num);
            progressBar.setFill((int)num/100f);
            puntuacion.text = $"{num}/100";

            if(num >= 80)
            {

                estatusNivel.text = "Completado";
                fondo.color = new Color(147f / 255f, 255f / 255f, 97f / 255f, 255f / 255f);
                
            }
            else
            {
                int matchid = PlayerPrefs.GetInt("MatchID");
                StartCoroutine(SendError(matchid, 2));
            }

            int matchID = PlayerPrefs.GetInt("MatchID");
            int userID = PlayerPrefs.GetInt("UserID");
            Debug.Log("userID " + userID);   
            int minigameIDVideojuego = PlayerPrefs.GetInt("FinalOption");
            Debug.Log("MinijuegoID " + minigameIDVideojuego);
            int minigameIDBaseDatos = ObtenerIDMinijuego(minigameIDVideojuego);
            Debug.Log("MinijuegoIDBD " + minigameIDBaseDatos);
            int tiempominijuego = PlayerPrefs.GetInt("TiempoJuego");
            Debug.Log("Tiempo " + tiempominijuego);
            StartCoroutine(EnviarRecibirDatos.UpdateMinigameInfo(matchID, userID, minigameIDBaseDatos,(int) num ,tiempominijuego));
            StartCoroutine(EnviarRecibirDatos.GetPoints());
            
            results.SetActive(true);
        }
        else
        {
            registerTries.text = $"Registros Restantes: {availableTries}";
            preResults.SetActive(true);
            if(indexBest == -3)
            {
                for(int i = 0; i < 4; i++)
                {
                    notFinalMarks[i].sprite = correct;
                }
            }else
            {
                Answer storedAnswer = AnswersData.answers[indexBest];
            

                if (storedAnswer.minigameId == muestreoID)
                {
                    notFinalMarks[0].sprite = correct;
                }
                else
                {
                    notFinalMarks[0].sprite = incorrect;
                }
                if (storedAnswer.idEspecie == NameId)
                {
                    notFinalMarks[1].sprite = correct;
                }
                else
                {
                    notFinalMarks[1].sprite = incorrect;
                }
                if (storedAnswer.imgId == ImageId)
                {
                    notFinalMarks[2].sprite = correct;
                }
                else
                {
                    notFinalMarks[2].sprite = incorrect;
                }
                if (storedAnswer.quantity == quantityInt)
                {
                    notFinalMarks[3].sprite = correct;
                }
                else
                {
                    notFinalMarks[3].sprite = incorrect;
                }
            }
         
        }

    }

    public void anotherRegister()
    {

        // Restablecer la interfaz de usuario de resultados previos para permitir un nuevo ingreso
        preResults.SetActive(false);

        // Restablecer los estados de validaci�n y la interfaz de usuario relacionada
        ResetUIAndStates();

        // Restablecer cualquier otro estado necesario para un nuevo registro
        ResetFormStates();

        // Reactivar el formulario para un nuevo registro
        forms.SetActive(true);
    }

    // Funci�n para restablecer la interfaz de usuario y los estados de validaci�n
    private void ResetUIAndStates()
    {
        // Restablecer las im�genes de estado a una imagen predeterminada o vaciarlas
        foreach (var image in status)
        {
            image.sprite = incorrect; // o asignar una imagen de 'incompleto' si disponible
        }

        // Restablecer indicadores de progreso o validaci�n
        m = n = im = q = false;

        // Restablecer la interactividad del bot�n de env�o
        send.interactable = false;

        // Ocultar advertencias si est�n visibles
        warning.gameObject.SetActive(false);

        // Restablecer el texto de cantidad y la cantidad interna
        quantityInt = 0;
        quantity.text = "0";

        indexBest = 0;
    }

    // Funci�n para restablecer los campos del formulario y cualquier dato asociado
    private void ResetFormStates()
    {
        // Restablecer los �ndices seleccionados a sus valores iniciales
        muestreoID = -1;
        NameId = -1;
        ImageId = -1;

        muestreoType.text = "Sin Asignar";
        nameText.text = "Sin Asignar";
        Image.sprite = null;
    }

    void validar()
    {
        bool found = false;
        int indexToRemove = -1;

        int maxCurrentAns = 0;

        // Revisa cada respuesta almacenada para ver si alguna coincide con la respuesta actual del usuario
        for (int i = 0; i < AnswersData.answers.Count; i++)
        {
            Answer storedAnswer = AnswersData.answers[i];

            // Verifica si la respuesta actual coincide con la almacenada
            if (storedAnswer.minigameId == muestreoID &&
                storedAnswer.idEspecie == NameId &&
                storedAnswer.imgId == ImageId &&
                storedAnswer.quantity == quantityInt)
            {
                found = true;
                indexToRemove = i;
                indexBest = -3;
                break;
            }else
            {
                int tempScore = 0;
                int matchid = PlayerPrefs.GetInt("MatchID");
                StartCoroutine(SendError(matchid,1));
                // Verifica cada campo individualmente y asigna puntos por aciertos parciales
                if (storedAnswer.minigameId == muestreoID)
                {
                    tempScore += 1;  // Acierto parcial en idEspecie
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
                if (tempScore > maxCurrentAns)
                {
                    indexBest = i;
                    maxCurrentAns = tempScore;
                }
            }
        }

        if (found)
        {
            // Incrementar la puntuaci�n si la respuesta es correcta
            finalScore += 4; // Asumiendo que cada respuesta correcta vale 4 puntos
                             // Eliminar la respuesta acertada de la lista para no volver a validarla
            if (indexToRemove != -1)
                AnswersData.answers.RemoveAt(indexToRemove);

            // Opcional: Mostrar alg�n feedback al usuario
            Debug.Log("Respuesta correcta! Puntuaci�n: " + finalScore);
        }
        else
        {
            finalScore += maxCurrentAns;
            // Opcional: Manejar respuestas incorrectas
            //Debug.Log("Respuesta incorrecta!");
        }
    }

    public void finished()
    {
        ipadPanel.SetActive(false);
        cerrar.interactable = true; 

    }

    private int ObtenerIDMinijuego(int minijuegoIDVideojuego)
    {   /*
               * minigameID Base de datos
               * 1 Aves
               * 2 Mamiferos
               * 3 Anfibios
               * 4 Insectos
               * 5 Flora
               * minigameID Videojuego
               * 1 Aves
               * 3 Mamiferos
               * 2 Anfibios
               * 4 Insectos
               * 5 Flora
        */
        int minijuegoIDBaseDatos = 0;

        if (minijuegoIDVideojuego == 1) minijuegoIDBaseDatos = 1;
        else if (minijuegoIDVideojuego == 2) minijuegoIDBaseDatos = 3;
        else if(minijuegoIDVideojuego == 3) minijuegoIDBaseDatos = 2;
        else if(minijuegoIDVideojuego == 4) minijuegoIDBaseDatos = 4;
        else if(minijuegoIDVideojuego == 5) minijuegoIDBaseDatos = 5;
        

        return minijuegoIDBaseDatos;
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

}


/*
public class TabletInfoDisplay : MonoBehaviour
{
    List<string> muestreos = new List<string>();

    public List<List<string>> infoEspecies = new List<List<string>>();
    public List<List<Sprite>> imagenEspecies = new List<List<Sprite>>();

    public List<string> infoInsectos = new List<string>();
    public List<string> infoAves = new List<string>();
    public List<string> infoAnfibios = new List<string>();
    public List<string> infoPlantas = new List<string>();
    public List<string> infoMamiferos = new List<string>();

    public List<Sprite> imgInsectos = new List<Sprite>();
    public List<Sprite> imgAves = new List<Sprite>();
    public List<Sprite> imgAnfibios = new List<Sprite>();
    public List<Sprite> imgPlantas = new List<Sprite>();
    public List<Sprite> imgMamiferos = new List<Sprite>();

    public Text quantity;
    private int quantityInt = 0;

    public Text muestreoType;
    private int muestreoID = -1;

    public Text nameText;
    private int NameId = -1;

    public Image Image;
    private int ImageId = -1;

    // Start is called before the first frame update
    void Start()
    {

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

    public void nextMuestreo()
    {
        muestreoID++;
        if(muestreoID > muestreos.Count - 1)
        {
            muestreoID = 0;
        }

        muestreoType.text = muestreos[muestreoID];
    }

    public void backMuestreo()
    {
        muestreoID--;
        if(muestreoID < 0)
        {
            muestreoID = muestreos.Count - 1;
        }
        muestreoType.text = muestreos[muestreoID];
    }

    public void nextImg()
    {

    }

    public void backImg()
    {

    }

    public void nextEspecie()
    {
        NameId++;

        if (NameId > infoEspecies[muestreoID].Count - 1)
        {
            NameId = 0;
        }

        // Debug.Log(infoEspecies[muestreoID][NameId]);
        nameText.text = infoEspecies[muestreoID][NameId];

    }

    public void BackEspecie()
    {
        NameId--;
        if (NameId < 0)
        {
            NameId = infoEspecies[muestreoID].Count - 1;
        }

        nameText.text = infoEspecies[muestreoID][NameId];
    }

    public void plusCantidad()
    {
        quantityInt++;
        //Debug.Log(quantityInt);
        quantity.text = quantityInt.ToString();
    }

    public void minusCantidad()
    {
        if(quantityInt > 0)
        {
            quantityInt--;
        }
        quantity.text = quantityInt.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/