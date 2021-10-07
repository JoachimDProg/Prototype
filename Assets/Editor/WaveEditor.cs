using UnityEditor;

[CustomEditor(typeof(EnemyWaves))]
[CanEditMultipleObjects]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // update menu
        serializedObject.Update();

        // show base menu
        base.OnInspectorGUI();

        // reference to current wave being modified
        EnemyWaves enemyWaves = target as EnemyWaves;

        // references to EnemyWaves structures
        SerializedProperty movementType = serializedObject.FindProperty("movementType");

        // create movementType selector
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(movementType);

        switch (movementType.intValue)
        {
            case 0:
                enemyWaves.moveTypeFlag = 0;
                break;

            case 1:
                enemyWaves.moveTypeFlag = 1;
                break;

            case 2:
                // create indent and sineParameters menu
                EditorGUI.indentLevel++;
                enemyWaves.isSine = true;
                enemyWaves.isInverted = EditorGUILayout.Toggle("Inverted", enemyWaves.isInverted);
                enemyWaves.amplitude = EditorGUILayout.FloatField("Amplitude", enemyWaves.amplitude);
                enemyWaves.frequency = EditorGUILayout.FloatField("Frequency", enemyWaves.frequency);
                enemyWaves.offset = EditorGUILayout.FloatField("Offset", enemyWaves.offset);
                enemyWaves.moveTypeFlag = 2;
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
