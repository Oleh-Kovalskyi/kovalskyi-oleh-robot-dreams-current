using UnityEngine;

public class RayGizmoDebugger : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private Vector3 origin = Vector3.zero;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private LayerMask rayMask = ~0; //


    [SerializeField] private Color hitColor = Color.green;
    [SerializeField] private Color missColor = Color.red;
    [SerializeField] private float hitPointSphereRadius = 0.1f;

    private void OnDrawGizmos()
    {
        if (cameraTransform != null)
        {
            origin = cameraTransform.position;
            direction = cameraTransform.forward;
        }

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask))
        {
            Gizmos.color = hitColor;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawWireSphere(hit.point, hitPointSphereRadius);
        }
        else
        {
            Gizmos.color = missColor;
            Gizmos.DrawLine(origin, origin + direction.normalized * rayDistance);
        }
    }
}