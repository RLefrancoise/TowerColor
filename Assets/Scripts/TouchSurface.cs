using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerColor
{
    /// <summary>
    /// Interface for touch surface
    /// </summary>
    public interface ITouchSurface
    {
        /// <summary>
        /// When touched
        /// </summary>
        event Action<Vector2> Touched; 
        
        /// <summary>
        /// When begin drag
        /// </summary>
        event Action<Vector2> DragBegun;
        
        /// <summary>
        /// When end drag
        /// </summary>
        event Action<Vector2> Dragging; 
    }
    
    /// <summary>
    /// Touch surface
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(EventTrigger))]
    public class TouchSurface : MonoBehaviour, ITouchSurface
    {
        /// <summary>
        /// Is dragging ?
        /// </summary>
        private bool _isDragging;
        
        /// <summary>
        /// Mouse drag position
        /// </summary>
        private Vector2 _mouseBeginDragPosition;
        
        /// <summary>
        /// Event trigger
        /// </summary>
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
            _isDragging = true;
            
            var input = eventData.currentInputModule.input;

            if (input.touchSupported)
            {
                var touch = input.GetTouch(0);
                DragBegun?.Invoke(new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height));
            }
            else
            {
                _mouseBeginDragPosition = new Vector2(input.mousePosition.x / Screen.width, input.mousePosition.y / Screen.height);
                DragBegun?.Invoke(_mouseBeginDragPosition);
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
                Dragging?.Invoke(new Vector2(input.mousePosition.x / Screen.width, input.mousePosition.y / Screen.height) - _mouseBeginDragPosition);
            }
        }

        private void OnEndDrag(BaseEventData eventData)
        {
            _isDragging = false;
        }
    }
}