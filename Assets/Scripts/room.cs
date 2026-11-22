using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public List<int> waitingList = new List<int>();

    public int type = 0;
    public bool isAvailable = true;
    public int position;
    public int floor;

    public int[] money = {5, 7, 10, 15, 25, 50, 100};

    private SpriteRenderer spriteRenderer;
    public Sprite officeEmpty;
    public Sprite officeBusy;

    // ========== ========== ========== ========== ========== Functions

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        officeEmpty = Resources.Load<Sprite>($"Rooms/Office Room {type} Empty");
        officeBusy  = Resources.Load<Sprite>($"Rooms/Office Room {type} Busy");
    }

    public void SetSpriteBusy()
    {
        spriteRenderer.sprite = officeBusy;
    }

    public void SetSpriteEmpty()
    {
        spriteRenderer.sprite = officeEmpty;
    }

    public void SendNext()
    {
        try {
            person person = GameObject.Find($"Person{waitingList[0]}").GetComponent<person>();
            waitingList.RemoveAt(0);
            person.FindAvailableRoom(person.tasks[person.currentTask]);
        } catch {return;}
    }
}