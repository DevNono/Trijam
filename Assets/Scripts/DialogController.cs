using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private Day manager;

    public void Accept()
    {
        Debug.Log("Accept");
        manager.EndPNJ(0);
    }

    public void Refuse()
    {
        Debug.Log("Refuse");
        manager.EndPNJ(1);
    }
}
