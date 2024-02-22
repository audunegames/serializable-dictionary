using System;
using UnityEngine;

namespace Audune.Utils.Dictionary
{
  // Attribute that defines options for drawer a serializable dictionary
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class SerializableDictionaryOptionsAttribute : PropertyAttribute
  {
    // Text for the key header of the reorderable list
    public string keyHeader { get; set; } = "Key";

    // Text for the value header of the reorderable list
    public string valueHeader { get; set; } = "Value";
  }
}