using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    private int maxRooms = 6;
    private int floors = 7;

    private float verticalStep = 1.5f;
    private float horizontalStep = 3f;

    public Room room;

    // ========== ========== ========== ========== ========== Start & Update§

    void Start()
    {
        for (int i = 1; i < floors+1; i++)
        {
            if (i > 1) CreateRoom(i, 0, 2);
            for (int j = 0; j < maxRooms; j++) {
                CreateRoom(i, j+1, 1);
            }
        }
    }
    private void CreateRoom(int floor, int position, int type)
    {
        Room newRoom = Instantiate(room, new Vector3(position * horizontalStep, (floor-1) * verticalStep, 1), Quaternion.identity);
        newRoom.name = $"Room{floor}{position}";
        newRoom.floor = floor;
        newRoom.position = position;
        newRoom.type = type;
        if (type == 1) {
            newRoom.price = (newRoom.money[floor-1] * 10) + ((position-2) * newRoom.money[floor-1] * 5);
        } else if (type == 2) {
            newRoom.price = (newRoom.money[floor-1] * 10) + (newRoom.money[floor-1] * 5);
        }
       
    }
}