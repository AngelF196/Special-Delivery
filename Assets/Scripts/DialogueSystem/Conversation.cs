using System.Collections;
using System.Collections.Generic;
using MessagePack;
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
    [Tooltip("WARNING: Do not mess with this value, I will figure out a way to make this field non-interactive.")]
    public bool alreadyPlayed;

    [Tooltip("If checked, a quest reference will be activated at the end of this conversation.")]
    [Space]
    [Header("Quest Activation")]
    public bool activateQuest = false;
    public Quest questToActivate;
    
    [Space]
    [Header("Followup Conversation")]
    public bool addAnotherConversation;
    public Conversation followupConversation;

    public override string ToString()
    {
        return "Characters from left to right: " + leftSpeaker + " & " + rightSpeaker +
        " | alreadyPlayed: " + alreadyPlayed + " | addAnotherConversation: " + addAnotherConversation + " | dialogueLines.Length: " + dialogueLines.Length;
    }
}

[MessagePackObject]
public class SerializableConversation
{
    [Key(0)] public bool convoPlayedFlag;

    public SerializableConversation(Conversation convo)
    {
        convoPlayedFlag = convo.alreadyPlayed;
    }

    [SerializationConstructor]
    public SerializableConversation(bool savedFlag)
    {
        convoPlayedFlag = savedFlag;
    }
}
