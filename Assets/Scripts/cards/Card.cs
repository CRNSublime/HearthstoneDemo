using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    private int m_id;
    public int id
    {
        get { return m_id; }
        set { m_id = value; }
    }

    private int m_crystal;
    public int crystal
    {
        get { return m_crystal; }
        set
        {
            m_crystal = value;
            crystalTxt.text = value.ToString();
        }
    }

    private int m_attack;
    public int attack
    {
        get { return m_attack; }
        set
        {
            m_attack = value;
            atkTxt.text = value.ToString();
        }
    }

    private int m_defence;
    public int defence
    {
        get { return m_defence; }
        set
        {
            m_defence = value;
            defTxt.text = value.ToString();
        }
    }

    private string m_name;
    public string cname
    {
        get { return m_name; }
        set
        {
            m_name = value;
            nameTxt.text = value;
        }
    }

    private string m_desc;
    public string desc
    {
        get { return m_desc; }
        set
        {
            m_desc = value;
            descTxt.text = value;
        }
    }

    private GameData.CARD_RACE m_race;
    public GameData.CARD_RACE race
    {
        get { return m_race; }
        set
        {
            m_race = value;
            raceTxt.text = GameData.RaceTextAry[(int)value];
        }
    }

    private GameData.CARD_PROFESSION m_cardPro;
    public GameData.CARD_PROFESSION cardPro
    {
        get { return m_cardPro; }
        set
        {
            m_cardPro = value;
            proffesion.material.mainTexture = InitCardProfeesion(GameData.ProffesionAry[(int)value]);
        }
    }

    private GameData.CARD_LEVEL m_cardLvl;
    public GameData.CARD_LEVEL cardLvl
    {
        get { return m_cardLvl; }
        set
        {
            m_cardLvl = value;
            colorPic.material.mainTextureOffset = GameData.QualityAry[(int)value];
        }
    }

    public Vector3 oriPos;
    public Vector3 curPos;
    public Vector3 oriRot;
    public Vector3 oriScale;

    //public string outCardPath = "Prefabs/normalRe"; // 战场随从模型
    //public Transform outCardParent; // 战场模型父节点
    //public GameObject arrowObj;

    public MeshRenderer mainPic;    // 卡牌图片
    public MeshRenderer colorPic;  // 卡牌质量
    public MeshRenderer proffesion; // 卡牌所属职业
    public TextMesh raceTxt;      // 随从所属种族
    public TextMesh atkTxt;       // 攻击力
    public TextMesh defTxt;      // 防御值
    public TextMesh crystalTxt;  // 消耗水晶
    public TextMesh nameTxt;   // 卡牌名称
    public TextMesh descTxt;  //  卡牌描述
    public GameObject useGreen; // 可使用状态 

    public bool isGold;
    public bool beSelect;
    public bool canOut;

    private Texture InitCardProfeesion(string path)
    {
        var pro = Resources.Load<Texture>(path);
        return pro;
    }

    public virtual void InitCard(Cards card)
    {
        id = card.ID;
        mainPic.material.mainTexture = Resources.Load<Texture>(card.mainSprite);
        attack = card.attack;
        defence = card.defense;
    }

    public virtual void InitCard(Card card)
    {
        id = card.id;
        mainPic.material.mainTexture = card.mainPic.material.mainTexture;
        attack = card.attack;
        defence = card.defence;
    }
}
