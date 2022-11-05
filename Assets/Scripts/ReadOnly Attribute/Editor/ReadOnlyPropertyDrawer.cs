using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //If the property is a custom class with no attributes, no need to draw it
        bool isEmpty = property.depth == 0 && !property.hasChildren;

        if (!isEmpty)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //If the property is a custom class with no attributes, no need to draw it
        bool isEmpty = property.depth == 0 && !property.hasChildren;
        if (isEmpty) return 0;
        else return base.GetPropertyHeight(property, label) * (property.isExpanded ? property.CountInProperty()+.9f : 1);  // assuming original is one row
    }
}
