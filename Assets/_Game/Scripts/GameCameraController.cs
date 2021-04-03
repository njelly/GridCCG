using System;
using UnityEngine;

namespace Tofunaut.GridCCG
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public float distanceToTarget;
        public Vector3 angleToTarget;
        public float minXRotation;
        public float maxXRotation;

        private Transform _cameraTransform;
        private Transform _targetTransform;
        private Camera _camera;
        private Plane _plane;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _cameraTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            _plane = new Plane(Vector3.up, Vector3.zero);
            _targetTransform = new GameObject("Camera Target").GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            // calculate the relative position and rotation to the target transform
            var targetPosition = _targetTransform.position;
            var clampedAngleToTarget = new Vector3(Mathf.Clamp(angleToTarget.x, minXRotation, maxXRotation),
                angleToTarget.y, angleToTarget.z);
            _cameraTransform.position = targetPosition + Quaternion.Euler(clampedAngleToTarget) * (Vector3.forward * -distanceToTarget);
            _cameraTransform.rotation = Quaternion.LookRotation(targetPosition - _cameraTransform.position);
        }
    }
}