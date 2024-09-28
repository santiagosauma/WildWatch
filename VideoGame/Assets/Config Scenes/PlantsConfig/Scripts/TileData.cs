// using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New TileData", menuName = "Tile Data/Create New Tile Data")]
public class TileData : ScriptableObject, IDataDisplayable
{
    public TileBase[] tiles;

    public string Name;
    public Sprite Image;
    public string Description;
    public int UniqueId;

    public string nombreComun;

    public string DisplayCommonName => nombreComun;

    // Implementación de las propiedades de IDataDisplayable
    public string DisplayName => Name;
    public string DisplayDescription => Description;
    public Sprite DisplayImage => Image;
}
