using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
    [SerializeField] public Transform spawn_point;

    List<Enemy> m_Enemies = new List<Enemy>();

    public void SubscribeEvents()
    {
    }

    public void UnsubscribeEvents()
    {
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        //enemies
        m_Enemies = GetComponentsInChildren<Enemy>().ToList();
    }

    private void Update()
    {
    }

    void ResetMovingItems()
    {
    }
}
