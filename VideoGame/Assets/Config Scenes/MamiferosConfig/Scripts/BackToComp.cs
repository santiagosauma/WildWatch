using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToComp : MonoBehaviour
{

    public void returnToComp()
    {
        SceneManager.LoadScene("MamiferosCOMP");
    }

    public void goToLeft()
    {
        SceneManager.LoadScene("MamiferosIzquierda");
    }

    public void goToMid()
    {
        SceneManager.LoadScene("MamiferosCentral");
    }

    public void goToRight()
    {
        SceneManager.LoadScene("MamiferosDerecha");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
