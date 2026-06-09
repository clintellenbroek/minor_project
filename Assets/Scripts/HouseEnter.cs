using UnityEngine;

public class HouseEnter : MonoBehaviour
{

    public GameObject roof;

    private void OnTriggerEnter(Collider other)
    {
        roof.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        roof.SetActive(true);
    }
}
