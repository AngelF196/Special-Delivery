using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DeliveryAgent : BaseNPC, ITalkable
{
    [SerializeField] private Conversation _conversation;
    [SerializeField] private DialogueController _dialogueController;

    public override void Interact()
    {
        Talk(_conversation);
    }

    public void Talk(Conversation conversation)
    {
        _dialogueController.DisplayNextLine(_conversation);
    }
}
