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

    // ========== ========== ========== ========== ========== Functions
}
