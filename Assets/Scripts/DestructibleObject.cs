using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DestructibleObject : MonoBehaviour
{
    public void DestroyObject()
    {
        Debug.Log($"{gameObject.name} разрушен.");
        Destroy(gameObject);
    }
}
