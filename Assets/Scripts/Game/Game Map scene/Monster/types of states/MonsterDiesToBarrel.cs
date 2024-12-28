using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDiesToBarrel : AStateBehaviour
{
    private MonsterExplodedManager monsterExplodedManager;
    private SpriteRenderer spriteRenderer;

    public override bool InitializeState()
    {
        monsterExplodedManager = GetComponent<MonsterExplodedManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        return monsterExplodedManager != null;
    }

    public override void OnStateStart()
    {
        monsterExplodedManager.StartExplodedAnimation();
        //spriteRenderer.color = Color.magenta;
    }

    
    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateEnd()
    {
       
    }

    public override int StateTransitionCondition()
    {
        if (monsterExplodedManager.IsExplodedAnimationFinished())
        {
            return (int)EShowcaseMonsterStates.Invalid;
        }

        return (int)EShowcaseMonsterStates.Invalid;
    }
    
}