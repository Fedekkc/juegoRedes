using juegoRedes.PlayerClass;
using juegoRedes.Stages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace juegoRedes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D bar;
        private Texture2D playerTexture;
        private Texture2D startButton;
        private Texture2D bartender;
        private Texture2D mostrador;
        private Texture2D door;
        private Texture2D dialogSquare;
        private Texture2D crimescene;
        private Texture2D taburete;
        private Player player;
        private SceneManager sceneManager;  // Gestor de escenas
        private StreamWriter logWriter;    // Escritor de log
        public const int SCREEN_WIDTH = 400;
        public const int SCREEN_HEIGHT = 400;
        private Dialog dialog;       // Objeto para manejar el diálogo
        private SpriteFont font;     // Fuente para el texto del diálogo
        private KeyboardState previousKeyboardState;


        private List<Scene> scenes = new List<Scene>();

        public Game1()
        {
            previousKeyboardState = Keyboard.GetState();

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
            bar = Content.Load<Texture2D>("bar");
            startButton = Content.Load<Texture2D>("newGameButton");
            playerTexture = Content.Load<Texture2D>("player");
            bartender = Content.Load<Texture2D>("barguy_front");
            door = Content.Load<Texture2D>("alfombra");
            mostrador = Content.Load<Texture2D>("mostrador");
            dialogSquare = Content.Load<Texture2D>("dialogSquare");
            crimescene = Content.Load<Texture2D>("crimescene 2x");
            taburete = Content.Load<Texture2D>("barchair");

            string text = "Bienvenido al mundo del juego. Presiona 'Nueva Partida' para comenzar.";
            Vector2 dialogPosition = new Vector2(100, 200); // Posición del texto
            dialog = new Dialog("Bienvenida", text, dialogSquare, font, dialogPosition, 50);

            string path = Directory.GetCurrentDirectory();
            logWriter = new StreamWriter(path + "../../../../Log/Log.log", true);

            // Configuración de escenas y elementos de juego
            ConfigureScenes();

            player = new Player("BetaTester", 200, 200, playerTexture, 3, 0.1f);
        }

        private void ConfigureScenes()
        {
            List<Dictionary<string, object>> sprites = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "texture", bartender },
                    { "position", new Vector2(130, 70) },
                    { "name", "bartender" },
                },
                new Dictionary<string, object>
                {
                    { "texture", mostrador },
                    { "position", new Vector2(50, 90) },
                    { "name", "mostrador" },
                },
                new Dictionary<string, object>
                {
                    { "texture", taburete },
                    { "position", new Vector2(192, 118) },
                    { "name", "taburete" },
                }
            };

            List<Dictionary<string, object>> doors = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "texture", door },
                    { "position", new Vector2(200, 370) },
                    { "name", "door_bar" },
                    { "scene", "bar" },
                }
            };

            Scene barScene = new Scene(
                "bar",
                bar,
                new List<Clue>(),
                sprites,
                doors,
                null,
                false,
                new List<InteractionZone>
                {
                    new InteractionZone(new Rectangle(150, 70, bartender.Width, bartender.Height + 50), "talkToBartender"),
                    new InteractionZone(new Rectangle(200, 400, door.Width, door.Height), "exitBar"),
                    new InteractionZone(new Rectangle(50, 90, mostrador.Width, mostrador.Height), "inspectCounter"),
                },
                new List<Rectangle>
                {
                    new Rectangle(300, 34, 50, 35), // Pared derecha
                    new Rectangle(460, 0, 30, 461), // Borde derecho
                    new Rectangle(0, 0, 40, 461)    // Borde izquierdo
                }
            );
            scenes.Add(barScene);

            Scene crimeScene = new Scene(
                "crimeScene",
                crimescene,
                new List<Clue>(),
                new List<Dictionary<string, object>>(),
                new List<Dictionary<string, object>>(),
                null,
                false,
                new List<InteractionZone>(),
                new List<Rectangle>
                {
                    new Rectangle(300, 34, 50, 55)
                }
            );
            scenes.Add(crimeScene);

            sceneManager = new SceneManager(scenes, barScene);
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Detectar si Space fue recién presionado
            bool spaceJustPressed = currentKeyboardState.IsKeyDown(Keys.Space) &&
                                    !previousKeyboardState.IsKeyDown(Keys.Space);

            bool EJustPressed = currentKeyboardState.IsKeyDown(Keys.E) &&
                                    !previousKeyboardState.IsKeyDown(Keys.E);

            List<Rectangle> obstacles = new List<Rectangle>();
            foreach (var sprite in sceneManager.CurrentScene.sprites)
            {
                if (sprite.ContainsKey("position") && sprite.ContainsKey("texture"))
                {
                    Texture2D texture = (Texture2D)sprite["texture"];
                    Vector2 position = (Vector2)sprite["position"];
                    obstacles.Add(new Rectangle((int)position.X + 110, (int)position.Y, texture.Width - 125, texture.Height - 30));
                }
            }

            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds, obstacles, limits: sceneManager.CurrentScene.limits);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                logWriter.Close();
                Exit();
            }




            if (spaceJustPressed)
            {
                if (player.isInteracting)
                {
                    dialog.Hide();
                    player.isInteracting = false;
                }
                else
                {
                    foreach (var zone in sceneManager.CurrentScene.InteractionZones)
                    {
                        if (zone.Area.Intersects(player.Bounds))
                        {
                            HandleInteraction(zone.Action);
                            logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha interactuado con {zone.Action}.");
                            logWriter.Flush();
                            break;  
                        }
                    }
                }
            }

            sceneManager.Update(gameTime);

            // Actualizar el estado previo del teclado
            previousKeyboardState = currentKeyboardState;
            base.Update(gameTime);
        }

        private void HandleInteraction(string action)
        {
            switch (action)
            {
                case "talkToBartender":
                    dialog.SetText("Hola, ¿qué tal? \n¿Quieres algo de beber?");
                    dialog.ShowLetterByLetter();
                    player.isInteracting = true;
                    break;

                case "exitBar":
                    sceneManager.ChangeScene("crimeScene");
                    break;

                case "inspectCounter":
                    dialog.SetText("El mostrador está limpio, \npero hay  huellas de vasos.");
                    dialog.ShowLetterByLetter();
                    player.isInteracting = true;
                    break;

                default:
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            sceneManager.CurrentScene.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            if (dialog.IsVisible())
            {
                dialog.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
