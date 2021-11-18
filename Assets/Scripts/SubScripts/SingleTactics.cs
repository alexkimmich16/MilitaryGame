using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[System.Serializable]
public class SingleTactics
{
    public int Num;
    public ListOfEnemies MyStats;
    private bool StartSave = true;

    public int TileAmountDifference;

    public List<int> Sent = new List<int>();

    private float Timer;

    private int RatioInCastle = 50;
    //replaceTile existance
    /// <summary>
    /// update at time of leaders skills and wit and whatever
    /// </summary>
    /// 
    public void PlayerReaction(bool Accepted)
    {
        int Reaction = Random.Range(0, 3);
        if (Reaction == 0)
        {
            //angered
            EventManager.instance.PlayerAllianceReactionMessage(Reaction, Num, Accepted);
        }
        else if(Reaction == 1)
        {
            //neutral
            EventManager.instance.PlayerAllianceReactionMessage(Reaction, Num, Accepted);
        }
        else
        {
            EventManager.instance.PlayerAllianceReactionMessage(Reaction, Num, Accepted);
        }
    }

    public void FinishedPeaceTalk(int Other, WalkingPerson person)
    {
        //use actual stats for this
        int OutCome = Random.Range(0, 3);
        if (OutCome == 0)
        {
            allegiances.instance.StartAlliance(Other, Num);
        }
        else if (OutCome == 1)
        {
            //decline
            //send back
        }
        else if (OutCome == 2)
        {
            person.Destory();
        }
        string Name = "";
        if (person.currentPerson.Faction == 0)
        {
            Name = "<b>" + "House " + person.currentPerson.FullName + "</b>";
        }
        else if (person.currentPerson.Faction == 1)
        {
            Name = "<color=green>" + "House " + person.currentPerson.FullName + "</color>";
        }
        else if (person.currentPerson.Faction == 2)
        {
            Name = "<color=blue>" + "House " + person.currentPerson.FullName + "</color>";
        }
        else if (person.currentPerson.Faction == 3)
        {
            Name = "<color=red>" + "House " + person.currentPerson.FullName + "</color>";
        }
        else if (person.currentPerson.Faction == 4)
        {
            Name = "<color=grey>" + "House " + person.currentPerson.FullName + "</color>";
        }
        else if (person.currentPerson.Faction == 5)
        {
            Name = "<color=cyan>" + "House " + person.currentPerson.FullName + "</color>";
        }
        EventManager.instance.CreateArrivalOutcome(Other, Num, OutCome, Name);
    }

