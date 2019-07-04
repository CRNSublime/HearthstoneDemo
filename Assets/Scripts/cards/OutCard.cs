using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class OutCard : Card {

    //嘲讽 战吼 亡语 冲锋 潜行 圣盾 持续光环 
    public List<GameData.CardMechanism> mechanisms = new List<GameData.CardMechanism>();

    GameObject tuantObj;
    bool getUp = false;
    TextMesh damageTxt;
    public Rigidbody cardRig;
    public bool invincible;
    void Awake()
    {
        Init();
    }

    void Init()
    {
        mainPic = transform.Find("mainPic").GetComponent<MeshRenderer>();
        atkTxt = transform.Find("attack").GetComponent<TextMesh>();
        defTxt = transform.Find("defense").GetComponent<TextMesh>();
        tuantObj = transform.Find("cf").gameObject;
        //useGreen = transform.Find(GameData.CardPrefabPath[(int)GameData.PathType.CanUse]).gameObject;
        cardRig = gameObject.GetComponent<Rigidbody>();
        //arrowObj = GameObject.Find("arrow");
        damageTxt = transform.Find("damageObj/damage").GetComponent<TextMesh>();
        DamageControl(false);
    }

    public void TauntEffect()
    {
        Debug.Log(" --=-=-=-=-  TAUNT!  -=-=-=-=-=-=-- ");
    }

    public override void InitCard(Card card)
    {
        base.InitCard(card);
        
    }

    void OnMouseEnter()
    {
        Debug.Log(" ----------- OutCard  Be Selected ---------- ");
    }

    void OnMouseExit()
    {
        Debug.Log(" ----------- OutCard OnPointerExit ---------- ");
        if(getUp)
        {

        }
    }

    void OnMouseDrag()
    {
        if (!getUp)
        {
            oriPos = transform.localPosition;
            transform.localPosition = oriPos + new Vector3(0, 0.2f, 0);
            ChangeRigBody(false);
            getUp = true;
        }
        //Cursor.visible = false;
        Vector3 cardPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cardPos.z;
        Vector3 mousPosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        UIController.instance.InitArrowSign(GameData.arrowObj, transform.localPosition, mousPosInWorld);
    }

    void OnMouseUp()
    {
        Debug.Log(" ======= OutCard Up ====== ");
        //Cursor.visible = true;
        GameData.arrowObj.SetActive(false);
        getUp = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if(Physics.Raycast(ray, out rayhit))
        {
            GameObject go = rayhit.collider.gameObject;
            if (go == gameObject)
            {
                ChangeRigBody(true);
                return;
            }
            OutCard card = go.GetComponent<OutCard>();
            Hero hero = go.GetComponent<Hero>();
            if (card)
            {
                FightController.instance.Attack(this, card);
            }
            else if(hero)
            {
                FightController.instance.Attack(this, hero);
            }
            else
            {
                ChangeRigBody(true);
            }
        }
    }

    public void ChangeRigBody(bool enabled)
    {
        cardRig.useGravity = enabled;
        cardRig.isKinematic = !enabled;
    }

    public void ChangeAttributes(int atk, int def)
    {
        attack = atk;
        defence = def;
    }

    public void DamageControl(bool isShow, int damage = 0)
    {
        if (invincible)
            return;
        GameObject damageObj = damageTxt.gameObject.transform.parent.gameObject;
        damageObj.SetActive(isShow);
        if(isShow)
        {
            string damageStr = "-" + damage.ToString();
            damageTxt.text = damageStr;
            defence -= damage;
            damageObj.transform.localScale = Vector3.zero;
            damageObj.transform.DOScale(Vector3.one * 0.12f, 0.4f).OnComplete(() =>
            {
                damageObj.SetActive(false);
            });
        }
    }
}
