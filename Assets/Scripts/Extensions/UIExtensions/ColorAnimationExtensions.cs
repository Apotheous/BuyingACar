using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ColorAnimationExtensions 
{
    public static void StartColorChangeAnimation(this MonoBehaviour monoBehaviour, Image[] images, Color lightPurple, Color darkPurple, float colorChangeSpeed, ColorAnimationControl control)
    {
        control.IsRunning = true;
        monoBehaviour.StartCoroutine(ChangeColorOverTime(images, lightPurple, darkPurple, colorChangeSpeed, control));
    }

    private static IEnumerator ChangeColorOverTime(Image[] images, Color lightPurple, Color darkPurple, float colorChangeSpeed, ColorAnimationControl control)
    {
        while (control.IsRunning)
        {
            for (float t = 0; t < 1; t += Time.deltaTime * colorChangeSpeed)
            {
                if (!control.IsRunning) yield break;
                Color currentColor = Color.Lerp(lightPurple, darkPurple, t);
                ApplyColorToImages(images, currentColor);
                yield return null;
            }

            for (float t = 1; t > 0; t -= Time.deltaTime * colorChangeSpeed)
            {
                if (!control.IsRunning) yield break;
                Color currentColor = Color.Lerp(lightPurple, darkPurple, t);
                ApplyColorToImages(images, currentColor);
                yield return null;
            }
        }
        ResetColors(images, lightPurple);

    }

    private static void ApplyColorToImages(Image[] images, Color color)
    {
        foreach (Image image in images)
        {
            image.color = color;
        }
    }

    private static void ResetColors(Image[] images, Color resetColor)
    {
        foreach (Image image in images)
        {
            image.color = resetColor;
        }
    }
}

public class ColorAnimationControl
{
    public bool IsRunning { get; set; }
}