    public void SecondPartyDecides(int InvitedBy)
    {
        int Accept = Random.Range(0, 3);
        //Debug.Log(Accept);
        if(Accept == 0 || Accept == 1)
        {
            //send someone over
            //FIND NEAREST CASTLE
            float Closest = 0;

            int MyNum = 0;
            int TheyNum = 0;
            for (int i = 0; i < allegiances.instance.Lists[InvitedBy].Castles.Count; i++)
            {
                for (int j = 0; j < allegiances.instance.Lists[Num].Castles.Count; j++)
                {
                    int X1 = allegiances.instance.Lists[InvitedBy].Castles[i].MyPos.x;
                    int Y1 = allegiances.instance.Lists[InvitedBy].Castles[i].MyPos.y;
                    int X2 = allegiances.instance.Lists[Num].Castles[i].MyPos.x;
                    int Y2 = allegiances.instance.Lists[Num].Castles[j].MyPos.y;

                    Vector2 Thiers = HexGenerator.instance.GetPosition(X1, Y1);
                    Vector2 Mine = HexGenerator.instance.GetPosition(X2, Y2);
                    float dist = Vector2.Distance(Thiers, Mine);
                    if (dist < Closest)
                    {
                        Closest = dist;
                        MyNum = j;
                        TheyNum = i;
                    }
                }
                
            }
            int Highest = 0;
            int InList = 0;
            for (int i = 0; i < allegiances.instance.People[Num].People.Count; i++)
            {
                if (allegiances.instance.People[Num].People[i].Diplomacy > Highest)
                {
                    allegiances.instance.People[Num].People[i].Diplomacy = Highest;
                    InList = i;
                }
            }
            int2 Spawn = allegiances.instance.Lists[InvitedBy].Castles[TheyNum].FindNeighbour();
            allegiances.instance.Lists[Num].Castles[MyNum].RemoveFromCastleWithDestination(allegiances.instance.People[Num].People[InList], Spawn.x, Spawn.y, InvitedBy);
            TacticsManager.instance.TacticsControllers[InvitedBy].ReactToDecision(true, Num);
            
        } 
        else if(Accept == 2)
        {
            TacticsManager.instance.TacticsControllers[InvitedBy].ReactToDecision(false, Num);
        }
    }
    //73 index out of range
    public void ReactToDecision(bool Accpeted, int Invited)
    {
        /*
        if(Accpeted == true)
        {
            //check if can get there
            int2 start = allegiances.instance.Lists[Num].Castles[0].FindNeighbour();
            int2 end = allegiances.instance.Lists[Invited].Castles[0].FindNeighbour();
            
            //Invited
            bool BasicPathExists = HexGenerator.instance.PM.PathExists(start, end, false, Num);
            bool FactionPath = HexGenerator.instance.PM.PathExists(start, end, true, Num);
            int AddedFaction = 0;
            if (FactionPath == false)
            {
                AddedFaction = HexGenerator.instance.PM.AlteredPathExists(start, end, Num);
            }
            else
            {
                AddedFaction = 7;
            }
            EventManager.instance.CreateAllianceInviteMessage(Num, Invited, Accpeted, true, BasicPathExists, AddedFaction);
            allegiances.instance.StartTempAlliance(Invited, Num);
        }
        else if (Accpeted == false)
        {
            int Angered = Random.Range(0, 2);
            if (Angered == 0)
            {
                //war
                //firse asked second
                EventManager.instance.CreateAllianceInviteMessage(Num, Invited, Accpeted, true, false, 0);
                allegiances.instance.StartWar(Invited, Num);
            }
            else
            {
                EventManager.instance.CreateAllianceInviteMessage(Num, Invited, Accpeted, false, false, 0);
            }
        }
        */
    }

    public void SpawnInfantry()
    {
        int limit = 100;
        /*
        if (allegiances.instance.Lists[Num].Castles[0].infantry > limit)
        {
            //Timer = 0
            int2 Spawn = allegiances.instance.Lists[Num].Castles[0].FindNeighbour();
            if (Spawn.x == 0 && Spawn.y == 0)
            {

            }
            else
            {
                PeopleSpawner.instance.SpawnInfantry(Spawn.x, Spawn.y, Num, limit);
                allegiances.instance.Lists[Num].Castles[0].infantry -= limit;
            }
            //allegiances.instance.Lists[Num].Castles[0].infantry -= 10;
            //infantry eat twice as much when not in castle
            //for (int i = 0; i < allegiances.instance.Lists.Count; i++) { }
        }
        */
        //for each tile connected to an enemy
    }

    public void ManageInfantry()
    {
        /*
        if (allegiances.instance.Lists[Num].LandBoarder.Count > 0 && allegiances.instance.Lists[Num].InfantryTransforms.Count > 0)
        {
            for (int i = 0; i < allegiances.instance.Lists[Num].InfantryTransforms.Count; i++)
            {
                Unit unit = allegiances.instance.Lists[Num].InfantryTransforms[i].GetComponent<Unit>();
                Infantry infantry = allegiances.instance.Lists[Num].InfantryTransforms[i].GetComponent<Infantry>();
                GameObject tile = HexGenerator.instance.TileTwoArray[unit.GridX, unit.GridY];
                if (allegiances.instance.Lists[Num].LandBoarder.Contains(tile.transform) || infantry.MovingToBoarder == true)
                {
                    //stay
                }
                else
                {
                    //find nearest tile to defened and defend it

                    int Closest = 0;
                    float MaxDistance = 0f;


                    for (int j = 0; j < allegiances.instance.Lists[Num].LandBoarder.Count; j++)
                    {
                        float Distance = Vector2.Distance(allegiances.instance.Lists[Num].LandBoarder[j].transform.position, allegiances.instance.Lists[Num].InfantryTransforms[i].position);

                        if (Distance < MaxDistance && allegiances.instance.Lists[Num].LandBoarder[j].transform.GetComponent<ClickableTile>().Manned == false)
                        {
                            Closest = j;
                            MaxDistance = Distance;
                        }
                    }
                    Transform NearestTile = allegiances.instance.Lists[Num].LandBoarder[Closest].transform;
                    ClickableTile FoundTile = NearestTile.GetComponent<ClickableTile>();
                    if (FoundTile.Reachable == true && unit.CurrentPath.Count == 0)
                    {
                        bool exists = HexGenerator.instance.PM.PathExists(new int2(unit.GridX, unit.GridY), new int2(FoundTile.tileX, FoundTile.tileY), false, Num);
                        //Debug.Log("exists:  " + exists);
                        if (exists == true)
                        {
                            //Debug.Log("Tile:  " + FoundTile.gameObject.name + "  Unit: " + unit.gameObject.name);
                            HexGenerator.instance.PM.FindPath(new int2(unit.GridX, unit.GridY), new int2(FoundTile.tileX, FoundTile.tileY), allegiances.instance.Lists[Num].InfantryTransforms[i].gameObject,
                            Num, Num, allegiances.instance.Lists[Num].Castles[0], false);
                            infantry.MovingToBoarder = true;
                            FoundTile.GetComponent<ClickableTile>().Manned = true;
                        }
                        else
                        {
                            FoundTile.Reachable = false;
                        }
                    }


                }
            }
        }
        */
        
    }

