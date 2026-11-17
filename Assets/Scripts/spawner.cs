using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public GameObject person;
    private int nextPersonID = 0;
    private float timer = 0;
    public float spawnRate = 3;
    public float spawnRandomness = 2;
    private float spawnPositionRandomness = 1;
    private float nextSpawn;

    private int maxTaskNumber = 3;
    private int[] rooms = {148, 149, 151, 152, 248, 249, 251, 252, 348, 349, 351, 352};
    //private int[] rooms = {151, 152, 251, 252};

    // ========== ========== ========== ========== ========== Start & Update 

    void Start()
    {
        nextSpawn = calculateNextSpawn();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawn) spawnPerson();
    }

    // ========== ========== ========== ========== ========== Functions 

    private void spawnPerson()
    {
        float spawnX = Random.Range(-spawnPositionRandomness, spawnPositionRandomness);
        GameObject spawnedPerson = Instantiate(person, new Vector2(spawnX, -0.5f), Quaternion.identity);
        
        person spawnedPersonScript = spawnedPerson.GetComponent<person>();
        spawnedPerson.name = $"Person{nextPersonID}";
        spawnedPersonScript.tasks = generateTasks();

        timer = 0f;
        nextPersonID++;
        nextSpawn = calculateNextSpawn();
    }

    private float calculateNextSpawn()
    {
        float randomOffset = Random.Range(-spawnRandomness, spawnRandomness);
        return spawnRate + randomOffset;
    }

    private List<int> generateTasks()
    {
        List<int> tasks = new List<int>();
        int taskNumber = Random.Range(1, maxTaskNumber+1);
        for (int i = 0; i < taskNumber; i++) {
            tasks.Add(rooms[Random.Range(0, rooms.Count())]);
        }
        return tasks;
    }
}