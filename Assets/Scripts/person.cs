using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Person : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    private int id;

    // Tasks
    public List<int> tasks = new List<int>();
    public int currentTask = 0;
    private Room destinationRoom;
    private Room waitingRoom;
    private Room elevatorRoom;
    private int exitRoom = 10;

    private bool hasBeenBusy = false;
    private float timer = 0;
    private float busyTime = 5;

    // State Variables
    public bool isWaiting = true;
    private bool isChangingFloors = false;
    private int floor = 1;
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

    // References
    private SpriteRenderer spriteRenderer;
    public PersonSpawner spawner;

    // ========== ========== ========== ========== ========== Start & Update

    void Start()
    {
        id = Convert.ToInt32(gameObject.name[6..]);
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = speed * UnityEngine.Random.Range(0.9f, 1.1f);
        //float size = UnityEngine.Random.Range(0.9f, 1.1f);
        //transform.localScale = new Vector2(size, size);

        //destinationRoom = FindRoom(tasks[currentTask]);
        tasks.Add(exitRoom); // fix
        //destinationRoom.waitingList.Add(id);

        waitingRoom = FindWaitingRoom();
        waitingPosition = UnityEngine.Random.Range(-waitingPositionRandomness, waitingPositionRandomness);

        FindAvailableRoom(tasks[0]);

        Sprite[] sprites = Resources.LoadAll<Sprite>("People");
        int sprite = UnityEngine.Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[sprite];
    }

    void Update()
    {
        timer += Time.deltaTime;
        //try {
        //    if (destinationRoom.waitingList[0] == id) isWaiting = false;
        //} catch {
        //    isWaiting = false;
        //}
        bool isChangingRoom = (!isWaiting || tasks[currentTask] == exitRoom) && !isChangingFloors;

        if (isChangingFloors) {
            GoToRoom(elevatorRoom.position* roomWidth, elevatorRoom.floor);
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
                UseElevator();
                return;
            }

            if (!isFullFillingTask) return;

            // Waiting
            if (!hasBeenBusy) StartTask();
            if (timer < busyTime
            ) return; 

            FinishTask();
        }
    }

    // ========== ========== ========== ========== ========== Movement Functions

    private void ChangeFloors() {
        elevatorRoom = FindRoom(Convert.ToInt32($"{floor}0"));
        isChangingFloors = true;
    }
    private void UseElevator() {
        //destinationRoom.waitingList.RemoveAt(0);
        //destinationRoom = findRoom(tasks[currentTask]);
        int destinationFloor = tasks[currentTask];
        if (destinationFloor == exitRoom) destinationFloor = 1;
        transform.position = new Vector2(transform.position.x, (destinationFloor-1) * 1.5f - 0.5f);
        floor = destinationFloor;

        baseHeight = transform.position.y + 0.06f;
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
    private void StartTask() {
        spriteRenderer.enabled = false;
        if (tasks[currentTask] != exitRoom) {
            destinationRoom.SetSpriteBusy();
        } else {spawner.count--; Destroy(gameObject);}
        timer = 0f;
        hasBeenBusy = true;
    }
    private void FinishTask() {
        spriteRenderer.enabled = true;
        if (tasks[currentTask] != exitRoom) {
            destinationRoom.AddMoney();
            destinationRoom.SetSpriteEmpty();
            destinationRoom.isAvailable = true;
        }
        hasBeenBusy = false;
        currentTask += 1;
        try {
            //destinationRoom.waitingList.RemoveAt(0);
            //destinationRoom = FindRoom(tasks[currentTask]);
            //destinationRoom.waitingList.Add(id);

            if (tasks[currentTask] == exitRoom) {
                destinationRoom = FindRoom(10);
                waitingPosition = 0;
            } else {
                FindAvailableRoom(tasks[currentTask]);  
                waitingRoom = FindWaitingRoom();
            }
            waitingPosition = UnityEngine.Random.Range(-waitingPositionRandomness, waitingPositionRandomness);

            waitingRoom.SendNext();
        } catch {
            spawner.count--;
            Destroy(gameObject);
        }
    }

    // ========== ========== ========== ========== ========== Other Functions

    public void FindAvailableRoom(int floor) {;
        for (int i = 1; true; i++) {
            try {
                Room currentRoom = FindRoom(Convert.ToInt32($"{floor}{i}"));
                if (currentRoom.isAvailable && currentRoom.hasBeenBought) {
                    destinationRoom = currentRoom;
                    destinationRoom.isAvailable = false;
                    isWaiting = false;
                    return;
                }
            } catch {
                isWaiting = true;
                waitingRoom.waitingList.Add(id);
                return;
            }
        }
    }

    private Room FindWaitingRoom()
    {
        int floor = tasks[currentTask];
        int id = Convert.ToInt32($"{floor}0");
        return FindRoom(id);
    }

    private Room FindRoom(int roomID)
    {
        return GameObject.Find($"Room{roomID}").GetComponent<Room>();
    }
}