using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    public int hp = 30;
    public int attack = 0;
    public virtual void ReleaseSkill()
    {
        
    }

    public virtual void BeInjured(int damage)
    {
        int leftHp = hp - damage;
        if(leftHp<=0)
        {
            Debug.Log("<<<<< ----- Dead ---- >>>>>>");
            PlayDeathAni();
        }
    }

    public virtual void PlayDeathAni()
    {

    }
}
