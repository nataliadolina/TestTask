using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    [SerializeField] StateBase idle;
    [SerializeField] StateBase pursuing;
    [SerializeField] StateBase attacking;
    [SerializeField] StateBase dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
