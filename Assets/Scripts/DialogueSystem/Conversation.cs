using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation/New Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public enum PortraitSide
    {
        LEFT_SIDE, RIGHT_SIDE
    }

    public enum Expressions
    {
        Normal, Surprised, Confused
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public PortraitSide speakerSideToHighlight;
        public Expressions expression;

        [TextArea(2, 3)]
        public string dialogueText;
    }
    
    public Character leftSpeaker;
    public Character rightSpeaker;
    public DialogueLine[] dialogueLines;
}
