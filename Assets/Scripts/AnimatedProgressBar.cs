using UnityEngine;
using UnityEngine.UI;

public class AnimatedProgressBar : MonoBehaviour
{
    public Image progressBarBackground;
    public Image progressBarFill;

    public float animationSpeed = 5.0f;

    private float currentProgress = 0.0f;
    private float targetProgress = 0.0f;

    void Update()
    {
        // Update the progress animation
        currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * animationSpeed);
        
        // Update the fill image's fillAmount based on the current progress
        progressBarFill.fillAmount = currentProgress;
    }

    // Method to add progress to the bar
    public void AddProgress(float amount)
    {
        SetProgress(currentProgress + amount);
    }

    // Method to set the progress to a specific value
    public void SetProgress(float value)
    {
        targetProgress = Mathf.Clamp01(value);
    }
}