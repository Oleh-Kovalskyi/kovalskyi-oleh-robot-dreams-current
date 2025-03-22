using System;
using System.Collections;
using UnityEngine;
using Dummies;

namespace Shooting
{
    public class HitScanGun : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        public event Action OnShot;
        
        [SerializeField] public  GunAimer _aimer;
        [SerializeField] public HitscanShotAspect _shotPrefab;
        [SerializeField] public Transform _muzzleTransform;
        [SerializeField] public float _decaySpeed;
        [SerializeField] public Vector3 _shotScale;
        [SerializeField] public float _shotRadius;
        [SerializeField] public float _shotVisualDiameter;
        [SerializeField] public string _tilingName;
        [SerializeField] public float _range;
        [SerializeField] public LayerMask _layerMask;

        public int _tilingId;

        public virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }

        public void OnEnable()
        {
            PhysX.InputController.OnPrimaryInput += PrimaryInputHandler;
        }

        public void OnDisable()
        {
            PhysX.InputController.OnPrimaryInput -= PrimaryInputHandler;
        }

        public virtual void PrimaryInputHandler()
        {
            Vector3 muzzlePosition = _muzzleTransform.position;
            Vector3 muzzleForward = _muzzleTransform.forward;
            Ray ray = new Ray(muzzlePosition, muzzleForward);
            Vector3 hitPoint = muzzlePosition + muzzleForward * _range;
            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
            {
                Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                hitPoint = muzzlePosition + rayVector;
                
                OnHit?.Invoke(hitInfo.collider);
            }

            HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _muzzleTransform.rotation);
            shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
            shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot));
            // 💥 Вызов визуальных эффектов выстрела
            if (_aimer != null)
            {
                Debug.Log("Вызов PlayEffects() из HitScanGun — _aimer найден");
                _aimer.PlayEffects();
            }
            else
            {
                Debug.LogWarning("_aimer в HitScanGun НЕ назначен!");
            }
            OnShot?.Invoke();
        }

        public IEnumerator ShotRoutine(HitscanShotAspect shot)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (shot.distance >= interval)
            {
                EvaluateShot(shot);
                yield return null;
                shot.distance -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        public void EvaluateShot(HitscanShotAspect shot)
        {
            shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
            Vector4 tiling = Vector4.one;
            tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
            shot.outerPropertyBlock.SetVector(_tilingId, tiling);
            shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }
    }
}