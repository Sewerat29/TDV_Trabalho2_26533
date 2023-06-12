using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monogame.Sprites
{
    public class Bullet : Sprite, ICollidable
    {
        private float _timer;

        public Bullet(Texture2D texture)
          : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                isRemoved = true;

            Position += Direction * LinearVelocity;
        }

        public virtual void OnCollide(Sprite sprite){}
    }
}
