using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    [SerializeField] Sprite[] spr_emoteList;
    [SerializeField] public Dictionary<int, Sprite> emoteDic;

    // Start is called before the first frame update
    void Start()
    {
        emoteDic = new Dictionary<int, Sprite>();

        // Emote Timer
        emoteDic.Add(100 + 0, spr_emoteList[0]);
        emoteDic.Add(100 + 1, spr_emoteList[1]);
        emoteDic.Add(100 + 2, spr_emoteList[2]);
        emoteDic.Add(100 + 3, spr_emoteList[3]);
        emoteDic.Add(100 + 4, spr_emoteList[4]);
        emoteDic.Add(100 + 5, spr_emoteList[5]);


        // Emoji
        emoteDic.Add(1000 + 0, spr_emoteList[6]); // Smile
        emoteDic.Add(1000 + 1, spr_emoteList[7]); // Sad
        emoteDic.Add(1000 + 2, spr_emoteList[8]); // Angry
        emoteDic.Add(1000 + 3, spr_emoteList[9]); // Happy
        emoteDic.Add(1000 + 4, spr_emoteList[10]); // !!
        emoteDic.Add(1000 + 5, spr_emoteList[11]); // ...

        // MainFood
        emoteDic.Add(10000 + 0, spr_emoteList[12]); // Basic Hotteok
        emoteDic.Add(10000 + 1, spr_emoteList[13]); // GreenTea Hotteok
        emoteDic.Add(10000 + 2, spr_emoteList[14]); // SweetPotato Hotteok

        // SideFood
        emoteDic.Add(11000 + 0, spr_emoteList[15]); // sikye
        emoteDic.Add(11000 + 1, spr_emoteList[16]); // slush
        emoteDic.Add(11000 + 2, spr_emoteList[17]); // fishcake

        // Count
        emoteDic.Add(12000 + 0, spr_emoteList[18]); // x1
        emoteDic.Add(12000 + 1, spr_emoteList[19]);
        emoteDic.Add(12000 + 2, spr_emoteList[20]);
        emoteDic.Add(12000 + 3, spr_emoteList[21]);
        emoteDic.Add(12000 + 4, spr_emoteList[22]);
        emoteDic.Add(12000 + 5, spr_emoteList[23]); // x6
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
