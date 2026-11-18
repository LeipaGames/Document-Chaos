using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class person : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    private int id;

    // Tasks
    public List<int> tasks = new List<int>();
    private int currentTask = 0;
    private room destinationRoom;
    private room waitingRoom;
    private room stairRoom;
    private int exitRoom = 150;

    private bool hasBeenBusy = false;
    private float timer = 0;
    private float busyTime = 5;

    // State Variables
    public bool isWaiting = true;
    private bool isChangingFloors = false;
    private int floor = 0;
    private float impatience = 0;
    private float waitingPosition;

    private float baseHeight = -0.44f;
    private float jumpPosition = 0;

    // Permanent Variables
    private float speed = 1.5f;
    private int roomWidth = 3;
    private float jumpheight = 0.1f;
    private Vector3 left = new Vector3(0, 0, 0);
    private Vector3 right = new Vector3(0, 180, 0);
    private float waitingPositionRandomness = 1;

    // ========== ========== ========== ========== ========== Start & Update

    void Start()
    {
        id = Convert.ToInt32(gameObject.name[6..]);
        speed = speed * UnityEngine.Random.Range(0.9f, 1.1f);
        //float size = UnityEngine.Random.Range(0.9f, 1.1f);
        //transform.localScale = new Vector2(size, size);

        destinationRoom = findRoom(tasks[currentTask]);
        tasks.Add(exitRoom);
        destinationRoom.waitingList.Add(id);

        waitingRoom = findWaitingRoom();
        waitingPosition = UnityEngine.Random.Range(-waitingPositionRandomness, waitingPositionRandomness);
    }

    void Update()
    {
        timer += Time.deltaTime;
        try {
            if (destinationRoom.waitingList[0] == id) isWaiting = false;            
        } catch {
            isWaiting = false;
        }
        bool isChangingRoom = (!isWaiting || tasks[currentTask] == exitRoom) && !isChangingFloors;

        if (isChangingFloors) {
            GoToRoom(stairRoom.position* roomWidth, stairRoom.floor);
        } else if (isWaiting) {
            GoToRoom(waitingRoom.position * roomWidth + waitingPosition, waitingRoom.floor, false);
        } else if (isChangingRoom) {
            GoToRoom(destinationRoom.position * roomWidth + 0.7f, destinationRoom.floor);
        } 
    }

    // ========== ========== ========== ========== ========== Movement

    private void GoToRoom(float destination, int destinationFloor, bool isFullFillingTask = true)
    {
        // Needs to change floor
        if (floor != destinationFloor) {
            ChangeFloors(); return;
        }
        
        // Moving
        if (transform.position.x != destination) {
            MoveTowardsRoom(destination);
            AnimateWalking();
            CorrectRotation(destination);
        
        // Reached room
        } else {
            StopAnimating();

            // Changing floors
            if (isChangingFloors) {
                UseStairs();
                return;
            }

            if (!isFullFillingTask) return;

            // Waiting
            if (!hasBeenBusy) {timer = 0f; hasBeenBusy = true; destinationRoom.setSpriteBusy();}
            if (timer < busyTime || tasks[currentTask] == exitRoom) return; 

            FinishTask();
        }
    }

    // ========== ========== ========== ========== ========== Movement Functions

    private void ChangeFloors() {
        stairRoom = findRoom(Convert.ToInt32($"{floor+1}50"));
        isChangingFloors = true;
    }
    private void UseStairs() {
        //destinationRoom.waitingList.RemoveAt(0);
        //destinationRoom = findRoom(tasks[currentTask]);
        transform.position = new Vector2(transform.position.x, destinationRoom.floor * 1.5f - 0.5f);
        baseHeight = transform.position.y;
        floor = destinationRoom.floor;
        isChangingFloors = false;
    }
    private void CorrectRotation(float destination) {
        if (destination > transform.position.x) {
            transform.rotation = Quaternion.Euler(right);
        } else {
            transform.rotation = Quaternion.Euler(left);
        }
    }
    private void MoveTowardsRoom(float destination) {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector2(destination, transform.position.y),
            Time.deltaTime * speed
        ); 
    }
    private void AnimateWalking() {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector2(transform.position.x, baseHeight + jumpPosition),
            Time.deltaTime * speed/1.5f
        );
        if (math.abs(transform.position.y - (baseHeight + jumpPosition)) < 0.01f) {
            jumpPosition = jumpPosition == jumpheight ? 0 : jumpheight;
        }
    }
    private void StopAnimating() {
        if (transform.position.y != baseHeight) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, baseHeight),
                Time.deltaTime * speed/1.5f
            );
        }
    }
    private void FinishTask() {
        destinationRoom.setSpriteEmpty();
        hasBeenBusy = false;
        currentTask += 1;
        try {
            destinationRoom.waitingList.RemoveAt(0);
            destinationRoom = findRoom(tasks[currentTask]);
            if (tasks[currentTask] == 150) return;
            destinationRoom.waitingList.Add(id);
            isWaiting = true;
            waitingRoom = findWaitingRoom();
            waitingPosition = UnityEngine.Random.Range(-waitingPositionRandomness, waitingPositionRandomness);
        } catch {
            GameObject.Destroy(gameObject);
        }
    }

    // ========== ========== ========== ========== ========== Other Functions

    private room findWaitingRoom()
    {
        int floor = findRoom(tasks[currentTask]).floor+1;
        int id = Convert.ToInt32($"{floor}50");
        return findRoom(id);
    }

    private room findRoom(int roomID)
    {
        return GameObject.Find($"Room{roomID}").GetComponent<room>();
    }
}