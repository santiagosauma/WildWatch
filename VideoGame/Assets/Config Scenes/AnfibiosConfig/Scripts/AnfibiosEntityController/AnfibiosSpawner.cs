// **********************************************************************
// Script Name: AnfibiosSpawner.cs
// Description:
//      * Manages the spawning of rocks and amphibians in the game.
//      * This script handles the creation of rocks and amphibians
//      * within predefined boundaries, ensuring that they are placed
//      * in valid locations and do not overlap with existing objects.
//      * It also updates game data based on the spawned amphibians.     
// Authors:
//      * Luis Santiago Sauma Peñaloza A00836418
//      * Edsel De Jesus Cisneros Bautista A00838063
//      * Abdiel Fritsche Barajas A01234933
//      * Javier Carlos Ayala Quiroga A01571468
//      * David Balleza Ayala A01771556
//      * Fernando Espidio Santamaria A00837570
// Date Created:  15/03/2024
// Last Modified: 20/07/2024 
// **********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnfibiosSpawner : MonoBehaviour
{
    /// <summary>
    /// Number of amphibian prefabs to be spawned in the game.
    /// </summary>
    public int numOfAnfibios = 5;

    /// <summary>
    /// Array of amphibian prefab GameObjects used for spawning.
    /// </summary>
    public GameObject[] anfibiosPrefabs;

    /// <summary>
    /// Number of rock prefabs to be spawned in the game.
    /// </summary>
    public int numOfRocks = 15;

    /// <summary>
    /// Rock prefab GameObject used for spawning.
    /// </summary>
    public GameObject rockPrefab;

    /// <summary>
    /// Radius used to detect potential overlaps between spawn positions.
    /// </summary>
    public float checkRadius = 0.5f;

    /// <summary>
    /// Maximum X coordinate for spawn positions.
    /// </summary>
    public float maxX = 10;

    /// <summary>
    /// Minimum distance required between spawned objects to prevent overlap.
    /// </summary>
    public float spacingRadius = 1.0f;

    /// <summary>
    /// Minimum X coordinate for spawn positions.
    /// </summary>
    public float minX = -10;

    /// <summary>
    /// Maximum Y coordinate for spawn positions.
    /// </summary>
    public float maxY = 10;

    /// <summary>
    /// Minimum Y coordinate for spawn positions.
    /// </summary>
    public float minY = -10;

    /// <summary>
    /// Offset applied to amphibian spawn positions to adjust their placement.
    /// </summary>
    public Vector2 anfibioOffset = new Vector2(0, -0.5f);

    /// <summary>
    /// List of possible spawn positions for amphibians and rocks.
    /// </summary>
    List<Vector2> spawnPositions = new List<Vector2>();

    /// <summary>
    /// Singleton instance of the AnfibiosSpawner class.
    /// </summary>
    public static AnfibiosSpawner instance;

    /// <summary>
    /// Reference to the AnswersData object for managing answer data.
    /// </summary>
    public AnswersData Happy;

    /// <summary>
    /// Flag indicating whether certain data has already been added.
    /// </summary>
    bool alreadyAdded = false;


    public List<Vector2> getList()
    {
        return spawnPositions;
    }


    /// <summary>
    /// Initializes the singleton instance of the AnfibiosSpawner class.
    /// If an instance already exists, the current game object is 
    /// destroyed to ensure only one instance.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called on the frame when a script is first enabled. 
    /// Spawns the rocks and amphibians in the game by calling the appropriate methods.
    /// </summary>
    void Start()
    {
        SpawnRocks();
        SpawnAnfibios();
    }

    /// <summary>
    /// Spawns rocks in random positions within the defined boundaries.
    /// Ensures that rocks are not placed too close to each other by checking for existing colliders.
    /// If the space is clear, a rock prefab is instantiated at the chosen position.
    /// </summary>
    void SpawnRocks()
    {
        int rocksSpawned = 0;

        while (rocksSpawned < numOfRocks)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, spacingRadius);
            bool isSpaceClear = true;

            // Check if any of the colliders is a rock
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Rock"))
                {
                    isSpaceClear = false;
                    break;
                }
            }

            if (isSpaceClear)
            {
                Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
                spawnPositions.Add(spawnPosition);
                rocksSpawned++;
            }
        }
    }


    private Dictionary<Vector2, GameObject> hiddenAnfibios = new Dictionary<Vector2, GameObject>();

    /// <summary>
    /// Spawns amphibians at random positions from the list of available positions.
    /// Each amphibian is instantiated at a unique position with a random prefab.
    /// Updates the AnswersData with the newly spawned amphibians.
    /// </summary>
    void SpawnAnfibios()
    {
        List<Vector2> availablePositions = new List<Vector2>(spawnPositions);

        for (int i = 0; i < numOfAnfibios; i++)
        {
            if (availablePositions.Count > 0)
            {
                Vector2 spawnPosition = GetRandomPosition(availablePositions);
                GameObject anfibioPrefab = SelectRandomAnfibioPrefab();
                Vector2 anfibioPosition = spawnPosition + anfibioOffset;

                GameObject anfibio = InstantiateAnfibio(anfibioPrefab, anfibioPosition);
                UpdateAnswersData(anfibioPrefab);

                anfibio.SetActive(false);
                hiddenAnfibios[spawnPosition] = anfibio;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Gets a random position from the list of available positions and removes it from the list.
    /// </summary>
    /// <param name="availablePositions">The list of positions to choose from.</param>
    /// <returns>A random position from the list.</returns>
    Vector2 GetRandomPosition(List<Vector2> availablePositions)
    {
        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2 position = availablePositions[randomIndex];
        availablePositions.RemoveAt(randomIndex);
        return position;
    }

    /// <summary>
    /// Selects a random amphibian prefab from the available prefabs.
    /// </summary>
    /// <returns>The selected amphibian prefab.</returns>
    GameObject SelectRandomAnfibioPrefab()
    {
        int anfibioIndex = Random.Range(0, anfibiosPrefabs.Length);
        return anfibiosPrefabs[anfibioIndex];
    }

    /// <summary>
    /// Instantiates an amphibian at the specified position.
    /// </summary>
    /// <param name="anfibioPrefab">The prefab to instantiate.</param>
    /// <param name="anfibioPosition">The position to place the instantiated amphibian.</param>
    /// <returns>The instantiated amphibian GameObject.</returns>
    GameObject InstantiateAnfibio(GameObject anfibioPrefab, Vector2 anfibioPosition)
    {
        return Instantiate(anfibioPrefab, anfibioPosition, Quaternion.identity);
    }

    /// <summary>
    /// Updates the AnswersData with information about the newly spawned amphibian.
    /// </summary>
    /// <param name="anfibioPrefab">The prefab of the amphibian that was spawned.</param>
    void UpdateAnswersData(GameObject anfibioPrefab)
    {
        int anfibioIndex = System.Array.IndexOf(anfibiosPrefabs, anfibioPrefab);
        bool isAlreadyAdded = false;

        foreach (var answer in Happy.answers)
        {
            if (answer.idEspecie == anfibioIndex)
            {
                isAlreadyAdded = true;
                answer.quantity += 1;
                break;
            }
        }

        if (!isAlreadyAdded)
        {
            Answer newAnswer = new Answer
            {
                imgId = anfibioIndex,
                idEspecie = anfibioIndex,
                minigameId = 1,
                quantity = 1
            };
            Happy.answers.Add(newAnswer);
        }
    }


    public GameObject GetHiddenAnfibioAtPosition(Vector2 position)
    {
        if (hiddenAnfibios.TryGetValue(position, out GameObject anfibio))
        {
            return anfibio;
        }
        return null;
    }


}
