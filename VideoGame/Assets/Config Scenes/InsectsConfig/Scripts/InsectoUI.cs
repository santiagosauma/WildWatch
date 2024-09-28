using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsectoUI : MonoBehaviour
{
    public Button[] buttonSpritesInsectos;
    private int UniqueId;

    private Dictionary<Button, int> ButtonIDMap = new Dictionary<Button, int>();

    public Dictionary<Button,int> getButtonIDMap()
    {
        return ButtonIDMap;
    }
    private void Awake()
    {
        // Inicializar el diccionario con sprites e IDs
        for (int i = 1; i < buttonSpritesInsectos.Length; i++)
        {
            ButtonIDMap[buttonSpritesInsectos[i]] = i + 1; // Asignar una ID única a cada sprite
        }
    }
    public int GetId()
    {
        return UniqueId;
    }
}
