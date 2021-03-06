using Cinemachine;
using DG.Tweening;
using Framework.Game;
using Framework.Views;
using Framework.Views.Default;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    /// <summary>
    /// Tower color start view
    /// </summary>
    public class TowerColorStartView : DefaultStartView
    {
        /// <summary>
        /// Game manager
        /// </summary>
        private GameManager _gameManager;
        
        /// <summary>
        /// Game data
        /// </summary>
        private GameData _gameData;
        
        /// <summary>
        /// Look around tower camera
        /// </summary>
        private CinemachineVirtualCamera _lookAroundTowerCamera;
        
        /// <summary>
        /// Follow point
        /// </summary>
        private GameObject _followPoint;
        
        /// <summary>
        /// Focus point
        /// </summary>
        private GameObject _focusPoint;

        [Inject]
        public void Construct(
            GameManager gameManager,
            GameData gameData,
            [Inject(Id = "LookAroundTowerCamera")] CinemachineVirtualCamera lookAroundTowerCamera)
        {
            _gameManager = gameManager;
            _gameData = gameData;
            _lookAroundTowerCamera = lookAroundTowerCamera;
        }

        protected override void OnShow()
        {
            base.OnShow();
            _lookAroundTowerCamera.gameObject.SetActive(true);

            //Create focus point
            _focusPoint = new GameObject("FocusPoint");
            _focusPoint.transform.SetParent(transform);
            _focusPoint.transform.position = _gameManager.Tower.transform.position;
            
            //Camera is looking at the focus point
            _lookAroundTowerCamera.LookAt = _focusPoint.transform;

            //Create follow point
            _followPoint = new GameObject("FollowPoint");
            _followPoint.transform.SetParent(transform);

            _followPoint.transform.position = _focusPoint.transform.position;
            _followPoint.transform.Translate(
                -_gameManager.Tower.transform.forward * _gameData.cameraDistanceFromTower 
                + _gameManager.Tower.transform.up * _gameData.cameraHeightOffsetFromTower, 
                Space.World);
            
            //Camera is following follow point
            _lookAroundTowerCamera.Follow = _followPoint.transform;
            
            //Move the focus from the bottom to the top of the tower
            var tween = _focusPoint.transform.DOMoveY(_gameManager.Tower.GetStepFocusPoint(_gameManager.Tower.Steps.Count - 1).position.y, 3f);
            tween.onUpdate += OnFocusPointMove;
            tween.onComplete += OnCameraMoveComplete;
        }

        protected override void OnHide()
        {
            base.OnHide();

            _lookAroundTowerCamera.Follow = null;
            _lookAroundTowerCamera.LookAt = null;
            _lookAroundTowerCamera.gameObject.SetActive(false);
            
            Destroy(_followPoint);
            _followPoint = null;
            
            Destroy(_focusPoint);
            _focusPoint = null;
        }

        /// <summary>
        /// When focus point moves
        /// </summary>
        private void OnFocusPointMove()
        {
            _followPoint.transform.position = new Vector3(
                _followPoint.transform.position.x, 
                _focusPoint.transform.position.y + _gameData.cameraHeightOffsetFromTower, 
                _followPoint.transform.position.z);
            
            _followPoint.transform.RotateAround(
                _gameManager.Tower.transform.position, 
                _gameManager.Tower.transform.up, 
                360f * Time.deltaTime / 3f);
        }
        
        /// <summary>
        /// When camera move complete
        /// </summary>
        private void OnCameraMoveComplete()
        {
            _gameManager.ChangeState(GameState.Playing);
        }
    }
}