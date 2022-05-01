using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStickBehaviour : Projectile
{
    public SunBehaviour source;

    // Update is called once per frame
    public new void Update()
    {
        transform.Rotate(0, 1, 0);
        Behaviour();
        if (touched)
        {
            source.isTouched = true;
            Destroy(gameObject);
        }
    }
}
