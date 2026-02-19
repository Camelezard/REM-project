using System.Collections;
using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    [SerializeField] protected string m_EffectMessage = "";

    protected Coroutine m_CurrentExecuteCoroutine;
    /// <summary>
    /// Launch the code of this effect
    /// </summary>
    /// <param name="pPawn"></param>
    /// 
    public void Execute(Pawn pPawn)
    {
        if (m_CurrentExecuteCoroutine != null) StopCoroutine(m_CurrentExecuteCoroutine);
        
        m_CurrentExecuteCoroutine = StartCoroutine(ExecuteCoroutine(pPawn));
    }

    protected IEnumerator ExecuteCoroutine(Pawn pPawn)
    {
        if (EffectMessageManager.Instance != null && !string.IsNullOrEmpty(m_EffectMessage))
        {
            EffectMessageManager.Instance.ShowMessage(m_EffectMessage);
        }

        yield return new WaitForSeconds(EffectMessageManager.Instance.totalDuration);

        ExecuteEffect(pPawn);
    }

    protected abstract void ExecuteEffect(Pawn pPawn);

    public virtual string GetDescription()
    {
        return "Tile effect is...";
    }
}
