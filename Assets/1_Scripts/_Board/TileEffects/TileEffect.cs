using System.Collections;
using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    [SerializeField] protected string m_EffectMessage = "";
    [SerializeField] protected Material m_TileMaterial;
    protected Material m_OriginalMat;
    protected Coroutine m_CurrentExecuteCoroutine;

    void OnDestroy()
    {
#if UNITY_EDITOR
        RestoreOriginalMat();
#endif
    }


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


    public void ChangeColor(Material pTileMat)
    {
        BoardTile BoardTile = GetComponent<BoardTile>();

        if (BoardTile == null)
        {
            Debug.Log("BoardTile null");
            return;
        }

        BoardTile.ChangeColor(pTileMat);
    }

    void RestoreOriginalMat()
    {
        m_OriginalMat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/NullTileEffectColor.mat");

        BoardTile tile = GetComponent<BoardTile>();
        if (tile == null || m_OriginalMat == null) return;

        tile.ChangeColor(m_OriginalMat);
    }
}
