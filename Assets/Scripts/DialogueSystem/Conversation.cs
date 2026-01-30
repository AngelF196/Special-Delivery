using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation/New Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;

        [TextArea(3, 5)]
        public string dialogueText;
    }
    
    public DialogueLine[] dialogueLines;
}
