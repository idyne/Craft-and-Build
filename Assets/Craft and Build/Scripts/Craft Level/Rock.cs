using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : FarmableObject
{
    protected override void Animate()
    {
        transform.LeanScale(transform.localScale - Vector3.one * 0.25f, 0.2f).setEaseInOutBack();
    }

    protected override void Respawn()
    {
        transform.LeanScale(Vector3.one, 0.2f).setEaseInOutBack().setOnComplete(ResetAmount);
    }
}
