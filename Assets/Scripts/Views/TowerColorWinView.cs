using System;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Views.Default;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace TowerColor.Views
{
    public class TowerColorWinView : DefaultWinView
    {
        private CinemachineVirtualCamera _playerGameCamera;
        private IHapticManager _hapticManager;
        private ISoundPlayer _soundPlayer;
        private GameManager _gameManager;
        private GameData _gameData;

        private TweenerCore<Vector3, Vector3, VectorOptions> _cameraTween;
        
        [SerializeField] private AudioSource winSound;
        
        [Inject]
        public void Construct(
            [Inject(Id = "GameCamera")] CinemachineVirtualCamera playerGameCamera, 
            IHapticManager hapticManager,
            ISoundPlayer soundPlayer,
            GameManager gameManager, 
            GameData gameData)
        {
            _playerGameCamera = playerGameCamera;
            _hapticManager = hapticManager;
            _soundPlayer = soundPlayer;
            _gameManager = gameManager;
            _gameData = gameData;
        }

        protected override async void OnShow()
        {
            base.OnShow();
            
            _soundPlayer.PlaySound(winSound);
            
            _playerGameCamera.gameObject.SetActive(true);

            var dir = (_playerGameCamera.transform.position - _gameManager.Tower.transform.position).normalized;
            var pos = _playerGameCamera.transform.position + dir * _gameData.cameraDistanceOnWin;

            _cameraTween = _playerGameCamera.transform.DOMove(pos, _gameData.cameraMoveDurationOnWin);
            _cameraTween.onComplete += () => _cameraTween = null;

            //Spawn win effect for each scale in the sequence
            foreach (var scale in _gameData.winEffectScalingSequence)
            {
                //Wait
                await UniTask.Delay(TimeSpan.FromSeconds(_gameData.winEffectTimeBetweenEach));
                
                var effect = Instantiate(_gameData.winEffect);
                effect.transform.position = _gameManager.Tower.transform.position;
                effect.transform.localScale = Vector3.one * scale;
                
                //Vibrate
                _hapticManager.Vibrate();
            }
        }

        protected override void OnHide()
        {
            base.OnHide();
            _playerGameCamera.gameObject.SetActive(false);
        }

        protected override void ClickOnContinue()
        {
            _cameraTween?.Kill();
            base.ClickOnContinue();
        }
    }
}