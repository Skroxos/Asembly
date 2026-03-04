using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StepManager))]
public class StepManagerEditor : Editor
{
    private int targetStepIndex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        var stepManager = (StepManager)target;

        var totalSteps = stepManager.GetTotalStepsDebug();

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);

        GUILayout.BeginHorizontal();

        if (totalSteps > 0)
        {
            if (GUILayout.Button("Skip to next step")) stepManager.AdvanceStepDebug();

            if (GUILayout.Button("Reset Steps")) stepManager.ResetStepsDebug();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            targetStepIndex = EditorGUILayout.IntSlider("Step Index:", targetStepIndex, 1, totalSteps);

            if (GUILayout.Button("Jump")) stepManager.JumpToStepDebug(targetStepIndex);
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("No steps defined in the procedure.", MessageType.Info);
        }


        EditorGUI.EndDisabledGroup();


        if (!Application.isPlaying)
            EditorGUILayout.HelpBox("Enter Play Mode to enable step controls.", MessageType.Info);
    }
}