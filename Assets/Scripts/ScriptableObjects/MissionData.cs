using UnityEngine;

[CreateAssetMenu(fileName = "NewMissionData", menuName = "Mission System/Mission Data")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public float timeLimit;
    public string deliveryDestination;
    public string[] dialogueLines;
}
