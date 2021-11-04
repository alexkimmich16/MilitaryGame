using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KingdomAssigner : MonoBehaviour
{
    #region Singleton

    public static KingdomAssigner instance;

    void Awake()
    {
        instance = this;
        //GetSpots();
    }

    #endregion

    public HexGenerator map;

    public Transform Center;

    public GameObject Dot;
    
    public Vector2 imageDim;
    public Vector2 KingdomRestraint;
    public int regionAmount;
    public List<Vector2> Spots = new List<Vector2>();
    private List<GameObject> Dots = new List<GameObject>();



    List<Color> colorList = new List<Color>()
     {
        Color.white,
        Color.green,
        Color.blue,
        Color.red,
        Color.gray,
        Color.cyan,
     };

    public void GetSpots()
    {
        if (Spots.Count != 0)
        {
            
            int Factions = Spots.Count;
            Spots.Clear();
            if (GameControl.instance.SpawnDots == true)
            {
                for (int i = 0; i < Factions; i++)
                {
                    GameObject ToRemove = Dots[i];
                    Dots.Remove(ToRemove);
                    Destroy(Dots[i]);
                }
            }
            
                
        }
        StartOrganiser organiser = StartOrganiser.instance;

        Vector3Int coordinate = new Vector3Int(map.RealWidth, map.RealHeight, 0);
        Vector3 TilePos = map.mountainMap.CellToWorld(coordinate);
        
        imageDim.x = TilePos.x;
        imageDim.y = TilePos.y;

        for (int i = 0; i < regionAmount; i++)
        {
            float X = Random.Range(0, TilePos.x);
            float Y = Random.Range(0, TilePos.y);

            Spots.Add(new Vector2(X, Y));

            if(GameControl.instance.SpawnDots == true)
            {
                GameObject DotObj = Instantiate(Dot, Spots[i], Quaternion.identity);
                Dots.Add(DotObj);
                ChangeColour(i, DotObj);
            }
        }
    }

    void Update()
    {
       if(GameControl.instance.ShowBoarder == true)
       {
            Debug.DrawRay(new Vector2(0,0), new Vector2(0, imageDim.y), Color.green);
            Debug.DrawRay(new Vector2(0, imageDim.y), new Vector2(imageDim.x, 0), Color.green);
            Debug.DrawRay(new Vector2(imageDim.x, imageDim.y), new Vector2(0, -imageDim.y), Color.green);
            Debug.DrawRay(new Vector2(0, 0), new Vector2(imageDim.x, 0), Color.green);
       }

    }

    public void ChangeColour(int Kingdom, GameObject DotObj)
    {
        DotObj.GetComponent<SymbolScript>().Kingdom = Kingdom;
        SpriteRenderer spriteRenderer = DotObj.GetComponent<SpriteRenderer>();

        
        /*
        else if (Kingdom == 6)
        {
            //spriteRenderer.color = Color.magenta;
        }
        */

    }
}
