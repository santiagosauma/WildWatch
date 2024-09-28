using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;  // Make sure Newtonsoft.Json is added to the project
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Text statusText;
    public GameObject InputsObject;
    public GameObject StatusObject;
    public Button PlayButton;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        StatusObject.SetActive(false);
    }

    private void OnLoginButtonClicked()
    {
        StartCoroutine(SendLoginRequest(usernameInputField.text, passwordInputField.text));
    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        string loginUrl = "https://10.22.156.99:7026/api/Videogame";

        // Using Newtonsoft.Json to serialize the data
        var userData = new
        {
            mail = username,
            password = password
        };
        string json = JsonConvert.SerializeObject(userData);
        Debug.Log("Sending JSON: " + json);  // This will print the JSON string to the Unity Console


        // Setting up the UnityWebRequest
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(loginUrl, "POST");
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
            StatusObject.SetActive(true);
            statusText.text = "Inicio de sesi�n erroneo ";
            usernameInputField.text = "";
            passwordInputField.text = "";
            

        }
        else
        {
            User User = new User();
            User= JsonConvert.DeserializeObject<User>(webRequest.downloadHandler.text);
            PlayerPrefs.SetInt("UserID",User.userID);
            PlayButton.gameObject.SetActive(true);
            InputsObject.SetActive(false);
            StatusObject.SetActive(false);
            loginButton.gameObject.SetActive(false);
            usernameInputField.gameObject.SetActive(false);
            passwordInputField.gameObject.SetActive(false);

        }
    }

}