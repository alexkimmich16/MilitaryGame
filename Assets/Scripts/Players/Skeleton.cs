using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    public Unit unit;
    public int Health;
    public int MaxHealth;
    public float Fallspeed;
    public float ColorValue;
    SpriteRenderer SR;
    private bool Walking;
    public Text text;
    public bool AtTarget;
    public Transform Target;
    public int AttackType;
    public int Damage;
    public int Range;

    public float AttackTimer;
    public float AttackTime;

    public Unit EnemyUnit;

    private bool Started = false;

    public bool HasLanded;

    //public enum Factions { FactionA, FactionB, FactionC, FactionD, FactionE, FactionF };

   // public Factions factions;

    void Start()
    {
        Health = MaxHealth;
        unit = gameObject.GetComponent<Unit>();
        SR = gameObject.GetComponent<SpriteRenderer>();

        PathFindingSpacer.instance.Skeletons.Add(this);
    }

    void AttackAnother()
    {
        int AttackDamage = Random.Range(Damage - Range, Damage + Range);

        if(Target == null)
        {
            return;
        }
        else if (Target.GetComponent<Infantry>())
        {
            //Target.GetComponent<Infantry>().AddDamage(AttackDamage);
        }
        else if (Target.GetComponent<Worker>())
        {
            //Target.GetComponent<Worker>().AddDamage(AttackDamage);
        }
        else if (Target.GetComponent<Mech>())
        {
            //Target.GetComponent<Mech>().AddDamage(AttackDamage);
        }
        else if (Target.GetComponent<Skeleton>())
        {
            //Target.GetComponent<Skeleton>().AddDamage(AttackDamage);
        }
    }
    //THIS
    void Update()
    {        
        bool found = false;
        if (Target != null)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Target.GetComponent<Unit>())
                {
                    if (unit.GridY % 2 == 1)
                    {
                        int X = unit.GridX + unit.map.neighbourOffsetArrayOdd[i].x;
                        int Y = unit.GridY + unit.map.neighbourOffsetArrayOdd[i].y;
                        if (Target.GetComponent<Unit>().GridX == X && Target.GetComponent<Unit>().GridY == Y)
                        {
                            
                            found = true;
                        }
                    }
                    else
                    {
                        int X = unit.GridX + unit.map.neighbourOffsetArrayEven[i].x;
                        int Y = unit.GridY + unit.map.neighbourOffsetArrayEven[i].y;
                        if (Target.GetComponent<Unit>().GridX == X && Target.GetComponent<Unit>().GridY == Y)
                        {
                            AtTarget = true;
                            found = true;
                        }
                    }

                }
                /*
                else if (Target.GetComponent<ClickableTile>())
                {

                }
                */
            }
            if(found == true)
            {
                AtTarget = true;
            }
            else if (found == false)
            {
                AtTarget = false;
            }
        }
        else
        {
            FindPath();
        }

        if (AtTarget == true)
        {
            AttackTimer += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
        }
        
        if (AtTarget == true && AttackTimer > AttackTime)
        {
            AttackTimer = 0;
            AttackAnother();
        }
        
        string SpeedDisplay = Health + "/" + MaxHealth;
        text.text = SpeedDisplay;

        ColorValue += Fallspeed;

        Color NewColor = new Color(1, ColorValue, ColorValue);
        SR.color = NewColor;

        if (Health <= 0)
        {
            PeopleManager.instance.All.Remove(transform);
            FindPath();
            unit.RemoveFromArmyList();
            Destroy(gameObject);
            PathFindingSpacer.instance.Skeletons.Remove(this);
        }
    }
    //calledBySpacer
    public void FindPath()
    {
        //Debug.Log("FIndPath1");
        //Debug.Log(unit.FactionNum - 1 + " FACTIONNUM");
        if(AtTarget == false)
        {
            HexGenerator.instance.TM.FindBuildings(transform, unit.FactionNum - 1);
            Started = true;
        }
        
    }

    public void TargetFound(Transform Tar, int Type)
    {
        AttackType = Type;
        Target = Tar;
        if(Tar != null)
        {
            EnemyUnit = Target.GetComponent<Unit>();
        }
        
    }

    public void AddDamage(int Damage)
    {
        ColorValue -= ColorValue;
        Health -= Damage;
    }
}

