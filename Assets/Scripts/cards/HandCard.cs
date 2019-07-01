﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard : Card {

    private float outDistance = 1;
    private Vector3 selectScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 dragScale = Vector3.one;

    public bool executeLogic;

    void OnMouseEnter()
    {
        //Debug.Log(" ----------- Be Selected ---------- ");
        oriPos = transform.position;
        oriRot = transform.eulerAngles;
        oriScale = transform.localScale;

        beSelect = true;
        oriScale = transform.localScale;
        transform.localScale = selectScale;
    }

    void OnMouseExit()
    {
        //Debug.Log(" ----------- OnPointerExit ---------- ");
        beSelect = false;
        transform.localScale = oriScale;
    }

    void OnMouseDrag()
    {
        transform.localScale = dragScale;
        Vector3 cardPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cardPos.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        transform.eulerAngles = Vector3.one;
    }

    void OnMouseUp()
    {
        curPos = transform.position;
        float offset = Vector3.Distance(transform.position, oriPos);
        if (offset < outDistance)
        {
            ResetCard();
        }
        else
        {
            if (Round.instance.curRound == Round.RoundState.Own && canOut)
            {
                Notification.instance.TriggerNotification((int)GameData.GameEvents.TakeOut);
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
        //id = card.ID;
        //mainPic.material.mainTexture = Resources.Load<Texture>(card.mainSprite);
        cardPro = (GameData.CARD_PROFESSION)card.profession;
        cardLvl = (GameData.CARD_LEVEL)card.color;
        race = (GameData.CARD_RACE)card.race;
        cname = card.name;
        desc = card.desc;
        crystal = card.crystalCost;
        //attack = card.attack;
        //defence = card.defense;
    }

    public void CheckCrystal()
    {        
        int curCrystal = Round.instance.crystalGroup[(int)Round.RoundState.Own];
        useGreen.SetActive(curCrystal >= crystal);
        canOut = curCrystal >= crystal;
    }
}
