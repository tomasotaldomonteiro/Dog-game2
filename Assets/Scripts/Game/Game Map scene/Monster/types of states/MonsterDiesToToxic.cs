using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDiesToToxic : AStateBehaviour
{
    private MonsterDiesManager monsterDiesManager;
    private SpriteRenderer spriteRenderer;

    public override bool InitializeState()
    {
        monsterDiesManager = GetComponent<MonsterDiesManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        return monsterDiesManager != null;
    }

    public override void OnStateStart()
    {
        monsterDiesManager.StartBurnAnimation();
        spriteRenderer.color = Color.magenta;
    }

    
    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateEnd()
    {
       
    }

    public override int StateTransitionCondition()
    {
        if (monsterDiesManager.IsBurnAnimationFinished())
        {
            return (int)EShowcaseMonsterStates.Invalid;
        }

        return (int)EShowcaseMonsterStates.Invalid;
    }
    
}