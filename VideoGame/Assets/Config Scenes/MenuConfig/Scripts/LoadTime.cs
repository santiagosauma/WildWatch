using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTime : MonoBehaviour
{
    public int time = 3;
    // Start is called before the first frame update
    void Start()
    {
        starTimer();
    }

    IEnumerator MatchTime()
    {
        // Se actualiza el tiempo cada segundo
        yield return new WaitForSeconds(1);
        time -= 1;

        // Valida si el tiempo ya se acabo, o debe seguir contando
        if (time == 0)
        {
            SceneManager.LoadScene("HouseScene");
        }
        else
        {
            StartCoroutine(MatchTime());
        }
    }

    public void starTimer()
    {
        StartCoroutine(MatchTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
