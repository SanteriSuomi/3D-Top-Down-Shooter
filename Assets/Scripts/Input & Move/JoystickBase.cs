using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Shooter.Inputs
{
    public abstract class JoystickBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        protected RectTransform rectTransform;
        protected RectTransform baseRectTransform;
        protected InputActions inputActions;
        protected Vector2 touchPosition;
        [SuppressMessage("Minor Code Smell", "S1450:Private fields only used as local variables in methods should become local variables", Justification = "Not applicable")]
        protected Vector2 touchPositionToLocalRect;
        protected Vector2 originalRectPosition;
        [SerializeField]
        protected float joystickResetSpeed = 300;
        [SerializeField]
        protected float joystickMaxRadius = 230;
        [SerializeField]
        protected float joystickSmooth = 0.8f;
        protected bool holdingDown;
        protected UnityEngine.Touch touch;
        protected int touchIndex;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            inputActions = new InputActions();
            EnhancedTouchSupport.Enable();
            baseRectTransform = transform.parent.GetComponent<RectTransform>();
            originalRectPosition = baseRectTransform.anchoredPosition;
        }

        protected virtual void OnEnable()
        {
            inputActions.Enable();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            int touchCount = Input.touchCount;
            Debug.Log($"TouchCount: {touchCount}");
            if (touchCount == 1)
            {
                touchIndex = 0;
                //touch = Input.GetTouch(0);
            }
            else if (touchCount == 2)
            {
                touchIndex = 1;
                //touch = Input.GetTouch(1);
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            holdingDown = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            touchIndex = 10;
            touchPositionToLocalRect = Vector2.zero;
            StartCoroutine(MoveToOriginalPosition());
        }

        protected virtual IEnumerator MoveToOriginalPosition()
        {
            while (!Mathf.Approximately(rectTransform.anchoredPosition.x, originalRectPosition.x)
                || !Mathf.Approximately(rectTransform.anchoredPosition.y, originalRectPosition.y))
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, originalRectPosition, joystickResetSpeed * Time.deltaTime);
                yield return null;
            }
        }

        protected virtual void Update()
        {
            if (touchIndex <= 1)
            {
                touchPosition = Input.GetTouch(touchIndex).position;
            }
            Debug.Log($"touchpos: {touchPosition}");

            if (holdingDown)
            {
                JoystickAction();
            }
            else
            {
                CancelJoystickAction();
            }
        }

        protected virtual void JoystickAction()
        {
            touchPositionToLocalRect = Vector2.ClampMagnitude(baseRectTransform.InverseTransformPoint(touchPosition), joystickMaxRadius);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, touchPositionToLocalRect, joystickSmooth);
        }

        protected abstract void CancelJoystickAction();

        protected virtual void OnDisable()
        {
            inputActions.Player.Disable();
        }

    }
}