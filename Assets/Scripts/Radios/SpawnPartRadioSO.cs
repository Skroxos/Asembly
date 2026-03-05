using System.Collections.Generic;
using DroneAssembly.Radios.GeneralRadios;
using DroneAssembly.StepManager;
using UnityEngine;

namespace DroneAssembly.Radios
{
    [CreateAssetMenu(menuName = "Event/SpawnPartRadioSO")]
    public class SpawnPartRadioSO : GeneralRadio<List<StepRequirement>>
    {
    }
}
