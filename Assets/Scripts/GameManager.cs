using Cinemachine;
using UnityEngine;
using Zenject;

namespace TowerColor
{
    /// <summary>
    /// The game manager handles the logic of the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private GameData _gameData;
        private TowerSpawner _towerSpawner;
        private CinemachineVirtualCamera _playerCamera;
        private Tower _tower;
        private TouchSurface _touchSurface;

        private GameObject _playerCameraFocusPoint;
        
        public int CurrentLevel { get; set; }

        [Inject]
        public void Construct(GameData gameData, TowerSpawner towerSpawner, CinemachineVirtualCamera playerCamera, TouchSurface touchSurface)
        {
            _gameData = gameData;
            _towerSpawner = towerSpawner;
            _playerCamera = playerCamera;
            _touchSurface = touchSurface;
        }
        
        private void Start()
        {
            //Create tower
            _tower = _towerSpawner.SpawnRandomTower(CurrentLevel);
            _tower.Init(CurrentLevel);
            _tower.EnablePhysics(false);
            
            _tower.CurrentStepChanged += OnTowerCurrentStepChanged;
            
            _tower.SetCurrentStep(_tower.Steps.Count - 1);
            
            //Listen touch surface
            _touchSurface.DragBegun += OnPlayerBeginDrag;
            _touchSurface.Dragging += OnPlayerDrag;
        }

        private void OnPlayerDrag(Vector2 dragDelta)
        {
            _playerCamera.transform.RotateAround(_tower.transform.position, _tower.transform.up, dragDelta.x * _gameData.towerRotateSpeed);
            
            Debug.LogFormat("Player dragging {0}", dragDelta);
        }

        private void OnPlayerBeginDrag(Vector2 beginDragPosition)
        {
            Debug.LogFormat("Player begin drag at {0}", beginDragPosition);
        }

        private void OnTowerCurrentStepChanged(int step)
        {
            if(_playerCameraFocusPoint) Destroy(_playerCameraFocusPoint);
            
            _playerCameraFocusPoint = new GameObject("FocusPoint");
            _playerCameraFocusPoint.transform.SetParent(transform, false);
            _playerCameraFocusPoint.transform.position = _tower.CurrentSubTowerFocusPoint.position;
            
            _playerCamera.transform.position = new Vector3(
                _playerCamera.transform.position.x, 
                _playerCameraFocusPoint.transform.position.y, 
                _playerCamera.transform.position.z);
            _playerCamera.LookAt = _playerCameraFocusPoint.transform;
        }
    }
}