using UnityEngine;

public abstract class Shapeshiftable : MonoBehaviour
{
    public enum ColliderDirection
    {
        XAxis,
        YAxis,
        ZAxis
    }

    public abstract Vector3 ColliderCenterValues {get;}
    public abstract float ColliderRadius {get;}
    public abstract float ColliderHeight {get;}
    public abstract ColliderDirection Direction { get; }
}
