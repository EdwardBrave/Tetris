using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MaterialGen : MonoBehaviour
{
    [ContextMenuItem("Red", "Red"),
     ContextMenuItem("Pink", "Pink"),
     ContextMenuItem("Purple", "Purple"),
     ContextMenuItem("DeepPurple", "DeepPurple"),
     ContextMenuItem("Indigo", "Indigo"),
     ContextMenuItem("Blue", "Blue"),
     ContextMenuItem("LightBlue", "LightBlue"),
     ContextMenuItem("Cyan", "Cyan"),
     ContextMenuItem("Teal", "Teal"),
     ContextMenuItem("Green", "Green"),
     ContextMenuItem("LightGreen", "LightGreen"),
     ContextMenuItem("Lime", "Lime"),
     ContextMenuItem("Yellow", "Yellow"),
     ContextMenuItem("Amber", "Amber"),
     ContextMenuItem("Orange", "Orange"),
     ContextMenuItem("DeepOrange", "DeepOrange"),
     ContextMenuItem("Brown", "Brown"),
     ContextMenuItem("White", "White"),
     ContextMenuItem("BlueGrey", "BlueGrey"),
     ContextMenuItem("Grey", "Grey"),
     ContextMenuItem("Black", "Black"),
     ContextMenuItem("Random", "RandomMaterialColor")]
    public Color32 color ;

    public bool usingRandom;
    
    public Pallet[] randomExclude;
    public Color32 givenColor
    {
        get => color;
        set
        {
            color = value;
            RefreshMaterial();
        }
    }

    void RefreshMaterial()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = givenColor;
        rend.sharedMaterial.SetFloat("_Metallic", 0.35F);
        rend.sharedMaterial.SetFloat("_Glossiness", 0.35F);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (usingRandom)
            RandomMaterialColor();
        RefreshMaterial();
    }

    public Color32 Red() => givenColor = Colors[(int)Pallet.Red];
    public Color32 Pink() => givenColor = Colors[(int)Pallet.Pink];
    public Color32 Purple() => givenColor = Colors[(int)Pallet.Purple];
    public Color32 DeepPurple() => givenColor = Colors[(int)Pallet.DeepPurple];
    public Color32 Indigo() => givenColor = Colors[(int)Pallet.Indigo];
    public Color32 Blue() => givenColor = Colors[(int)Pallet.Blue];
    public Color32 LightBlue() => givenColor = Colors[(int)Pallet.LightBlue];
    public Color32 Cyan() => givenColor = Colors[(int)Pallet.Cyan];
    public Color32 Teal() => givenColor = Colors[(int)Pallet.Teal];
    public Color32 Green() => givenColor = Colors[(int)Pallet.Green];
    public Color32 LightGreen() => givenColor = Colors[(int)Pallet.LightGreen];
    public Color32 Lime() => givenColor = Colors[(int)Pallet.Lime];
    public Color32 Yellow() => givenColor = Colors[(int)Pallet.Yellow];
    public Color32 Amber() => givenColor = Colors[(int)Pallet.Amber];
    public Color32 Orange() => givenColor = Colors[(int)Pallet.Orange];
    public Color32 DeepOrange() => givenColor = Colors[(int)Pallet.DeepOrange];
    public Color32 Brown() => givenColor = Colors[(int)Pallet.Brown];
    public Color32 White() => givenColor = Colors[(int)Pallet.White];
    public Color32 BlueGrey() => givenColor = Colors[(int)Pallet.BlueGrey];
    public Color32 Grey() => givenColor = Colors[(int)Pallet.Grey];
    public Color32 Black() => givenColor = Colors[(int)Pallet.Black];
    public Color32 RandomMaterialColor()
    {
        return givenColor = RandomColor(randomExclude);
    }
    
    public enum Pallet
    {
        Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen,
        Lime, Yellow, Amber, Orange, DeepOrange, Brown, White, BlueGrey, Grey, Black
    };

    public static readonly Color32[] Colors =
    {
        new Color32(198, 40, 40, 255),
        new Color32(173, 20, 87, 255),
        new Color32(106, 27, 154, 255),
        new Color32(69, 39, 160, 255),
        new Color32(40, 53, 147, 255),
        new Color32(21, 101, 192, 255),
        new Color32(2, 119, 189, 255),
        new Color32(0, 131, 143, 255),
        new Color32(0, 105, 92, 255),
        new Color32(46, 125, 50, 255),
        new Color32(85, 139, 47, 255),
        new Color32(158, 157, 36, 255),
        new Color32(249, 168, 37, 255),
        new Color32(255, 143, 0, 255),
        new Color32(239, 108, 0, 255),
        new Color32(216, 67, 21, 255),
        new Color32(78, 52, 46, 255),
        new Color32(245, 245, 245, 255),
        new Color32(55, 71, 79, 255),
        new Color32(66, 66, 66, 255),
        new Color32(27, 27, 27, 255),
    };
    
    
    private static Random _rand = new Random();
    public static Color32 RandomColor(Pallet[] exclude = null)
    {
        int value;
        do
        {
            value = _rand.Next(0,Colors.Length);
        } while (exclude != null && Array.IndexOf(exclude, (Pallet) value) != -1);
        
        return Colors[value];
    }
    
    
}
