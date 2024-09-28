using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDesarmar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform startParent;
    private Camera mainCamera;
    private Vector3 originalScale;  // Variable para almacenar la escala original


    public ItemReference itemReference;

    void Awake()
    {
        mainCamera = Camera.main;
        itemReference = GetComponent<ItemReference>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        originalScale = transform.localScale;  // Guardar la escala original
        transform.localScale = originalScale * 0.5f;  // Reducir la escala a la mitad durante el arrastre
        GetComponent<Collider2D>().enabled = false;  // Desactivar el collider mientras se arrastra
        GetComponent<Button>().enabled = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Button>().enabled = true;

        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (var hitCollider in colliders)
        {
            if (hitCollider.gameObject.CompareTag("PlaceableObject"))
            {

                Item item = hitCollider.gameObject.GetComponent<ItemReference>().GetItemForCondition(hitCollider.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name);
                if (item != null)
                {
                    item.quantity++;
                }
                Destroy(hitCollider.gameObject);
                break;
            }
        }

        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }
}

