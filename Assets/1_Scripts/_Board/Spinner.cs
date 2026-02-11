using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{
    [Header("Gameplay Related")]
    public int minMovementPoint = 1;
    public int maxMovementPoint = 4;

    [Header("Number Related")]
    public RectTransform wheel;
    public int numberFontSize = 100;
    public Color numberFontColor = Color.black;
    public float numberWheelRadius;

    [Header("Wheel Anim Related")]
    public AnimationCurve spinAnimCurveStart;
    public AnimationCurve spinAnimCurveEnd;
    public float startingSpinDuration;
    public float endingSpinDuration;


    void Start()
    {
        GenerateNumberOnWheel();
        SpinWheel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateNumberOnWheel()
    {
        GameObject lNumberObject;
        TextMeshProUGUI lNumberText;
        RectTransform lNumberRectTransform;

        float lAngle = 0;

        for (int i = 0; i < maxMovementPoint; i++)
        {
            lNumberObject = new GameObject("Number " + i.ToString());
            lNumberText = lNumberObject.AddComponent<TextMeshProUGUI>();
            lNumberRectTransform = lNumberObject.GetComponent<RectTransform>();

            lNumberText.text = (i + 1).ToString();
            lNumberText.fontSize = numberFontSize;
            lNumberText.color = numberFontColor;
            lNumberText.alignment = TextAlignmentOptions.Center;

            lNumberObject.transform.SetParent(wheel, false);

            lAngle = (90f - (i * 360f / maxMovementPoint)) * Mathf.Deg2Rad;

            lNumberRectTransform.sizeDelta = Vector2.one * numberFontSize * 1.5f;

            lNumberRectTransform.anchoredPosition = new Vector3(
                Mathf.Cos(lAngle) * numberWheelRadius,
                Mathf.Sin(lAngle) * numberWheelRadius, 0);

            lNumberRectTransform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * lAngle) - 90f);
        }
    }

    public void SpinWheel()
    {
        int lRandomMovementPoint = Random.Range(minMovementPoint, maxMovementPoint);
        StartCoroutine(WheelAnimation());
    }

    private IEnumerator WheelAnimation()
    {
        float lRatio = 0;
        float lElaspedTime = 0;

        //Quaternion lStartRot = wheel.rotation;
        //Quaternion lEndRot = wheel.rotation * Quaternion.Euler(0, 0, 360f * 6f);

        float lStartRot = wheel.localEulerAngles.z;
        float lEndRot = lStartRot + (360f * 6f);

        while (lElaspedTime < startingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / startingSpinDuration;

            //wheel.rotation = Quaternion.Slerp(lStartRot, lEndRot, spinAnimCurveStart.Evaluate(lRatio));

            float lCurrentZ = Mathf.Lerp(lStartRot, lEndRot, spinAnimCurveStart.Evaluate(lRatio));
            wheel.localEulerAngles = new Vector3(0, 0, lCurrentZ);

            yield return null;
        }

        //wheel.rotation = lEndRot;
        wheel.localEulerAngles = new Vector3(0, 0, lEndRot);

        float lRotationSpeed = (360f * 6f) / startingSpinDuration;
        float lCurrentRotation = lEndRot;
        bool lStopWheelRequested = false;

        while (!lStopWheelRequested)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                lStopWheelRequested = true;
                break;
            }

            lCurrentRotation += lRotationSpeed * Time.deltaTime;
            wheel.localEulerAngles = new Vector3 (0, 0, lCurrentRotation);

            yield return null;
        }


        yield return null;
    }
}
