using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dialog", menuName = "trijam/Dialog", order = 0)]
public class Dialog : ScriptableObject {

    [Serializable]
    public class ResourceCostEntry {

        [Serializable]
        public class Pair<T> {
            public T Option1;
            public T Option2;
        }

        public ResourceType key;
        public Pair<int> values;
    }

    public string text;
    public bool availableByDefault = false;
    public DialogType dialogType;
    public List<Dialog> nextDialogs;
    public ResourceCostEntry[] resourcesCosts;
}