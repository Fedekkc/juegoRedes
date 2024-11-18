using juegoRedes.PlayerClass;
using juegoRedes.Stages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace juegoRedes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;
        private Texture2D playerTexture;
        private Texture2D startButton;
        private Player pj;
        private SceneManager sceneManager;  // Gestor de escenas

        public const int SCREEN_WIDTH = 1920;
        public const int SCREEN_HEIGHT = 1080;

        private Dialog dialog;       // Objeto para manejar el diálogo
        private SpriteFont font;     // Fuente para el texto del diálogo

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial");
            background = Content.Load<Texture2D>("intro");  // Fondo de la intro
            startButton = Content.Load<Texture2D>("newGameButton");  // Cargar el botón de "Nueva Partida"
            playerTexture = Content.Load<Texture2D>("player");  

            // Inicializa el diálogo
            string text = "Bienvenido al mundo del juego. Presiona 'Nueva Partida' para comenzar.";
            Vector2 dialogPosition = new Vector2(100, 800); // Posición del texto
            dialog = new Dialog("Bienvenida", text, font, dialogPosition, 50);
            dialog.ShowLetterByLetter();  // Muestra el texto letra por letra

            // Crear las escenas
            sceneManager = new SceneManager();

            // Escena de introducción
            Scene introScene = new Scene("intro", background, new List<Clue>(), new List<Dictionary<string, object>>(), new List<Dictionary<string, object>>(), null, false);
            sceneManager.AddScene(introScene);

            // Escena del juego
            Player player = new Player("PlayerName", 100, 100, playerTexture, 10, 10);

            Scene gameScene = new Scene("game", null, new List<Clue>(), new List<Dictionary<string, object>>(), new List<Dictionary<string, object>>(), player, false);
            sceneManager.AddScene(gameScene);

            // Iniciar con la escena de introducción
            sceneManager.ChangeScene("intro");
        }

        protected override void Update(GameTime gameTime)
        {
            // Salir del juego si se presiona Escape
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Actualizar la escena activa
            sceneManager.Update(gameTime);

            // Si estamos en la escena de introducción, detectar clic en el botón de inicio
            if (sceneManager.CurrentScene.getUid() == "intro")
            {
                MouseState mouseState = Mouse.GetState();
                if (startButton != null && new Rectangle(500, 600, startButton.Width, startButton.Height).Contains(mouseState.X, mouseState.Y))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        // Cambiar a la escena del juego al presionar el botón
                        sceneManager.ChangeScene("game");
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Dibujar la escena activa
            sceneManager.Draw(_spriteBatch);

            // Si estamos en la escena de introducción, dibujar el botón de "Nueva Partida"
            if (sceneManager.CurrentScene.getUid() == "intro")
            {
                _spriteBatch.Draw(startButton, new Vector2(500, 600), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
