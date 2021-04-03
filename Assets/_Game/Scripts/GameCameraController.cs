using System;
using Mirror.Examples.MultipleMatch;
using Mirror.Examples.Pong;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.GridCCG
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(PlayerInput))]
    public class GameCameraController : MonoBehaviour
    {
        private const float DragPlayDistance = 10f;
        
        public float distanceToTarget;
        public Vector3 angleToTarget;
        public float minXRotation;
        public float maxXRotation;
        //public float orbitSpeed;
        
        private Transform _cameraTransform;
        private Transform _targetTransform;
        private PlayerInput _playerInput;
        private bool _doDrag;
        private Vector3 _prevDragPos;
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _cameraTransform = GetComponent<Transform>();
            _playerInput = GetComponent<PlayerInput>();
            
            _playerInput.currentActionMap.Enable();
            _playerInput.currentActionMap.FindAction("MousePosition").started += OnPlayerMousePosition;
            _playerInput.currentActionMap.FindAction("MousePosition").performed += OnPlayerMousePosition;
            _playerInput.currentActionMap.FindAction("MousePosition").canceled += OnPlayerMousePosition;
            _playerInput.currentActionMap.FindAction("DoDrag").started += OnPlayerDoDrag;
            _playerInput.currentActionMap.FindAction("DoDrag").performed += OnPlayerDoDrag;
            _playerInput.currentActionMap.FindAction("DoDrag").canceled += OnPlayerDoDrag;
        }

        private void Start()
        {
            _targetTransform = new GameObject("Camera Target").GetComponent<Transform>();
        }

        private void OnPlayerDoDrag(InputAction.CallbackContext obj)
        {
            if (obj.started)
                _doDrag = true;
            else if (obj.canceled)
                _doDrag = false;
        }

        private void OnPlayerMousePosition(InputAction.CallbackContext context)
        {
            var mousePoint = context.ReadValue<Vector2>();

            // using inverse transform point avoids jitters that come from updating the position of the camera while raycasting on the same frame
            var targetPosition = _cameraTransform.InverseTransformPoint(_targetTransform.position);
            var planePoint = MouseToPlanePosition(mousePoint, new Plane(Vector3.up, targetPosition));

            if (_doDrag)
            {
                var targetPosToPrevDragPos = _prevDragPos - targetPosition;
                var targetPosToPlanePoint = planePoint - targetPosition;
                var angleDelta = Vector3.SignedAngle(targetPosToPrevDragPos, targetPosToPlanePoint, Vector3.up);
                angleToTarget -= new Vector3(0f, angleDelta, 0f);
            }
                
            _prevDragPos = planePoint;
        }

        private Vector3 MouseToPlanePosition(Vector2 mousePosition, Plane plane)
        {
            var ray = _camera.ScreenPointToRay(mousePosition);
            plane.Raycast(ray, out var enterPoint);
            return _cameraTransform.InverseTransformPoint(ray.GetPoint(enterPoint));
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