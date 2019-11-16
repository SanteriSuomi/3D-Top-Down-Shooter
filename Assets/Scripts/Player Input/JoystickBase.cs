using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shooter.Inputs
{
    public abstract class JoystickBase : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected RectTransform rectTransform;
        protected RectTransform baseRectTransform;
        protected Touch currentTouch;
        protected Vector2 touchPosition;
        protected Vector2 touchPositionToLocalRect;
        protected Vector2 originalRectPosition;
        [SerializeField]
        protected float joystickResetSpeed = 300;
        [SerializeField]
        protected float joystickMaxRadius = 230;
        [SerializeField]
        protected float joystickSmooth = 0.8f;
        protected bool holdingDown;
        protected int currentTouchIndex;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            baseRectTransform = transform.parent.GetComponent<RectTransform>();
            originalRectPosition = baseRectTransform.anchoredPosition;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            currentTouchIndex = Input.touchCount - 1;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            holdingDown = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            ResetTouchPosition();
            StartCoroutine(MoveToOriginalPosition());
        }

        private void ResetTouchPosition()
        {
            touchPosition = Vector2.zero;
            touchPositionToLocalRect = Vector2.zero;
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
            GetTouchPosition();

            if (holdingDown)
            {
                JoystickAction();
            }
            else
            {
                CancelJoystickAction();
            }
        }

        private void GetTouchPosition()
        {
            if (Input.touchCount > 0)
            {
                currentTouch = Input.GetTouch(currentTouchIndex);
                touchPosition = currentTouch.position;
            }
        }

        protected virtual void JoystickAction()
        {
            touchPositionToLocalRect = Vector2.ClampMagnitude(baseRectTransform.InverseTransformPoint(touchPosition), joystickMaxRadius);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, touchPositionToLocalRect, joystickSmooth);
        }

        protected abstract void CancelJoystickAction();
    }
}