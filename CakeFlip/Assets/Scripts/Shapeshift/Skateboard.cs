using System;
using UnityEngine;

public class Skateboard : Shapeshiftable
{
    public override Vector3 ColliderCenterValues => new Vector3(0f, .25f, 0f);
    public override float ColliderRadius => .3f;
    public override float ColliderHeight => 2.75f;
    public override ColliderAxis Direction => ColliderAxis.ZAxis;
    public override Vector3 CameraOffset => new Vector3(0f, .5f, 0f);

    public static event Action<Trick.TrickType> TrickCompleted;

    public void OnTrickComplete(Trick.TrickType whichType)
    {
        TrickCompleted?.Invoke(whichType);
    }
}
