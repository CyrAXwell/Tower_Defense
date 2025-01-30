using Unity.Entities;

// [UpdateInGroup(typeof(LateSimulationSystemGroup))]
// partial class AudioManagerSystem : SystemBase
// {
//     protected override void OnUpdate()
//     {
//         foreach (RefRW<Audio> audio in SystemAPI.Query<RefRW<Audio>>())
//         {
//             if (audio.ValueRO.shoot)
//             {
//                 float volume = 0.1f;
//                 AudioManager.Instance.PlaySFX(AudioManager.Instance.Shoot, volume);
//                 audio.ValueRW.shoot = false;
//             }
//         }
//     }
// }
