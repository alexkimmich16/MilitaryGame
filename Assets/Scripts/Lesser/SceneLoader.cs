using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton

    public static SceneLoader instance;

    void Awake()
    {
        instance = this;
        Strings.Add("UpgradeMenu");
        Strings.Add("StartMenu");
        Strings.Add("GameScene1");
        Strings.Add("GameScene2");
        Strings.Add("GameScene3");
        Strings.Add("GameScene4");
        Strings.Add("GameScene5");
        Strings.Add("GameScene6");
        Strings.Add("GameScene7");
        Strings.Add("GameScene8");
        Strings.Add("GameScene9");
        Strings.Add("GameScene10");
        Strings.Add("GameScene11");
        Strings.Add("GameScene12");
        Strings.Add("GameScene13");
        Strings.Add("GameScene14");
        Strings.Add("GameScene15");
        Strings.Add("GameScene16");
        Strings.Add("GameScene17");
        Strings.Add("GameScene18");
        Strings.Add("GameScene19");
        Strings.Add("GameScene20");
        Strings.Add("GameScene21");
        Strings.Add("GameScene22");
        Strings.Add("GameScene23");
        Strings.Add("GameScene24");
        Strings.Add("GameScene25");
        Strings.Add("GameScene26");
        Strings.Add("GameScene27");
        Strings.Add("GameScene28");
        Strings.Add("GameScene29");
    }

    #endregion

    public static List<string> Strings = new List<string>();
    public static string FirstScene = "SampleScene";
    
    public void LoadScene(int Num)
    {
        SceneManager.LoadScene(FirstScene);
    }
    
}
