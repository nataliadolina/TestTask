using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataa : MonoBehaviour
{
    [SerializeField] private float health;

    public float HitPoint
    {
        get => health;
        set => health -= value;
    }

    public UnitDataa aim;
}
