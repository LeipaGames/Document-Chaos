using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public GameObject person;
    private int nextPersonID = 0;
    private float timer = 0;
    public float spawnRate = 0;
    public float spawnRandomness = 1f;
    private float spawnPositionRandomness = 1;
    private float nextSpawn;

    private int maxCount = 50;
    public int count = 0;

    public int floorCount = 0;
    public int roomCount = 0;
    //private int[] rooms = {151, 152, 251, 252};

    // ========== ========== ========== ========== ========== Start & Update 

    void Start()
    {
        nextSpawn = calculateNextSpawn();
        //floorCount = 3; //temp
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawn && count < maxCount) spawnPerson();
    }

    // ========== ========== ========== ========== ========== Functions 

    private void spawnPerson()
    {
        if (spawnRate == 0) return;
        float spawnX = Random.Range(-spawnPositionRandomness, spawnPositionRandomness);
        GameObject spawnedPerson = Instantiate(person, new Vector2(spawnX, -0.5f), Quaternion.identity);
        count++;
        
        Person spawnedPersonScript = spawnedPerson.GetComponent<Person>();
        spawnedPerson.name = $"Person{nextPersonID}";
        spawnedPersonScript.tasks = generateTasks();
        spawnedPersonScript.spawner = GetComponent<PersonSpawner>();

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
        int maxTaskNumber = Mathf.RoundToInt(floorCount*0.66f);
        List<int> tasks = new List<int>();
        int taskNumber = Random.Range(1, maxTaskNumber+1);
        for (int i = 0; i < taskNumber; i++) {
            int newTask = Random.Range(1, floorCount+1);
            if (i > 0) {
                while(newTask == tasks[i-1]) {
                    newTask = Random.Range(1, floorCount+1);
                }
            }
            tasks.Add(newTask);
        }
        return tasks;
    }

    public void RecalculateSpawnRate()
    {
        spawnRate = 15/roomCount;
    }
}