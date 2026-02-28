using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation/New Character", order = 2)]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite normalSprite;
    public Sprite surprisedSprite;
    public Sprite confusedSprite;
}
