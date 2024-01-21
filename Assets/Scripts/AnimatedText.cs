using System.Collections;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float letterAppearDelay = 0.1f;
    public Color highlightedColor = Color.blue;
    public string currentUUID = "";

    // Method to start the text animation
    public void StartTextAnimation(string fullText)
    {
        StartCoroutine(AnimateText(fullText));
    }

    IEnumerator AnimateText(string fullText)
    {
        // Generate a unique id for this coroutine
        string UUID = currentUUID = System.Guid.NewGuid().ToString();
        textMeshPro.text = ""; // Clear the text initially

        string[] words = fullText.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            if (UUID != currentUUID)
                yield break;
            if (i > 0)
                textMeshPro.text += " "; // Add space between words

            if (words[i].StartsWith("<color="))
            {
                int endIndex = words[i].IndexOf('>');
                string coloredWord = words[i].Substring(endIndex + 1);
                string colorTag = words[i].Substring(0, endIndex + 1);

                int startIndex = textMeshPro.text.Length;

                textMeshPro.text += colorTag;

                foreach (char letter in coloredWord)
                {
                    if (UUID != currentUUID)
                        yield break; // Stop the coroutine if the UUID has changed
                    if (letter == '<')
                        break;
                    textMeshPro.text += letter;
                    yield return new WaitForSeconds(letterAppearDelay);
                }

                textMeshPro.text += "</color>";
            }
            else
            {
                foreach (char letter in words[i])
                {
                    if (UUID != currentUUID)
                        yield break; // Stop the coroutine if the UUID has changed
                    textMeshPro.text += letter;
                    yield return new WaitForSeconds(letterAppearDelay);
                }
            }
        }
    }
}
