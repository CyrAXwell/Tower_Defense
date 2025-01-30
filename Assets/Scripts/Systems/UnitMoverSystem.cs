using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    public const float REACHED_TARGET_POSITION_DISTANCE_SQ = 2f;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJob unitMoverJob = new UnitMoverJob {
            deltaTime = SystemAPI.Time.DeltaTime,
        };

        unitMoverJob.ScheduleParallel();

        // foreach ((
        //     RefRW<LocalTransform> localTransform, 
        //     RefRO<UnitMover> unitMover,
        //     RefRW<PhysicsVelocity> physicsVelocity) 
        //     in SystemAPI.Query<
        //         RefRW<LocalTransform>, 
        //         RefRO<UnitMover>,
        //         RefRW<PhysicsVelocity>>())
        // {   
        //     float3 targetPosition = localTransform.ValueRW.Position + new float3(10, 0, 0);
        //     float3 moveDirection = targetPosition - localTransform.ValueRW.Position;
        //     moveDirection = math.normalize(moveDirection);

        //     localTransform.ValueRW.Rotation = 
        //         math.slerp(localTransform.ValueRO.Rotation, 
        //                 quaternion.LookRotation(moveDirection, math.up()), 
        //                 unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);
        //     physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.moveSpeed;
        //     physicsVelocity.ValueRW.Angular = float3.zero;

        //     // localTransform.ValueRW.Position += moveDirection * unitMover.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
        // }
    }
}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, ref UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {     
        float3 moveDirection = unitMover.targetPosition - localTransform.Position;

        float reachedTargetDistance = UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ;
        if (math.lengthsq(moveDirection) < reachedTargetDistance)
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }
        
        moveDirection = math.normalize(moveDirection);

        localTransform.Rotation = 
            math.slerp(localTransform.Rotation, 
                    quaternion.LookRotation(moveDirection, math.up()), 
                    unitMover.rotationSpeed * deltaTime);
        physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
        physicsVelocity.Angular = float3.zero;

        unitMover.onMoving = true;
    }
}
