using UnityEngine;

public class STJFinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.GetInstance().WinGame(GameManager.GetInstance().GetCurrentPlayerTurn());
        }
    }
}
