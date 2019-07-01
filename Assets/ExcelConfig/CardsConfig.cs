using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardsConfig : ScriptableObject {
    public List<Cards> cardsList = new List<Cards>();
    public string assetName = "CardsAsset";
}
[Serializable]
public struct Cards
{
    public int ID;
    public string name;
    public string mainSprite;
    public int color;
    public string desc;
    public int crystalCost;
    public int attack;
    public int defense;
    public int profession;
    public int race;
    public string model;
}
