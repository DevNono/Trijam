using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedKickDialog : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject canvas;
    public float fadeOutDuration = 0.2f;

    public void SpawnPrefabAtPosition(Vector3 spawnPosition)
    {
        if (prefabToSpawn != null && canvas != null)
        {
            // Instantiate the UI prefab at the specified position on the Canvas
            GameObject uiPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            
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

            // Start the fade-out animation
            StartCoroutine(FadeOutAndDestroy(uiChildToDelete, fadeOutDuration));
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

    IEnumerator FadeOutAndDestroy(GameObject uiObj, float duration)
    {
        Graphic graphic = uiObj.GetComponent<Graphic>();

        if (graphic != null)
        {
            Color startColor = graphic.color;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
                graphic.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the UI element is fully faded out
            graphic.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

            // Destroy the object after fade-out
            Destroy(uiObj);
        }
        else
        {
            Debug.LogWarning("Graphic component not found on the UI object.");
        }
    }
}
