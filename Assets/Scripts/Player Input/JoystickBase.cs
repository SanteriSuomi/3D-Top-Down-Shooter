using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shooter.Inputs
{
    public abstract class JoystickBase : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {
        //
        // JoystickBase contains base functionality for both joysticks.
        //
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
        protected int currentTouchIndex;
        protected bool holdingDown;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            baseRectTransform = transform.parent.GetComponent<RectTransform>();
            originalRectPosition = baseRectTransform.anchoredPosition;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            // Every time a pointer (a finger) enters the joystick, update the touchIndex according to how many fingers are touching.
            currentTouchIndex = Input.touchCount - 1;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            // Indicate that the user is holding down the pointer.
            holdingDown = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            holdingDown = false;
            ResetTouch();
            StartCoroutine(MoveToOriginalPosition());
        }

        private void ResetTouch()
        {
            // When touch gets reset, reset all the corresponding touchPositions.
            touchPosition = Vector2.zero;
            touchPositionToLocalRect = Vector2.zero;
        }

        protected virtual IEnumerator MoveToOriginalPosition()
        {
            // While the joystick isn't near the middle of the joystick's base transform, it's position gets updated closer to the middle.
            while (!Mathf.Approximately(rectTransform.anchoredPosition.x, originalRectPosition.x)
                || !Mathf.Approximately(rectTransform.anchoredPosition.y, originalRectPosition.y))
            {
                // Move the joystick to the middle linearly.
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
            try
            {
                // Make sure there are fingers touching the joysticks..
                if (Input.touchCount > 0)
                {
                    // Then update the currentTouch from the corresponding touchIndex.
                    currentTouch = Input.GetTouch(currentTouchIndex);
                    // Touch position should currentTouch's position.
                    touchPosition = currentTouch.position;
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                #if UNITY_EDITOR
                Debug.Log(e);
                #endif
            }
        }

        protected virtual void JoystickAction()
        {
            // Convert the global touch position to be relative to the base of the joystick.
            touchPositionToLocalRect = Vector2.ClampMagnitude(baseRectTransform.InverseTransformPoint(touchPosition), joystickMaxRadius);
            // Linearly move the joystick to the previous position.
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, touchPositionToLocalRect, joystickSmooth);
        }

        protected abstract void CancelJoystickAction();
    }
}