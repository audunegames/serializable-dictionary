# Audune Serializable Dictionary

[![openupm](https://img.shields.io/npm/v/com.audune.utils.dictionary?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.audune.utils.dictionary/)

Since Unity doesn't serialize dictionary classes by default, usually some serialization workaround is required to acoomplish serializing its data. This package contains a serializable dictionary class that takes away the hassle of defining custom classes.

## Features

* A `SerializableDictionary<TKey, TValue>` class to use in place of the C# `Dictionary<TKey, TValue>` class, but with the benefit of the data being serialized in Unity.
* Some dictionary-related LINQ extension methods.

## Installation

## Requirements

This package depends on the following packages:

* [UnityEditor Utilities](https://openupm.com/packages/com.audune.utils.unityeditor/), version **1.0.1** or higher.

If you're installing the required packages from the [OpenUPM registry](https://openupm.com/), make sure to add a scoped registry with the URL `https://package.openupm.com` and the required scopes before installing the packages.

## Installing from the OpenUPM registry

To install this package as a package from the OpenUPM registry in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Edit › Project Settings... › Package Manager**.
* Add the following Scoped Registry, or edit the existing OpenUPM entry to include the new Scope:

```
Name:     package.openupm.com
URL:      https://package.openupm.com
Scope(s): com.audune.utils.dictionary
```

* Navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package by name...**
* Enter the following name in the corresponding field and click **Add**:

```
com.audune.utils.dictionary
```

## Installing as a Git package

To install this package as a Git package in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/serializabledictionary.git
```

## Usage

### Serializable dictionary

To use the serialized dictionary class, simply make a serialized field in a Unity component. The inspector draws a reorderable list with the contents of the dictionary:

```csharp
using Audune.Utils.Dictionary;

// It's as easy as this!
public SerializableDictionary<int, string> dictionary;
```

You can also define options that customize how the dictionary is displayed in the inspector using a property attribute:

```csharp
using Audune.Utils.Dictionary;

// Custom headers for the keys and values
[SerializableDictionaryOptions(keyHeader = "Priority", valueHeader = "String")]
public SerializableDictionary<int, string> dictionary;

// Draw the reorderable list in a foldout and include a label with info about the dictionary (this is the default)
[SerializableDictionaryOptions(listOptions = ReorderableListOptions.DrawFoldout | ReorderableListOptions.DrawInfoField)]
public SerializableDictionary<int, string> dictionary;

// Don't draw a foldout or an info label
[SerializableDictionaryOptions(listOptions = ReorderableListOptions.None)]
public SerializableDictionary<int, string> dictionary;
```

Because the serialized dictionary class inherits from both `IDictionary<TKey, TValue>` and `IReadOnlyDictionary<TKey, TValue>`, access of properties and methods works just like a regular dictionary, and a reference to the serializable dictionary can be used almost in every place where one of those interfaces are expected.

### Extension methods

Selecting keys or values or mapping them in LINQ operations has never been easier! These extension methods work on serializable dictionaries or everything that inherits from `IEnumerable<KeyValuePair<TKey, TValue>>`:

```csharp
// Convert an enumerable of dictionary entries to a dictionary without providing selectors
var newDictionary = dictionary.SelectOnKey(key => key * 2).ToDictionary();
// ... is equivalent to ...
var newDictionary = dictionary.Select(e => new KeyValuePair<int, string>(e.Key * 2, e.Value)).ToDictionary(e => e.Key, e => e.Value);

// Select the key for each dictionary entry
var listOfKeys = dictionary.SelectKey().ToList();
// ... is equivalent to ...
var listOfKeys = dictionary.Select(e => e.Key).ToList();

// Select the value for each dictionary entry
var listOfValues = dictionary.SelectValue().ToList();
// ... is equivalent to ...
var listOfKeys = dictionary.Select(e => e.Value).ToList();

// Select a mapped key for each dictionary entry
var doubledKeys = dictionary.SelectOnKey(key => key * 2).ToDictionary();
// ... is equivalent to ...
var doubledKeys = dictionary.ToDictionary(e => e.Key * 2, e => e.Value);

// Select a mapped value for each dictionary entry
var boldValues = dictionary.SelectOnValue(value => $"<b>{value}</b>").ToDictionary();
// ... is equivalent to ...
var doubledKeys = dictionary.ToDictionary(e => e.Key, e => $"<b>{e.Value}</b>");
```

## Contributing

Contributions to this package are more than welcome! Contributing can be done by making a pull request with your updated code.

## License

This package is licensed under the GNU GPL 3.0 license. See `LICENSE.txt` for more information.
