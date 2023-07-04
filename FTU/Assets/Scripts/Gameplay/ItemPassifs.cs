using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPassifs : ItemStats
{
    public override IEnumerator OnUse()
    {
        Debug.Log("Oue");
        yield return 0;
    }
}
