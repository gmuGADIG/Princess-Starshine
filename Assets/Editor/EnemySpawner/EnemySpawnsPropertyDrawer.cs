using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnemySpawner.EnemySpawn))]
public class EnemySpawnsPropertyDrawer : PropertyDrawer
{
    private static float fieldSizeRatio = 0.66f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position,GUIUtility.GetControlID(FocusType.Passive) ,label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect objectRect = new Rect(position.x, position.y, position.width * fieldSizeRatio, position.height);
        Rect chanceRect = new Rect(position.x + position.width * fieldSizeRatio + 5, position.y, position.width * (1 - fieldSizeRatio) - 5, position.height);

        EditorGUI.PropertyField(objectRect, property.FindPropertyRelative("enemyType"), GUIContent.none);
        EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("probabilityWeight"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
