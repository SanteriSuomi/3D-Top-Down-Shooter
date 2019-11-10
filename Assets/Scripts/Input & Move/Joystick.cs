using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Shooter.Input
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform rectTransform;
        [SerializeField]
        private RectTransform baseRectTransform = default;
        private InputActions inputActions;
        private Vector2 originalRectPosition;
        private Vector2 touchPosition;
        private Vector2 localToRectTouchPosition;
        private bool holdingDown;
        [SerializeField]
        private float resetSpeed = 0.1f;
        [SerializeField]
        private float joystickMaxRadius = 235;
        [SerializeField]
        private float joystickSpeed = 0.9f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            inputActions = new InputActions();
            inputActions.Player.TouchPosition.performed += TouchPositionPerformed;
        }

        private void Start()
        {
            originalRectPosition = baseRectTransform.anchoredPosition;
            Debug.Log($"OriginalPos: {originalRectPosition}");
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void TouchPositionPerformed(InputAction.CallbackContext context)
        {
            touchPosition = context.ReadValue<Vector2>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            holdingDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            localToRectTouchPosition = Vector2.zero;
            StartCoroutine(MoveToOriginalPosition());
        }

        private IEnumerator MoveToOriginalPosition()
        {
            while (rectTransform.anchoredPosition != originalRectPosition || !holdingDown)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, originalRectPosition, resetSpeed);
                yield return null;
            }
        }

        private void Update()
        {
            Debug.Log($"Local: {localToRectTouchPosition.normalized}");
            if (holdingDown)
            {
                localToRectTouchPosition = Vector2.ClampMagnitude(baseRectTransform.InverseTransformPoint(touchPosition), joystickMaxRadius);
                InputEventHandler.InvokeJoystickMove();
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, localToRectTouchPosition, joystickSpeed);
            }
            else
            {
                InputEventHandler.InvokeJoystickStop();
            }
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }
    }
}