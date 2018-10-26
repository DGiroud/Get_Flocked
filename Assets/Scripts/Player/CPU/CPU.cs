using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CPUMode
{
    Balanced,
    Offensive,
    Defensive
}

[RequireComponent(typeof(NavMeshAgent))]
public class CPU : BaseActor
{
    // cpu mode which determines aggression
    [Header("CPU Specific")]
    public CPUMode CPUMode;

    // other relevant A.I variables
    [SerializeField]
    private float artificialThinkTime = 0.5f;
    public float ArtificialThinkTime { get { return artificialThinkTime; } }
}
