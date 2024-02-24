using Audune.Utils.UnityEditor;
using System;
using UnityEngine;

namespace Audune.Utils.Dictionary
{
  // Attribute that defines options for drawing a serializable dictionary
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class SerializableDictionaryOptionsAttribute : PropertyAttribute
  {
    // Text for the key header of the dictionary
    public string keyHeader { get; set; } = "Key";

    // Text for the value header of the dictionary
    public string valueHeader { get; set; } = "Value";

    // Options to draw the underlying reorderable list
    public ReorderableListOptions listOptions { get; set; } = ReorderableListOptions.DrawFoldout | ReorderableListOptions.DrawInfoField;
  }
}