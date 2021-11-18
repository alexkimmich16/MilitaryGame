using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Worker : MonoBehaviour
{
    public Unit unit;
    public GameObject Boarder;
    public bool IsSelected;
    public bool Building = false;
    private GameObject hitObject;
    public bool IsBuilding1;
    public bool RequirementsMet;
    public bool SentMode;
    public bool Backup;
    public ClickableTile tile;
    public int Health;
    public int MaxHealth;
    public float ColorValue;
    public float Fallspeed;

    SpriteRenderer SR;
    void Start()
    {
        Health = MaxHealth;
        unit = gameObject.GetComponent<Unit>();
        //ClickScript.instance.Workers.Add(this);
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeBool()
    {
        IsSelected = !IsSelected;
    }
    public void ClearActive()
    {
        IsSelected = false;
    }
    // Update is called once per frame
    void Update()
    {
        ColorValue += Fallspeed;

        Color NewColor = new Color(1, ColorValue, ColorValue);
        SR.color = NewColor;



        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        if (IsSelected == true)
        {
            Boarder.SetActive(true);
        }
        else
        {
            Boarder.SetActive(false);
        }

        if (RequirementsMet == true && SentMode == true)
        {
            hitObject.GetComponent<BuildProgress>().ChangeWorker(true);
            hitObject = null;
            IsBuilding1 = false;
            SentMode = false;
        }

        IsBuilding1 = HexGenerator.instance.CheckForBuilding(unit.GridX, unit.GridY);
        //Debug.Log(unit.GridX + " " + unit.GridY);
        if (IsBuilding1 == true)
        {
            Vector2 point = new Vector2(transform.position.x, transform.position.y);
            int layerMask = (1 << 8);
            Collider2D hit = Physics2D.OverlapPoint(point, layerMask);
            if (hit.gameObject.GetComponent<BuildProgress>())
            {
                hitObject = hit.gameObject;
            }
            if (hitObject != null)
            {
                tile = hitObject.GetComponent<ClickableTile>();
                //RequirementsMet = HexGenerator.instance.CheckForBuilding(tile.tileX, tile.tileY);
            }
        }
        else
        {
            RequirementsMet = false;
            if (RequirementsMet == false && SentMode == false)
            {
                SentMode = true; 
                if (hitObject != null)
                {
                    hitObject.GetComponent<BuildProgress>().ChangeWorker(false);
                }
                    
            }
        }
    }

    public void AddDamage(int Damage)
    {
        ColorValue -= ColorValue;
        Health -= Damage;
    }
}
