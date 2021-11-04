using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnGenerator : MonoBehaviour
{
    #region Singleton

    public static TurnGenerator instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }
    public static event EventHandler<OnTickEventArgs> OnTick;
   
    public BarracksManager barracks;

    private float TicTimerMax;
    private int tick;
    private float TicTimer;
    public float FPS;

    public static event EventHandler<OnTickEventArgs> OnTick2;
    private float TicTimerMax2;
    private int tick2;
    private float TicTimer2;
    public float FPS2;

    private void Start()
    {
        tick = 0;
        TicTimerMax = 1 / FPS;

        tick2 = 0;
        TicTimerMax2 = 1 / FPS2;
    }

    void Update()
    {
        TicTimer += Time.deltaTime;
        if (TicTimer >= TicTimerMax)
        {
            TicTimer -= TicTimerMax;
            tick += 1;
            if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
        }
        TicTimer2 += Time.deltaTime;
        if (TicTimer2 >= TicTimerMax2)
        {
            TicTimer2 -= TicTimerMax2;
            tick2 += 1;
            if (OnTick2 != null) OnTick2(this, new OnTickEventArgs { tick = tick });
        }
    }
}
