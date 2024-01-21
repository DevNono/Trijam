using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PNJ", menuName = "trijam/PNJ", order = 0)]
public class PNJ : ScriptableObject {
    public Sprite sprite;
    public PNJType type;
    public List<Dialog> possibleDialogs;
}