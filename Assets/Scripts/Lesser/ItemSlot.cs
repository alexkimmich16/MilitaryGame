using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image UIImage;
    public Button removeButton;
    public BarracksManager barracks;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddSprite(Sprite NewIcon)
    {
        UIImage.sprite = NewIcon;
    }
    public void RemoveObject()
    {
        UIImage.sprite = null;
        barracks.Quoe[num] = 0;
    }
}
