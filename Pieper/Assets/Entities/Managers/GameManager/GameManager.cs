using PieperEngine.Scenes;
using PieperEngine.Time;

namespace Pieper.Assets.Entities
{
    public class GameManager : WorldEntity
    {
        private float _lastDeathTime;
        private float _respawnTime = 1f;

        private bool _isAlive = true;
        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (!value)
                {
                    TimeSystem.SetScale(0f);
                    _lastDeathTime = TimeSystem.Time;
                }
                _isAlive = value;
            }
        }

        public GameManager(string key) : base(key)
        {
            Name = "GameManager";
        }

        public override void Update()
        {
            if (!IsAlive)
            {
                float timeSinceLastDeath = TimeSystem.Time - _lastDeathTime;
                if (timeSinceLastDeath > _respawnTime)
                {
                    var currentScene = SceneManager.GetCurrentScene();
                    SceneManager.RunScene(currentScene.Name);
                }
            }
        }
    }
}
