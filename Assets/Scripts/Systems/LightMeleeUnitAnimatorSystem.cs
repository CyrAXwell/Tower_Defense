using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(LightMeleeUnitAnimatorSpawnSystem))]
partial struct LightMeleeUnitAnimatorSystem : ISystem
{
    public const string ANIMATOR_MOVE_TRIGGER = "Walk";
    public const string ANIMATOR_STOP_MOVE_TRIGGER = "StopWalk";
    public const string ANIMATOR_ATTACK_TRIGGER = "Attack";

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((
            LocalTransform localTransform, 
            LightMeleeUnitAnimator botAnimator, 
            UnitMover unitMover, 
            MeleeAttack meleeAttack) 
            in SystemAPI.Query<
                LocalTransform, 
                LightMeleeUnitAnimator, 
                UnitMover, 
                MeleeAttack>())
        {
            if (meleeAttack.onAttack)
            {
                botAnimator.animator.SetTrigger(ANIMATOR_ATTACK_TRIGGER);   
                botAnimator.animator.ResetTrigger(ANIMATOR_MOVE_TRIGGER);
                botAnimator.animator.ResetTrigger(ANIMATOR_STOP_MOVE_TRIGGER);
            }
            else if (unitMover.onMoving)
            {
                botAnimator.animator.SetTrigger(ANIMATOR_MOVE_TRIGGER);
                botAnimator.animator.ResetTrigger(ANIMATOR_STOP_MOVE_TRIGGER);
            }
            else
                botAnimator.animator.SetTrigger(ANIMATOR_STOP_MOVE_TRIGGER);
        
            botAnimator.animator.transform.position = localTransform.Position;
            botAnimator.animator.transform.rotation = localTransform.Rotation;
        }
    }
}
