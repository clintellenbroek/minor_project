using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
            cam = FindObjectOfType<Camera>();
    }

    void LateUpdate()
    {
        if (cam == null) return;
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
    }
}