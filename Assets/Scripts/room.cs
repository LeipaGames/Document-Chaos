using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public List<int> waitingList = new List<int>();

    public int type = 0; // 0: None, 1: Waiting room, 2: Office
    public bool isAvailable = true;
    public int position;
    public int floor;

    private SpriteRenderer spriteRenderer;
    public Sprite officeEmpty;
    public Sprite officeBusy;

    // ========== ========== ========== ========== ========== Functions

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //officeEmpty = Resources.Load<Sprite>("Office Room Empty");
        //officeBusy  = Resources.Load<Sprite>("Office Room Busy");
    }

    public void setSpriteBusy()
    {
        spriteRenderer.sprite = officeBusy;
    }

    public void setSpriteEmpty()
    {
        spriteRenderer.sprite = officeEmpty;
    }
}