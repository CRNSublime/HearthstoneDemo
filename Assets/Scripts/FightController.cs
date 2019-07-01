using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FightController : Singleton<FightController> {

	public void Attack(OutCard attacker, OutCard victim)
    {
        Vector3 oriPos = attacker.transform.localPosition;
        Tween tweenGo = attacker.transform.DOMove(victim.transform.localPosition, 0.5f, false);
        tweenGo.OnComplete(() => {
            attacker.DamageControl(true, victim.attack);
            victim.DamageControl(true, attacker.attack);
            attacker.transform.DOMove(oriPos, 0.5f).OnComplete(() =>
            {
                attacker.DamageControl(false);
                victim.DamageControl(false);
                attacker.cardRig.useGravity = true;
                attacker.cardRig.isKinematic = false;
            });
        });
    }

    public void Attack(OutCard attacker, Hero hero)
    {
        attacker.defence -= hero.attack;
        hero.hp -= attacker.attack;
    }
}
