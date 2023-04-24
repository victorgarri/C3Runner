using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverInfo
{
    public int playerNumber = 0;
    public string name = "";
    public int carUniqueID = 0;
    public bool isAI = false;
    public int lastRacePosition = 0;
    public int championshipPoints = 0;

    public DriverInfo(int playerNumber, string name, int carUniqueID, bool isAI)
    {
        this.playerNumber = playerNumber;
        this.name = name;
        this.carUniqueID = carUniqueID;
        this.isAI = isAI;
    }
}
