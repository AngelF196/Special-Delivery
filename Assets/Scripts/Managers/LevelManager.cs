using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _stampPrefab;
    public List<Collectible> stamps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerCollectedStamp(Collectible stamp)
    {
        stamps.Remove(stamp);
        Destroy(stamp.gameObject);
    }

    public void LoadCollectibles(FileSaveData fileData)
    {
        // Clear the current list of collectibles
        List<Collectible> neglectedCollectibles = new List<Collectible>();
        foreach(Collectible negStamp in stamps)
        {
            neglectedCollectibles.Add(negStamp);
        }
        foreach(Collectible stamp in neglectedCollectibles)
        {
            stamps.Remove(stamp);
            Destroy(stamp.gameObject);
        }
        neglectedCollectibles.Clear();

        // Load saved list of collectibles
        foreach (SerializableCollectible serializedStamp in fileData.savedCollectibles)
        {
            // Set up stamp game object
            GameObject savedStamp = Instantiate(_stampPrefab);
            Collectible stampComponent = savedStamp.GetComponent<Collectible>();
            savedStamp.transform.parent = GameObject.Find("Collectibles").transform;  // Add child gems under the parent "Gems" game object

            // Load in stamp object data
            savedStamp.transform.position = serializedStamp.collectiblePosition;
            stamps.Add(stampComponent);
        }
    }
}
