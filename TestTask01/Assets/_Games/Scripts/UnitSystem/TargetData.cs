using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Character;

public static class TargetData 
{
    private static Dictionary<Target, Predicate<Transform>> keys = new Dictionary<Target, Predicate<Transform>>
    {
        { Target.Player, (Transform t) => Vector3.SqrMagnitude(CharacterControl.Instance.playerTransform.position - t.position) <= 2/*value*/},
        { Target.Enemy, (Transform t) => Vector3.SqrMagnitude(CharacterControl.Instance.playerTransform.position - t.position) <= 2/* value*/}
    };

    public static Predicate<Transform> GetFunc(Target typeTarget) => keys[typeTarget];  
}

public enum Target
{
    Enemy,
    Player,
}
