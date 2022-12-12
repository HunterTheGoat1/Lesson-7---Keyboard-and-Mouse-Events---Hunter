using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Lesson_7___Keyboard_and_Mouse_Events___Hunter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState keyboardState;
        MouseState mouseState;
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D currentPacTexture;
        Rectangle pacRect;
        Texture2D exitTexture;
        Rectangle exitRect;
        Texture2D barrierTexture;
        List<Rectangle> barriers;
        Texture2D coinTexture;
        List<Rectangle> coins;
        int pacSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);
            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));
            exitRect = new Rectangle(700, 380, 100, 100);
            base.Initialize();
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 300, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 300, coinTexture.Width, coinTexture.Height));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pacDownTexture = Content.Load<Texture2D>("PacDown");
            pacUpTexture = Content.Load<Texture2D>("PacUp");
            pacRightTexture = Content.Load<Texture2D>("PacRight");
            pacLeftTexture = Content.Load<Texture2D>("PacLeft");
            currentPacTexture = pacRightTexture;
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            coinTexture = Content.Load<Texture2D>("coin");

        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (pacRect.X < 0)
            {
                pacRect.X = 0;
            }
            if (pacRect.Y < 0)
            {
                pacRect.Y = 0;
            }
            if (pacRect.Right > _graphics.PreferredBackBufferWidth)
            {
                pacRect.X = _graphics.PreferredBackBufferWidth - pacRect.Width;
            }
            if (pacRect.Y > _graphics.PreferredBackBufferHeight - pacRect.Height)
            {
                pacRect.Y = _graphics.PreferredBackBufferHeight - pacRect.Height;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Right;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Left - pacRect.Width;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Bottom;
                    }

            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Top - pacRect.Height;
                    }
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
                if (exitRect.Contains(mouseState.X, mouseState.Y))
                    Exit();
            if (exitRect.Contains(pacRect))
                Exit();
            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}