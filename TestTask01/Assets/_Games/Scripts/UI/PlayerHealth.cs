using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider healthBar;

    void Start()
    {
        healthBar = GetComponent<Slider>();
        PlayerPersecutionState state = FindObjectOfType<PlayerPersecutionState>();
        healthBar.maxValue = state.GetComponentInParent<UnitDataa>().HitPoint;
        healthBar.minValue = 0f;
    }

    private void ChangeValue(float harm)
    {
        healthBar.value -= harm;
    }
}
