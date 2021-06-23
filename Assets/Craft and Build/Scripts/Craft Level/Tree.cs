using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : FarmableObject
{
    protected override void Animate()
    {
        transform.LeanMoveY(transform.position.y - 2f, 0.2f).setEaseInQuart();
    }

    protected override void Respawn()
    {
        transform.LeanMoveY(0, 0.2f).setEaseInQuart().setOnComplete(ResetAmount);
    }
}
