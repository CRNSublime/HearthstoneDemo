using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCard : Retinue {

    int damageValue = 2;

    private void Damage()
    {
        Vector3 cardPos = Camera.main.WorldToScreenPoint(curPos);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cardPos.z;
        Vector3 mousPosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        UIController.instance.InitArrowSign(GameData.arrowObj, curPos, mousPosInWorld);
    }

    void Update()
    {
        CheckCrystal();
        if (!executeLogic)
            return;
        Damage();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit))
            {
                GameObject obj = rayhit.collider.gameObject;
                OutCard card = obj.GetComponent<OutCard>();
                Hero hero = obj.GetComponent<Hero>();
                if (card)
                {
                    executeLogic = false;
                    card.DamageControl(true, damageValue);
                }
                else if (hero)
                {

                }
                else
                {

                }
            }
        }
    }
}
