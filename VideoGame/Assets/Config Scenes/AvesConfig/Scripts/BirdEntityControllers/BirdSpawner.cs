// **********************************************************************
// Script Name: BirdSpawner.cs
// Description:
//      * This script manages the spawning of birds in the game.
//      * It includes functionalities for initializing the singleton
//      * instance, handling the countdown before spawning, and
//      * spawning birds at random positions. It also ensures that
//      * birds are spawned in a unique sequence without repetition
//      * until all have been visited. 
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
using UnityEngine.VFX;
using UnityEngine.UI;

public class BirdSpawner : MonoBehaviour
{
    /// <summary>
    /// Array of bird prefabs to be spawned.
    /// </summary>
    public GameObject[] birdPrefabs;

    /// <summary>
    /// Minimum height at which birds can be spawned.
    /// </summary>
    public float minHeight;

    /// <summary>
    /// Maximum height at which birds can be spawned.
    /// </summary>
    public float maxHeight;

    /// <summary>
    /// Maximum right position for bird spawning.
    /// </summary>
    public float rightMax;

    /// <summary>
    /// Maximum left position for bird spawning.
    /// </summary>
    public float leftMax;

    /// <summary>
    /// Minimum right position for bird spawning.
    /// </summary>
    public float rightMin;

    /// <summary>
    /// Minimum left position for bird spawning.
    /// </summary>
    public float leftMin;

    /// <summary>
    /// UI element for displaying countdown.
    /// </summary>
    public Text countDown;

    /// <summary>
    /// Flag to control if birds can be spawned.
    /// </summary>
    public bool canSpawn = true;

    /// <summary>
    /// Reference to the sound effects manager for birds.
    /// </summary>
    public SFXManagerBirds sfxManager;

    /// <summary>
    /// Countdown time in seconds before spawning starts.
    /// </summary>
    public int countdownTime = 3;

    /// <summary>
    /// Array to track visited spawn points.
    /// </summary>
    bool[] visited = new bool[5];

    /// <summary>
    /// ID for the current bird.
    /// </summary>
    private int IDbird = -1;

    /// <summary>
    /// Singleton instance of the BirdSpawner.
    /// </summary>
    public static BirdSpawner Instance;

    /// <summary>
    /// Flag to check if bird is already added.
    /// </summary>
    bool alreadyAdded = false;

    /// <summary>
    /// Reference to the answers data.
    /// </summary>
    public AnswersData Happy;


    /// <summary>
    /// Initializes the singleton instance of the BirdSpawner class.
    /// Ensures that only one instance of the BirdSpawner exists in the scene.
    /// If an instance already exists, destroys the current game object to maintain the singleton pattern.
    /// </summary>
    private void Awake()
    {
        // Check if an instance of BirdSpawner already exists
        if (Instance == null)
        {
            // If no instance exists, set this as the instance
            Instance = this;
        }
        else if (Instance != this)
        {
            // If an instance already exists and it's not this one, destroy this game object
            Destroy(gameObject);
        }
    }

    void Start()
    {
        countDown.gameObject.SetActive(false);
    }

    ///<summary>
    /// Spawns a bird at a random position and updates tracking data.
    ///</summary>
    void SpawnBird()
    {
        if (canSpawn)
        {
            int side = Random.Range(0, 2);
            int randomIndex = GetRandomBirdIndex();
            UpdateVisitedStatus(randomIndex);

            PlayerPrefs.SetInt("birdType", randomIndex);

            GameObject birdToSpawn = birdPrefabs[randomIndex];
            Vector3 spawnPosition = GetSpawnPosition(side);

            Instantiate(birdToSpawn, spawnPosition, Quaternion.identity);
            UpdateBirdData(randomIndex);
        }
    }

    ///<summary>
    /// Gets a random bird index that hasn't been visited yet.
    ///</summary>
    ///<returns>Returns an unvisited random bird index.</returns>
    int GetRandomBirdIndex()
    {
        int randomIndex = Random.Range(0, birdPrefabs.Length);
        bool allVisited = true;

        if (visited[randomIndex])
        {
            for (int i = 0; i < visited.Length; i++)
            {
                if (!visited[i])
                {
                    allVisited = false;
                    break;
                }
            }

            if (allVisited)
            {
                Debug.Log("All indices have been visited.");
            }
            else
            {
                while (visited[randomIndex])
                {
                    randomIndex = (randomIndex + 1) % birdPrefabs.Length;
                }
            }
        }

        return randomIndex;
    }

