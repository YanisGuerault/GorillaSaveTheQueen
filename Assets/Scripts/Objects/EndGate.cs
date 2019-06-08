using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.Raise(new AskToGoToNextLevelEvent());
        }
    }
}
