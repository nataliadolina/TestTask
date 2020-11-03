using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatesHandler
{
    State CurrentState { get; set; }
}
