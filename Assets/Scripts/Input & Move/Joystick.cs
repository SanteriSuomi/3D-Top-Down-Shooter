using System.Collections;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Minor Code Smell", "S1450:Private fields only used as local variables in methods should become local variables", Justification = "Not applicable")]
        private Vector2 touchPositionToLocalRect;
        private Vector2 rotationValue;
        private bool holdingDown;
        [SerializeField]
        private float joystickResetSpeed = 50f;
        [SerializeField]
        private float joystickMaxRadius = 235;
        [SerializeField]
        private float joystickSmooth = 0.9f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            inputActions = new InputActions();
            inputActions.Player.TouchPosition.performed += TouchPositionPerformed;
            originalRectPosition = baseRectTransform.anchoredPosition;
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void TouchPositionPerformed(InputAction.CallbackContext inputAction)
        {
            touchPosition = inputAction.ReadValue<Vector2>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            holdingDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            touchPositionToLocalRect = Vector2.zero;
            StartCoroutine(MoveToOriginalPosition());
        }

        private IEnumerator MoveToOriginalPosition()
        {
            while (!Mathf.Approximately(rectTransform.anchoredPosition.x, originalRectPosition.x)
                || !Mathf.Approximately(rectTransform.anchoredPosition.y, originalRectPosition.y))
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, originalRectPosition, joystickResetSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void Update()
        {
            if (holdingDown)
            {
                MoveJoystick();
            }
            else
            {
                CancelMoveJoystick();
            }
        }

        private void MoveJoystick()
        {
            touchPositionToLocalRect = Vector2.ClampMagnitude(baseRectTransform.InverseTransformPoint(touchPosition), joystickMaxRadius);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, touchPositionToLocalRect, joystickSmooth);
            InputEventHandler.InvokeJoystickMove(move: true);
            InputEventHandler.InvokeJoystickInput(touchPositionToLocalRect);
        }

        private static void CancelMoveJoystick()
        {
            InputEventHandler.InvokeJoystickMove(move: false);
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }
    }
}