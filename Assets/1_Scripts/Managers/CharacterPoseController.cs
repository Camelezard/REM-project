using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using UnityEngine.U2D.Animation;

public enum PoseEnum { Idle, Joy, Deception, PoseOne, PoseTwo, PoseTree }
public enum ExpresionEnum { Neutral, Joy, Sob, Effort }

public class CharacterPoseController : MonoBehaviour
{
    private const string IdleTrigger = "TrIdle";
    private const string JoyTrigger = "TrJoy";
    private const string DeceptionTrigger = "TrDeception";
    private const string PoseOneTrigger = "TrDanceOne";
    private const string PoseTwoTrigger = "TrDanceTwo";
    private const string PoseTreeTrigger = "TrDanceTree";

    private Coroutine currentPoseRoutine;
    private Coroutine currentFaceRoutine;

    [SerializeField] public Animator animator;
    [SerializeField] public SpriteResolver Face;


    // positions
    public void PlayOneShotPose(PoseEnum pPoseTrigger, float pDuration)
    {
        if (currentPoseRoutine != null)
        {
            StopCoroutine(currentPoseRoutine);
        }

        //print(pPoseTrigger);

        currentPoseRoutine = StartCoroutine(PoseRoutine(pPoseTrigger, pDuration));
    }

    public void PlayPermanentPose(PoseEnum pPoseTrigger)
    {
        if (currentPoseRoutine != null)
        {
            StopCoroutine(currentPoseRoutine);
        }
        animator.SetTrigger(PoseEmum2StringLabel(pPoseTrigger));
    }

    private IEnumerator PoseRoutine(PoseEnum pPose, float pDuration)
    {
        string lposeTrigger = PoseEmum2StringLabel(pPose);

        animator.SetTrigger(lposeTrigger);

        yield return new WaitForSeconds(pDuration);

        animator.SetTrigger(IdleTrigger);
        currentPoseRoutine = null;

        print("time out"); 
    }
    

    // Face expresions
    public void SetFaceExpressionOneShot(ExpresionEnum pExpression, float pDuration)
    {
        if (currentFaceRoutine != null)
        {
            StopCoroutine(currentFaceRoutine);
        }

        currentFaceRoutine = StartCoroutine(FaceRoutine(pExpression, pDuration));
    }


    private IEnumerator FaceRoutine(ExpresionEnum pExpression, float pDuration)
    {
        Face.SetCategoryAndLabel("FaceExpression", ExpersionEmum2StringLabel(pExpression));

        yield return new WaitForSeconds(pDuration);

        Face.SetCategoryAndLabel("FaceExpression", ExpersionEmum2StringLabel(ExpresionEnum.Neutral));

        currentFaceRoutine = null;
    }

    public void SetFaceExpressionPermanent(ExpresionEnum pExpression)
    {
        if (currentFaceRoutine != null)
        {
            StopCoroutine(currentFaceRoutine);
            currentFaceRoutine = null;
        }

        Face.SetCategoryAndLabel("FaceExpression", ExpersionEmum2StringLabel(pExpression));
    }


    // gets
    private string ExpersionEmum2StringLabel(ExpresionEnum pExpresion)
    {
        string exspresionLabel;
        switch (pExpresion)
        {
            case ExpresionEnum.Neutral: exspresionLabel = "Face_Default"; break;
            case ExpresionEnum.Sob: exspresionLabel = "Face_Sob"; break;
            case ExpresionEnum.Joy: exspresionLabel = "Face_Joy"; break;
            case ExpresionEnum.Effort: exspresionLabel = "Face_Effort"; break;

            default: exspresionLabel = "Face_Default"; break;
        }
        return exspresionLabel;
    }


    private string PoseEmum2StringLabel(PoseEnum pPose)
    {
        string lposeTrigger = IdleTrigger;
        switch (pPose)
        {
            case PoseEnum.Idle:
                lposeTrigger = IdleTrigger;
                break;

            case PoseEnum.Joy:
                lposeTrigger = JoyTrigger;
                break;

            case PoseEnum.Deception:
                lposeTrigger = DeceptionTrigger;
                break;

            case PoseEnum.PoseOne:
                lposeTrigger = PoseOneTrigger;
                break;

            case PoseEnum.PoseTwo:
                lposeTrigger = PoseTwoTrigger;
                break;

            case PoseEnum.PoseTree:
                lposeTrigger = PoseTreeTrigger;
                break;
        }

        return lposeTrigger;
    }
}
