using UnityEngine;

namespace Shooting
{
    public class GunAimer : MonoBehaviour
    {
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance = 50f;
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private float _aimSpeed = 5f;
        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private Transform _muzzleFlashPoint;
        [SerializeField] private GameObject _hitEffectPrefab;
        private RaycastHit _lastHitInfo;
        private bool _isHit;
        private Vector3 _hitPointFromCamera; // hitPoint1
        private Vector3 _finalShotPoint;     // hitPoint2
        private float _aimValue;
        private float _targetAimValue;

        public Vector3 AimPoint => _finalShotPoint;

        private void OnEnable()
        {
            PhysX.InputController.OnSecondaryInput += SecondaryInputHandler;
        }

        private void OnDisable()
        {
            PhysX.InputController.OnSecondaryInput -= SecondaryInputHandler;
        }

        private void FixedUpdate()
        {
            
            // 1. Луч из камеры
            Ray camRay = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPointFromCamera = _cameraTransform.position + _cameraTransform.forward * _rayDistance;

            if (Physics.Raycast(camRay, out RaycastHit camHit, _rayDistance, _rayMask))
            {
                _hitPointFromCamera = camHit.point;
            }

            // 2. Повернуть пушку на первую точку
            _gunTransform.LookAt(_hitPointFromCamera);

            // 3. Луч из ствола вперёд
            Vector3 muzzlePosition = _gunTransform.position;
            Vector3 muzzleForward = _gunTransform.forward;
            _finalShotPoint = muzzlePosition + muzzleForward * _rayDistance;
            _isHit = false;
            _finalShotPoint = muzzlePosition + muzzleForward * _rayDistance;

            if (Physics.Raycast(muzzlePosition, muzzleForward, out RaycastHit gunHit, _rayDistance, _rayMask))
            {
                _finalShotPoint = gunHit.point;
                _lastHitInfo = gunHit;
                _isHit = true;
            }
        }

        private void Update()
        {
            _aimValue = Mathf.MoveTowards(_aimValue, _targetAimValue, _aimSpeed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (_gunTransform == null || _cameraTransform == null) return;

            // Луч от камеры
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_cameraTransform.position, _hitPointFromCamera);
            Gizmos.DrawWireSphere(_hitPointFromCamera, 0.05f);

            // Луч от оружия (результат выстрела)
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_gunTransform.position, _finalShotPoint);
            Gizmos.DrawWireSphere(_finalShotPoint, 0.05f);
        }

        private void SecondaryInputHandler(bool performed)
        {
            _targetAimValue = performed ? 1f : 0f;
        }
        public void PlayEffects()
        {
            Debug.Log("PlayEffects() вызван");

            if (_muzzleFlashPrefab != null && _muzzleFlashPoint != null)
            {
                Debug.Log("Создаём дульный спалах");
                GameObject flash = Instantiate(_muzzleFlashPrefab, _muzzleFlashPoint.position, Quaternion.identity);
                Destroy(flash, 0.3f);
            }
            else
            {
                Debug.LogWarning("MuzzleFlash Prefab или Point не назначены!");
            }

            if (_isHit && _hitEffectPrefab != null)
            {

                Debug.Log("Создаём эффект попадания");
                Debug.Log("Hit point: " + _lastHitInfo.point + ", normal: " + _lastHitInfo.normal);

                GameObject hitFx = Instantiate(_hitEffectPrefab, _lastHitInfo.point, Quaternion.identity);
                Destroy(hitFx, 0.5f);
            }
            else if (!_isHit)
            {
                Debug.Log("Эффект попадания не создаётся — нет попадания (_isHit = false)");
            }
            else if (_hitEffectPrefab == null)
            {
                Debug.LogWarning("HitEffect Prefab не назначен!");
            }
        }
    }
}