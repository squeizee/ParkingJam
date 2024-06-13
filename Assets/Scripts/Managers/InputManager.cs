using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        
        private InputActions _inputActions;

        private Vector2 _initialPos;
        private const float MinDistance = .2f;
        private const float DirectionThreshold = .9f;
        private bool _isClicked;
        
        private Vehicle _selectedVehicle;
        private Vector2 CurrentPos => _inputActions.Player.Position.ReadValue<Vector2>();
        
        private void OnEnable()
        {
            _inputActions = InputActions.Instance;

            _inputActions.Player.Click.performed += OnClickActionPerformed;
            _inputActions.Player.Click.canceled += OnClickActionCanceled;

            _inputActions.Player.Position.performed += OnPositionActionPerformed;
        }

        private void OnDisable()
        {
            _inputActions.Player.Click.performed -= OnClickActionPerformed;
            _inputActions.Player.Click.canceled -= OnClickActionCanceled;

            _inputActions.Player.Position.performed -= OnPositionActionPerformed;
        }

        private void OnPositionActionPerformed(InputAction.CallbackContext ctx)
        {
            if (_isClicked)
                DetectSwipe();
        }

        private void OnClickActionPerformed(InputAction.CallbackContext ctx)
        {
            var ray = mainCam.ScreenPointToRay(CurrentPos);

            if (Physics.Raycast(ray, out var hit) && hit.transform.TryGetComponent(out _selectedVehicle))
            {
                _initialPos = CurrentPos;
                _isClicked = true;
                _selectedVehicle.isSelected = true;
            }
            else
            {
                _isClicked = false;
                _selectedVehicle = null;
            }
        }

        private void OnClickActionCanceled(InputAction.CallbackContext ctx)
        {
            _isClicked = false;
            _selectedVehicle = null;
        }

        private void DetectSwipe()
        {
            if (!(Vector2.Distance(_initialPos, CurrentPos) > MinDistance)) return;
            
            _isClicked = false;
            Vector3 direction = CurrentPos - _initialPos;
            var direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }

        private void SwipeDirection(Vector2 direction)
        {
            Vector3 correctDirection = Vector3.zero;
            
            if (Vector2.Dot(Vector2.up, direction) > DirectionThreshold)
            {
                correctDirection = Vector3.forward;
            }
            else if (Vector2.Dot(Vector2.down, direction) > DirectionThreshold)
            {
                correctDirection = Vector3.back;
            }
            else if (Vector2.Dot(Vector2.right, direction) > DirectionThreshold)
            {
                correctDirection = Vector3.right;
            }
            else if (Vector2.Dot(Vector2.left, direction) > DirectionThreshold)
            {
                correctDirection = Vector3.left;
            }

            if(correctDirection != Vector3.zero)
                _selectedVehicle.Move(correctDirection);
        }
    }
}