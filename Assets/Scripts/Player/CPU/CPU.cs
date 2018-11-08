using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CPUSeekMode
{
    Nearest,
    HighestWorth,
    LowestWorth
}

[RequireComponent(typeof(NavMeshAgent))]
public class CPU : BaseActor
{
    // cpu mode which determines aggression
    [Header("CPU Specific")]
    public CPUSeekMode cpuSeekMode;
    [SerializeField]
    [Range(0, 100)]
    private float roomForError = 10.0f;

    // other relevant A.I variables
    [SerializeField]
    private float artificialThinkTime = 0.5f;
    public float ArtificialThinkTime { get { return artificialThinkTime; } }
    
    public void Start()
    {
        actorType = ActorType.CPU;
    }
}
