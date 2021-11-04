using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelScript : MonoBehaviour
{
    public Material[] mat;
    public LineRenderer lr;
    //private TextMeshProUGUI text;
    public void DrawLine(Transform End, int ColorSet)
    {
        float duration = 0.05f;
        //gameObject.AddComponent<LineRenderer>();

        //LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        if (ColorSet == 0)
        {
            lr.SetColors(Color.red, Color.red);
        }
        else if (ColorSet == 1)
        {
            lr.SetColors(Color.green, Color.green);
        }
        else if (ColorSet == 2)
        {
            lr.SetColors(Color.black, Color.black);
        }

        lr.SetWidth(0.3f, 0.3f);
        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -5f));
        lr.SetPosition(1,  new Vector3(End.position.x, End.position.y, -5f));
        lr.sharedMaterials = mat;
        //lr.sharedMaterials.color = Color.white;
    }
    /*
    public void SetText(string Text)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = Text;
    }
    */
}
