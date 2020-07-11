using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerColor
{
    public interface ITouchSurface
    {
        event Action<Vector2> Touched; 
        event Action<Vector2> DragBegun;
        event Action<Vector2> Dragging; 
    }
    
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(EventTrigger))]
    public class TouchSurface : MonoBehaviour, ITouchSurface
    {
        private bool _isDragging;
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
                eventID = EventTriggerType.PointerUp, //On up to avoid conflict with drag
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
            
            //End drag
            var endDragEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.EndDrag,
                callback = new EventTrigger.TriggerEvent()
            };
            endDragEvent.callback.AddListener(OnEndDrag);

            eventTrigger.triggers.Add(touchedEvent);
            eventTrigger.triggers.Add(beginDragEvent);
            eventTrigger.triggers.Add(dragEvent);
            eventTrigger.triggers.Add(endDragEvent);
        }

        private void OnTouched(BaseEventData eventData)
        {
            //If we are dragging, no touch event, we don't want conflicts, drag has more priority than touch
            if(_isDragging) return;
            
            Debug.Log("Touch");
            
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
            Debug.Log("Begin drag");

            _isDragging = true;
            
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

        private void OnEndDrag(BaseEventData eventData)
        {
            Debug.Log("End drag");

            _isDragging = false;
        }
    }
}