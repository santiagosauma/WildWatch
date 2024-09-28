using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootPrintGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] validTiles;
    public GameObject footprintPrefab;
    public Sprite[] footprintSprites;
    public int footprintCount = 10;

    private List<Vector3Int> validPositions = new List<Vector3Int>();

    void Start()
    {
        CacheValidPositions();
        GenerateFootprints();
    }

    void CacheValidPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && System.Array.Exists(validTiles, t => t == tile))
                {
                    Vector3Int localPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                    validPositions.Add(localPos);
                }
            }
        }
    }

    void GenerateFootprints()
    {
        for (int i = 0; i < footprintCount && validPositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 worldPosition = tilemap.CellToWorld(validPositions[randomIndex]) + new Vector3(0.5f, 0.5f, 0);
            GameObject newFootprint = Instantiate(footprintPrefab, worldPosition, Quaternion.identity);
            AdjustFootprint(newFootprint);
            
        }

    }
    void AdjustFootprint(GameObject footprint)
    {
        // Verifica si el objeto footprint y su componente SpriteRenderer son nulos antes de acceder a ellos
        if (footprint != null && footprint.GetComponentInChildren<SpriteRenderer>() != null)
        {
            Vector3 dimension = Vector3.one * Random.Range(0.5f, 0.8f);
            footprint.transform.localScale = dimension;
            int spriteIndex = dimension.x < 0.6f ? 0 : dimension.x < 0.7f ? 1 : 2;
            footprint.GetComponentInChildren<SpriteRenderer>().sprite = footprintSprites[spriteIndex];
            float randomAngle = Random.Range(0f, 270f);

            footprint.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        }
        else
        {
            Debug.LogWarning("El objeto de huella o su componente SpriteRenderer son nulos.");
        }
    }
}
