using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation/New Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public enum PortraitSide
    {
        LEFT_SIDE, RIGHT_SIDE
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public PortraitSide portraitSideToHighlight;
        public string speakerName;

        [TextArea(2, 3)]
        public string dialogueText;
    }
    
    public DialogueLine[] dialogueLines;
}
