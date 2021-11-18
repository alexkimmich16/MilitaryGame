using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    
    
    //public int width = 256;
    //public int Height = 256;

    //public float scale = 20f;

    private float OffsetX = 0f;
    private float OffsetY = 0f;

    //float XCoord = (float)X / width * scale + OffsetX;
    //float YCoord = (float)Y / Height * scale + OffsetY;

    public float GetValue(float X, float Y, float Scale)
    {
        float XCoord = (float)X / Scale + OffsetX;
        float YCoord = (float)Y / Scale + OffsetY;
        float height = Mathf.PerlinNoise(XCoord, YCoord);
        //HexGen.Value = height;
        return height;
    }

    public void Awake()
    {
        OffsetX = Random.Range(0f, 9999f);
        OffsetY = Random.Range(0f, 9999f);
    }
    /*
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }
    
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, Height);

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }
    
    Color CalculateColor(int x, int y)
    {
        float xCoord = (float) x / width * scale + OffsetX;
        float yCoord = (float)y / Height * scale + OffsetY;
        
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
    */
}
