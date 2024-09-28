using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// using static UnityEditor.Progress;

public class ItemReference : MonoBehaviour
{
    public List<Item> itemData;
    public Item GetItemForCondition(string spriteName)
    {
        // Aquí puedes agregar lógica para seleccionar el item correcto basado en la condición
        foreach (Item item in itemData)
        {
            if (item.imagenItem.name == spriteName) // Compara el nombre del sprite del ítem con el nombre del sprite del objeto colisionado
            {
                return item;
            }
        }
        return null; // Retornar null o un default si no se encuentra nada
    }
}
