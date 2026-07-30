using System;
using UnityEngine;

public class Skateboard : Shapeshiftable
{
    public override Vector3 ColliderCenterValues => new Vector3(0f, .25f, 0f);
    public override float ColliderRadius => .3f;
    public override float ColliderHeight => 2.75f;
    public override ColliderDirection Direction => ColliderDirection.ZAxis;

    public static event Action<Trick.TrickType> TrickCompleted;

    public void OnTrickComplete(Trick.TrickType whichType)
    {
        TrickCompleted?.Invoke(whichType);
    }
}
