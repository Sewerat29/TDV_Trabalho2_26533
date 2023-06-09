using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho2_Tecnicas_26533
{
    public class PLayer
    {
        private Vector2 _position = new(100, 100);
        private readonly float _speed = 200f;
        private readonly AnimationManager _anims = new();

        public void Player()
        {
            var playerTexture = Globals.Content.Load<Texture2D>("Walk");
            _anims.AddAnimation(new Vector2(0, 1), new(playerTexture, 8, 8, 0.1f, 1));
            _anims.AddAnimation(new Vector2(-1, 0), new(playerTexture, 8, 8, 0.1f, 2));
            _anims.AddAnimation(new Vector2(1, 0), new(playerTexture, 8, 8, 0.1f, 3));
            _anims.AddAnimation(new Vector2(0, -1), new(playerTexture, 8, 8, 0.1f, 4));
            
        }

        public void Update()
        {
            if (InputManager.Moving)
            {
                _position += Vector2.Normalize(InputManager.Direction) * _speed * Globals.TotalSeconds;
            }

            _anims.Update(InputManager.Direction);
        }
        
        public void Draw()
        {
            _anims.Draw(_position);
        }
    }
}
