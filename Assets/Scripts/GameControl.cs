using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    void Awake() { instance = this;}

    public bool SpeedWithoutCastle;

    public bool ColorTiles;

    public bool SpawnDots;
    public bool ShowBoarder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
