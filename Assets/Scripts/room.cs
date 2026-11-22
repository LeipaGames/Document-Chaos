using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

public class Room : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public List<int> waitingList = new List<int>();

    public bool hasBeenBought = false;
    public bool isVisible = false;
    public int price;

    public bool isAvailable = true;
    public int type = 0;
    public int position;
    public int floor;

    private SpriteRenderer spriteRenderer;
    public GameObject canvas;
    public TextMeshProUGUI canvasText;
    public Sprite officeEmpty;
    public Sprite officeBusy;
    public Money moneyObject;
    public GameManager gameManager;
    public PersonSpawner personSpawner;

    public int[] money = {1, 2, 5, 10, 25, 50, 100};

    // ========== ========== ========== ========== ========== Functions

    void Start()
    {
        personSpawner = GameObject.Find("Room10").GetComponent<PersonSpawner>();
        gameManager = GameObject.Find("Room10").GetComponent<GameManager>();
        canvasText.text = $"${price}";
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (type == 1) {
            officeEmpty = Resources.Load<Sprite>($"Rooms/Office Room {floor} Empty");
            officeBusy  = Resources.Load<Sprite>($"Rooms/Office Room {floor} Busy");
            SetSpriteEmpty();            
        } else if (type == 2)
        {
            Sprite elevatorRoom = Resources.Load<Sprite>($"Rooms/Elevator Room");
            spriteRenderer.sprite = elevatorRoom;
        }

        spriteRenderer.enabled = false;
        canvas.SetActive(false);
        if (gameObject.name == "Room10") MakeVisible();
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
            Person person = GameObject.Find($"Person{waitingList[0]}").GetComponent<Person>();
            waitingList.RemoveAt(0);
            person.FindAvailableRoom(person.tasks[person.currentTask]);
        } catch {return;}
    }

    public void AddMoney()
    {
        Money newMoneyObject = Instantiate(moneyObject, new Vector2(transform.position.x + 0.7f, transform.position.y), quaternion.identity);
        SpriteRenderer moneySpriteRenderer = newMoneyObject.GetComponent<SpriteRenderer>();
        Sprite moneySprite = Resources.Load<Sprite>($"Money/{floor}");
        moneySpriteRenderer.sprite = moneySprite;
        gameManager.money += money[floor-1];
        gameManager.UpdateLabel();
    }

    public void MakeVisible()
    {
        spriteRenderer.enabled = true;
        canvas.SetActive(true);
    }

    public void Buy()
    {
        if (gameManager.money >= price) {
            canvas.SetActive(false);
            hasBeenBought = true;
            SendNext();
            gameManager.money -= price;
            gameManager.UpdateLabel();

            if (type == 2) {
                personSpawner.floorCount++;

                Room nextRoom = GameObject.Find($"Room{floor}{position+1}").GetComponent<Room>();
                nextRoom.MakeVisible();
                try {
                    Room nextElevatorRoom = GameObject.Find($"Room{floor+1}{position}").GetComponent<Room>();
                    nextElevatorRoom.MakeVisible();
                } catch {
                    return;
                }

            } else if (type == 1) {
                personSpawner.roomCount++;
                personSpawner.RecalculateSpawnRate();

                try {
                    Room nextRoom = GameObject.Find($"Room{floor}{position+1}").GetComponent<Room>();
                    nextRoom.MakeVisible();                     
                } catch {
                    return;
                }
            }
        }
    }
}