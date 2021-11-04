using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerScript : MonoBehaviour
{
    #region Singleton

    public static BannerScript instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Text text;

    public void SetBanner(string Words)
    {
        text.text = Words;
    }
}
