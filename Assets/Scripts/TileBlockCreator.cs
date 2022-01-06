using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBlockCreator : MonoBehaviour
{
    public Transform dotParent;
    public Transform wallParent;
    public Tilemap tilemap;
    public TileBase ruleTile;
    public TileBase dotTile;
    public GameObject wallPrefab;
    public GameObject dotPrefab;
    public int range;
    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        Debug.Log(tilemap.localBounds.size);
        Debug.Log(tilemap.localBounds.center);

        for(int x = 0; x < bounds.size.x; x++)
        {
            for(int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile == ruleTile)
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);

                    wall.transform.parent = wallParent;
                }
            }
        }

        for(int x = 0; x < bounds.size.x; x++)
        {
            for(int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile == dotTile)
                {
                    GameObject dot = Instantiate(dotPrefab, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);

                    dot.transform.parent = dotParent;
                }
            }
        }
    }

}
