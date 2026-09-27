using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float spawnsPerMin;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnAngle;
    [SerializeField] private bool startOnLoad;
    [SerializeField] private bool spawning;


    void Start()
    {
        ToggleSpawns(startOnLoad);
    }

    IEnumerator SpawnLoop()
    {
        while (true) 
        {
            if (!spawning) break;
            Spawn();
            yield return new WaitForSeconds(60/spawnsPerMin);
        }
    }

    private void Spawn()
    {
        Instantiate(objectToSpawn, spawnPoint.position, Quaternion.Euler(0f, 0f, spawnAngle));
    }

    public void UpdateSpawnRate(float updatedSpawnsPerMin)
    {
        spawnsPerMin = updatedSpawnsPerMin;
    }

    public void ToggleSpawns(bool On)
    {
        spawning = On;
        if (spawning)
        {
            StartCoroutine(SpawnLoop());
        }
    }
}
