using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public  class MapHelper : MonoBehaviour
{
    public Texture2D _ground = null;
    public Tilemap[] _tilemaps;

    public List<Vector2> _groundCoordinates;
    public List<Vector2> _bonesCoordinates;
    public List<Vector2> _diggedCoordinates;

    public static MapHelper Singleton;

    private Color diggedColor = new Color(0, 0, 0,0.2f);
    private float boneOpacity = 0.4f;
    private BoneManager bonePlacementComponent;
    public Vector2 spawnPlayerPosition;
    public Vector2[] spawnDogsPosition;

    int dogsCount = 0;

    public void Awake()
    {
        Singleton = this;
        _tilemaps = GetComponentsInChildren<Tilemap>();

        bonePlacementComponent = GetComponent<BoneManager>();

        spawnDogsPosition = new Vector2[10];
    }

    private void Start()
    {
       LoadMap("/Resources/map/map_1.png");
       PlaceBones(60);
    }

    public  bool GetTileWithPosition(Vector3 pos)
   {
        int X = Mathf.FloorToInt( pos.x);
        int Y = Mathf.FloorToInt(pos.y );

        if (X > _ground.width || Y > _ground.height)
            return false;

        if (_ground.GetPixel(X, Y) == Color.white || _ground.GetPixel(X, Y).a == boneOpacity)
            return true;
        else return false;
    }

    public bool IsStillDigged(Vector2 loc)
    {
        return _diggedCoordinates.Find(x => x == loc) != null ;
    }

    public Vector2 GetClosestDigged(Vector3 pos)
    {
        Vector2 posPlayer = pos;

        Vector2 closest = Vector2.zero;
        float minDistance = 9999;
        foreach( Vector2 loc in _diggedCoordinates)
        {
            float dist = (loc - posPlayer).sqrMagnitude;
            if (dist < minDistance)
            { 
                minDistance = dist;
                closest = loc;
            }
        }
        return closest;

    }
    public bool SetTileWithBone(Vector3 pos, Bone bone)
    {
        int X = Mathf.FloorToInt(pos.x);
        int Y = Mathf.FloorToInt(pos.y);
        if (X > _ground.width || Y > _ground.height)
            return false;

        if (_ground.GetPixel(X, Y).a == diggedColor.a)
        {
            Color color = bone.Color;
            color.a = boneOpacity;
            BoneManager.Singleton.DestroyBone(new Vector2(X, Y), bone);
            _diggedCoordinates.Remove(new Vector2(X, Y));

            _tilemaps[0].SetTile(new Vector3Int(X, Y, 0), TilesResourcesLoader.GetGroundUsedTile());
            _ground.SetPixel(X, Y, color);
            return true;
        }

        return false;
    }

    public bool SetTileDigged(Vector3 pos)
    {
        int X = Mathf.FloorToInt(pos.x);
        int Y = Mathf.FloorToInt(pos.y);
        if (X > _ground.width || Y > _ground.height)
            return false;
        if (_ground.GetPixel(X, Y) == Color.white)
        {
            _tilemaps[0].SetTile(new Vector3Int(X, Y, 0), TilesResourcesLoader.GetUsedTile());
            _ground.SetPixel(X, Y, diggedColor);
            _diggedCoordinates.Add(new Vector2(X, Y));
        }
        if (_ground.GetPixel(X, Y).a == boneOpacity)
        {
            Color color = _ground.GetPixel(X, Y);
            color.a = 1;
            bonePlacementComponent.SpawnBone(new Vector3(X , Y, 0), color);
            _tilemaps[0].SetTile(new Vector3Int(X, Y, 0), TilesResourcesLoader.GetUsedTile());
            _ground.SetPixel(X, Y, diggedColor);
            _diggedCoordinates.Add(new Vector2(X, Y));
            return true;
        }

        return false;
    }


    void PlaceBones(int count)
    {
        count = Mathf.Min(count, _groundCoordinates.Count);
        int bones = 0;

        while ( bones < count )
        {
            int index = Random.Range(0, _groundCoordinates.Count - 1);
            Vector2 ground = _groundCoordinates[index];
            _groundCoordinates.RemoveAt(index);
            _bonesCoordinates.Add(ground);

            _ground.SetPixel((int)ground.x, (int)ground.y, new Color(1,1,1, boneOpacity));
            bones++;
        }
    }

    public void LoadMap(string filePath)
    {
        byte[] fileData;
        Tile wallTile = TilesResourcesLoader.GetWallTile();
        Tile groundTile = TilesResourcesLoader.GetGroundTile();

        string path = Application.dataPath + filePath;
        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            _ground = new Texture2D(2, 2);
            _ground.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        Vector3 cellSize = _tilemaps[0].editorPreviewSize;
        for (int i = 0; i < _ground.width; i++)
        {
            for (int j = 0; j < _ground.height; j++)
            {
                Color color = _ground.GetPixel(i, j);
                Debug.Log(color);

                if (color == Color.black)
                {
                    _tilemaps[1].SetTile(new Vector3Int(i, j, 0), wallTile);
                }

                else
                {
                    _ground.SetPixel(i, j, Color.white);
                    _tilemaps[0].SetTile(new Vector3Int(i, j, 0), groundTile);
                    _groundCoordinates.Add(new Vector2(i, j));

                    if (color.r == 1)
                    {
                        spawnPlayerPosition = new Vector2(i, j);
                    }
                    else if (color.g == 1)
                    {
                        spawnDogsPosition[dogsCount] = new Vector2(i, j);
                        dogsCount++;
                    }
                }
            }

            _ground.Apply();
        }
    }
}
