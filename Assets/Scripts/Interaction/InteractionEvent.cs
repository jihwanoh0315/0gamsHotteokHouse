using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;

    public Dialogue[] GetDialogue(int ID_)
    {
        dialogue.dialogues = DatabaseManager.Instance.GetDialogue(ID_, (int)dialogue.line.x, (int)dialogue.line.y);
        return dialogue.dialogues;
    }
    public Dialogue[] GetDialogueWithLines(int ID_, int start_, int end_)
    {
        dialogue.dialogues = DatabaseManager.Instance.GetDialogue(ID_, start_, end_);
        return dialogue.dialogues;
    }
}
