using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;
using UnityEngine.Animations;

[MessagePackObject]
public class FileSaveData  // Doesn't inherit from MonoBehaviour b/c it's not a game object to instantiate AND MessagePack will complain that MonoBehaviour isn't a MessagePackObject (which is true :( )
{
    [Key(0)] public Vector3 playerPosition;
    [Key(1)] public List<SerializableCollectible> collectiblesToCollect = new List<SerializableCollectible>();
    [Key(2)] public Dictionary<int, SerializableConversation[]> interactedConvos = new Dictionary<int, SerializableConversation[]>();

    // Saving file data
    public FileSaveData(PlayerMove player, LevelManager levelManager)
    {
        playerPosition = player.transform.position;
        
        List<Collectible> leftoverStamps = levelManager.stamps;
        foreach (Collectible savedStamp in leftoverStamps)
        {
            SerializableCollectible serializedCollectible = new SerializableCollectible(savedStamp);
            collectiblesToCollect.Add(serializedCollectible);
        }

        Dictionary<DeliveryAgent, Conversation[]> convoFlags = GetNPC_Conversations(levelManager.npcs);
        foreach (var pair in convoFlags)
        {
            int npcID = pair.Key.GetInstanceID();
            List<SerializableConversation> flags = new List<SerializableConversation>();
            foreach (Conversation convo in pair.Value)
            {
                Debug.Log(convo);
                SerializableConversation flag = new SerializableConversation(convo);
                flags.Add(flag);
            }
            interactedConvos[npcID] = flags.ToArray();
            Debug.Log("-----------------------------");
        }
    }
    
    // When creating a PlayerData object from deserializing a file, the constructor with the SerializationConstructor field will be called 
    [SerializationConstructor]
    public FileSaveData(Vector3 savedPos, List<SerializableCollectible> savedStamps, Dictionary<int, SerializableConversation[]> savedInteractions)
    {
        Debug.Log("MessagePack deserialization constructor called");
        playerPosition = savedPos;
        collectiblesToCollect = savedStamps;
        interactedConvos = savedInteractions;
    }

    // -------------------------------------------------
    // Helper methods
    private Dictionary<DeliveryAgent, Conversation[]> GetNPC_Conversations(DeliveryAgent[] npcs)
    {
        Dictionary<DeliveryAgent, Conversation[]> npcConvos = new Dictionary<DeliveryAgent, Conversation[]>();
        foreach (DeliveryAgent npc in npcs)
        {
            List<Conversation> conversations = new List<Conversation>();
            conversations.Add(npc.currentConversation);
            if (npc.currentConversation.addAnotherConversation == true && npc.currentConversation.followupConversation != null)
            {
                GetFollowUpConversations(npc.currentConversation, ref conversations);
            }
            npcConvos[npc] = conversations.ToArray();
        }

        return npcConvos;
    }

    private void GetFollowUpConversations(Conversation currentConvo, ref List<Conversation> conversations)
    {
        Conversation followUpConvo = currentConvo.followupConversation;
        conversations.Add(followUpConvo);
        if (followUpConvo.addAnotherConversation == true && followUpConvo.followupConversation != null)
        {
            GetFollowUpConversations(followUpConvo, ref conversations);
        }
    }
}
