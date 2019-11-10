using System.Collections;
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
            originalRectPosition = rectTransform.anchoredPosition;
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
            localToRectTouchPosition = rectTransform.InverseTransformPoint(touchPosition);
            if (holdingDown && Vector2.Distance(localToRectTouchPosition, originalRectPosition) < joystickMaxRadius)
            {
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