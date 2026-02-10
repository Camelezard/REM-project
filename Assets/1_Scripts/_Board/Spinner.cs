using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        Quaternion lStartRot = wheel.localRotation;
        Quaternion lEndRot = wheel.localRotation * Quaternion.Euler(0, 0, 360f * 6f);

        while (lElaspedTime < startingSpinDuration)
        {
            lElaspedTime += Time.deltaTime;
            lRatio = lElaspedTime / startingSpinDuration;

            wheel.localRotation = Quaternion.Slerp(lStartRot, lEndRot, spinAnimCurveStart.Evaluate(lRatio));
            yield return null;
        }

        wheel.localRotation = lEndRot;

        yield return null;
    }
}
