using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : MonoBehaviour
{
    public GameObject Boarder;
    public GameObject bullet;
    //public GameObject Gun;
    public bool IsSelected;
    public Unit unit;
    public int Health;
    public int MaxHealth;
    public Transform Target;
    //public Transform firepoint;
    //public float force;
    public List<RaycastHit2D> Hits = new List<RaycastHit2D>();
    public bool CharacterFirst;
    public bool MountainFirst;
    public bool AbleToShoot;
    public float MaxTime;
    public float Elapsed;
    
    public bool AimingAtTarget;

    public GameObject MussleFlash;
    public bool MussleFlashBool;
    public float FlashTimer;

    public GameObject Ring;
    public float Range;
    public float ColorValue;

    SpriteRenderer SR;
    public float Fallspeed;

    public int AmountInside;

    public bool MovingToBoarder;

    void DrawLine(Vector3 start, Vector3 end)
    {
        //Color color = color.red;
        float duration = 0.2f;
        GameObject myLine = new GameObject();
        myLine.transform.parent = unit.PathParent;
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.SetColors(Color.red, Color.red);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.sharedMaterials = unit.mat;
        GameObject.Destroy(myLine, duration);
    }

    // Start is called before the first frame update
    public void Attack(Transform target)
    {
        Target = target;
    }
    void Start()
    {
        Health = MaxHealth;
        unit = gameObject.GetComponent<Unit>();
        //ClickScript.instance.infantry.Add(this);
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
    void Update()
    {
        ColorValue += Fallspeed;

        Color NewColor = new Color(1, ColorValue, ColorValue);
        SR.color = NewColor;


        if (AbleToShoot == true)
        {
            Ring.SetActive(true);
            Ring.transform.localScale = new Vector3(Range, Range, Range);
        }
        else
        {
            Ring.SetActive(false);
        }

        if (MussleFlashBool == true)
        {
            FlashTimer += Time.deltaTime;
            MussleFlash.SetActive(true);
            if (FlashTimer > 0.2f)
            {
                MussleFlash.SetActive(false);
                MussleFlashBool = false;
                FlashTimer = 0;
            }
        }
        else
        {
            MussleFlash.SetActive(false);
        }
        
        
        if (IsSelected == true)
        {
            Boarder.SetActive(true);
        }
        else
        {
            Boarder.SetActive(false);
        }

        if(Target != null)
        {
            Vector3 start = unit.map.TileCoordToWorldCoord(transform.position.x, transform.position.y) +
                    new Vector3(0, 0, -0.5f);
            Vector3 end = unit.map.TileCoordToWorldCoord(Target.position.x, Target.position.y) +
                new Vector3(0, 0, -0.5f);
            DrawLine(start, end);

            /*
            while (AbleToShoot == true)
            {
                
            }
            */

            Vector2 direction = Target.position - transform.position;
            RaycastHit2D[] Casts = Physics2D.RaycastAll(transform.position, direction, Range / 2.2f);
            for (int i = 0; i < Casts.Length; i++)
            {

                if (CharacterFirst == false && MountainFirst == false)
                {
                    if (Casts[i].transform.tag == "Mountain")
                    {
                        MountainFirst = true;
                    }
                    if (Casts[i].transform.tag == "Enemy")
                    {
                        CharacterFirst = true;
                    }
                }
                if (Casts[i].transform.tag == "Mountain")
                {
                }
                if (Casts[i].transform.tag == "Enemy")
                {
                }
            }
        }
        

        Transform Current;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            if (allegiances.instance.Lists[unit.FactionNum].State[unit.FactionNum] == -1 && i != unit.FactionNum)
            {
                for (int j = 0; j < allegiances.instance.Lists[unit.FactionNum].InfantryTransforms.Count; j++)
                {
                    float dist = Vector3.Distance(allegiances.instance.Lists[i].InfantryTransforms[j].position, transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        Target = allegiances.instance.Lists[i].InfantryTransforms[j];
                    }
                }
            }
        }
        
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        //RaycastHit hit;
        int layerMask = (1 << 10) | (1 << 11);
        
        if (CharacterFirst == true)
        {
            AbleToShoot = true;
        }
        else
        {
            AbleToShoot = false;
        }
        CharacterFirst = false;
        MountainFirst = false;

        Elapsed += Time.deltaTime;
        if (Elapsed > MaxTime)
        {
            if(AbleToShoot == true)
            {
                Elapsed = 0;
                Shoot();
            }
            else
            {
                Elapsed = 0;
            }
            
        }
    }

    public void Shoot()
    {
        MussleFlashBool = true;

        Target.GetComponent<Skeleton>().AddDamage(1);


    }

    public void AddDamage(int Damage)
    {
        ColorValue -= ColorValue;
        Health -= Damage;
    }
}

