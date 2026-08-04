using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smothTime = 0.15f;
    
    private Vector3 _velocity;

    private void LateUpdate()
    {
        if (!target)
        {
            return;
        }
        
        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z);
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _velocity,
            smothTime);
    }
}
