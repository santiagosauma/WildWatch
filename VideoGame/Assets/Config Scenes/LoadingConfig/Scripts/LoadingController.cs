using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    List<string> NombresMinijuegos = new List<string>();
    List<string> DescripcionMinijuegos = new List<string>();
    public List<Sprite> ImagesMinigame = new List<Sprite>();

    public Text title;
    public Text description;
    public Image gameImage;

    private int typeOfGame;

    public void enterMinigame()
    {
        //int typeOfGame = PlayerPrefs.GetInt("TypeOfGame");

        if (typeOfGame == 1)
        {
            SceneManager.LoadScene("AvesScene");
        }
        else if(typeOfGame == 2)
        {
            SceneManager.LoadScene("AnfibiosScene");
        }else if(typeOfGame == 3)
        {
            SceneManager.LoadScene("MamiferosCentral");
        }
        else if(typeOfGame==4)
        {
            SceneManager.LoadScene("InsectosScene");
        }
        else if(typeOfGame==5)
        {
            SceneManager.LoadScene("PlantasScene");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        typeOfGame = PlayerPrefs.GetInt("TypeOfGame");
        NombresMinijuegos.Add("Alas y Eco");
        NombresMinijuegos.Add("Secretos Subrocosos");
        NombresMinijuegos.Add("Huellas Misteriosas");
        NombresMinijuegos.Add("Recuerdo de un Zumbido a oscuras");
        NombresMinijuegos.Add("Jardín Secreto");

        /*
        DescripcionMinijuegos.Add("El objetivo del juego es lograr ver el ave que aparece. Tienes tres rondas para intentar ver tres aves. Al finalizar cada ronda, recuerda intentar identificar el sonido para obtener más puntos. RECOMENDACIÓN: Escucha de donde sale el sonido\r\n");
        DescripcionMinijuegos.Add("El objetivo del juego es encontrar el máximo número de anfibios que puedas. Tienes un tiempo límite para encontrarlos a todos, cada que encuentres uno, pícale para registrarlo. Recuerda que los anfibios suelen estar debajo de las piedras, sería buena idea picarles a éstas.\r\n");
        DescripcionMinijuegos.Add("Parece que han pasado animales por ciertas áreas en el mapa y han dejado huellas y heces. Analízalas y elige el tipo de trampa correcta para identificar al animal. CONSEJO: Revisa el tamaño de las evidencias para determinar qué tipo de animal es. \r\n");
        DescripcionMinijuegos.Add("A los insectos les gusta la luz, el objetivo del juego es saber si puedes identificar al grupo de insectos correcto. Pícale a todos los insectos que son del mismo grupo, aunque cuidado, una vez se va la luz al principio no podrás verlos de nuevo. ¿Serás capaz de recordarlos?\r\n");
        DescripcionMinijuegos.Add("El objetivo del juego es recolectar el mayor número de plantas que puedas. Muévete a través del mapa y busca la mayor cantidad que puedas, nada más pon atención por donde caminas, en cualquier momento podrías encontrarte con algún obstáculo. \r\n");
        */

        DescripcionMinijuegos.Add(" REGLAS \r\n \r\n 1.- El objetivo del juego es ver tres aves diferentes en tres rondas. \r\n \r\n 2.- Al final de cada ronda, escucha y adivina el sonido del ave para puntos extra.\r\n \r\n 3.- Al final se sumarán todos los puntos por cada ave vista y por identificar correctamente el sonido. \r\n \r\n ¡Disfruta el juego! \r\n");
        DescripcionMinijuegos.Add("REGLAS \r\n \r\n 1.- El objetivo del juego es encontrar el máximo número de anfibios dentro del tiempo límite. \r\n \r\n 2.- Tienes un tiempo determinado para encontrar todos los anfibios. \r\n \r\n 3.- Busca debajo de las piedras, donde suelen esconderse los anfibios. \r\n \r\n 4.-  Interactúa con cada anfibio que encuentres para registrarlos. \r\n \r\n ¡Buena suerte encontrando esos anfibios! \r\n");
        DescripcionMinijuegos.Add("REGLAS \r\n \r\n 1.- Analiza las huellas dejadas por animales en ciertas áreas del mapa para identificar el tipo de animal. \r\n \r\n 2.- Observa el tamaño y la forma de las huellas para decidir que tipo de trampa usar. \r\n \r\n 3.- Presta atención al tamaño de las evidencias, ya que te ayudará a determinar qué tipo de animal podría ser. \r\n \r\n ¡Buena suerte analizando las evidencias y atrapando al animal correcto!\r\n");
        DescripcionMinijuegos.Add("REGLAS \r\n \r\n 1.- El objetivo del juego es memorizar la posición de los insectos antes de que se apague la luz. \r\n \r\n 2.- Una vez se vaya la luz, debes identificar y seleccionar todos los insectos que pertenezcan al mismo grupo. \r\n \r\n 3.- Una vez que se apague la luz al principio, no podrás ver los insectos de nuevo. Debes recordar su ubicación y grupo para seleccionarlos correctamente. \r\n \r\n ¡Buena suerte recordando los insectos!\r\n");
        DescripcionMinijuegos.Add("REGLAS \r\n \r\n 1.- El objetivo del juegos es recolectar la mayor cantidad de plantas posible mientras te mueves por el mapa. \r\n \r\n 2.- Navega por el mapa en busca de plantas. Ten cuidado, ya que pueden aparecer obstáculos en tu camino en cualquier momento. \r\n \r\n ¡Diviértete explorando el mapa y recolectando plantas! \r\n");

        title.text = NombresMinijuegos[typeOfGame - 1];
        description.text = DescripcionMinijuegos[typeOfGame - 1];
        gameImage.sprite = ImagesMinigame[typeOfGame - 1];


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
