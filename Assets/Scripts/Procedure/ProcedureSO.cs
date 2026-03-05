using System.Collections.Generic;
using DroneAssembly.StepManager;
using UnityEngine;

namespace DroneAssembly.Procedure
{
    [CreateAssetMenu(menuName = "Procedure")]
    public class ProcedureSO : ScriptableObject
    {
        public string procedureName;
        public List<Step> steps;
    }
}