using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace monogame.Sprites
{
    public class Player : Sprite, ICollidable
    {
        public Bullet Bullet;
        public int Health { get; set; }
        public float Speed { get; set; }
        public int Score;
        public bool HasDied = false;

        private const float bulletSpawnDelay = 1; // seconds
        private float remainingBulletSpawnDelay = bulletSpawnDelay;

        MouseState ms = new();

        public bool isDead
        {
            get
            {
                return Health <= 0;

                  
            }
        }

        public Player(Texture2D texture)
          : base(texture)
        {
            Speed = 5f;
            Health = 3;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            ms = Mouse.GetState(); 
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Move();

            remainingBulletSpawnDelay -= timer;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && remainingBulletSpawnDelay <= 0)
            {
                AddBullet(sprites);

                remainingBulletSpawnDelay = bulletSpawnDelay;
            }

            if (isDead) return;

            Position += Velocity;

            // Keep the sprite on the screen
            Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Rectangle.Width);

            // Resest the velocity for when the user isn't holding down a key
            Velocity = Vector2.Zero;
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y -= 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X -= 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X += 3;
            }
        }
        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            Vector2 mousePos = new(ms.X, ms.Y) ;

            bullet.Position = Position;
            bullet.Direction = Vector2.Normalize(mousePos-Position);
            bullet.Rotation = Vector2.Normalize(mousePos - Position).Y;
            bullet.LinearVelocity = LinearVelocity * 2;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }
        public virtual void OnCollide(Sprite sprite)
        {
            Console.WriteLine("Player");
        }
    }
}
