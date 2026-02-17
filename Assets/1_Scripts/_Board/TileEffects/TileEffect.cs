using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    /// <summary>
    /// Launch the code of this effect
    /// </summary>
    /// <param name="pPawn"></param>
    public abstract void Execute(Pawn pPawn);

    public virtual string GetDescription()
    {
        return "Tile effect is...";
    }
}
