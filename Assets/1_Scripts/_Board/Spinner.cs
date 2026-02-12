using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spinner : MonoBehaviour
{
    public event Action<int> OnSpinnerStopAtNumber;

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

    private float[] angles;

    void Start()
    {
        GenerateNumberOnWheel();
        //SpinWheel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateNumberOnWheel()
    {
        angles = new float[maxMovementPoint];
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
            angles[i] = -(lAngle * Mathf.Rad2Deg - 90f);

            lNumberRectTransform.sizeDelta = Vector2.one * numberFontSize * 1.5f;

            lNumberRectTransform.anchoredPosition = new Vector3(
                Mathf.Cos(lAngle) * numberWheelRadius,
                Mathf.Sin(lAngle) * numberWheelRadius, 0);

            lNumberRectTransform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * lAngle) - 90f);
        }
    }

    public int SpinWheel()
    {
        int lRandomMovementPoint = Random.Range(minMovementPoint, maxMovementPoint);
        Debug.Log(angles[lRandomMovementPoint - 1] + "  --  " + lRandomMovementPoint);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        StartCoroutine(WheelAnimation(lRandomMovementPoint));
        return lRandomMovementPoint;
    }

    private IEnumerator WheelAnimation(int pValueToStopAt)
    {
        float lRatio = 0;
        float lElaspedTime = 0;
        float lCurrentZ = 0;

        //Quaternion lStartRot = wheel.rotation;
        //Quaternion lEndRot = wheel.rotation * Quaternion.Euler(0, 0, 360f * 6f);

        float lStartRot = wheel.localEulerAngles.z;
        float lEndRot = lStartRot + (360f * 6f);

        while (lElaspedTime < startingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / startingSpinDuration;

            //wheel.rotation = Quaternion.Slerp(lStartRot, lEndRot, spinAnimCurveStart.Evaluate(lRatio));

            lCurrentZ = Mathf.Lerp(lStartRot, lEndRot, spinAnimCurveStart.Evaluate(lRatio));
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
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                lStopWheelRequested = true;
                break;
            }

            lCurrentRotation += lRotationSpeed * Time.deltaTime;
            wheel.localEulerAngles = new Vector3 (0, 0, lCurrentRotation);

            yield return null;
        }

        lElaspedTime = 0;
        lRatio = 0;
        lCurrentZ = 0;

        lStartRot = wheel.localEulerAngles.z;
        lEndRot = angles[pValueToStopAt - 1] + 360f * 6f/* + Random.Range(-20f, 20f)*/;

        while (lElaspedTime < endingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / endingSpinDuration;

            lCurrentZ = Mathf.Lerp(lStartRot, lEndRot, spinAnimCurveEnd.Evaluate(lRatio));
            wheel.localEulerAngles = new Vector3(0,0, lCurrentZ);

            yield return null;
        }

        wheel.localEulerAngles = new Vector3(0, 0, lEndRot);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        OnSpinnerStopAtNumber?.Invoke(pValueToStopAt);

        yield return null;
    }
}