    public void Update()
    {
        if (StartSave == true)
        {
            MyStats = allegiances.instance.Lists[Num];
            StartSave = false;
            TileAmountDifference = TacticsManager.TileAmountDifference;
        }
        if (StartOrganiser.SelectedCastle == false)
        {
            return;
        }
        
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            if(i != Num)
            {

            }
        }
        SetNumbers();
        if (Num != 0 && StartSave == false)
        {
            SpawnInfantry();
            ManageInfantry();
        }
        

        //for each active infantry
        for (int i = 0; i < allegiances.instance.Lists[Num].InfantryTransforms.Count; i++)
        {
            //if not doing anything, return to castle or send it to an unmanned tile


            //towers act as an outpost
            //
        }
        //Debug.Log("pt4");
        //more likly to be partners if they have more connecting land
        if (StartOrganiser.SelectedCastle == true)
        {
            //Debug.Log("PT4.1");
            for (int i = 0; i < allegiances.instance.Lists.Count; i++)
            {
               // Debug.Log("PT4.2  I: " + i + "  Num: " + Num);
                if (i != Num && Num != 0)
                {
                    //Debug.Log("PT4.3");
                    if (MyStats.State[i] == 0 && Sent[i] == 0)
                    {
                        //think about if they might accept
                        Sent[i] = 1;
                        int PowerDif = allegiances.instance.Lists[i].Power - allegiances.instance.Lists[Num].Power;
                        //Debug.Log(PowerDif+ "difference");
                        if (PowerDif < TileAmountDifference && PowerDif > -TileAmountDifference)
                        {
                            if (i == 0)
                            {
                                //for person
                                EventManager.instance.InvitePlayerAllianceMessage(Num);
                                //TacticsManager.instance.AskPlayerForAlliance(Num);
                            }
                            else
                            {
                                TacticsManager.instance.TacticsControllers[i].SecondPartyDecides(Num);
                            }
                        }
                    }
                }
            }
        }
        //Debug.Log("pt5");
    }

    void SetNumbers()
    {
        int Alliences = 0;
        int Enemies = 0;
        int Neutral = 0;
        int powerlevel = 0;
        for (int i = 0; i < allegiances.instance.Lists.Count; i++)
        {
            if (i != Num)
            {
                if (MyStats.State[i] == -1)
                {
                    Enemies += 1;
                }
                else if(MyStats.State[i] == 0)
                {
                    Neutral += 1;
                }
                else if (MyStats.State[i] == 1)
                {
                    Alliences += 1;
                }
            }
            
        }
        MyStats.Alliences = Alliences;
        MyStats.Enemies = Enemies;
        MyStats.Neutral = Neutral;
        
        powerlevel = allegiances.instance.Lists[Num].Land + allegiances.instance.Lists[Num].Money;
        allegiances.instance.Lists[Num].Power = powerlevel;
    }
}
