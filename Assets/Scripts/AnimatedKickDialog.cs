using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedKickDialog : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject canvas;

    public void SpawnPrefab(GameObject pnjToFollow)
    {
        if (prefabToSpawn != null && canvas != null)
        {
            // Instantiate the UI prefab at the specified position on the Canvas
            GameObject uiPrefab = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity) as GameObject;
            uiPrefab.GetComponent<KickButton>().SetPNJToFollow(pnjToFollow);
            // Set the parent to the canvas
            uiPrefab.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Debug.LogWarning("Prefab or Canvas not assigned in the inspector.");
        }
    }

    public void Delete(int indexToDelete)
    {
        if (canvas != null && indexToDelete >= 0 && indexToDelete < canvas.transform.childCount)
        {
            // Get the UI child GameObject at the specified index
            GameObject uiChildToDelete = canvas.transform.GetChild(indexToDelete).gameObject;

            // Destroy the UI child GameObject
            Destroy(uiChildToDelete);
        }
        else
        {
            Debug.LogWarning("Invalid index or Canvas not assigned.");
        }
    }

    public void Move(Vector3 targetPosition, int indexToMove)
    {
        if (canvas != null && indexToMove >= 0 && indexToMove < canvas.transform.childCount)
        {
            // Get the UI child GameObject at the specified index
            GameObject uiChildToMove = canvas.transform.GetChild(indexToMove).gameObject;
            uiChildToMove.GetComponent<RectTransform>().anchoredPosition = targetPosition;
        }
        else
        {
            Debug.LogWarning("Invalid index or Canvas not assigned.");
        }
    }
}
