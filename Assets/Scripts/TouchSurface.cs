using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerColor
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(EventTrigger))]
    public class TouchSurface : MonoBehaviour
    {
        private Vector2 _mouseBeginDragPosition;
        
        [SerializeField] private EventTrigger eventTrigger;

        public event Action<Vector2> Touched; 
        
        public event Action<Vector2> DragBegun;
        public event Action<Vector2> Dragging; 
        
        private void Start()
        {
            //Touched
            var touchedEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick,
                callback = new EventTrigger.TriggerEvent()
            };
            
            touchedEvent.callback.AddListener(OnTouched);
            
            //Begin drag
            var beginDragEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.BeginDrag, 
                callback = new EventTrigger.TriggerEvent()
            };
            beginDragEvent.callback.AddListener(OnBeginDrag);
            
            //Drag
            var dragEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Drag, 
                callback = new EventTrigger.TriggerEvent()
            };
            dragEvent.callback.AddListener(OnDrag);

            eventTrigger.triggers.Add(touchedEvent);
            eventTrigger.triggers.Add(beginDragEvent);
            eventTrigger.triggers.Add(dragEvent);
        }

        private void OnTouched(BaseEventData eventData)
        {
            var input = eventData.currentInputModule.input;

            if (input.touchSupported)
            {
                var touch = input.GetTouch(0);
                Touched?.Invoke(new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height));
            }
            else
            {
                Touched?.Invoke(new Vector2(input.mousePosition.x / Screen.width, input.mousePosition.y / Screen.height));
            }
        }
        
        private void OnBeginDrag(BaseEventData eventData)
        {
            var input = eventData.currentInputModule.input;

            if (input.touchSupported)
            {
                var touch = input.GetTouch(0);
                DragBegun?.Invoke(new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height));
            }
            else
            {
                _mouseBeginDragPosition = input.mousePosition;
                DragBegun?.Invoke(new Vector2(input.mousePosition.x / Screen.width, input.mousePosition.y / Screen.height));
            }
        }

        private void OnDrag(BaseEventData eventData)
        {
            var input = eventData.currentInputModule.input;

            if (input.touchSupported)
            {
                var touch = input.GetTouch(0);
                Dragging?.Invoke(new Vector2(touch.deltaPosition.x / Screen.width, touch.deltaPosition.y / Screen.height));
            }
            else
            {
                Dragging?.Invoke(new Vector2(
                    (input.mousePosition.x - _mouseBeginDragPosition.x) / Screen.width, 
                    (input.mousePosition.y - _mouseBeginDragPosition.y) / Screen.height));
            }
        }
    }
}