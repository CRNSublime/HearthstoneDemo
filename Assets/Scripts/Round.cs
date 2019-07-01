using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Singleton<Round> {

    public enum RoundState
    {
        Own,
        Opposite,
    }

    public int roundNum;
    public int[] crystalGroup = new int[2];
    public RoundState curRound;

    public void RoundStart(RoundState rdState)
    {
        Debug.LogFormat("----------- Round Start -------- {0} Round ", rdState);
        curRound = rdState;
        crystalGroup[(int)curRound]++;
        roundNum++;
    }

    public void AddCrystal(int crystal)
    {
        Debug.LogFormat(" ---------->>>>>> ADD CRYSTAL : {0} <<<<<<<-------- ",crystal);
        crystalGroup[(int)curRound] = crystal;
    }

    public void DelCrystal(int crystal)
    {
        Debug.LogFormat(" ---------->>>>>> DEL CRYSTAL : {0} <<<<<<<-------- ", crystal);
        crystalGroup[(int)curRound] = crystal;
    }
}
