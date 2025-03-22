using UnityEngine;

namespace Dummies
{
    public class FullBillboard : BillboardBase
    {
        private Transform _cameraTransform;
        private Camera _camera;
        private Transform _transform;
        
        public override void SetCamera(Camera camera)
        {
            _transform = transform;
            _camera = camera;
            _cameraTransform = _camera.transform;
        }

        private void LateUpdate()
        {
            if (_camera == null || _cameraTransform == null)
            {
                Debug.LogError("Camera is NULL! Убедитесь, что в сцене есть активная камера.");
                return;
            }

            Vector3 direction = (_camera.transform.position - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        private void Awake()
        {
            if (Camera.main != null)
            {
                SetCamera(Camera.main);
            }
            else
            {
                Debug.LogError("Camera.main is NULL! Убедитесь, что в сцене есть активная камера.");
            }
        }
    }
}