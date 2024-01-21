using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class Queue : MonoBehaviour {
    public static readonly int QUEUE_MAX_SIZE = 4;
    [SerializeField]
    private List<PNJ> allPnjs;
    public List<PNJ> pnjs;
    private readonly HashSet<string> runnableDialogs = new();

    public void Initialize() {
        // Fill queue with basic PNJ
        runnableDialogs.AddRange(allPnjs.SelectMany(pnj => pnj.possibleDialogs.FindAll(diag => diag.availableByDefault)).Select(diag => diag.name));
        for (int i = 0; i < QUEUE_MAX_SIZE; i++) AddPNJToQueue();
    }

    private void AddPNJToQueue() {
        // Add a PNJ to the queue, depending on which dialogs have already been drawn
        List<PNJ> pnjList = allPnjs.FindAll(pnj => pnj.possibleDialogs.Find(diag => runnableDialogs.Contains(diag.name)));
        pnjs.Add(pnjList[Random.Range(0, pnjList.Count)]);
    }

    public PNJ PopPNJ() {
        // Remove next pnj and adds a new one to the end of the queue
        PNJ chosenPNJ = pnjs[0];
        pnjs.RemoveAt(0);
        AddPNJToQueue();
        return chosenPNJ;
    }

    public Dialog ChooseDialog(PNJ pnj) {
        List<Dialog> possibleDialogs = pnj.possibleDialogs.FindAll(diag => runnableDialogs.Contains(diag.name));
        Dialog dialog = possibleDialogs[Random.Range(0, possibleDialogs.Count)];
        runnableDialogs.AddRange(dialog.nextDialogs.Select(diag => diag.name));
        return dialog;
    }

    public void Kick(int position) {
        if (position < pnjs.Count && position > 0) pnjs.RemoveAt(position);
        AddPNJToQueue();
    }

}