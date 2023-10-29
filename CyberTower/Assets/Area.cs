using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<Transform> objects;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Unit"))
        {
            objects.Add(collider.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Unit"))
        {
            objects.Remove(collider.gameObject.transform);
        }
    }
}
