using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharColour : MonoBehaviour
{
    public int Hairs;
    public int Skins;
    public int Eyes;
    void Start()
    {
        //Hair
        //int Num = System.Enum.GetValues(typeof(allegiances.instance.Lists.HairColours)).Length;
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            int ColourHairNum = Random.Range(0, Hairs - 1);
            allegiances.instance.Lists[i].ChangeHairColour(ColourHairNum);

            int ColourSkinNum = Random.Range(0, Skins - 1);
            allegiances.instance.Lists[i].ChangeSkinColour(ColourSkinNum);

            int ColourEyeNum = Random.Range(0, Eyes - 1);
            allegiances.instance.Lists[i].ChangeEyeColour(ColourEyeNum);
        }

        
    }
}
