using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionGen : MonoBehaviour
{
    #region Singleton
    public static DescriptionGen instance;
    void Awake() { instance = this; }
    #endregion
    
    public List<Sequence> Sequences = new List<Sequence>();
    //Sequence
    public string N0, N1, N2, N3, N4, N5, N6, n1;

    private bool First = true;

    public string GenerateSentance(Person person, int Type)
    {
        string HeShe = "";
        string HimHer = "";
        string HisHer = "";
        if ((int)person.gender == 0)
        {
            HeShe = "she";
            HimHer = "her";
            HisHer = "her";
        }
        else
        {
            HeShe = "he";
            HimHer = "him";
            HisHer = "his";
        }
        int SequnceNum = Random.Range(0, Sequences[Type].Discriptions.Count);
        Description D = Sequences[Type].Discriptions[SequnceNum];
        string Endstring = "";
        int Chunks = 0;
        int RandomList = 0;
        
        for (int i = 0; i < D.sequence.Count; i++)
        {
            bool Capital = false;

            int Total = D.sequence[i];
            //string String = Total.ToString();
            char first = D.sequence[i].ToString()[0];
            int Current = (int)char.GetNumericValue(first);
            //int.TryParse(first, out int Current);

            //is double digits
            if (D.sequence[i] > 10)
            {
                //Debug.Log(Capital);
                Capital = true;
                
            }

            if (First == true)
            {
                //Capital = true;
            }

            //get check second didget
            //if, turn to captital when necicarry

            if (Current == 0)
            {
                Endstring = Endstring + " " + D.TextChunks[Chunks];
                Chunks += 1;
            }
            else if (Current == 1)
            {
                int Num = Random.Range(0, D.Blanks[RandomList].BlankStrings.Count);
                Endstring = Endstring + " " + D.Blanks[RandomList].BlankStrings[Num];
                RandomList += 1;
            }
            else if (Current == 2)
            {
                Endstring = Endstring + " " + person.FullName;
            }
            else if (Current == 3)
            {
                Endstring = Endstring + " " + person.First;
            }
            else if (Current == 4)
            {
                if (Capital == true)
                {
                    string Caps = char.ToUpper(HimHer[0]) + HimHer.Substring(1);
                    //Debug.Log(Caps);
                    Endstring = Endstring + " " + Caps;
                }
                else
                {
                    Endstring = Endstring + " " + HimHer;
                }
            }
            else if (Current == 5)
            {
                if (Capital == true)
                {
                    string Caps = char.ToUpper(HeShe[0]) + HeShe.Substring(1);
                    //Debug.Log(Caps);
                    Endstring = Endstring + " " + Caps;
                }
                else
                {
                    Endstring = Endstring + " " + HeShe;
                }
                
            }
            else if (Current == 6)
            {
                if (Capital == true)
                {
                    string Caps = char.ToUpper(HisHer[0]) + HisHer.Substring(1);
                    //Debug.Log(Caps);
                    Endstring = Endstring + " " + Caps;
                }
                else
                {
                    Endstring = Endstring + " " + HisHer;
                }
                
            }
        }
        First = false;
        return Endstring;
    }

    /*
    public string SelectSequence(int Num, bool Male, string First, string Second)
    {
        if (Num == 1)
        {
            FirstSentence(Male,);
        }
        else if (Num == 2)
        {

        }
        else if (Num == 3)
        {

        }
    }
    */
}
