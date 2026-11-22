using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public GameObject person;
    private int nextPersonID = 0;
    private float timer = 0;
    public float spawnRate = 2;
    public float spawnRandomness = 1f;
    private float spawnPositionRandomness = 1;
    private float nextSpawn;

    private int maxCount = 50;
    public int count = 0;

    private int floorCount;
    private int maxTaskNumber;
    //private int[] rooms = {151, 152, 251, 252};

    // ========== ========== ========== ========== ========== Start & Update 

    void Start()
    {
        nextSpawn = calculateNextSpawn();
        floorCount = 3; //temp
        maxTaskNumber = Mathf.RoundToInt(floorCount*0.66f);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawn && count < maxCount) spawnPerson();
    }

    // ========== ========== ========== ========== ========== Functions 

    private void spawnPerson()
    {
        float spawnX = UnityEngine.Random.Range(-spawnPositionRandomness, spawnPositionRandomness);
        GameObject spawnedPerson = Instantiate(person, new Vector2(spawnX, -0.5f), Quaternion.identity);
        count++;
        
        person spawnedPersonScript = spawnedPerson.GetComponent<person>();
        spawnedPerson.name = $"Person{nextPersonID}";
        spawnedPersonScript.tasks = generateTasks();
        spawnedPersonScript.spawner = GetComponent<spawner>();

        timer = 0f;
        nextPersonID++;
        nextSpawn = calculateNextSpawn();
    }

    private float calculateNextSpawn()
    {
        float randomOffset = UnityEngine.Random.Range(-spawnRandomness, spawnRandomness);
        return spawnRate + randomOffset;
    }

    private List<int> generateTasks()
    {
        List<int> tasks = new List<int>();
        int taskNumber = UnityEngine.Random.Range(1, maxTaskNumber+1);
        for (int i = 0; i < taskNumber; i++) {
            int newTask = UnityEngine.Random.Range(1, floorCount+1);
            if (i > 0) {
                while(newTask == tasks[i-1]) {
                    newTask = UnityEngine.Random.Range(1, floorCount+1);
                }
            }
            tasks.Add(newTask);
        }
        return tasks;
    }
}