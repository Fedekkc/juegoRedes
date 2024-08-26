using juegoRedes.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace juegoRedes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;

        private Player pj;

        public const int SCREEN_WIDTH = 1920;
        public const int SCREEN_HEIGHT = 1080;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carga la textura del background y del player
            background = Content.Load<Texture2D>("background");
            Texture2D playerTexture = Content.Load<Texture2D>("player");

            // Inicializa al jugador con su textura y posición inicial
            pj = new Player("Player1", SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2, playerTexture, 10, 0.1f, 4);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Maneja la entrada del jugador
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pj.movePlayer("up");
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                pj.movePlayer("down");
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                pj.movePlayer("left");
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                pj.movePlayer("right");
            }

            // Actualiza el jugador
            pj.update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Dibujo el background
            _spriteBatch.Draw(background, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

            // Dibujo al jugador
            pj.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
