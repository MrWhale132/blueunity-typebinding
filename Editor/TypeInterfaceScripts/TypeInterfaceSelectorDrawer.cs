using System.Collections.Generic;
using Theblueway.TypeBinding.Runtime.TypeInterfaceScripts;
using Theblueway.Core.Editor.EditorWindows;
using UnityEditor;
using UnityEngine;

namespace Theblueway.TypeBinding.Editor.TypeInterfaceScripts
{
    [CustomPropertyDrawer(typeof(TypeInterfaceSelectorAttribute))] 
    public class TypeInterfaceSelectorDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IEnumerable<TypeInterfaceInfo> typeInterfaces = TypeInterface.GetTypeInterfaceInfos();

            var currentInterfaceInfo = TypeInterface.GetTypeInterfaceInfo(property.longValue);

            EditorGUI.BeginProperty(position, label, property);

            Rect fieldRect = EditorGUI.PrefixLabel(position, label);

            // Detect click BEFORE drawing anything that may consume the event
            if (Event.current.type == EventType.MouseDown && fieldRect.Contains(Event.current.mousePosition))
            {
                DropDownWindow.Open(
                    options: typeInterfaces,
                    currentValue: currentInterfaceInfo,
                    newValue =>
                    {
                        property.longValue = newValue.Id;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    tostring:TypeInterfaceInfoTextDisplay
                    );

                Event.current.Use(); // swallow the click
            }

            // Draw a non-editable field
            using (new EditorGUI.DisabledScope(true))
            {
                string text = "";
                if(property.longValue == 0)
                {
                    text = "click to select (unassigned)";
                }
                else
                {
                    text = property.longValue.ToString();
                }

                EditorGUI.TextField(fieldRect, text, EditorStyles.popup);
            }

            EditorGUI.EndProperty();
        }


        public static string TypeInterfaceInfoTextDisplay(TypeInterfaceInfo info)
        {
            return $"{info.Id} - {info.HandledType.Name} - {info.HandledType.Assembly.GetName().Name} - {info.HandledType.Namespace}";
        }
    }
}
