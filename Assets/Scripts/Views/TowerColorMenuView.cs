using Cinemachine;
using Framework.Views.Default;
using TowerColor.Views.Installers;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    /// <summary>
    /// Tower color menu view
    /// </summary>
    [RequireComponent(typeof(TowerColorMenuViewInstaller))]
    public class TowerColorMenuView : DefaultMenuView
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
        /// Options manager
        /// </summary>
        private IOptionsManager _optionsManager;

        /// <summary>
        /// Player camera
        /// </summary>
        private Camera _playerCamera;
        
        /// <summary>
        /// Player game camera
        /// </summary>
        private CinemachineVirtualCamera _playerGameCamera;

        [Inject]
        public void Construct(
            GameManager gameManager, 
            GameData gameData, 
            IOptionsManager optionsManager,
            Camera playerCamera,
            [Inject(Id = "GameCamera")] CinemachineVirtualCamera playerGameCamera)
        {
            _gameManager = gameManager;
            _gameData = gameData;
            
            _optionsManager = optionsManager;
            
            _playerCamera = playerCamera;
            _playerGameCamera = playerGameCamera;
        }

        private void Start()
        {
            //Load options if needed
            if (_optionsManager.HasSavedOptions)
            {
                if (!_optionsManager.LoadOptions())
                {
                    Debug.LogError("Failed to load options");
                }
            }
        }
        
        protected override void OnShow()
        {
            base.OnShow();

            _playerGameCamera.gameObject.SetActive(true);

            var pos = _gameManager.Tower.transform.position -
                      _gameManager.Tower.transform.forward * _gameData.cameraDistanceFromTower
                      + _gameManager.Tower.transform.up * _gameData.cameraHeightOffsetFromTower;

            _playerCamera.transform.position = pos;
            
            _playerGameCamera.transform.position = pos;
            _playerGameCamera.LookAt = _gameManager.Tower.GetStepFocusPoint(_gameManager.Tower.Steps.Count / 2);
        }

        protected override void OnHide()
        {
            base.OnHide();
            
            _playerGameCamera.gameObject.SetActive(false);
        }
    }
}