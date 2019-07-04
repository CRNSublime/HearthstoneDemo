using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardManager : Singleton<CardManager>
{

    List<Cards> cardsConfig = new List<Cards>();
    List<HandCard> selfHandCards = new List<HandCard>();
    List<OutCard> selfOutCards = new List<OutCard>();
    List<Vector3> selfOutCardsPos = new List<Vector3>();

    Vector3 midOutCardPos = new Vector3(-0.153f,0.003f,-0.023f);
    Vector3 moveInterval = new Vector3(0.4f, 0, 0);

    Vector3 leftPos = new Vector3(-1.7f, 0.3f, -2.8f);  // 最左端卡牌位置
    Vector3 rightPos = new Vector3(1.3f, 0.3f, -2.8f); // 最右端卡牌位置
    Vector3 rotate = new Vector3(0, -6, 0);            // 最左端卡牌旋转角度
    public void InitHandCard<T>(int cardID) where T : HandCard
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
            else if (i == cardsConfig.Count - 1)
            {
                Debug.LogErrorFormat("Init Card Error!  CardId {0} is invalid!", cardID);
                return;
            }
        }

        HandCard card = PoolManager.instance.GetHandCards(curCard.ID);
        if (card)
        {
            card.InitCard(curCard);
            selfHandCards.Add(card);
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
            selfHandCards.Add(cardCp);
        }
    }

    public void InitOutCard<T>(HandCard card) where T : OutCard
    {
        GameObject prefab = Resources.Load(GameData.outCardPath) as GameObject;
        GameObject obj = Instantiate(prefab, GameData.selfOutTrans);
        //Debug.Log("-=-=-=-=-=-=-=-  POS: " + CalOutCardPos(card));
        obj.transform.position = CalOutCardPos(card);
        T cardCp = obj.transform.GetComponent<T>();
        if (!cardCp)
        {
            cardCp = obj.AddComponent<T>();
        }
        cardCp.InitCard(card);
        PoolManager.instance.AddHandCardsPool(card);
        selfHandCards.Remove(card);
        RefreshOutCardsPos(true);
        selfOutCards.Add(cardCp);
        selfOutCardsPos.Add(obj.transform.position);
    }

    public void AdjustHandCardPos()
    {
        int count = selfHandCards.Count;
        float distance = Vector3.Distance(leftPos, rightPos);
        Vector3 transvec = new Vector3(distance / (count + 1), 0.02f, 0); // 间隔距离
        float rotateRange = Vector3.Distance(rotate, -rotate);
        Vector3 rotatevec = new Vector3(0, rotateRange / count, 0);  // 旋转角度
        for (int i = 0; i < count; i++)
        {
            selfHandCards[i].transform.position = leftPos + transvec * (i + 1);
            selfHandCards[i].transform.localEulerAngles = (rotate + (count >= 5 ? rotatevec : -rotate)) * (i + 1);
        }
    }

    private void RefreshOutCardsPos(bool isRefCard)
    {
        if (isRefCard)
        {
            for (int i = 0; i < selfOutCards.Count; i++)
            {
                selfOutCardsPos[i] = selfOutCards[i].transform.position;
            }
        }
        else
        {
            for (int i = 0; i < selfOutCards.Count; i++)
            {
                selfOutCards[i].transform.position = selfOutCardsPos[i];
            }
        }
    }

    public void AdjustOutCardPos(Vector3 newOutCardPos)
    {
        if (newOutCardPos.z < -0.6f || newOutCardPos.z > 0.4f)
        {
            RefreshOutCardsPos(false);
            return;
        }
        Vector3 move;
        for (int i = 0; i < selfOutCards.Count; i++)
        {
            float offsetx = newOutCardPos.x - selfOutCardsPos[i].x;
            if (offsetx > 0)
            {
                move = new Vector3(offsetx > moveInterval.x ? moveInterval.x : offsetx, 0, 0);
                selfOutCards[i].transform.position = selfOutCardsPos[i] - move;
            }
            else
            {
                move = new Vector3(-offsetx > moveInterval.x ? moveInterval.x : -offsetx, 0, 0);
                selfOutCards[i].transform.position = selfOutCardsPos[i] + move;
            }
            
        }
    }

    public Vector3 CalOutCardPos(HandCard card)
    {
        int count = selfOutCards.Count;
        if (count == 0)
            return midOutCardPos;
        else
        {
            Vector3 cardPos;
            int idx = 0;
            Vector3[] pos = InitRandomPos();
            for (int i = 0; i < pos.Length; i++)
            {
                if (card.transform.position.x < pos[i].x)
                    break;
                idx++;
            }
            if (idx == count - 1)
                cardPos = pos[0] - moveInterval;
            else if (idx == count)
                cardPos = pos[count-1] + moveInterval;
            else
                cardPos = (pos[idx - 1] + pos[idx]) / 2;
            Debug.LogFormat(" ======CalOutCardPos ....  idx: {0},  ... cardPos{1}: ", idx, cardPos);
            return cardPos;
        }
    }

    Vector3[] InitRandomPos()
    {
        int count = selfOutCards.Count;
        float[] posArray = new float[count];
        for (int i = 0; i < count; i++)
        {
            posArray[i] = selfOutCardsPos[i].x;
        }
        Array.Sort(posArray);
        Vector3[] posVector = new Vector3[count];
        for (int j = 0; j < count; j++)
        {
            for (int k = 0; k < count; k++)
            {
                if (selfOutCardsPos[k].x == posArray[j])
                {
                    posVector[j] = selfOutCardsPos[k];
                    Debug.LogFormat(" -=-=-=-=-=-=-=-= InitRandomPos:  {0}  ---------  {1}", j, posVector[j]);
                    break;
                }
            }
        }
        return posVector;
    }
}