using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListOfEnemies
{
    public string DominentHouseName;
    public string LandName;
    public string Color;

    public Person Leader;

    public HairColours Hair;
    public SkinColours Skin;
    public EyeColours Eye;
    
    public int Money;
    public int Land;
    public List<int> State = new List<int>();
    public List<Transform> ArmyTransforms = new List<Transform>();
    public List<Transform> InfantryTransforms = new List<Transform>();
    public List<Transform> LandBoarder = new List<Transform>();
    public List<Transform> WaterBoarder = new List<Transform>();

    public List<Castle> Castles = new List<Castle>();

    //[HideInInspector]
    public int Alliences;

    //[HideInInspector]
    public int Enemies;

    //[HideInInspector]
    public int Neutral;

    //[HideInInspector]
    public int Power;
    //private int AlliencePower;
    //private int EnemyPower;

    public enum EyeColours
    {
        Black,
        Blue,
        Green,
        Yellow,
        Red,
        Brown
    }
    public enum SkinColours
    {
        White,
        Black,
        Brown
    }
    public enum HairColours
    {
        Red,
        White,
        Black,
        Blue
    }

    public void ChangeSkinColour(int Num)
    {
        if (Num == 0)
        {
            Skin = SkinColours.White;
        }
        else if (Num == 1)
        {
            Skin = SkinColours.Black;
        }
        else if (Num == 2)
        {
            Skin = SkinColours.Brown;
        }
    }

    public void ChangeEyeColour(int Num)
    {
        if (Num == 0)
        {
            Eye = EyeColours.Black;
        }
        else if (Num == 1)
        {
            Eye = EyeColours.Black;
        }
        else if (Num == 2)
        {
            Eye = EyeColours.Green;
        }
        else if (Num == 3)
        {
            Eye = EyeColours.Yellow;
        }
        else if (Num == 4)
        {
            Eye = EyeColours.Red;
        }
        else if (Num == 5)
        {
            Eye = EyeColours.Brown;
        }
    }

    public void ChangeHairColour(int Num)
    {
        if(Num == 0)
        {
            Hair = HairColours.Red;
        }
        else if (Num == 1)
        {
            Hair = HairColours.White;
        }
        else if (Num == 2)
        {
            Hair = HairColours.Black;
        }
        else if (Num == 3)
        {
            Hair = HairColours.Blue;
        }
    }

}
