using System;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public static event Action<Trick.TrickType> TrickCompleted;

    public void OnTrickComplete(Trick.TrickType whichType)
    {
        //TODO score some points? build some combo? idk man what is this game

        TrickCompleted?.Invoke(whichType);
    }
}
