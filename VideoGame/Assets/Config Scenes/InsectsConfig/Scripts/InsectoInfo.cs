using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InsectoInfo", menuName = "Animal Data/Insecto Info")]
public class InsectoInfo : ScriptableObject, IDataDisplayable
{
    public string Name;
    public string Description;
    public Sprite Image;
    public int UniqueId;

    public string nombreComun;

    public string DisplayCommonName => nombreComun;

    // Propiedades de la interfaz IDataDisplayable
    public string DisplayName => Name;
    public string DisplayDescription => Description;
    public Sprite DisplayImage => Image;
}