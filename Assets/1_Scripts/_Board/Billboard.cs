using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform _Camera;
    void Start()
    {
        _Camera = CameraManager.Instance.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = _Camera.rotation;
    }
}
