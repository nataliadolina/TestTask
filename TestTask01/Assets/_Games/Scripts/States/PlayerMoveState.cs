using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
    [SerializeField] private float speed;
    [SerializeField] private State nextState;

    private Transform playerTransform;
    private StatesExecuter playerExecuterStates;

    protected override void Start()
    {
        playerExecuterStates = GetComponentInParent<StatesExecuter>();
        playerTransform = playerExecuterStates.gameObject.transform;
    }

    public override void Run()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        Move(direction);
        Rotate(direction);

        if (Input.GetMouseButtonDown(0))
        {
            playerExecuterStates.CurrentState = nextState;
        }
    }

    private void Move(Vector3 direction)
    {
        playerTransform.position += direction * (Time.deltaTime * speed);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction.magnitude > 0)
            playerTransform.localRotation = Quaternion.RotateTowards(from: transform.rotation,
                to: Quaternion.LookRotation(direction), maxDegreesDelta: Time.deltaTime * 720);
    }
}
