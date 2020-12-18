using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTurnScript : MonoBehaviour
{
    public int currentTurn = 1;

    public int getTurn()
    {
        return currentTurn;
    }    
    public void finishedTurn()
    {
        currentTurn += 1;
    }
}
