using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedure")]
public class ProcedureSO : ScriptableObject
{
    public string procedureName;
    public List<Step> steps;
}