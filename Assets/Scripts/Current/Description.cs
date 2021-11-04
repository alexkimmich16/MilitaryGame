using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Description
{
    public string Name;
    public List<string> TextChunks = new List<string>();
    public List<int> sequence = new List<int>();
    public List<DoubleString> Blanks = new List<DoubleString>();

}
