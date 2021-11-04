using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region Singleton

    public static EventManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public float Timer;
    public float ChangePercent;
    private string OnWar = "has declaired war on";

    public List<string> HouseNames = new List<string>();

    public void PlayerAllianceReactionMessage(int Reaction, int House, bool Accepted)
    {
        string FinalText = "";
        

        if(Accepted == false)
        {
            if (Reaction == 0)
            {
                //angered
                FinalText = HouseNames[House] + " feels insulted by declining the invitation, and declares war";
            }
            else if (Reaction == 1)
            {
                //neutral
                FinalText = HouseNames[House] + " is saddened by the denial, but takes no further action";
            }
            else
            {
                //diplomatic
                FinalText = HouseNames[House] + " is saddened by the denial, but hopes to one day work towards being close allies";
            }
        }
        else
        {
            FinalText = HouseNames[House] + " is glad that you are willing to discus the possiblility";
        }
        

        
        TypeOfBanner banner = new TypeOfBanner();
        banner.IsYesNo = false;
        banner.Text = FinalText;
        UIManager.instance.BannerBuffer.Add(banner);
    }

    public void CreateArrivalOutcome(int FirstHouse, int SecondHouse, int OutCome, string personText)
    {
        if (OutCome == 0)
        {
            //UpgradeScript.Vehicles.Add(new vehicleClass());
            string FinalText = HouseNames[FirstHouse] + " successfully persuaded " + HouseNames[SecondHouse] + " to join a formal alliance.";
            TypeOfBanner banner = new TypeOfBanner();
            banner.IsYesNo = false;
            banner.Text = FinalText;
            UIManager.instance.BannerBuffer.Add(banner);
        }
        else if (OutCome == 1)
        {
            string FinalText = HouseNames[FirstHouse] + " failed to persuade " + HouseNames[SecondHouse] + " to join a formal alliance.";
            TypeOfBanner banner = new TypeOfBanner();
            banner.IsYesNo = false;
            banner.Text = FinalText;
            UIManager.instance.BannerBuffer.Add(banner);
        }
        else if (OutCome == 2)
        {
            string FinalText = "It was a rouse! " + personText + " was murderered by " + HouseNames[SecondHouse] + " in their sleep!";
            TypeOfBanner banner = new TypeOfBanner();
            banner.IsYesNo = false;
            banner.Text = FinalText;
            UIManager.instance.BannerBuffer.Add(banner);
        }
    }
    // Update is called once per frame
    public void CreateHouseTexts()
    {
        HouseNames[0] = "<b>" + "House " + allegiances.instance.Lists[0].DominentHouseName + "</b>";
        HouseNames[1] = "<color=green>" + "House " + allegiances.instance.Lists[1].DominentHouseName + "</color>";
        HouseNames[2] = "<color=blue>" + "House " + allegiances.instance.Lists[2].DominentHouseName + "</color>";
        HouseNames[3] = "<color=red>" + "House " + allegiances.instance.Lists[3].DominentHouseName + "</color>";
        HouseNames[4] = "<color=grey>" + "House " + allegiances.instance.Lists[4].DominentHouseName + "</color>";
        HouseNames[5] = "<color=cyan>" + "House " + allegiances.instance.Lists[5].DominentHouseName + "</color>";
    }
    //, int OtherKingdom
    public void CreateAllianceInviteMessage(int FirstHouse, int SecondHouse, bool Accepted, bool Angered, bool CanGetThere, int HasToAsk)
    {
        CreateHouseTexts();
        string FinalText = "";
        
        if (Accepted == true)
        {
            if (CanGetThere == true)
            {
                if(HasToAsk == 7)
                {
                    //can get there without any other factions help
                    FinalText = HouseNames[FirstHouse] + " has invited " + HouseNames[SecondHouse] + " to discuss a potential alliance." + HouseNames[SecondHouse] + " accepted and is beginning the trip.";
                }
                else
                {
                    //can get there only with another factions help
                    FinalText = HouseNames[FirstHouse] + " has invited " + HouseNames[SecondHouse] + " to discuss a potential alliance." + HouseNames[SecondHouse] + " accepted " +
                        "but cannot travel without permission from " + HouseNames[HasToAsk];
                }

            }
            else
            {
                FinalText = HouseNames[FirstHouse] + " has invited " + HouseNames[SecondHouse] + " to discuss a potential alliance." + HouseNames[SecondHouse] + " has no way to get there so they cannot accept.";
            }
            
        }
        else
        {
            if(Angered == true)
            {
                FinalText = HouseNames[FirstHouse] + " has invited " + HouseNames[SecondHouse] + " to discuss a potential alliance. " + HouseNames[SecondHouse] + " declined. " + HouseNames[FirstHouse] + " was angered by this and declaired war.";
            }
            else
            {
                FinalText = HouseNames[FirstHouse] + " has invited " + HouseNames[SecondHouse] + " to discuss a potential alliance. " + HouseNames[SecondHouse] + " declined.";
            }
        }
        TypeOfBanner banner = new TypeOfBanner();
        banner.IsYesNo = false;
        banner.Text = FinalText;
        
        UIManager.instance.BannerBuffer.Add(banner);
    }

    public void InvitePlayerAllianceMessage(int InvitedHouse)
    {
        CreateHouseTexts();
        string FinalText = "";
        FinalText = HouseNames[InvitedHouse] + " has invited you" + " to discuss a potential alliance. " + "Do you accept?";
        //yes no
        TypeOfBanner banner = new TypeOfBanner();
        banner.IsYesNo = true;
        banner.Text = FinalText;
        banner.Faction = InvitedHouse;
        UIManager.instance.BannerBuffer.Add(banner);
    }

    void Update()
    {
        Timer += Time.deltaTime * SpeedScript.instance.CurrentSpeed;
        if (Timer > 3)
        {
            Timer = 0;
            float Num = Random.Range(0.0f, 100.0f);
            if (Num < ChangePercent)
            {
                
                int Num2 = Random.Range(0, 1);
                if (Num2 == 0)
                {
                    //RandomWar();
                }
                else if (Num2 == 1)
                {
                    //RandomAllience();
                }
            }
        }
    }

    public void RandomWar()
    {
        int Army1 = Random.Range(0, 6);
        int Army2 = Random.Range(0, 6);

        SpeedScript.instance.CurrentSpeed = 0;

        ChangePercent = ChangePercent * .9f;
        allegiances.instance.StartWar(Army1, Army2);
    }
}
