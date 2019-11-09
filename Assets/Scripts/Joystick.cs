using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform rectTransform;
        private InputActions inputActions;
        private Vector2 originalRectPosition;
        private Vector2 localTouchPosition;
        private bool holdingDown;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            inputActions = new InputActions();
            inputActions.Player.TouchPosition.performed += TouchPositionPerformed;
        }

        private void Start()
        {
            originalRectPosition = rectTransform.anchoredPosition;
            Debug.Log($"OriginalPosition: {originalRectPosition}");
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void TouchPositionPerformed(InputAction.CallbackContext context)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();
            localTouchPosition = rectTransform.InverseTransformPoint(touchPosition);
            Debug.Log($"LocalTouchPosition: {localTouchPosition}");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            holdingDown = true;
            Debug.Log("PressedJoystick");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            rectTransform.anchoredPosition = originalRectPosition;
        }

        private void Update()
        {
            Debug.Log($"Distance: {Vector2.Distance(localTouchPosition, originalRectPosition)}");
            if (holdingDown && Vector2.Distance(localTouchPosition, originalRectPosition) <= 500)
            {
                rectTransform.anchoredPosition = localTouchPosition;
            }
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }
    }
}