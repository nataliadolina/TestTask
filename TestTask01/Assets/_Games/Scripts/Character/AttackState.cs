using Character;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // статическая ссылка на игрока - херня как-та
    public static CharacterControl _characterControl;
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControl.CurrentState = CharacterControl.State.DEFAULT;
    }

}
