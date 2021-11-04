using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class NameAssigner : MonoBehaviour
{
    #region Singleton

    public static NameAssigner instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    public List<string> HouseNameFront = new List<string>();
    public List<string> HouseNameBack = new List<string>();

    
    public List<string> FirstNameMale = new List<string>();
    public List<string> FirstNameFemale = new List<string>();

    public List<NameSequence> Sequences = new List<NameSequence>();

    public List<Person> People = new List<Person>();

    public int HousePoints;
    public int PointsLeft;

    public List<int> TakenNames = new List<int>();

    public int GenerationGap;
    public int GenerationLeaniency;

    public int MinimumChildrenAge;

    public Transform FamilyTree;
    public GameObject Panel;

    //public Vector2 Up;
    public float Gap;
    public float Right;
    public float Up;
    //0 = Adjective
    //1 = noun
    //2 = The
    //3 = HouseName
    public List<string> FirstNameMaleSave = new List<string>();
    public List<string> FirstNameFemaleSave = new List<string>();

    public List<GameObject> Panels = new List<GameObject>();

    int NumberSpawned;

    private bool WaitFrame = true;

    private bool Firstbool = true;

    public void GenerateName(int Num, string House)
    {
        
        List<string> StringList = new List<string>();
        People.Add(new Person());
        allegiances.instance.People[Num].People.Add(People[People.Count - 1]);
        int Gender = Random.Range(0, 2);
        string First = "";
        int X = 0;
        People[People.Count - 1].PositionInList = allegiances.instance.People[Num].People.Count - 1;
        People[People.Count - 1].Faction = Num;
        if (Gender == 1)
        {
            X = Random.Range(0, FirstNameMaleSave.Count);
            First = FirstNameMaleSave[X];
            People[People.Count - 1].GenderChoose(1);
            FirstNameMaleSave.Remove(FirstNameMaleSave[X]);
        }
        else if(Gender == 0)
        {
            X = Random.Range(0, FirstNameFemaleSave.Count);
            First = FirstNameFemaleSave[X];
            People[People.Count - 1].GenderChoose(0);
            FirstNameFemaleSave.Remove(FirstNameFemaleSave[X]);
        }
        for (int i = 0; i < 10; i++)
        {
            People[People.Count - 1].Relations.Add("");
        }

        if (allegiances.instance.People[Num].People.Count > 1)
        {
            int PersonToRelateTo = Random.Range(0, allegiances.instance.People[Num].People.Count - 1);
            if (allegiances.instance.People[Num].People[PersonToRelateTo] == People[People.Count - 1])
            {
                //Debug.Log("Same");
            }
            int ToSubtract = 0;
            if (allegiances.instance.People[Num].People[PersonToRelateTo].Age < 25)
            {
                ToSubtract = 1;
            }
            
            int FamilyRole = Random.Range(0, 4 - ToSubtract);
            //Debug.Log(FamilyRole);
            Person ThisPerson = People[People.Count - 1];
            //i=0
            //this == 1
            //other == 0
            Person Other = allegiances.instance.People[Num].People[PersonToRelateTo];
            if (Gender == 1 || Gender == 0)
            {
                if (FamilyRole == 0 || FamilyRole == 2)
                {
                    //other is son
                    //this is father
                    
                    Other.Relations[ThisPerson.PositionInList] = "Son";
                    ThisPerson.Relations[Other.PositionInList] = "Father";
                    ThisPerson.Children.Add(Other);
                    Other.Parent1 = People[People.Count - 1];
                    Other.HasParent = true;

                    int Minimum = Other.Age + GenerationGap;
                    int Maximum = Other.Age + GenerationGap + (GenerationLeaniency);

                    ThisPerson.Age = Random.Range(Minimum, Maximum);
                }
                else if (FamilyRole == 1)
                {
                    //both are siblings
                    ThisPerson.Relations[Other.PositionInList] = "Sibling";
                    Other.Relations[ThisPerson.PositionInList] = "Sibling";

                    int Minimum = Other.Age - GenerationLeaniency;
                    int Maximum = Other.Age + GenerationLeaniency;
                    ThisPerson.Age = Random.Range(Minimum, Maximum);
                }
                else if (FamilyRole == 3)
                {
                    //other is father
                    //this is son
                    ThisPerson.Relations[Other.PositionInList] = "Son";
                    Other.Relations[ThisPerson.PositionInList] = "Father";

                    Other.Children.Add(ThisPerson);
                    ThisPerson.Parent1 = Other;
                    ThisPerson.HasParent = true;

                    int Minimum = Other.Age - GenerationGap - (GenerationLeaniency);
                    int Maximum = Other.Age - GenerationGap;
                    if (Minimum < 0)
                    {
                        Minimum = 0;
                    }
                    ThisPerson.Age = Random.Range(Minimum, Maximum);
                }
            }
            else if (Gender == 0)
            {
                if (FamilyRole == 0 && FamilyRole == 2)
                {
                    //Mother
                    //Debug.Log("Mother.1");
                    Other.Relations[Other.PositionInList] = "Daughter";
                    ThisPerson.Relations[People.Count - 1] = "Father";

                    ThisPerson.Children.Add(ThisPerson);
                    Other.Parent1 = ThisPerson;
                    Other.HasParent = true;


                    int Minimum = Other.Age + GenerationGap;
                    int Maximum = Other.Age + GenerationGap + (GenerationLeaniency);

                    People[People.Count - 1].Age = Random.Range(Minimum, Maximum);
                    //Debug.Log("Mother.1");
                }
                else if (FamilyRole == 1)
                {
                    //both are siblings
                    int Minimum = Other.Age - GenerationLeaniency;
                    int Maximum = Other.Age + GenerationLeaniency;

                    People[People.Count - 1].Age = Random.Range(Minimum, Maximum);

                    ThisPerson.Relations[Other.PositionInList] = "Sibling";
                    //Debug.Log("Sister2.3" + "  NUM= " + Num + "  PersonToRelateTo= " + PersonToRelateTo + "  Count= " + allegiances.instance.People[Num].People.Count + "  PosinList= " + allegiances.instance.People[Num].People[PersonToRelateTo].PositionInList);
                    
                    allegiances.instance.People[Num].People[PersonToRelateTo].Relations[allegiances.instance.People[Num].People.Count] = "Sibling";
                }
                else if (FamilyRole == 3)
                {
                    //Daughter
                    ThisPerson.Relations[allegiances.instance.People[Num].People[PersonToRelateTo].PositionInList] = "Son";
                    allegiances.instance.People[Num].People[PersonToRelateTo].Relations[allegiances.instance.People[Num].People.Count] = "Father";
                    allegiances.instance.People[Num].People[PersonToRelateTo].Children.Add(ThisPerson);
                    ThisPerson.Parent1 = allegiances.instance.People[Num].People[PersonToRelateTo];
                    ThisPerson.HasParent = true;
                    int Minimum = allegiances.instance.People[Num].People[PersonToRelateTo].Age - GenerationGap - (GenerationLeaniency);
                    int Maximum = allegiances.instance.People[Num].People[PersonToRelateTo].Age - GenerationGap;
                    if (Minimum < 0)
                    {
                        Minimum = 0;
                    }
                    ThisPerson.Age = Random.Range(Minimum, Maximum);
                    

                }
            }
        }
        else
        {
            People[People.Count - 1].Age = Random.Range(0, 100);
        }
        //Debug.Log("PT2.1");

        StringList.Add(First + " ");
        StringList.Add(House + "");

        People[People.Count - 1].First = First;
        People[People.Count - 1].Last = House;
        string Final = "";
        for (int k = 0; k < StringList.Count; k++)
        {
            Final += StringList[k];
        }
        People[People.Count - 1].FullName = Final;
        People[People.Count - 1].House = Num;

        int Command = Random.Range(0, 5);
        PointsLeft -= Command;
        People[People.Count - 1].Command = Command;

        int Wits = Random.Range(0, 5);
        PointsLeft -= Wits;
        People[People.Count - 1].Wits = Wits;

        int Diplomacy = Random.Range(0, 5);
        PointsLeft -= Diplomacy;
        People[People.Count - 1].Diplomacy = Diplomacy;

        int Attractiveness = Random.Range(0, 5);
        PointsLeft -= Attractiveness;
        People[People.Count - 1].Attractiveness = Attractiveness;


        People[People.Count - 1].OverallPositionInList = NumberSpawned;
        
        NumberSpawned += 1;

        //get discription
        string Discription1 = DescriptionGen.instance.GenerateSentance(People[People.Count - 1], 0);
        string Discription2 = DescriptionGen.instance.GenerateSentance(People[People.Count - 1], 1);
        string Discription3 = DescriptionGen.instance.GenerateSentance(People[People.Count - 1], 2);
        string final = Discription1 + Discription2 + Discription3;
        People[People.Count - 1].Discription = final;

        if (Firstbool == true)
        {
            Firstbool = false;
            //Debug.Log("1: " + Discription1);
            //Debug.Log("2: " + Discription2);
            //Debug.Log("3: " + Discription3);
        }
    }

    public void GenerateHouseName(int Num)
    {
        int SequenceNum = Random.Range(0, Sequences.Count);
        int SequenceCount = Sequences[SequenceNum].Sequence.Count;
        List<string> StringList = new List<string>();
        int X = 0;
        int Y = 0;
        if (SequenceNum == 0)
        {
            for (int j = 0; j < SequenceCount; j++)
            {
                
                if (Sequences[SequenceNum].Sequence[j] == 3)
                {
                    X = Random.Range(0, HouseNameFront.Count);
                    string House = HouseNameFront[X];
                    StringList.Add(House);
                }
                else if (Sequences[SequenceNum].Sequence[j] == 4)
                {
                    Y = Random.Range(0, HouseNameBack.Count);
                    string House = HouseNameBack[Y];
                    StringList.Add(House);
                }
            }
        }
        int Contain = (X * HouseNameFront.Count) + Y;
        if (TakenNames.Contains(Contain))
        {
            GenerateHouseName(Num);
            return;
        }
        else
        {
            int ToAdd = (X * HouseNameFront.Count) + Y;
            TakenNames.Add(ToAdd);
        }
        string Final = "";
        for (int k = 0; k < StringList.Count; k++)
        {
            Final += StringList[k];
        }
        allegiances.instance.Lists[Num].DominentHouseName = Final;
    }

    public void AssignCharacterRoles()
    {
        //factions Get Leader
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            //all in faction
            int HighestAge = 14;
            int LevelInCount = 0;
            bool FindFemale = false;
            bool FindAnyone = false;
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                //if age bigger then 14 and is male and is older than 14
                if (allegiances.instance.People[i].People[j].Age >= HighestAge && allegiances.instance.People[i].People[j].gender == Person.Gender.Male)
                {
                    HighestAge = allegiances.instance.People[i].People[j].Age;
                    LevelInCount = j;
                }
            }
            if (HighestAge != 14)
            {
                allegiances.instance.Lists[i].Leader = allegiances.instance.People[i].People[LevelInCount];
            }
            else
            {
                FindFemale = true;
            }
            if (FindFemale == true)
            {
                for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
                {
                    //if is older than 14 with gender not mattering
                    if (allegiances.instance.People[i].People[j].Age >= HighestAge)
                    {
                        HighestAge = allegiances.instance.People[i].People[j].Age;
                        LevelInCount = j;
                    }
                }
                if (HighestAge != 14)
                {
                    allegiances.instance.Lists[i].Leader = allegiances.instance.People[i].People[LevelInCount];
                }
                else
                {
                    FindAnyone = true;
                }
            }
            if (FindAnyone == true)
            {
                HighestAge = 0;
                for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
                {
                    //if age bigger then 0 and gender doesn't matter
                    if (allegiances.instance.People[i].People[j].Age >= HighestAge)
                    {
                        HighestAge = allegiances.instance.People[i].People[j].Age;
                        LevelInCount = j;
                    }

                }
                if (HighestAge != 14)
                {
                    allegiances.instance.Lists[i].Leader = allegiances.instance.People[i].People[LevelInCount];
                }
                else
                {
                    FindAnyone = true;
                }
            }
        }
        
        
    }

    public void CreateFamily()
    {
        //for i in every factions peopl
        CreateGenerations();
    }

    public void CreateGenerations()
    {
        //find lowest generation
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                int MaxNum = 0;
                Person ThisPerson = allegiances.instance.People[i].People[j];
                if (ThisPerson.Children.Count > 0)
                {
                    MaxNum = 1;
                    for (int k = 0; k < ThisPerson.Children.Count; k++)
                    {
                        //ThisPerson.Parent1.Generation += 1;
                        Person Child = ThisPerson.Children[k];
                        if (Child.Children.Count > 0)
                        {
                            if (2 > MaxNum)
                            {
                                MaxNum = 2;
                            }
                            for (int a = 0; a < Child.Children.Count; a++)
                            {
                                Person GrandChild = Child.Children[a];
                                if (GrandChild.Children.Count > 0)
                                {
                                    if (3 > MaxNum)
                                    {
                                        MaxNum = 3;
                                    }
                                    for (int b = 0; b < GrandChild.Children.Count; b++)
                                    {
                                        Person GreatGrandChild = GrandChild.Children[b];
                                        if (GreatGrandChild.Children.Count > 0)
                                        {
                                            if (4 > MaxNum)
                                            {
                                                MaxNum = 4;
                                            }
                                            for (int c = 0; c < GreatGrandChild.Children.Count; c++)
                                            {
                                                Person GreatGreatGrandChild = GreatGrandChild.Children[c];
                                                if (GreatGreatGrandChild.Children.Count > 0)
                                                {
                                                    if (5 > MaxNum)
                                                    {
                                                        MaxNum = 5;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                         
                    }
                    
                }
                ThisPerson.Generation = MaxNum;
            }
        }
    }

    void Start()
    {
        



        CreateGenerations();
        //AssignCharacterRoles();  
    }

    public void MakeFamilyTree()
    {
        int SaveI = 0;
        int SaveJ = 0;
        float CurrentX = -50f;
        float CurrentY = 40f;

        //put all siblings together
        // do x times
        
        for (int x = 0; x < 5; x++)
        {
            //factions
            for (int i = 0; i < allegiances.instance.Lists.Count; i++)
            {
                //ppl in factions 1
                for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
                {
                    //ppl in factions 2
                    for (int k = 0; k < allegiances.instance.People[i].People.Count; k++)
                    {
                        //if ppl1.ppl2 name is a sibling
                        Person Person1 = allegiances.instance.People[i].People[j];
                        Person Person2 = allegiances.instance.People[i].People[k];
                        //1 and 2 are siblings
                        if (Person1.Relations[Person2.PositionInList] == "Sibling")
                        {
                            //check the rest
                            for (int m = 0; m < Person2.Relations.Count; m++)
                            {
                                //Person1.Relations[allegiances.instance.People[i].People[m].PositionInList] == "Sibling"
                                if (Person1.Relations[m] == "Sibling")
                                {
                                    //allegiances.instance.People[i].People[j].Relations[allegiances.instance.People[i].People[m].PositionInList] = "Sibling";
                                    Person2.Relations[m] = "Sibling";
                                }
                            }

                            //opposite is sibling
                            //Debug.Log(i + "  " + j + "   " + k);


                        }
                    }
                }
            }
        }

        //Now for parents
        // if sibling's parent exists its also this's
        //person 2 being checked
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            //ppl in factions 1
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                //ppl in factions 2
                for (int k = 0; k < allegiances.instance.People[i].People.Count; k++)
                {
                    //if ppl1.ppl2 name is a sibling
                    Person Person1 = allegiances.instance.People[i].People[j];
                    Person Person2 = allegiances.instance.People[i].People[k];
                    //1 and 2 are siblings
                    if (Person1.Relations[Person2.PositionInList] == "Sibling")
                    {
                        //check 2's father
                        for (int m = 0; m < Person2.Relations.Count; m++)
                        {
                            //Person1.Relations[allegiances.instance.People[i].People[m].PositionInList] == "Sibling"
                            if (Person2.Relations[m] == "Father")
                            {
                                //allegiances.instance.People[i].People[j].Relations[allegiances.instance.People[i].People[m].PositionInList] = "Sibling";
                                Person1.Relations[m] = "Father";
                            }
                        }
                    }
                        
                }
            }
        }

        //now if 2 parents parent the same child they are married
        //if child is young enough chance for them to be unwed

        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                //Debug.Log("I= " + i + "   J= " + j + "   SaveI= " + SaveI + "   SaveJ= " + SaveJ);

                CurrentX += Right;
                CurrentY = allegiances.instance.People[i].People[j].Generation * Up + 40f;
                    //Gap
                GameObject PanelSpawn = Instantiate(Panel, new Vector3(0, 0 , 2.53f), transform.rotation) as GameObject;
                RectTransform rectTransform = PanelSpawn.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector3(CurrentX, CurrentY, 0);

                PanelSpawn.transform.parent = FamilyTree;
                Transform Child = PanelSpawn.transform.GetChild(0);
                TextMeshProUGUI text = Child.GetComponent<TextMeshProUGUI>();


                text.text = allegiances.instance.People[i].People[j].FullName;

                Panels.Add(PanelSpawn);

                SaveI = i;
                SaveJ = j;
            }
        }

        //removeExtraSlots
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            int CountCurrent = allegiances.instance.People[i].People[0].Relations.Count;
            int CountOccupied = allegiances.instance.People[i].People.Count;
            int RemoveCount = CountCurrent - CountOccupied;
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                //to remove
                for (int k = 0; k < RemoveCount; k++)
                {
                    //Debug.Log("remove");
                    allegiances.instance.People[i].People[j].Relations.RemoveAt(allegiances.instance.People[i].People[j].Relations.Count - 1);
                }
            }
        }

        //assignTHis
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            //ppl in factions 1
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                Person Person1 = allegiances.instance.People[i].People[j];
                Person1.Relations[Person1.PositionInList] = "this";
            }
        }

        //make siblings same Gen
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            //ppl in factions 1
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                //ppl in factions 2
                for (int k = 0; k < allegiances.instance.People[i].People.Count; k++)
                {
                    //if ppl1.ppl2 name is a sibling
                    Person Person1 = allegiances.instance.People[i].People[j];
                    Person Person2 = allegiances.instance.People[i].People[k];
                    //1 and 2 are siblings
                    if (Person1.Relations[Person2.PositionInList] == "Sibling")
                    {
                        //check the rest
                        if (Person1.Children.Count > 0 && Person2.Children.Count == 0)
                        {
                            Person2.Generation = Person1.Generation;
                        }
                        else if (Person2.Children.Count > 0 && Person1.Children.Count == 0)
                        {
                            Person1.Generation = Person2.Generation;
                        }
                        else if (Person2.Children.Count > 0 && Person1.Children.Count == 0)
                        {
                            //both have children
                        }
                        else if (Person2.Children.Count > 0 && Person1.Children.Count == 0)
                        {
                            //neither have children
                        }

                        //opposite is sibling
                        //Debug.Log(i + "  " + j + "   " + k);


                    }
                }
            }
        }
        //SpawnReal();
        
    }

    public void SpawnReal()
    {
        //foreach kigdom
        for (int i = 0; i < allegiances.instance.Lists.Count - 1; i++)
        {
            //ppl in factions 1
            for (int j = 0; j < allegiances.instance.People[i + 1].People.Count; j++)
            {
                //Debug.Log(i + 1 + " " + j);
                allegiances.instance.Lists[i + 1].Castles[0].AddToCastle(allegiances.instance.People[i + 1].People[j], false);
            }
        }
    }
    public void Update()
    {
        if (WaitFrame == true)
        {
            WaitFrame = false;
            SpawnReal();
        }
    }
    public void DrawFamilyTree()
    {
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            for (int j = 0; j < allegiances.instance.People[i].People.Count; j++)
            {
                for (int k = 0; k < allegiances.instance.People[i].People.Count; k++)
                {
                    //Debug.Log(i + "" + j + "" + k);
                    Person Person1 = allegiances.instance.People[i].People[j];
                    Person Person2 = allegiances.instance.People[i].People[k];
                    if (Person1.Relations[Person2.PositionInList] != "")
                    {
                        if (Person1.Relations[Person2.PositionInList] == "Father")
                        {
                            //Debug.Log("Drawline6");
                            int Start = Person1.OverallPositionInList;
                            Panels[Start].GetComponent<PanelScript>().DrawLine(Panels[Person2.OverallPositionInList].transform, 2);
                        }
                        else if (Person1.Relations[Person2.PositionInList] == "Daught")
                        {
                            //Debug.Log("Drawline6");
                            int Start = Person1.OverallPositionInList;
                            Panels[Start].GetComponent<PanelScript>().DrawLine(Panels[Person2.OverallPositionInList].transform, 2);
                        }
                        else if (Person1.Relations[Person2.PositionInList] == "Son")
                        {
                            //Debug.Log("Drawline6");
                            int Start = Person1.OverallPositionInList;
                            Panels[Start].GetComponent<PanelScript>().DrawLine(Panels[Person2.OverallPositionInList].transform, 2);
                        }
                        else if (Person1.Relations[Person2.PositionInList] == "Sibling")
                        {
                            //Debug.Log("Drawline6");
                            int Start = Person1.OverallPositionInList;
                            Panels[Start].GetComponent<PanelScript>().DrawLine(Panels[Person2.OverallPositionInList].transform, 0);
                        }
                    }


                }
            }
        }
    }
}