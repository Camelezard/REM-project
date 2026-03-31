using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Spinner : MonoBehaviour
{
    public static Spinner instance;
    public static event Action<int> OnSpinnerStopAtNumber;


    [Header("Gameplay Related")]
    public int minMovementPoint = 1;
    public int maxMovementPoint = 4;

    [Header("Number Related")]
    public bool generateNumbers = false;
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
        instance = this;
        GenerateNumberOnWheel();
        EnhancedTouchSupport.Enable();
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
            lAngle = (90f - (i * 360f / maxMovementPoint)) * Mathf.Deg2Rad;
            angles[i] = -(lAngle * Mathf.Rad2Deg - 90f);

            if (generateNumbers)
            {
                lNumberObject = new GameObject("Number " + i.ToString());
                lNumberText = lNumberObject.AddComponent<TextMeshProUGUI>();
                lNumberRectTransform = lNumberObject.GetComponent<RectTransform>();

                lNumberText.text = (i + 1).ToString();
                lNumberText.fontSize = numberFontSize;
                lNumberText.color = numberFontColor;
                lNumberText.alignment = TextAlignmentOptions.Center;

                lNumberObject.transform.SetParent(wheel, false);

                lNumberRectTransform.sizeDelta = Vector2.one * numberFontSize * 1.5f;

                lNumberRectTransform.anchoredPosition = new Vector3(
                    Mathf.Cos(lAngle) * numberWheelRadius,
                    Mathf.Sin(lAngle) * numberWheelRadius, 0);

                lNumberRectTransform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * lAngle) - 90f);
            }
            
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

        float lStartRot = wheel.localEulerAngles.z;
        float lEndRot = lStartRot + (360f * 6f);

        while (lElaspedTime < startingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / startingSpinDuration;

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
            if (DetectSingleTouch())
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
        lEndRot = angles[pValueToStopAt - 1] + 360f * 6f + Random.Range(-25f, 25f);

        while (lElaspedTime < endingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / endingSpinDuration;

            lCurrentZ = Mathf.Lerp(lStartRot, lEndRot, spinAnimCurveEnd.Evaluate(lRatio));
            wheel.localEulerAngles = new Vector3(0,0, lCurrentZ);

            yield return null;
        }

        wheel.localEulerAngles = new Vector3(0, 0, lEndRot);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        OnSpinnerStopAtNumber?.Invoke(pValueToStopAt);

        yield return null;
    }

    private bool DetectSingleTouch()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                return true;
            }
            //else return false;
        }
        return false;
    }
}
