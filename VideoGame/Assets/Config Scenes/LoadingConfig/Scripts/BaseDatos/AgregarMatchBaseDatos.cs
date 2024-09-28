using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class AgregarMatchBaseDatos : MonoBehaviour
{
    private void Awake()
    {
        // Mandar dummy data a base de datos
        int userID = PlayerPrefs.GetInt("UserID");
        int minigameIDVideojuego = PlayerPrefs.GetInt("FinalOption");
        StartCoroutine(InsertMatch(userID, ObtenerIDMinijuego(minigameIDVideojuego), 0, 0));
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
        else if (minijuegoIDVideojuego == 3) minijuegoIDBaseDatos = 2;
        else if (minijuegoIDVideojuego == 4) minijuegoIDBaseDatos = 4;
        else if (minijuegoIDVideojuego == 5) minijuegoIDBaseDatos = 5;


        return minijuegoIDBaseDatos;
    }

    public IEnumerator InsertMatch(int userid, int minigameid, int puntos, int tiempo)
    {
        string minigameURL = "https://10.22.156.99:7026/api/Videogame/Minigame";

        // Using Newtonsoft.Json to serialize the data
        var minigameData = new
        {
            userID = userid,
            minigameID = minigameid,
            points = puntos,
            time = tiempo
        };
        string json = JsonConvert.SerializeObject(minigameData);
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
            ErrorMatch error = new ErrorMatch();
            error = JsonConvert.DeserializeObject<ErrorMatch>(webRequest.downloadHandler.text);
            PlayerPrefs.SetInt("MatchID", error.MatchID);
        }
    }



}
