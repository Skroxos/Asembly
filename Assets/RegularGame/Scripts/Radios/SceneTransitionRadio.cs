using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.Scene;
using UnityEngine;

namespace DroneAssembly.Radios
{
    [CreateAssetMenu(menuName = "Event/SceneTransitionRadio")]
    public class SceneTransitionRadio : GeneralRadio<SceneDataSO>
    {
    }
}