using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool hasCollided = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlaceableObject")
        {
            hasCollided = true;
        }
    }
}
