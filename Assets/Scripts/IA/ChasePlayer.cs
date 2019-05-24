using UnityEngine;
using UnityEngine.AI;
public class ChasePlayer : StateMachineBehaviour
{
    private const float PathUpdateInterval = 0.3f;
    private GameObject _player;
    private NavMeshAgent _agent;
    private float _lastUpdateTime = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _agent = animator.gameObject.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > _lastUpdateTime + PathUpdateInterval)
        {
            _lastUpdateTime = Time.time;
            _agent.SetDestination(_player.transform.position);
        }
    }
}