using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dialog", menuName = "trijam/Dialog", order = 0)]
public class Dialog : ScriptableObject {
    public string text;
    public bool availableByDefault = false;
    public DialogType dialogType;
    public List<Dialog> nextDialogs;
    public Dictionary<ResourceType, (int, int)> resourcesCosts = new Dictionary<ResourceType, (int, int)>();
}