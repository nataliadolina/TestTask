using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hitPoint;
    public int HitPoint
    {
        get => hitPoint;
        set
        {
            hitPoint -= value;
        }
    }

    public Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
}
