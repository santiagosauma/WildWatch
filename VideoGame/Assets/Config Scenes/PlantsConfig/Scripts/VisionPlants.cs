using UnityEngine;
using UnityEngine.UI; // Importa el espacio de nombres para UI

public class VisionPlants : MonoBehaviour
{
    public int zoomedIn = 25;
    public int zoomedOut = 60;
    public bool isZoomed;

    public float smoothView = 4.5f;
    public GameObject binocularsOverlay; // Agrega una referencia al GameObject del overlay de binoculares

    private Camera targetCamera;

    void Start()
    {
        // Encuentra y asigna la cámara principal
        GameObject cameraGameObject = GameObject.FindWithTag("MainCamera");
        if (cameraGameObject != null)
        {
            targetCamera = cameraGameObject.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("No camera found with tag 'MainCamera'");
        }

        // Inicializa la visibilidad del overlay de binoculares
        binocularsOverlay.SetActive(false);
    }

    void Update()
    {
        CheckZoom();
    }

    void CheckZoom()
    {
        if (targetCamera != null) // Asegúrate de que la cámara esté asignada
        {
            if (!Input.GetKey("space"))
            {
                targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, zoomedIn, Time.deltaTime * smoothView);
                isZoomed = true;
                binocularsOverlay.SetActive(true); // Activa el overlay de los binoculares
            }
            else
            {
                targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, zoomedOut, Time.deltaTime * smoothView);
                isZoomed = false;
                binocularsOverlay.SetActive(false); // Desactiva el overlay de los binoculares
            }
        }
    }
}
