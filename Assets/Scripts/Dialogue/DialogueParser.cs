using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string CSVFileName_)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        TextAsset csvData = Resources.Load<TextAsset>(Path.Combine("Dialogue", CSVFileName_));

        string[] data = csvData.text.Remove(csvData.text.Length -1, 1).Split(new char[] { '\n' }); // Split with Enter

        bool isLeft = false;
        int currPortrait = 0;

        Dialogue prevDialogue = new Dialogue();

        prevDialogue.name = "default";
        prevDialogue.isLeft = false;


        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});
            Dialogue dialogue = new Dialogue();

            dialogue.name = row[1];
            //Debug.Log(row[1]);

            //////////////////////////////////
            /// 2 ÃÊ»óÈ­ À§Ä¡
            if (row[3] == "0")
            {
                dialogue.isLeft = false;
                isLeft = false;
            }
            else if (row[3] == "1")
            {
                dialogue.isLeft = true;
                isLeft = true;
            }
            if (row[3] == "")
            {
                dialogue.isLeft = isLeft;
            }

            //////////////////////////////////
            /// 3 ´ë»ç ÃÊ»óÈ­ Ç¥Á¤
            /// 0. ÀÏ¹Ý
            ///  1. ½½ÇÄ
            ///  2. ¿ôÀ½
            ///  3.³î¶÷
            ///  4. È­³²
            if (row[4] == "")
            {
                dialogue.portrait = currPortrait;
            }
            else
            {
                dialogue.portrait = int.Parse(row[4]);
            }

            //////////////////////////////////
            /// 4 µô·¹ÀÌ
            if (row[5] == "")
            {
                dialogue.delay = 0.0f;
            }
            else
            {
                dialogue.delay = float.Parse(row[5]);
            }

            //////////////////////////////////
            /// 5 ¼Óµµ
            if (row[6] == "")
            {
                dialogue.speechSpeed = 0.02f;
            }
            else
            {
                dialogue.speechSpeed = float.Parse(row[6]);
            }

            //////////////////////////////////
            /// 6 Ä³¸¯ÅÍ ID
            /// ¿µ°¨ 100
            /// ¸Á¾ß 200
            /// ¿À¸¼À½ 300
            /// ÇÏ»Ú 400
            /// ºñ¹Ð¼Ò³à 500
            /// ½Ã¾È 600
            /// È÷À×µû 700
            /// »£Âî 800
            /// ±è·ç¾ß 900
            /// µ¶°íÇýÁö 1000
            /// º°ÂîÀÌ 1100
            /// ¶ó¸®¾ç 1200
            /// ºø½Ã 1300
            /// 
            /// È£¿ì 10000
            /// 

            if (row[7] == "")
            {
                dialogue.ID = 100;
            }
            else
            {
                dialogue.ID = int.Parse(row[7]);
            }

            List<string> contextList = new List<string>();
            //Debug.Log(row[2]);


            do
            {
                contextList.Add(row[2]);
                //Debug.Log(row[2]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                    ;
                }
                else
                {
                    break;
                }

            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

}
