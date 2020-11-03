using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TypeTargetHandler
{
    private static Dictionary<TypeTarget, Predicate<Transform>> keys = new Dictionary<TypeTarget, Predicate<Transform>>
    {
        { TypeTarget.Player, (Transform t) => t.CompareTag("Player") },
        { TypeTarget.Enemy, (Transform t) => t.CompareTag("Enemy") },
    };

    public static Predicate<Transform> GetFunc(TypeTarget typeTarget) => keys[typeTarget];
}

public enum TypeTarget
{
    Player,
    Enemy,
}
