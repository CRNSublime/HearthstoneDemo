using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardManager<T> : Singleton<CardManager<T>> where T : Card {

    List<Cards> cardsConfig = new List<Cards>();
    public void InitHandCard(int cardID)
    {
        if (cardsConfig.Count == 0)
        {
            CardsConfig config = LoadAsset.instance.LoadAssets<CardsConfig>();
            cardsConfig = config.cardsList;
        }
        Cards curCard = new Cards();
        for (int i = 0; i < cardsConfig.Count; i++)
        {
            if (cardsConfig[i].ID == cardID)
            {
                curCard = cardsConfig[i];
                break;
            }
            else if(i == cardsConfig.Count - 1)
            {
                Debug.LogErrorFormat("Init Card Error!  CardId {0} is invalid!", cardID);
                return;
            }
        }

        HandCard card = PoolManager.instance.GetHandCards(curCard.ID);
        if (card)
        {
            card.InitCard(curCard);
        }
        else
        {
            GameObject rePrefab = Resources.Load(curCard.model) as GameObject;
            GameObject reObj = Instantiate(rePrefab, GameData.selfHandTrans);

            T cardCp = reObj.transform.GetComponent<T>();
            if (!cardCp)
            {
                cardCp = reObj.AddComponent<T>();
            }
            cardCp.InitCard(curCard);
        }
    }

    public void InitOutCard(Card card)
    {
        GameObject prefab = Resources.Load(GameData.outCardPath) as GameObject;
        GameObject obj = Instantiate(prefab, GameData.selfOutTrans);
        obj.transform.localPosition = card.transform.localPosition;
        T cardCp = obj.transform.GetComponent<T>();
        if (!cardCp)
        {
            cardCp = obj.AddComponent<T>();
        }
        cardCp.InitCard(card);
        PoolManager.instance.AddHandCardsPool(card);
    }
}
