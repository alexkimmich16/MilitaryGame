using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    #region Singleton

    public static CameraSwitcher instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public bool IsMain = true;
    public Camera Main;
    public Camera World;
    // Start is called before the first frame update
    void Start()
    {
        Main.enabled = true;
        World.enabled = false;
    }
    public void SetCamera(bool MainTrue)
    {
        Main.enabled = MainTrue;
        World.enabled = !MainTrue;
        IsMain = MainTrue;
    }
    // Update is called once per frame
    void Update()
    {
        Main.enabled = IsMain;
        World.enabled = !IsMain;
    }
}
