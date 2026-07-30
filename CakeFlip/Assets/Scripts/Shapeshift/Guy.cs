using UnityEngine;

public class Guy : Shapeshiftable
{
    public override Vector3 ColliderCenterValues => new Vector3(0f, 1.5f, 0f);
    public override float ColliderRadius => .5f;
    public override float ColliderHeight => 3f;
    public override ColliderDirection Direction => ColliderDirection.YAxis;
}
