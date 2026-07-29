using System;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public Vector3 BoxColliderCenterValues = new Vector3(0, 0.25f, 0);
    public Vector3 BoxColliderSizeValues = new Vector3(.75f, .5f, 2.5f);

    public static event Action<Trick.TrickType> TrickCompleted;

    public void OnTrickComplete(Trick.TrickType whichType)
    {
        //TODO score some points? build some combo? idk man what is this game

        TrickCompleted?.Invoke(whichType);
    }
}
