using System;
using System.Collections.Generic;
using NUnit.Framework;
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

    // State Variables
    public bool isWaiting = true;
    private int floor = 0;
    private bool isChangingFloors = false;
    private float impatience = 0;
    private float baseHeight = -0.5f;
    private float jumpPosition = 0;

    // Permanent Variables
    private float speed = 1.5f;
    private int roomWidth = 3;
    private float jumpheight = 0.1f;
    private Vector3 left = new Vector3(0, 0, 0);
    private Vector3 right = new Vector3(0, 180, 0);

    // ========== ========== ========== ========== ========== Start & Update

    void Start()
    {
        id = Convert.ToInt32(gameObject.name[5..]);
        speed = speed * UnityEngine.Random.Range(0.9f, 1.1f);

        destinationRoom = findRoom(tasks[currentTask]);
        tasks.Add(150);
        destinationRoom.waitingList.Add(id);
    }

    void Update()
    {
        if (destinationRoom.waitingList[0] == id) isWaiting = false;
        if (!isWaiting) {
            GoToRoom(destinationRoom.position * roomWidth, destinationRoom.floor);
        }
    }

    // ========== ========== ========== ========== ========== Movement

    private void GoToRoom(int destination, int destinationFloor)
    {
        if (floor != destinationFloor) {
            destinationRoom = findRoom(Convert.ToInt32($"{floor+1}50"));
            isChangingFloors = true;
            return;
        }

        // Rotation
        if (destination > transform.position.x) {
            transform.rotation = Quaternion.Euler(right);
        } else {
            transform.rotation = Quaternion.Euler(left);
        }

        // Movement
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector2(destination, transform.position.y),
            Time.deltaTime * speed
        );

        // Jumping
        if (transform.position.x != destination) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, baseHeight + jumpPosition),
                Time.deltaTime * speed/1.5f
            );
            if (math.abs(transform.position.y - (baseHeight + jumpPosition)) < 0.01f) {
                jumpPosition = jumpPosition == jumpheight ? 0 : jumpheight;
            }
        
        // Finished
        } else {
            if (isChangingFloors) {
                destinationRoom = findRoom(tasks[currentTask]);
                transform.position = new Vector2(transform.position.x, destinationRoom.floor * 1.5f - 0.5f);
                baseHeight = transform.position.y;
                floor = destinationRoom.floor;
                isChangingFloors = false;
                return;
            }

            currentTask += 1;
            try {
                destinationRoom.waitingList.RemoveAt(0);
                destinationRoom = findRoom(tasks[currentTask]);
            } catch {
                GameObject.Destroy(gameObject);
            }
            
        }
    }

    // ========== ========== ========== ========== ========== Other Functions

    private room findRoom(int roomID)
    {
        return GameObject.Find($"Room{roomID}").GetComponent<room>();
    }

}