using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCard : Retinue {

    int damageValue = 2;

    void Update()
    {
        if (!executeLogic)
            return;
        DrawArrow();
        GameObject go = RayReturn();
        if(go)
        {
            if (go != gameObject)
            {
                OutCard card = go.GetComponent<OutCard>();
                Hero hero = go.GetComponent<Hero>();
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
