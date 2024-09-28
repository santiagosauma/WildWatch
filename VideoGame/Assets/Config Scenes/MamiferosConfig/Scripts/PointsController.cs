using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public Text PointsText;
    public Text PointsPupUpText;
    private int points = 0;
    private int puntosMaximos = 0;
    private string HuellaprefabTag = "Huella";
    private string TrampaprefabTag = "PlaceableObject";
    private List<GameObject> huellas = new List<GameObject>();
    private List<GameObject> trampas = new List<GameObject>();
    public AudioClip pointSound;
    public ReturnHouse Comandos;
    private float zoomSpeed = 1.0f;
    private float minZoom = 10f;
    private float maxZoom = 19.5f;

    public ListaAnimales animales;
    public AnswersData Answers;

    public MamiferosInfo[] hprebas = new MamiferosInfo[3];

    int UniqueRegisters;

    public void Start()
    {
        UniqueRegisters = 0;
        AgregarHuellas();
        puntosMaximos = huellas.Count * 20; 
    }

    private void Update()
    {
        ActualizarPuntos();
    }
    public void EmpezarACalcularPuntos()
    {
        
        StartCoroutine(ProcesarPuntosSecuenciales());

    }

    public void SetZoom(bool zoomIn)
    {
        float zoomChange = zoomIn ? -zoomSpeed : zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + zoomChange, minZoom, maxZoom);
    }

    IEnumerator ProcesarPuntosSecuenciales()
    {   
        AgregarTrampas();
        AsociarTrampasAHuellas();
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
        SetZoom(false);
        Comandos.DisableMovement();
        Comandos.DisableOtherButtons();
        foreach (GameObject huella in huellas)
        {
            GameObject trampaAsignada = huella.GetComponent<Huellas>().getTrampa();
            int puntosCalculados = CalcularPuntosPorTrampa(huella, trampaAsignada);
            points += puntosCalculados;

            // Muestra puntos y espera antes de continuar
            yield return StartCoroutine(WaitForNextPointAdding(trampaAsignada, puntosCalculados));
        }

        // Opcionalmente, mostrar alg�n UI o mensaje al finalizar todos los c�lculos
        showEndMinigame();
    }

    public void AsociarTrampasAHuellas()
    {
        Debug.Log("InicioAsociarTrampasAHuellas");
        //Ciclo para iterar por las huellas
        foreach (GameObject huella in huellas)
        {
            Debug.Log("CicloHuellas");
            float distanciaMaxima = float.MaxValue;
            Debug.Log("Distancia Maxima" + distanciaMaxima);
            Vector2 posicionHuella = new Vector2(huella.transform.position.x, huella.transform.position.y);

            foreach (GameObject trampa in trampas)
            {
                Debug.Log("CicloTrampas");

                //Ciclo para iterar por todas las trampas por cada huella
                Vector2 posicionTrampa = new Vector2(trampa.transform.position.x, trampa.transform.position.y);

                //Calcular la distancia entre la huella y cada trampa
                float distancia = Vector2.Distance(posicionHuella, posicionTrampa);
                Debug.Log("Distancia entre huella y camara" + distancia);

                if (distancia < distanciaMaxima && distancia <= 3f)
                {
                    Debug.Log("Asignando: ");
                    Debug.Log(trampa.GetComponentInChildren<SpriteRenderer>().sprite.name);
                    huella.GetComponent<Huellas>().AsignarTrampa(trampa);
                    distanciaMaxima = distancia;
                }
                else
                {
                    continue;
                }
            }
        }

        foreach (GameObject huella in huellas)
        {
            Debug.Log("Asociacion");
            Debug.Log(huella.GetComponentInChildren<SpriteRenderer>().sprite.name + huella.GetComponent<Huellas>().GetTrampaAsignada());
        }
    }

    void ObtenerHuellas()
    {
        bool g = false, m = false, p = false;

        foreach (GameObject huella in huellas)
        {
            if(huella.GetComponentInChildren<SpriteRenderer>().sprite.name == "HuellaGrande")
            {
                if(g)
                {
                    for(int i = 0; i < Answers.answers.Count; i++)
                    {
                        if (Answers.answers[i].idEspecie == 0)
                        {
                            Answers.answers[i].quantity += 1;
                        }
                    }
                }
                else
                {
                    g = true;
                    Answer answer = new Answer();
                    answer.imgId = 0;
                    answer.idEspecie = 0;
                    answer.minigameId = 2;
                    answer.quantity = 1;
                    Answers.answers.Add(answer);
                    Debug.Log(UniqueRegisters);
                    UniqueRegisters += 1;
                    Debug.Log(UniqueRegisters);

                }

            }

            if (huella.GetComponentInChildren<SpriteRenderer>().sprite.name == "HuellaMediana")
            {
                if (m)
                {
                    for (int i = 0; i < Answers.answers.Count; i++)
                    {
                        if (Answers.answers[i].idEspecie == 1)
                        {
                            Answers.answers[i].quantity += 1;
                        }
                    }
                }
                else
                {
                    m = true;
                    Answer answer = new Answer();
                    answer.imgId = 1;
                    answer.idEspecie = 1;
                    answer.minigameId = 2;
                    answer.quantity = 1;
                    Answers.answers.Add(answer);
                    Debug.Log(UniqueRegisters);
                    UniqueRegisters += 1;
                    Debug.Log(UniqueRegisters);

                }
            }
            if (huella.GetComponentInChildren<SpriteRenderer>().sprite.name == "HuellaChica")
            {
                if (p)
                {
                    for (int i = 0; i < Answers.answers.Count; i++)
                    {
                        if (Answers.answers[i].idEspecie == 2)
                        {
                            Answers.answers[i].quantity += 1;
                        }
                    }
                }
                else
                {
                    p = true;
                    Answer answer = new Answer();
                    answer.imgId = 2;
                    answer.idEspecie = 2;
                    answer.minigameId = 2;
                    answer.quantity = 1;
                    Debug.Log(answer);
                    Answers.answers.Add(answer);
                    Debug.Log(UniqueRegisters);
                    UniqueRegisters+=1;
                    Debug.Log(UniqueRegisters);

                }
            }

            Debug.Log("Asociacion");
            Debug.Log(huella.GetComponentInChildren<SpriteRenderer>().sprite.name + huella.GetComponent<Huellas>().GetTrampaAsignada());
        }
    }

    public void CalcularPuntos()
    {
        foreach (GameObject huella in huellas)
        {
            GameObject trampahuellaAsignada = huella.GetComponent<Huellas>().getTrampa();

            int puntos = CalcularPuntosPorTrampa(huella, trampahuellaAsignada);
            points += puntos;
        }
        //showEndMinigame();
    }

    public int CalcularPuntosPorTrampa(GameObject huella, GameObject trampa)
    {
        int puntoscalculados = 0;

        string trampatipo;
        string huellatipo = huella.GetComponentInChildren<SpriteRenderer>().sprite.name.ToString();

        Debug.Log(huellatipo);
        if (trampa != null)
        {
            trampatipo = trampa.GetComponentInChildren<SpriteRenderer>().sprite.name.ToString();
        }
        else
        {
            trampatipo = "";
        }
        Debug.Log(trampatipo);
        int matchid = PlayerPrefs.GetInt("MatchID");
        switch (huellatipo)
        {
            case "HuellaGrande":

                switch (trampatipo)
                {
                    case "Camara":
                        puntoscalculados += 20;
                        break;
                    case "ShermanTrap":
                        puntoscalculados += 5;
                        StartCoroutine(SendError(matchid, 3));
                        break;
                    case "Tomahok":
                        puntoscalculados += 20;
                        break;
                    default:
                        StartCoroutine(SendError(matchid, 3));
                        break;
                }
                break;
            case "HuellaMediana":
                switch (trampatipo)
                {
                    case "Camara":
                        puntoscalculados += 10;
                        StartCoroutine(SendError(matchid, 3));
                        break;
                    case "ShermanTrap":
                        puntoscalculados += 5;
                        StartCoroutine(SendError(matchid, 3));
                        break;
                    case "Tomahok":
                        puntoscalculados += 20;
                        break;
                    default:
                        StartCoroutine(SendError(matchid, 3));
                        break;
                }
                break;
            case "HuellaChica":
                switch (trampatipo)
                {
                    case "Camara":
                        puntoscalculados += 5;
                        StartCoroutine(SendError(matchid, 3));
                        break;
                    case "ShermanTrap":
                        puntoscalculados += 20;
                        break;
                    case "Tomahok":
                        puntoscalculados += 10;
                        StartCoroutine(SendError(matchid, 3));
                        break;
                    default:
                        StartCoroutine(SendError(matchid, 3));
                        break;
                }
                break;
        }
              
        return puntoscalculados;
    }

    IEnumerator WaitForNextPointAdding(GameObject trampa, int puntoscalculados)
    {
        if (trampa != null) 
        {
            PointsPupUpText.transform.position = trampa.transform.position; 
        }
       
        PointsPupUpText.gameObject.SetActive(true);
        PointsPupUpText.text = "+" + puntoscalculados;
        AudioSource.PlayClipAtPoint(pointSound, PointsPupUpText.transform.position, 0.5f);
        yield return new WaitForSeconds(1f);

        PointsPupUpText.gameObject.SetActive(false);
    }

    public void ActualizarPuntos()
    {
        PointsText.text = "Puntuaci�n: " + points;
    }

    void AgregarHuellas()
    {
        GameObject[] instances = GameObject.FindGameObjectsWithTag(HuellaprefabTag);

        foreach (GameObject instance in instances)
        {
            print(instance.GetComponentInChildren<SpriteRenderer>().sprite.name);
            huellas.Add(instance);
            print("Huella agregada");
        }
    }

    void AgregarTrampas()
    {
        GameObject[] instances = GameObject.FindGameObjectsWithTag(TrampaprefabTag);

        foreach (GameObject instance in instances)
        {
            print(instance.GetComponentInChildren<SpriteRenderer>().sprite.name);
            trampas.Add(instance);
            print("Trampa Agregada");
        }
    }

    private int huellasIdentificadas()
    {
        int cantidad = 0;
        foreach(var huella in huellas)
        {
            if (huella.GetComponent<Huellas>().getTrampa() != null)
            {
                cantidad++;
            }
        }
        return cantidad;
    }

    //public GameObject uiText;
    public GameObject uiEnd;
    public Text PuntuacionEnd;
    public Text HuellasEnd;
    public Button buttonListo;

    public void showEndMinigame()
    {
        Comandos.EnableOtherButtons();
        PuntuacionEnd.text = $"Puntuaci�n Obtenida: {points}";
        PlayerPrefs.SetFloat("PuntuacionFinal", points);
        int registros = Answers.answers.Count;

        PlayerPrefs.SetInt("RegisterNum", UniqueRegisters);

        animales.Mamiferos.Add(hprebas[0]);
        animales.Mamiferos.Add(hprebas[1]);
        animales.Mamiferos.Add(hprebas[2]);
        ObtenerHuellas();
        HuellasEnd.text = $"Huellas Identificadas: {huellasIdentificadas()}";
        uiEnd.SetActive(true);
        buttonListo.gameObject.SetActive(false);


        
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
    public void goHomeEnd()
    {
        PlayerPrefs.SetFloat("PuntuacionFinal", points);
        PlayerPrefs.SetInt("RegisterNum", UniqueRegisters);
        SceneManager.LoadScene("HouseScene");
    }

}