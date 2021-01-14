using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class BoneManager : MonoBehaviour
{
    public static BoneManager Singleton;

    public GameObject[] bonePrefabs;
    public List<Bone> bones;

    Dictionary<Vector2, GameObject> bonesByPos;

    private void Awake()
    {
        Singleton = this;
        bonesByPos = new Dictionary<Vector2, GameObject>();
        if (bonePrefabs.Length == 0)
            Application.Quit();
    }

    public Bone GetClosestBone(Vector3 pos)
    {
        Bone closest = null;
        float minDist = 9999999;
        foreach( Bone bone in bones)
        {
            float dist = (bone.transform.position - pos).sqrMagnitude;
            if (dist < minDist)
            {
                closest = bone;
                minDist = dist;
            }
        }
        return closest;
    }

    public void SpawnBone(Vector3 pos, Color color)
    {
        color.a = 1;
        GameObject obj;
        // :: new bone
        bonesByPos.TryGetValue(new Vector2(pos.x, pos.y), out obj);
        if (obj == null)
        {
            GameObject boneprefab = bonePrefabs[Random.Range(0, bonePrefabs.Length)];
            obj = GameObject.Instantiate(boneprefab, pos, Quaternion.identity);
            bones.Add(obj.GetComponent<Bone>());
        }
        else
        {
            bonesByPos.Remove(new Vector2(pos.x, pos.y));
        }

        Bone bone = obj.GetComponent<Bone>();
        bone.Burried(false);
        bone.Color = color;
    }

    public void DestroyBone(Vector2 pos, Bone bone)
    {
        bonesByPos.Add(pos, bone.gameObject);
        bone.Burried(true);
    }

    void GetPoints()
    {
        int point = 0;
       foreach(Bone bone in bones)
        { 
            if ( bone.Color == new Color(1,0.5f, 0f,1.0f))
            {
                point += bone.points;
            }
        }
        Debug.Log(point);

    }
    // Start is called before the first frame update
    void Update()
    {

        if ( Input.GetKeyDown(KeyCode.B))
        {
            GetPoints();
        }
        //byte[] fileData;
        //Tile wallTile = TilesResourcesLoader.GetWallTile();
        //Tile groundTile = TilesResourcesLoader.GetGroundTile();

        //string path = Application.dataPath + "/Resources/map/map_1.png";
        //if ( File.Exists(path))
        //{
        //    fileData = File.ReadAllBytes(path);
        //    MapHelper.ground = new Texture2D(2, 2);
        //    MapHelper.ground.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        //}
        //Vector3 cellSize = tilemaps[0].editorPreviewSize;
        //for ( int i = 0; i < MapHelper.ground.width; i ++ )
        //{
        //    for (int j = 0; j < MapHelper.ground.height; j++)
        //    {
        //        Color color = MapHelper.ground.GetPixel(i, j);
        //        Debug.Log("color" + color);

        //        if ( color == Color.black)
        //        {
        //            tilemaps[1].SetTile(new Vector3Int(i, j, 0), wallTile);
        //        }
        //        else if ( color.a == 0 )
        //        {
        //            MapHelper.ground.SetPixel(i, j, Color.white);
        //            tilemaps[0].SetTile(new Vector3Int(i, j, 0), groundTile);
        //        }
        //    }

        //    MapHelper.ground.Apply();
        //}

       
      
    }

    // Update is called once per frame
}
