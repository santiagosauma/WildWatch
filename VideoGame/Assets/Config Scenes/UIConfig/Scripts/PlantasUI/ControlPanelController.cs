using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public Text TimeText;
    public Text PointsText;
    public MapManager mapManager;
    private int time = 30;


    void Start()
    {
        PlayerPrefs.SetInt("TiempoJuego", time);
        ActiveText();
        StartTimer();
    }
    public void ActiveText()
    {
        TimeText.text = "Tiempo: " + time;
    }

    //Contador
    IEnumerator MatchTime()
    {
        yield return new WaitForSeconds(1);
        time -= 1;
        ActiveText();

        if(time <= 0)
        {
            MapManager.Instance.showEndMinigame();
        }

        StartCoroutine(MatchTime());

    }

    public void StartTimer()
    {
        StartCoroutine(MatchTime());
    }
    private void Update()
    {
        UpdatePointsText();
    }

    private void UpdatePointsText()
    {
        float points = mapManager.GetPointsOnClick();
        // Actualizar el texto de la puntuación
        PointsText.text = "Puntuación: " + (int)points;
    }
}
