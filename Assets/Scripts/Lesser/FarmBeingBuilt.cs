using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBeingBuilt : MonoBehaviour
{
    public float Max;
    public float Current;
    private ClickableTile CT;
    public Castle castle;
    // Start is called before the first frame update
    void Start()
    {
        CT = gameObject.GetComponent<ClickableTile>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Current > Max)
        {
            CT.map.TM.ReplaceWithFarm(CT.tileX, CT.tileY, CT.Kingdom, castle);
        }
    }
}