    ///<summary>
    /// Updates the visited status of a bird index.
    ///</summary>
    ///<param name="index">Index of the bird to update.</param>
    void UpdateVisitedStatus(int index)
    {
        bool allVisited = true;

        for (int i = 0; i < visited.Length; i++)
        {
            if (!visited[i])
            {
                allVisited = false;
                break;
            }
        }

        if (!allVisited)
        {
            visited[index] = true;
        }
    }

    ///<summary>
    /// Gets a random spawn position based on the side.
    ///</summary>
    ///<param name="side">Side to spawn the bird on (0 for left, 1 for right).</param>
    ///<returns>Returns a Vector3 representing the spawn position.</returns>
    Vector3 GetSpawnPosition(int side)
    {
        Vector3 spawnPosition;
        if (side == 0)
        {
            spawnPosition = new Vector3(Random.Range(leftMin, leftMax), Random.Range(minHeight, maxHeight), 0);
        }
        else
        {
            spawnPosition = new Vector3(Random.Range(rightMin, rightMax), Random.Range(minHeight, maxHeight), 0);
        }

        // Fixed spawn position for testing
        spawnPosition = new Vector3(29.44f, -10f, 0.00f);

        return spawnPosition;
    }

    ///<summary>
    /// Updates the bird data in the tracking system.
    ///</summary>
    ///<param name="randomIndex">Index of the bird prefab to update.</param>
    void UpdateBirdData(int randomIndex)
    {
        BirdBehavior birdBehaviorScript = birdPrefabs[randomIndex].GetComponent<BirdBehavior>();
        if (birdBehaviorScript != null)
        {
            IDbird = birdBehaviorScript.getId() - 1;
        }

        for (int i = 0; i < Happy.answers.Count; i++)
        {
            if (Happy.answers[i].idEspecie == IDbird)
            {
                alreadyAdded = true;
                Happy.answers[i].quantity += 1;
                break;
            }
        }

        if (!alreadyAdded)
        {
            Answer answer = new Answer
            {
                imgId = IDbird,
                idEspecie = IDbird,
                minigameId = 0,
                quantity = 1
            };
            Happy.answers.Add(answer);
        }
    }


    /// <summary>
    /// Coroutine that handles the countdown before starting the bird spawning routine.
    /// Updates the countdown text and waits for the specified countdown time.
    /// </summary>
    /// <returns>Yields a WaitForSeconds to create a delay between countdown updates.</returns>
    IEnumerator CountdownRoutine()
    {
        int timeLeft = countdownTime; // Sets the initial countdown time

        while (timeLeft > 0)
        {
            countDown.text = timeLeft.ToString(); // Updates the countdown text
            yield return new WaitForSeconds(1); // Waits for 1 second
            timeLeft--; // Decreases the countdown time
        }

        countDown.gameObject.SetActive(false); // Hides the countdown text

        if (!canSpawn)
        {
            canSpawn = true;
            StartCoroutine(SpawnerRoutine()); // Starts the bird spawning routine
        }
    }


    /// <summary>
    /// Coroutine that handles the routine for spawning birds.
    /// Continues to spawn birds at regular intervals while spawning is enabled.
    /// </summary>
    /// <returns>Yields a WaitForSeconds to create a delay between spawns.</returns>
    IEnumerator SpawnerRoutine()
    {
        while (canSpawn)
        {
            // Waits for 3 seconds before spawning the next bird
            yield return new WaitForSeconds(3);

            // Spawns a new bird
            SpawnBird();

            // Optionally stops the spawning process
            StopSpawning();
        }
    }


    public void StopSpawning()
    {
        canSpawn = false;
    }

    public void StartSpawning()
    {
        countDown.gameObject.SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    public int getIdBird()
    {
        return IDbird;
    }
}
