using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private Vector3 _guardPosition;
    private Animator _animator;
    private int _playerOnSightHash;
    private int _reachedGuardPointHash;

    public Vector3 GuardPosition { get { return _guardPosition; } }
    public int ReachedGuardPointHash { get { return _reachedGuardPointHash; } }
    private void Start()
    {
        _guardPosition = transform.position;
        _animator = GetComponent(typeof(Animator)) as Animator;
        _playerOnSightHash = Animator.StringToHash("PlayerOnSight");
        _reachedGuardPointHash = Animator.StringToHash("ReachedGuardPoint");
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _animator.SetBool(_playerOnSightHash, true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _animator.SetBool(_playerOnSightHash, false);
        }
    }
}
