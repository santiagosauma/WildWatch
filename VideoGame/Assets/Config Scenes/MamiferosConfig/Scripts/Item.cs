using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
   public int id;
   public int quantity;
   public string description;
   public Sprite imagenItem;
}
