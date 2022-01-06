using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MapCreator : MonoBehaviour
{
    public AlertInfo alertInfo;
    public InputField inputField; 
    public Camera mainCam;
    public TileBase playerTile;
    public TileBase enemyTile;
    public TileBase wallTile;
    public TileBase dotTile;
    public Tilemap tilemap;
    public Tilemap highlightMap;

    
    public GameObject gridTilemap;
    public GameObject LevelCreated;
    public GameObject playerSpawnPoint;
    public GameObject enemySpawnPoint;
    public GameObject wallPrefab;
    public GameObject dotPrefab;
    public GameObject ground;

    public Transform enemySpawnParent;
    public Transform dotParent;
    public Transform wallParent;

    [SerializeField] private int range;
    [SerializeField] public int directionRange;
    private Vector3 touchPos;
    private Vector3Int previous;
    private TileBase currentTile;
    private Vector3Int cell;
    private float minFov = 15f;
    private float maxFov = 90f;
    private float sensitivity = -10f;
    private bool isBuilt;
    private bool canBuild;
    private bool canBeBuild;
    private int playerTilesCount;

    void Start()
    {
        currentTile = wallTile;
    }
    void Update()
    {
        CameraZoom();

        if(canBuild)
        {
            //Simple tool system with inputs
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentTile = playerTile;
                highlightMap.SetTile(cell, currentTile);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentTile = wallTile;
                highlightMap.SetTile(cell, currentTile);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentTile = dotTile;
                highlightMap.SetTile(cell, currentTile);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentTile = enemyTile;
                highlightMap.SetTile(cell, currentTile);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                if(!isBuilt)
                {
                    BuildTilesButton();
                }   
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            if(!gridTilemap.activeInHierarchy)
            {
                gridTilemap.SetActive(true);
            }
            else
            {
                gridTilemap.SetActive(false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }

    public void SetNameSubmitButton()
    {
        //Checking if map name isn't empty to start 
        if(inputField.text != "")
        {
            LevelCreated.name = inputField.text;
            Cursor.visible = false;
            canBuild = true;
            inputField.interactable = false;
        }
        else
        {
            string info = "Fill the 'level name' and press start create a map.";
            alertInfo.SetAlertInfo(info);
        }
    }
    private void LateUpdate()
    {
        if(canBuild)
        {
            //Setting cell position to muse inputs, to draw tiles using mouse 
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellCoords = tilemap.WorldToCell(touchPos);
            cell = new Vector3Int(cellCoords.x-6, cellCoords.y-150, cellCoords.z);
            cell.x += directionRange;

            //Setting tiles in place of mouse as a 
            if(cell != previous)
            {
                highlightMap.SetTile(cell, currentTile);

                highlightMap.SetTile(previous, null);
                previous = cell;
            }

            if(Input.GetMouseButton(0))
            {
                //Placing tile on tilemap
                tilemap.SetTile(cell, currentTile);
            }
            else if(Input.GetMouseButton(1))
            {
                //Erasing tile from tilemap
                tilemap.SetTile(cell, null);
            }
        }
    }

    //If the map is created build system check all specific tiles to build on those positions specific object prefabs
    private void BuildTilesButton()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for(int x = 0; x < bounds.size.x; x++)
        {
            for(int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile == wallTile)
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);

                    wall.transform.parent = wallParent;
                }
                if(tile == dotTile)
                {
                     GameObject dot = Instantiate(dotPrefab, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);

                     dot.transform.parent = dotParent;
                }
                if(tile == playerTile)
                {
                    playerTilesCount++;
                    if(playerTilesCount > 1)
                    {
                        string buildInfo = "There can be only one player spawn point on map!";
                        alertInfo.SetAlertInfo(buildInfo);
                        canBeBuild = false;
                        break;
                    }
                    else
                    {
                        canBeBuild = true;
                        GameObject playerSpawn = Instantiate(playerSpawnPoint, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);
                        ground.transform.position = playerSpawn.transform.position;
                        playerSpawn.transform.parent = LevelCreated.transform;

                        Vector3 playerPos = playerSpawn.transform.position;
                        playerPos = new Vector3(playerPos.x, playerPos.y+0.6f, playerPos.z);
                        playerSpawn.transform.position = playerPos;

                        Vector3 newCamPos = playerSpawn.transform.position;
                        newCamPos = new Vector3(newCamPos.x, mainCam.transform.position.y, newCamPos.z);
                        mainCam.transform.position = newCamPos;
                        
                    }
                    
                }
                if(tile == enemyTile)
                {
                    GameObject enemySpawn = Instantiate(enemySpawnPoint, new Vector3(tilemap.localBounds.center.x - (tilemap.localBounds.size.x / 2), 0 ,tilemap.localBounds.center.z - (tilemap.localBounds.size.z / 2) + range) + new Vector3(x * range, 0 ,y*range), Quaternion.identity);

                    enemySpawn.transform.parent = enemySpawnParent;
                }
            }
        }

        if(canBeBuild)
        {   
            SaveMap();
            tilemap.gameObject.SetActive(false);
            highlightMap.gameObject.SetActive(false);
            gridTilemap.SetActive(false);
            isBuilt = true;
        }
    }

    private void CameraZoom()
    {
        float fov = mainCam.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        mainCam.fieldOfView = fov;
    }

    public void SaveMap()
    {
        //Saving map as a prefab to the folder, just for now works only for the editor as a simple creator. 
        string infoEditor = "Saving map works only in editor mode, it saves map into prefab in path: Assets/Prefabs/LevelsCreated/";
        alertInfo.SetAlertInfo(infoEditor);

        #if(UNITY_EDITOR)

            string localPath = "Assets/Prefabs/LevelsCreated/" + LevelCreated.name+".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAssetAndConnect(LevelCreated, localPath, InteractionMode.UserAction);

            string info = "Map was succesfully created and prefab was saved in path: Assets/Prefabs/LevelsCreated/";
            alertInfo.SetAlertInfo(info);
            
        #endif
    }

}
