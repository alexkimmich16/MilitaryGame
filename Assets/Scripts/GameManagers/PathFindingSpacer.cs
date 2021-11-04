using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingSpacer : MonoBehaviour
{
    #region Singleton

    public static PathFindingSpacer instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public List<Skeleton> Skeletons = new List<Skeleton>();

    public int INum;
    public float Timer;
    public float AjustedTimer;
    public float AllTimer;

    // Start is called before the first frame update
    void Start()
    {
        
        INum = 0;
    }

    void SkeletonPathFind()
    {
        if (Skeletons.Count > 0)
        {
            if (INum > Skeletons.Count - 1)
            {
                INum = 0;
            }
            Skeletons[INum].FindPath();
            INum += 1;
        }
        
        
        //Debug.Log(INum);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > AjustedTimer)
        {
            SkeletonPathFind();
            Timer = 0;
        }
        AjustedTimer = AllTimer / Skeletons.Count;
    }
}
