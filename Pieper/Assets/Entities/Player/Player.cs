using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using PieperEngine.Physics;
using PieperEngine.Physics.Collisions;
using PieperEngine.Rendering.Camera;
using PieperEngine.Rendering.Renderables;
using PieperEngine.Scenes;
using PieperEngine.Input;
using PieperEngine.Time;

using Pieper.Assets.Scripts;

namespace Pieper.Assets.Entities
{
    public class Player : WorldEntity
    {
        private GameManager _gameManager = null!;
        private Collider _collider;
        private Trail _trail;

        private float _moveSpeed = 700f;
        private float _jumpForce = 1475f;
        private float _rotateSpeed = -450f;
        private float _tRotate = 60f;

        //Temporary: put this into a camera entity later!!!!!!!
        private Vector3 _camOffset = new (384f, 192f, 0f);

        private LayerMask _groundLayer;
        private LayerMask _hazardLayer;

        private bool _slowDown = false;

        public Player(string key) : base(key)
        {
            Name = GetType().Name;
            Bitmask = new LayerMask("Player");

            renderer = new SpriteRenderer(SpriteAtlas.GetTexture("spr_cube004"));
            _rb2d = new(5f);
            _collider = new(entity: this, renderer.Renderable.Mesh, ColliderState.Active, ColliderType.Solid);
            _collider.FreezeRotation = true;
            _trail = new(marker: this);

            AddComponent(renderer);
            AddComponent(_collider);
            AddComponent(_rb2d);
        }

        public override void Start()
        {
            _trail.Start();

            _groundLayer = LayerMask.GetMask("Ground");
            _hazardLayer = LayerMask.GetMask("Hazard");

            var gm = SceneManager.FindEntityInCurrentScene<GameManager>();
            if (gm != null)
            {
                _gameManager = gm;
            }
        }

        public override void Update()
        {
            _trail.Update();

            bool isGrounded = IsGrounded();
            if (isGrounded)
            {
                float rotation = MathHelper.RadiansToDegrees(Transform.Rotation);
                float endRotation = MathF.Round(rotation / 90f) * 90f;
                endRotation = MathHelper.DegreesToRadians(endRotation);

                Transform.Rotation = (Math.Abs(endRotation - rotation) > 15f)
                        ? float.Lerp(Transform.Rotation, endRotation, _tRotate * TimeSystem.DeltaTime)
                        : endRotation;
                
                if (InputSystem.GetKey(Keys.Up))
                {
                    _rb2d.Velocity.SetY(0f);
                    _rb2d.ApplyForceY(_jumpForce);
                }
            }
            else
            {
                Transform.Rotation += MathHelper.DegreesToRadians(_rotateSpeed) * TimeSystem.DeltaTime;
            }

            Vector2 newVel = new(0, 0);
            if (InputSystem.GetKey(Keys.Left))
            {
                newVel -= _moveSpeed * Vector2.UnitX;
            }
            if (InputSystem.GetKey(Keys.Right))
            {
                newVel += _moveSpeed * Vector2.UnitX;
            }

            if (InputSystem.GetKey(Keys.Period))
            {
                Camera2D.Main.Zoom -= 2.5f * TimeSystem.DeltaTime;
            }
            if (InputSystem.GetKey(Keys.Comma))
            {
                Camera2D.Main.Zoom += 2.5f * TimeSystem.DeltaTime;
            }
            if (InputSystem.GetKeyDown(Keys.S))
            {
                _slowDown = !_slowDown;
            }

            if (_gameManager.IsAlive)
            {
                if (_slowDown) TimeSystem.SetScale(0.1f);
                else TimeSystem.SetScale(1f);
            }

            _rb2d.Velocity.Set(newVel.X, _rb2d.Velocity.Y);
            Camera2D.Main.SetPosition(Transform.Position + _camOffset);
        }

        public override void OnCollisionEnter(CollisionEventArgs args)
        {
            base.OnCollisionEnter(args);
            Camera2D.Main.SetPosition(Transform.Position + _camOffset);
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.Entity.Bitmask.Contains(_hazardLayer) &&
                _gameManager.IsAlive) _gameManager.IsAlive = false;
        }

        private bool IsGrounded()
        {
            return CollisionSystem.BoxCast(_collider, Vector2.UnitY * -1f, _groundLayer);
        }
    }
}
