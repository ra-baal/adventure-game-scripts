using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractiveObject
{
    protected override void getProfits()
    {
        PointsCounter.AddPoints(10);
    }
}
