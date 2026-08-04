using UnityEngine;

public abstract class Shapeshiftable : MonoBehaviour
{
    public enum ColliderAxis
    {
        XAxis,
        YAxis,
        ZAxis
    }

    public abstract Vector3 CameraOffset { get; }
    public abstract Vector3 ColliderCenterValues { get; }
    public abstract float ColliderRadius { get; }
    public abstract float ColliderHeight { get; }
    public abstract ColliderAxis Direction { get; }
}
