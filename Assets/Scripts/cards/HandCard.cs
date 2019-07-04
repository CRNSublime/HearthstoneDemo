using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : Card {

    private float outDistance = 2.5f;
    private Vector3 selectScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 selectPos = new Vector3(0, 0.8f, 1.2f);
    private Vector3 dragScale = Vector3.one;

    public bool executeLogic;

    void OnMouseEnter()
    {
        //Debug.Log(" ----------- Be Selected ---------- ");
        oriPos = transform.position;
        oriRot = transform.eulerAngles;
        oriScale = transform.localScale;

        beSelect = true;
        transform.localScale = selectScale;
        transform.position += selectPos;
    }

    void OnMouseExit()
    {
        //Debug.Log(" ----------- OnPointerExit ---------- ");
        beSelect = false;
        if(!executeLogic)
            ResetCard();
    }

    void OnMouseDrag()
    {
        transform.localScale = dragScale;
        Vector3 cardPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cardPos.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        transform.eulerAngles = Vector3.one;
        CardManager.instance.AdjustOutCardPos(transform.position);
    }

    void OnMouseUp()
    {
        curPos = transform.position;
        float offset = Vector3.Distance(transform.position, oriPos);
        //Debug.LogFormat(" -=-=-=-=- HandCards OnMouseUp offest: {0}   canOut: {1}", offset, canOut);
        if (offset < outDistance)
        {
            ResetCard();
        }
        else
        {
            if (Round.instance.curRound == Round.RoundState.Own && canOut)
            {
                //CardManager.instance.AdjustOutCardPos(transform.position);
                Notification.instance.TriggerNotification((int)GameData.GameEvents.TakeOut);
                CardManager.instance.AdjustHandCardPos();
                executeLogic = true;
            }
            else
            {
                ResetCard();
            }
        }
    }

    public void ResetCard()
    {
        transform.position = oriPos;
        transform.eulerAngles = oriRot;
        transform.localScale = oriScale;
    }

    public override void InitCard(Cards card)
    {
        base.InitCard(card);
        cardPro = (GameData.CARD_PROFESSION)card.profession;
        cardLvl = (GameData.CARD_LEVEL)card.color;
        race = (GameData.CARD_RACE)card.race;
        cname = card.name;
        desc = card.desc;
        crystal = card.crystalCost;
    }

    public void CheckCrystal()
    {        
        int curCrystal = Round.instance.crystalGroup[(int)Round.RoundState.Own];
        useGreen.SetActive(curCrystal >= crystal);
        canOut = curCrystal >= crystal;
    }

    void FixedUpdate()
    {
        CheckCrystal();
    }

    public void DrawArrow()
    {
        Vector3 cardPos = Camera.main.WorldToScreenPoint(curPos);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cardPos.z;
        Vector3 mousPosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        UIController.instance.InitArrowSign(GameData.arrowObj, curPos, mousPosInWorld);
    }

    public GameObject RayReturn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit))
            {
                GameObject obj = rayhit.collider.gameObject;
                return obj;
            }
        }
        return null;
    }
}
