using Audune.Utils.UnityEditor;
using Audune.Utils.UnityEditor.Editor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.Dictionary.Editor
{
  // Class that defines a property drawer for a serializable dictionary
  [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
  public class SerializableDictionaryDrawer : PropertyDrawer
  {
    // Delegate for a callback that is invoked when the window should add an element to the serialized dictionary
    public delegate void ElementAddCallback(SerializedProperty key, SerializedProperty value);


    // Reorderable list for the entries
    private ReorderableList _entriesList;

    // List of duplicated keys in the entries
    private List<object> _duplicatedKeys;


    // Draw the property GUI
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      using var scope = new EditorGUI.PropertyScope(rect, label, property);

      var entries = property.FindPropertyRelative("_entries");
      var options = fieldInfo.GetCustomAttribute<SerializableDictionaryOptionsAttribute>() ?? new SerializableDictionaryOptionsAttribute();
      if (_entriesList == null || _entriesList.serializedProperty != property)
        InitializeEntriesList(entries, options);

      _entriesList.DoList(rect, options.listOptions, scope.content);
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var entries = property.FindPropertyRelative("_entries");
      var options = fieldInfo.GetCustomAttribute<SerializableDictionaryOptionsAttribute>() ?? new SerializableDictionaryOptionsAttribute();
      if (_entriesList == null || _entriesList.serializedProperty != property)
        InitializeEntriesList(entries, options);

      return _entriesList.GetHeight(options.listOptions);
    }


    // Return the duplicated keys in an array property
    private List<object> FindDuplicates(SerializedProperty property)
    {
      return property.GetArrayElements()
        .Select(property => property.FindPropertyRelative("key"))
        .GroupBy(e => e.boxedValue)
        .Where(g => g.Count() > 1)
        .Select(g => g.Key)
        .ToList();
    }

    // Initialize the reorderable list for the entries
    private void InitializeEntriesList(SerializedProperty property, SerializableDictionaryOptionsAttribute options)
    {
      if (_entriesList == null)
      {
        _entriesList = new ReorderableListBuilder()
          .SetDraggable(false)
          .SetHeaderDrawer(EntriesListHeaderDrawer(options))
          .SetElementDrawer(EntriesListElementDrawer(), EntriesListElementDrawerHeight())
          .Create(property.serializedObject, property);
        _duplicatedKeys = FindDuplicates(property);
      }
      else
      {
        _entriesList.serializedProperty = property;
        _duplicatedKeys = FindDuplicates(property);
      }
    }

    // Return a header drawer function
    private ReorderableListBuilder.HeaderDrawer EntriesListHeaderDrawer(SerializableDictionaryOptionsAttribute options)
    {
      return (list, rect) => {
        EditorGUI.LabelField(rect.AlignLeft(0.5f * (rect.width - EditorGUIUtility.standardVerticalSpacing)), new GUIContent(options.keyHeader));
        EditorGUI.LabelField(rect.AlignRight(0.5f * (rect.width - EditorGUIUtility.standardVerticalSpacing)), new GUIContent(options.valueHeader));
      };
    }

    // Return an element drawer function for an entry
    private ReorderableListBuilder.ElementDrawer EntriesListElementDrawer()
    {
      return (list, rect, element, index) => {
        var key = element.FindPropertyRelative("key");
        var value = element.FindPropertyRelative("value");

        var keyRect = rect.AlignLeft(0.5f * (rect.width - EditorGUIUtility.standardVerticalSpacing));
        var valueRect = rect.AlignRight(0.5f * (rect.width - EditorGUIUtility.standardVerticalSpacing));

        if (_duplicatedKeys.Contains(key.boxedValue))
        {
          var warningRect = keyRect.AlignLeft(16.0f, EditorGUIUtility.standardVerticalSpacing, out keyRect);
          EditorGUI.LabelField(warningRect, new GUIContent("⚠️"));
        }

        EditorGUI.PropertyField(keyRect, key, GUIContent.none);
        EditorGUI.PropertyField(valueRect, value, GUIContent.none);
      };
    }

    // Return an element drawer height function for an entry
    private ReorderableListBuilder.ElementDrawerHeight EntriesListElementDrawerHeight()
    {
      return (list, element, index) => {
        var key = element.FindPropertyRelative("key");
        var value = element.FindPropertyRelative("value");

        return Mathf.Max(EditorGUI.GetPropertyHeight(key), EditorGUI.GetPropertyHeight(value));
      };
    }
  }
}