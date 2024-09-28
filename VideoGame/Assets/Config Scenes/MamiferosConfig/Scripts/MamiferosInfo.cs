using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MamiferosInfo", menuName = "Animal Data/Mamiferos Info")]
public class MamiferosInfo : ScriptableObject, IDataDisplayable
{
    public string Name;
    public string Description;
    public Sprite Image;
    public int id;

    public string nombreComun;

    public string DisplayCommonName => nombreComun;

    public string DisplayName => Name;
    public string DisplayDescription => Description;
    public Sprite DisplayImage => Image;
}