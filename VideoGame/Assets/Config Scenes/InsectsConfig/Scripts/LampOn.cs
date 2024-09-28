using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LampOn : MonoBehaviour
{
    public Light2D lightComponent; // Referencia al componente de luz
    public GameObject lightOff; // Objeto con sprite de la luz apagada
    public GameObject lightOn;
    public GeneradorInsectos generadorInsectos;
    public int timeapagar = 25;
    public int timeencender = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        lightOn.SetActive(false);
        lightOff.SetActive(true);
        lightComponent.intensity = 1; // Activa la luz al inicio
        StartTimer();
    }

    IEnumerator ApagarLuz()
    {
        // Se actualiza el tiempo cada segundo
        yield return new WaitForSeconds(1);
        timeapagar -= 1;
        timeencender += 1;

        if (timeapagar <= 0 && timeencender >= 25)
        {
            lightOff.SetActive(false); // Desactiva el sprite de la luz apagada
            lightOn.SetActive(false); // Activa el sprite de la luz prendida
            lightComponent.intensity = 1; // Desactiva la luz
            lightComponent.pointLightOuterRadius = 12;
            lightComponent.pointLightInnerRadius = 12;
        }
        // Valida si el tiempo ya se acabó, o debe seguir contando
        else if (timeapagar <= 0 && timeencender <= 25)
        {
            lightOff.SetActive(false); // Desactiva el sprite de la luz apagada
            lightOn.SetActive(true); // Activa el sprite de la luz prendida
            lightComponent.intensity = 0; // Desactiva la luz
            generadorInsectos.DetenerMovimientoInsectos();
            generadorInsectos.CambiaImagen();
            StartCoroutine(ApagarLuz());
        }
        else
        {
            StartCoroutine(ApagarLuz());
        }
    }

    public void StartTimer()
    {
        StartCoroutine(ApagarLuz());
    }
 
}