using Framework.UI;
using Framework.Views.Installers;
using TowerColor.UI;
using UnityEngine;
using Zenject;

namespace TowerColor.Views.Installers
{
    public class TowerColorPlayingViewInstaller : ViewInstaller
    {
        [SerializeField] private Transform colorChangeMessageAnchor;
        [SerializeField] private Transform feedbackMessageAnchor;
        [SerializeField] private Transform ballGainedMessageAnchor;
        [SerializeField] private Transform feverMessageAnchor;
        [SerializeField] private GameObject ballSpawnerPrefab;
        [SerializeField] private TouchSurface touchSurface;

        private Camera _playerCamera;
        
        [Inject]
        public void Construct(Camera playerCamera)
        {
            _playerCamera = playerCamera;
        }
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            //Ball spawner
            Container.BindInterfacesAndSelfTo<BallSpawner>().FromComponentInNewPrefab(ballSpawnerPrefab).AsSingle().OnInstantiated<BallSpawner>(
                (ctx, spawner) =>
                {
                    spawner.transform.SetParent(_playerCamera.transform, true);
                    spawner.transform.localScale = Vector3.one;
                });
            
            //Touch surface
            Container.BindInterfacesAndSelfTo<TouchSurface>().FromInstance(touchSurface).AsSingle();

            //Color change message anchor
            Container.Bind<Transform>().WithId("ColorChangeMessageAnchor").FromInstance(colorChangeMessageAnchor);
            
            //Feedback message anchor
            Container.Bind<Transform>().WithId("FeedbackMessageAnchor").FromInstance(feedbackMessageAnchor);
            
            //Ball gained message anchor
            Container.Bind<Transform>().WithId("BallGainedMessageAnchor").FromInstance(ballGainedMessageAnchor);
            
            //Fever message anchor
            Container.Bind<Transform>().WithId("FeverMessageAnchor").FromInstance(feverMessageAnchor);
            
            //Balls gained message
            Container.BindFactory<Object, BallGainedMessage, BallGainedMessage.Factory>()
                .FromFactory<PrefabFactory<BallGainedMessage>>();
            
            //Fever message
            Container.BindFactory<Object, FeverMessage, FeverMessage.Factory>()
                .FromFactory<PrefabFactory<FeverMessage>>();
            
            //Fever gauge
            Container.Bind<FeverGauge>().FromComponentInChildren().AsSingle();
            
            //Level progress gauge
            Container.Bind<LevelProgressGauge>().FromComponentInChildren().AsSingle();
        }
    }
}