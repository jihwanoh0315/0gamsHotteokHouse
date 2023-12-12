using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    [SerializeField] string[] csv_FileName;

    Dictionary<int, Dictionary<int, Dialogue>> dialogueDic = new ();

    public static bool isFinish = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();
            for(int i = 0; i < csv_FileName.Length; i++)
            {
                Dialogue[] dialogues = theParser.Parse(csv_FileName[i]);
                Dictionary<int, Dialogue> dialoguePage = new Dictionary<int, Dialogue>(); ;
                for (int j = 0; j < dialogues.Length; j++)
                {
                    dialoguePage.Add(j + 1, dialogues[j]);
                }
                dialogueDic.Add(i, dialoguePage);
            }
            isFinish = true;
        }
    }
    public Dialogue[] GetDialogue(int ID_, int startNum_, int endNum_)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for(int i = 0; i <= endNum_ - startNum_; i++)
        {
            dialogueList.Add(dialogueDic[ID_][startNum_ + i]);
        }

        return dialogueList.ToArray();
    }

    public Dialogue[] GetDialogueWithID(int startNum_, int endNum_, Dictionary<int, Dialogue> dialogueDic_)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = 0; i <= endNum_ - startNum_; i++)
        {
            dialogueList.Add(dialogueDic_[startNum_ + i]);
        }

        return dialogueList.ToArray();
    }
}
