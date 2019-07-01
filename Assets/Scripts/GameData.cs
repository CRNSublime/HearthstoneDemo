using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {

    public static string[] CardPrefabPath =
    {
       "attributeGroup/pic",
       "attributeGroup/quality",
       "attributeGroup/proffesion",
       "attributeGroup/race",
       "attributeGroup/name",
       "attributeGroup/description",
       "attributeGroup/attack",
       "attributeGroup/defense",
       "attributeGroup/cost",
       "attributeGroup/bailight",
    };

    public enum PathType  // 卡牌路径
    {
        MainPic,
        Quality,
        Proffesion,
        Race,
        Name,
        Desc,
        Attack,
        Defense,
        Cost,
        CanUse,
    }

	public enum GameEvents  // 通知事件
    {
        TakeOut,
    }

    public enum GameCards  // 卡牌ID
    {
        CARD_LSXY = 1001,
        CARD_GUN = 1002,
        CARD_XYR = 1003,
        CARD_MTY = 1004,
        CARD_TSTG = 1005,
    }

    public enum CARD_PROFESSION  // 职业
    {
        Mage,             // 法师
        Warlock,          // 术士
        Paladin,          // 圣骑
        Priest,          // 牧师
        Shaman,          // 萨满
        Dryad,          // 德鲁伊
        Thief,          // 盗贼
        Warrior,          // 战士
        Hunter,          // 猎人
        Neutrality,          // 中立
    }

    public enum CARD_LEVEL   // 卡牌质量等级
    {
        White,    // 白卡
        Blue,     // 蓝卡
        Purple,   // 紫卡
        Orange,   // 橙卡
    }

    public enum CARD_RACE   // 种族
    {
        FishMan,    // 鱼人
        WildBeast,  // 野兽
        Totem,      // 图腾

    }
    
    public static string[] RaceTextAry =
    {
        "鱼人","野兽","图腾",
    };

    public static string[] ProffesionAry =
    {
        "MainPro/mfs","MainPro/mss","MainPro/msq","MainPro/mms","MainPro/msm",
        "MainPro/mdly","MainPro/mdz","MainPro/mzs","MainPro/mlr","MainPro/mzl",
    };

    public static Vector2[] QualityAry =
    {
        Vector2.zero,
        new Vector2(0.5f,0),
        new Vector2(0,0.5f),
        new Vector2(0.5f,0.5f),
    };

    static GameState curState;
    public enum GameState
    {
        GAMEING, // 游戏中
        ONLINE,  // 在线
    }

    public static void SetGameState(GameState state)
    {
        curState = state;
    }
    public static GameState GetGameState()
    {
        return curState;
    }

    public static Transform hcPoolTrans;
    public static Transform selfHandTrans;
    public static Transform selfOutTrans;
    public static string outCardPath;
    public static void Init()
    {
        hcPoolTrans = GameObject.Find("handCardsPool").transform;
        selfHandTrans = GameObject.Find("selfHandCards").transform;
        selfOutTrans = GameObject.Find("selfOutCards").transform;
        outCardPath = "Prefabs/normalRe";
    }
}
