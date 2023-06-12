using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;
        public float indexAnimation = 0;

        public Vector2 Origin;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 Velocity;
        public Matrix Transform;
        protected float _rotation;

        public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;
        
        public Sprite Parent;
        public float LifeSpan = 0f;
        public bool isRemoved = false;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Rectangle[] sourceRectangles = new Rectangle[4] { new Rectangle(0, 0, 16, 16), new Rectangle(0, 16, 16, 16), new Rectangle(0, 32, 16, 16), new Rectangle(0, 48, 16, 16) };

        public Color[] TextureData;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            TextureData = new Color[_texture.Width * _texture.Height];
            _texture.GetData(TextureData);
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height), MathHelper.Max(Rectangle.Width, Rectangle.Height));
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }

        public Matrix Trasnform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                    Matrix.CreateRotationZ(_rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0));
            }
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites){}

        public void getAnimationSprite(Keys key, bool idle = false)
        {
            // Player Animation Logic
            if (idle) {
                indexAnimation = 0;
            } else
            {
                if(key == Keys.W)
                {
                    sourceRectangles[0] = new Rectangle(16, 0, 16, 16);
                    sourceRectangles[1] = new Rectangle(16, 16, 16, 16);
                    sourceRectangles[2] = new Rectangle(16, 32, 16, 16);
                    sourceRectangles[3] = new Rectangle(16, 48, 16, 16);
                }
                if (key == Keys.S)
                {
                    sourceRectangles[0] = new Rectangle(0, 0, 16, 16);
                    sourceRectangles[1] = new Rectangle(0, 16, 16, 16);
                    sourceRectangles[2] = new Rectangle(0, 32, 16, 16);
                    sourceRectangles[3] = new Rectangle(0, 48, 16, 16);
                }
                if (key == Keys.A)
                {
                    sourceRectangles[0] = new Rectangle(32, 0, 16, 16);
                    sourceRectangles[1] = new Rectangle(32, 16, 16, 16);
                    sourceRectangles[2] = new Rectangle(32, 32, 16, 16);
                    sourceRectangles[3] = new Rectangle(32, 48, 16, 16);
                }
                if (key == Keys.D)
                {
                    sourceRectangles[0] = new Rectangle(48, 0, 16, 16);
                    sourceRectangles[1] = new Rectangle(48, 16, 16, 16);
                    sourceRectangles[2] = new Rectangle(48, 32, 16, 16);
                    sourceRectangles[3] = new Rectangle(48, 48, 16, 16);
                }

                if (indexAnimation == 30) // The animation cycle runs in 30 frames 
                {
                    indexAnimation = 0;
                } else
                {
                    indexAnimation++;
                }
            }

            //TODO: Enemy Animation Logic
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this is Player || this is Enemy)
            {
                if (_texture != null)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        getAnimationSprite(Keys.W);
                    } else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        getAnimationSprite(Keys.S);
                    } else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        getAnimationSprite(Keys.A);
                    } else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        getAnimationSprite(Keys.D);
                    } else { getAnimationSprite(Keys.Delete, true); }

                    float indexAnimationRounded = (float)Math.Floor(indexAnimation / 10); // Logic to cut the tranform the frame number in a rounded index

                    spriteBatch.Draw(_texture, Position, sourceRectangles[(int)indexAnimationRounded], Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
                }
            }

            if(this is Bullet && _texture != null)
            {
                spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, 128, 128), Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
            }
        }

        public bool Intersects(Sprite sprite)
        {
            if(this is Bullet && sprite is Enemy)
            {
                isRemoved = true;
                sprite.isRemoved = true;
                Console.WriteLine("hit");
            }

            return false;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
