using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace monogame.Sprites
{
    internal class Enemy : Sprite, ICollidable
    {
        public int Health { get; set; }
        public bool HasDied = false;

        public float Speed { get; set; }

        public bool isDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Enemy(Texture2D texture)
         : base(texture)
        {
            Speed = 50f;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // reminder: the code is expecting that the player is the first one to be added to the sprites List;
            Move(sprites[0], dt);

            if (isDead) return;

            Position += Velocity;
        }

        private void Move(Sprite player, float dt)
        {
            Vector2 moveDir = player.Position - Position;
            moveDir.Normalize();

            Position += moveDir * Speed * dt;
        }

        public void OnCollide(Sprite sprite)
        {
            if(sprite is Bullet)
            {
                isRemoved= true;
                sprite.isRemoved = true;
            }
        }
    }
}
