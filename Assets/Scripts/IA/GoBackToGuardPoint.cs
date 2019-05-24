using UnityEngine;
using UnityEngine.AI;

public class GoBackToGuardPoint : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private Guard _manager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent == null)
        {
            _agent = animator.gameObject.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            _manager = animator.gameObject.GetComponent(typeof(Guard)) as Guard;
        }
        _agent.SetDestination(_manager.GuardPosition);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent.remainingDistance < 0.2f)
        {
            animator.SetTrigger(_manager.ReachedGuardPointHash);
        }
    }
}