using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    DialogueManager theDM;
    [SerializeField] InteractionEvent eventForTest;
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!theDM.isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                theDM.ShowDialogue(eventForTest.GetDialogueWithLines(0, 1, 7));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                theDM.ShowDialogue(eventForTest.GetDialogueWithLines(1, 1, 6));
            }
        }
    }
}
