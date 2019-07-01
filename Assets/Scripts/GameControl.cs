using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameControl : MonoBehaviour {

    // Use this for initialization
    public Transform cardCanvas;
    void Start()
    {
        Debug.Log(" --------------  GAME START --------- ");

        GameData.Init();

        Round.instance.RoundStart(Round.RoundState.Own);
        Round.instance.AddCrystal(10);

        Texture2D handMouse = Resources.Load<Texture2D>("Cursor/down");
        Cursor.SetCursor(handMouse, Vector3.zero, CursorMode.Auto);

        //CardManager<Retinue>.instance.InitHandCard((int)GameData.GameCards.CARD_LSXY);
        //CardManager<Retinue>.instance.InitHandCard((int)GameData.GameCards.CARD_TSTG);

        CardManager<GunCard>.instance.InitHandCard((int)GameData.GameCards.CARD_GUN);
    }
   
}
