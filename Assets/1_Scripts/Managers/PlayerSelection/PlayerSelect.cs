using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] private PlayerSelectManger _PlayerManager = null;
    private Player _SelfPlayer = null;
    private bool _IsReady = false;


    public void OnRedyClick()
    {
        if (!_PlayerManager)
        {
            Debug.Log("_PlayerManager not refered");
            return;
        }

        if (!_IsReady)
        {
            _PlayerManager.AddPlayer(_SelfPlayer);
        }
        else
        {
            _PlayerManager.RemovePlayer(_SelfPlayer);
        }

        _IsReady = !_IsReady;
    }
}
