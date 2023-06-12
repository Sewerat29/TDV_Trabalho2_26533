using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Scripts;
using monogame.Sprites;


namespace monogame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        private Camera Camera;

       
        private SpriteFont StartButtonFont;
        private SpriteFont CreditsFont;
        private bool HasStarted = false;
        private bool HasEnded = false;

        private SpriteFont ScoreFont;
        private int Score = 0;

        private SpriteFont TimerFont;
        private int TimeLeft = 45; // seconds
        private float SecondsState = 1;

        private List<Sprite> _sprites;
        private int maxNumberOfEnemys = 50;
        private int currentNumberOfEnemys = 0;

        private const float enemySpawnDelay = 1.2f; // seconds
        private float remainingEnemySpawnDelay = enemySpawnDelay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = false;
            this.Window.Title = "Top Down Game!";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Camera = new Camera();

            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            StartButtonFont = Content.Load<SpriteFont>("StartButton");
            CreditsFont = Content.Load<SpriteFont>("Credits");
            ScoreFont = Content.Load<SpriteFont>("Score");
            TimerFont = Content.Load<SpriteFont>("Timer");

            var backgroundTexture = Content.Load<Texture2D>("Background");
            var playerTexture = Content.Load<Texture2D>("player-sheet");
            var bulletTexture = Content.Load<Texture2D>("kunai");

            _sprites = new List<Sprite>()
            {
                //new Sprite(Content.Load<Texture2D>("Background")),
                new Player(playerTexture)
                {
                    Position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2),
                    Origin = new Vector2(playerTexture.Width / 2, playerTexture.Height / 2),
                    Bullet = new Bullet(bulletTexture)
                    {
                        Origin = new Vector2(bulletTexture.Width / 2, bulletTexture.Height / 2),
                    },
                },

                new Background(backgroundTexture)
                {
                    Position = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    Origin = new Vector2(backgroundTexture.Width, backgroundTexture.Height),
                }
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (HasEnded) return;

            if(Keyboard.GetState().IsKeyDown(Keys.Space)) HasStarted = true; //Starts the Game when Space is pressed

            if (!HasStarted) return;

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Time counter logic
            SecondsState -= timer;

            if (SecondsState < 0)
            {
                TimeLeft--;
                SecondsState = 1;
            }

            if(TimeLeft < 0)
            {
                HasEnded = true;
            }

            Camera.Follow(_sprites[0]);

            // Enemy spawn timing and maxEnemyNumber Logic
            remainingEnemySpawnDelay -= timer;

            if (currentNumberOfEnemys < maxNumberOfEnemys && remainingEnemySpawnDelay <= 0)
            {
                Random r = new();

                var enemyTexture = Content.Load<Texture2D>("enemy-sheet");
                int x = r.Next(0, ScreenWidth);
                int y = r.Next(0, ScreenHeight);

                _sprites.Add(
                    new Enemy(enemyTexture)
                    {
                        Position = new Vector2(x, y),
                        Origin = new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2),
                    }
                );

                currentNumberOfEnemys++;
                remainingEnemySpawnDelay = enemySpawnDelay;
            }

            foreach (var sprite in _sprites.ToList())
                sprite.Update(gameTime, _sprites);

            PostUpdate();

            base.Update(gameTime);
        }

        public void PostUpdate()
        {
            var collidableBulletSprites = _sprites.Where(c => c is Bullet);
            var collidableEnemysSprites = _sprites.Where(c => c is Enemy);

            foreach (var spriteA in collidableBulletSprites)
            {
                foreach (var spriteB in collidableEnemysSprites)
                {
                    // Don't do anything if they're the same sprite!
                    if (spriteA == spriteB)
                        continue;

                    if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
                        continue;

                    if (spriteA.Intersects(spriteB))
                    ((ICollidable)spriteA).OnCollide(spriteB);
                }
            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].isRemoved)
                {
                    if (_sprites[i] is Enemy)
                    {
                        Score++;
                        currentNumberOfEnemys--;
                    }

                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            spriteBatch.Begin(transformMatrix: Camera.Transform);

            if (HasEnded)
            {
                spriteBatch.DrawString(CreditsFont, "Your Score " + Score, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.Black);
            }
            else if(HasStarted)
            {
                foreach (var sprite in _sprites)
                    sprite.Draw(spriteBatch);

                
                spriteBatch.DrawString(ScoreFont, "Score: " + Score, new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(TimerFont, "Time Left: " + TimeLeft, new Vector2(100, 115), Color.Black);
            } else
            {
                spriteBatch.DrawString(StartButtonFont, "PRESS SPACE TO START!", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}