using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorAnimation : MonoBehaviour
{
    public GameObject m_Manager;
    public Image damagePartsImg;
    public Image paintedPartsImg;
    public Image maxSpeedImg;
    public Image torqueImg;

    public Color lightPurple = new Color(0.8f, 0.4f, 0.8f);
    public Color darkPurple = new Color(0.4f, 0.0f, 0.4f);

    public float colorChangeSpeed = 1.0f;
    public bool isAnimationRunning;
    

    private ColorAnimationControl colorAnimationControl = new ColorAnimationControl();

    private void Update()
    {
        if (m_Manager.gameObject.GetComponent<MrSellerManager>().BttnSelectCar != null) { StartColorChange(); }else { StopColorChange(); }
    }
    public void StartColorChange()
    {
        Image[] images = { damagePartsImg, paintedPartsImg, maxSpeedImg, torqueImg };
        this.StartColorChangeAnimation(images, lightPurple, darkPurple, colorChangeSpeed, colorAnimationControl);
    }

    public void StopColorChange()
    {
        colorAnimationControl.IsRunning = false;
        ResetColors();
    }

    private void ResetColors()
    {
        Image[] images = { damagePartsImg, paintedPartsImg, maxSpeedImg, torqueImg };
        foreach (Image image in images)
        {
            image.color = lightPurple;
        }
    }
}
