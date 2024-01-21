using System.Collections;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float letterAppearDelay = 0.1f;
    public Color highlightedColor = Color.blue;

    // Method to start the text animation
    public void StartTextAnimation(string fullText)
    {
        StartCoroutine(AnimateText(fullText));
    }

    IEnumerator AnimateText(string fullText)
    {
        textMeshPro.text = ""; // Clear the text initially

        string[] words = fullText.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
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
                    if (letter == '<')
                        break;
                    textMeshPro.text += letter;
                    yield return new WaitForSeconds(letterAppearDelay);
                }

                textMeshPro.text += "</color>";

                // // Highlight the colored word
                // textMeshPro.text = textMeshPro.text.Remove(startIndex, colorTag.Length + coloredWord.Length + 7);
                // textMeshPro.text = textMeshPro.text.Insert(startIndex, "<color=" + ColorUtility.ToHtmlStringRGB(highlightedColor) + ">" + coloredWord + "</color>");

            }
            else
            {
                foreach (char letter in words[i])
                {
                    textMeshPro.text += letter;
                    yield return new WaitForSeconds(letterAppearDelay);
                }
            }
        }
    }
}
