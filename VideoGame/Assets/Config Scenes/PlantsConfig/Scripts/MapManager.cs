using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
    private List<TileData> selectedTileDataList; // Lista de plantas seleccionadas
    private List<int> UniqueId;

    private float points = 0;
    private int totalPlants = 0;

    public Text obtainAnimation;

    public AudioClip correct;


    bool first;

    public static MapManager Instance;

    bool delayTime = false;

    private Dictionary<Vector3Int, bool> tileSelectionState;

    public ListaAnimales animales;
    public AnswersData AnswersData;

    float pointsGame;
    int typesOfPlants;

    int uniqueRegister;

    bool[] alreadyAdded = new bool[5];

    public AnswersData Happy; 

    private void Awake()
    {
        // Si la instancia no existe, la establece a este script.
        if (Instance == null)
        {
            Instance = this;
            // Opcional: si quieres que el spawner persista entre escenas, descomenta la siguiente línea.
            // DontDestroyOnLoad(gameObject);
        }
        // Si la instancia existe y no es este script, destruye este objeto.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        dataFromTiles = new Dictionary<TileBase, TileData>();
        selectedTileDataList = new List<TileData>();

        totalPlants = 0;
        points = 0;

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        tileSelectionState = new Dictionary<Vector3Int, bool>();

    }

    private void MarkTileAsSelected(Vector3Int gridPosition)
    {
        // Marcar la tile actual como seleccionada
        tileSelectionState[gridPosition] = true;

        // También debes marcar las tiles adyacentes
        MarkAdjacentTiles(gridPosition);
    }

    private void MarkAdjacentTiles(Vector3Int gridPosition)
    {
        // Las direcciones de las 8 tiles alrededor de una posición central
        Vector3Int[] adjacentDirections = {
        new Vector3Int(0, 1, 0), // Arriba
        new Vector3Int(1, 0, 0), // Derecha
        new Vector3Int(0, -1, 0), // Abajo
        new Vector3Int(-1, 0, 0), // Izquierda
        new Vector3Int(-1, 1, 0), // Izquierda Arriba
        new Vector3Int(-1, -1, 0), // Izquierda Abajo
        new Vector3Int(1, 1, 0), // Derecha Arriba
        new Vector3Int(1, -1, 0), // Derecha Abajo
    };

        foreach (Vector3Int direction in adjacentDirections)
        {
            Vector3Int adjacentPosition = gridPosition + direction;
            if (!tileSelectionState.ContainsKey(adjacentPosition))
            {
                tileSelectionState[adjacentPosition] = true;
            }
        }
    }

    public int returnPlants()
    {
        return totalPlants;
    }

    void addPerfect()
    {
        Answer answer = new Answer();
        answer.imgId = 0;
        answer.idEspecie = 0;
        answer.minigameId = 4;
        answer.quantity = 7;
        Happy.answers.Add(answer);

        Answer answer2 = new Answer();
        answer2.imgId = 1;
        answer2.idEspecie = 1;
        answer2.minigameId = 4;
        answer2.quantity = 4;
        Happy.answers.Add(answer2);
        //Debug.Log("SALIO O NO");

        Answer answer3 = new Answer();
        answer3.imgId = 2;
        answer3.idEspecie = 2;
        answer3.minigameId = 4;
        answer3.quantity = 4;
        Happy.answers.Add(answer3);
        //Debug.Log("SALIO O NO");

        Answer answer4 = new Answer();
        answer4.imgId = 3;
        answer4.idEspecie = 3;
        answer4.minigameId = 4;
        answer4.quantity = 6;
        Happy.answers.Add(answer4);
        //Debug.Log("SALIO O NO");

        Answer answer5 = new Answer();
        answer5.imgId = 4;
        answer5.idEspecie = 4;
        answer5.minigameId = 0;
        answer5.quantity = 3;
        Happy.answers.Add(answer5);
        //Debug.Log("SALIO O NO");
        //Debug.Log("SALIO O NO");
    }

    private void Start()
    {
        addPerfect();
        obtainAnimation.gameObject.SetActive(false);
        pointsGame = 0;
        first = false;
        typesOfPlants = 0;
        uiEnd.SetActive(false);
        //uiText.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            if (!tileSelectionState.ContainsKey(gridPosition) || !tileSelectionState[gridPosition])
            {
                TileBase clickedTile = map.GetTile(gridPosition);
                if (clickedTile != null)
                {
                    // Marcar la tile y las adyacentes como seleccionadas
                    MarkTileAsSelected(gridPosition);
                    // ... El resto de tu lógica aquí ...
                    // Obtener los datos del tile clickeado
                    TileData tileData = dataFromTiles[clickedTile];

                    // Comprobar si el tileData ya está en la lista de tiles seleccionados
                    if (!selectedTileDataList.Contains(tileData) && !delayTime)
                    {
                        // Agregar el tile clickeado a la lista de tiles seleccionados
                        selectedTileDataList.Add(tileData);
                        animales.Plantas.Add(tileData);


                        //uniqueRegister++;
                        typesOfPlants++;
                        if (!first)
                        {
                            first = true;
                            ShowSavedData.instance.ShowInit();
                        }
                    }

                    points += 4.17f;
                    if (points > 100) points = 100;
                    Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
                    // Directamente usa screenCenter porque ShowPointsAnimation espera una posición de pantalla
                    ShowPointsAnimation(cameraCenter);
                    totalPlants++;
                    AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);
                    pointsGame += 4.17f;
                    if(pointsGame > 100) pointsGame = 100;


                    // Logica de registro
                    int tileDataIndex = tileDatas.IndexOf(tileData);
                    if (!alreadyAdded[tileDataIndex])
                    {
                        alreadyAdded[tileDataIndex] = true;
                        Debug.Log(tileDataIndex);
                        Answer answer = new Answer()
                        { 
                            imgId = tileDataIndex,
                            idEspecie = tileDataIndex,
                            minigameId = 4,  // Asumiendo un ID de minijuego específico para insectos
                            quantity = 1
                        };
                        AnswersData.answers.Add(answer);
                        uniqueRegister++;
                    }
                    else
                    {
                        foreach (var answer in AnswersData.answers)
                        {
                            if (answer.idEspecie == tileDataIndex)
                            {
                                answer.quantity += 1;
                                break;
                            }
                        }
                    }
                    /*
                    delayTime = true;
                    StartCoroutine(SetDelayTimeFalseAfterDelay(3));
                    */
                }
            }
        }
        /*
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            // Verificar si la tile ya fue seleccionada.
        if (!tileSelectionState.ContainsKey(gridPosition) || !tileSelectionState[gridPosition]) {
            // Si no ha sido seleccionada, proceder con la lógica de selección.
            TileBase clickedTile = map.GetTile(gridPosition);

            if (clickedTile != null) {
                // ... tu lógica de selección de planta aquí ...
                MarkTileAsSelected(gridPosition);
                    // ... más lógica de selección aquí ...
                    // Obtener los datos del tile clickeado
                    TileData tileData = dataFromTiles[clickedTile];

                    // Comprobar si el tileData ya está en la lista de tiles seleccionados
                    if (!selectedTileDataList.Contains(tileData) && !delayTime)
                    {
                        // Agregar el tile clickeado a la lista de tiles seleccionados
                        selectedTileDataList.Add(tileData);
                        typesOfPlants++;
                        if (!first)
                        {
                            first = true;
                            ShowSavedData.instance.ShowInit();
                        }
                    }

                    points += 15;
                    Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
                    // Directamente usa screenCenter porque ShowPointsAnimation espera una posición de pantalla
                    ShowPointsAnimation(cameraCenter);
                    totalPlants++;
                    AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);
                    pointsGame += 15;

                    delayTime = true;
                    StartCoroutine(SetDelayTimeFalseAfterDelay(3));
                }
            
        }

            /*
            TileBase clickedTile = map.GetTile(gridPosition);

            if (clickedTile != null && !delayTime)
            {
                // Obtener los datos del tile clickeado
                TileData tileData = dataFromTiles[clickedTile];

                // Comprobar si el tileData ya está en la lista de tiles seleccionados
                if (!selectedTileDataList.Contains(tileData))
                {
                    // Agregar el tile clickeado a la lista de tiles seleccionados
                    selectedTileDataList.Add(tileData);
                    typesOfPlants++;
                    if (!first)
                    {
                        first = true;
                        ShowSavedData.instance.ShowInit();
                    }
                }

                points += 15;
                Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
                // Directamente usa screenCenter porque ShowPointsAnimation espera una posición de pantalla
                ShowPointsAnimation(cameraCenter);
                totalPlants++;
                AudioSource.PlayClipAtPoint(correct, Camera.main.transform.position, 0.5f);
                pointsGame += 15;

                delayTime = true;
                StartCoroutine(SetDelayTimeFalseAfterDelay(3));
            }
            

        }
    */
    }

    private IEnumerator SetDelayTimeFalseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        delayTime = false;
    }

    public void ShowPointsAnimation(Vector2 birdPosition)
    {
        StartCoroutine(PointsAnimation(birdPosition));
    }

    IEnumerator PointsAnimation(Vector3 birdPosition)
    {
        // Convierte la posición del mundo del ave a una posición en la UI
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(birdPosition);

        // Mueve el texto a la posición convertida
        //obtainAnimation.transform.position = screenPosition;

        // Activa el GameObject del texto para mostrarlo
        obtainAnimation.gameObject.SetActive(true);

        // Espera un poco para leer el "+1"
        yield return new WaitForSeconds(1f);


        // Desactiva el GameObject del texto para ocultarlo
        obtainAnimation.gameObject.SetActive(false);
    }

    public float GetPointsOnClick()
    {
        return points;
    }
    public List<TileData> GetSelectedTileDataList()
    {
        return selectedTileDataList;
    }

    public GameObject uiText;
    public GameObject uiEnd;
    public Text PuntuacionEnd;
    public Text animalesEnd;

    public void showEndMinigame()
    {
        PuntuacionEnd.text = $"Puntuación Obtenida: {(int)pointsGame}";
        animalesEnd.text = $"Tipos de Plantas Vistas: {typesOfPlants}/5";
        uiEnd.SetActive(true);
        uiText.SetActive(false);
    }

    public void goHomeEnd()
    {
        PlayerPrefs.SetFloat("PuntuacionFinal", points);
        Debug.Log(uniqueRegister);
        PlayerPrefs.SetInt("RegisterNum", uniqueRegister);
        SceneManager.LoadScene("HouseScene");
    }

}