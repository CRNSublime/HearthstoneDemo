using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Retinue : HandCard
{
    void Init()
    {
        mainPic = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.MainPic]).GetComponent<MeshRenderer>();
        colorPic = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Quality]).GetComponent<MeshRenderer>();
        proffesion = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Proffesion]).GetComponent<MeshRenderer>();
        raceTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Race]).GetComponent<TextMesh>();
        nameTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Name]).GetComponent<TextMesh>();
        descTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Desc]).GetComponent<TextMesh>();
        atkTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Attack]).GetComponent<TextMesh>();
        defTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Defense]).GetComponent<TextMesh>();
        crystalTxt = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.Cost]).GetComponent<TextMesh>();
        useGreen = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.CanUse]).gameObject;
        //outCardParent = GameObject.Find("selfOutCards").transform;
    }

    public void Register()
    {
        Notification.instance.RegisterNotification((int)GameData.GameEvents.TakeOut, TakeEffect);
    }

    public void TakeEffect()
    {
        if (beSelect)
        {
            Debug.Log(" ------ RETINUE TAKE EFFECT ------- ");
            CardManager<OutCard>.instance.InitOutCard(this);
            beSelect = false;
        }
    }

    void Awake()
    {
        Init();
        Register();
    }

}

