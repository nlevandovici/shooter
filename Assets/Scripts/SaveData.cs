using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public int winCount;

    public int loseCount;



    public SaveData()
    {
        winCount = 0;

        loseCount = 0;
    }
}
