using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager> {

    List<HandCard> handCardsPool = new List<HandCard>();
    public void AddHandCardsPool(Card handCard)
    {
        handCard.gameObject.SetActive(false);
        handCardsPool.Add((HandCard)handCard);
        handCard.transform.SetParent(GameData.hcPoolTrans);
    }

    public HandCard GetHandCards(int cardId)
    {
        foreach (HandCard card in handCardsPool)
        {
            if (card.id == cardId)
            {
                card.gameObject.SetActive(true);
                handCardsPool.Remove(card);
                return card;
            }
        }
        return null;
    }

    public void RemoveHandCards(HandCard handCard)
    {
        handCardsPool.Remove(handCard);
    }
}
