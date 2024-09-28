using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneradorInsectos : MonoBehaviour
{
    public GameObject insectoPrefab; // Prefab del insecto
    public int cantidadInsectos = 10; // Cantidad de insectos a generar
    public Sprite[] spritesInsectos;
    private Insecto[] insectosGenerados;
    public Sprite imagenAcambiar;
    public Canvas CanvasUI;
    [SerializeField] private GameObject spawner;
    //private int time = 0;
    private int time = 0;

    // Nuevo: Diccionario para asignar una ID a cada sprite
    public Dictionary<Sprite, int> spriteIDMap = new Dictionary<Sprite, int>();

    public static GeneradorInsectos instance;

    private void Awake()
    {
        // Verifica si instance ya está establecida a otro GeneradorInsectos en la escena
        if (instance != null && instance != this)
        {
            // Si es así, destruye este objeto ya que debe haber solo una instancia de GeneradorInsectos
            Destroy(this.gameObject);
            return; // Sale del método Awake para prevenir que se ejecuten las siguientes líneas de código
        }

        // Si instance no está establecida, o está establecida a este objeto (this), 
        // entonces asigna this a instance y opcionalmente marca para no ser destruido al cargar una nueva escena
        instance = this;
        // DontDestroyOnLoad(this.gameObject); // Comenta esta línea si deseas que el objeto se destruya al cargar una nueva escena
        
        // Inicializar el diccionario con sprites e IDs
        for (int i = 0; i < spritesInsectos.Length; i++)
        {
            spriteIDMap[spritesInsectos[i]] = i; // Asignar una ID única a cada sprite
        }
    }

    /*
    IEnumerator MatchTime()
    {
        //int pointsGame = ClickOnInsecto.Instance.returnPuntuacion();
        int pointsGame = 0;

        // Se actualiza el tiempo cada segundo
        yield return new WaitForSeconds(1);
        time += 1;
        // Se actualiza el texto
        //ActiveText();

        Debug.Log(pointsGame);

        // Valida si el tiempo ya se acabo, o debe seguir contando
        if (pointsGame == 8)
        {
            showEndMinigame();
            GameController.Instance.ActiveEndScene();
        }
        else
        {
            StartCoroutine(MatchTime());
        }
    }
    
    */
    public Insecto[] getInsectosGenerados()
    {
        return insectosGenerados;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        uiEnd.SetActive(false);
        insectosGenerados = new Insecto[cantidadInsectos];
        for (int i = 0; i < cantidadInsectos; i++)
        {
            Sprite spriteInsecto = spritesInsectos[Random.Range(0, spritesInsectos.Length)];

            // Genera una posición aleatoria
            Vector3 randomPosition = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), 0);
            randomPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            randomPosition.z = 0f;

            // Instancia un nuevo insecto
            GameObject nuevoInsecto = Instantiate(insectoPrefab, randomPosition, Quaternion.identity);
            nuevoInsecto.GetComponent<Image>().sprite = spriteInsecto;
            nuevoInsecto.gameObject.GetComponent<Insecto>().SetImagenOriginal(spriteInsecto);
            Debug.Log(spriteInsecto.ToString());
            nuevoInsecto.transform.SetParent(spawner.transform, false);

            // Asignar ID al insecto basado en el sprite
            if (spriteIDMap.TryGetValue(spriteInsecto, out int insectoID))
            {
                nuevoInsecto.GetComponent<Insecto>().SetID(insectoID);
            }

            insectosGenerados[i] = nuevoInsecto.GetComponent<Insecto>();

            //RegistrarAvistamiento(i);

        }

        //StartTimer();

    }
    

    /*
    void Start()
    {
        uiEnd.SetActive(false);
        insectosGenerados = new Insecto[cantidadInsectos];
        for (int i = 0; i < cantidadInsectos; i++)
        {
            GenerarInsecto();
        }
    }

    void GenerarInsecto()
    {
        int spriteIndex = Random.Range(0, spritesInsectos.Length);
        Sprite spriteInsecto = spritesInsectos[spriteIndex];
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

        GameObject insectoObj = Instantiate(insectoPrefab, randomPosition, Quaternion.identity);
        insectoObj.GetComponent<SpriteRenderer>().sprite = spriteInsecto;
        Insecto insectoScript = insectoObj.GetComponent<Insecto>();
        insectoScript.SetID(spriteIndex);

        // Registro de avistamientos
        RegistrarAvistamiento(spriteIndex);
    }
    

    void RegistrarAvistamiento(int id)
    {
        Answer answer = Happy.answers.Find(a => a.idEspecie == id);
        if (answer != null)
        {
            answer.quantity++;
        }
        else
        {
            Happy.answers.Add(new Answer { idEspecie = id, quantity = 1, minigameId = 1 });
        }
    }
    */
    private void Update()
    {
        StartCoroutine(contarTiempo());
        float gamePoints = ClickOnInsecto.Instance.returnPuntuacion();
        if (gamePoints >= 100) showEndMinigame();
    }

    public void StartTimer()
    {
        //StartCoroutine(MatchTime());
    }

    public void DetenerMovimientoInsectos()
    {
        foreach (Insecto insecto in insectosGenerados)
        {
            insecto.StopMovement();
        }
    }

    public void CambiaImagen()
    {
        foreach (Insecto insecto in insectosGenerados)
        {
            Sprite nuevoSprite = imagenAcambiar;
            insecto.GetComponent<Image>().sprite = nuevoSprite;
        }
    }

    public GameObject uiText;
    public GameObject uiEnd;
    public Text PuntuacionEnd;
    public Text animalesEnd;

    private IEnumerator contarTiempo()
    {
        yield return new WaitForSeconds(1f);
        time++;
    }


    public void showEndMinigame()
    {
        StopCoroutine(contarTiempo());
        float pointsGame = ClickOnInsecto.Instance.returnPuntuacion();
        int equivocaciones = ClickOnInsecto.Instance.returnEquivocaciones();
        PuntuacionEnd.text = $"Puntuación Obtenida: {pointsGame}";
        animalesEnd.text = $"Equivocaciones Obtenidas: {equivocaciones}";
        PlayerPrefs.SetInt("TiempoJuego", time);
        uiEnd.SetActive(true);
        uiText.SetActive(false);
    }

    public void endMinigame()
    {
        SceneManager.LoadScene("HouseScene");
    }
}
