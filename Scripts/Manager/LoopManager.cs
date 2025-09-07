using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeOfDay
{
    Morning,
    Afternoon,
    Evening,
    Night
}

public enum LoopPhase
{
    Early,
    Mid,
    Late,
    Final
}
public class LoopManager : MonoBehaviour
{
    private LoopPhase currentPhase;
    public LoopPhase CurrentPhase { get { return currentPhase; } }
    private void Init()
    {
        
    }

}