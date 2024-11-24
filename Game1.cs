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
        Player player;
        private SceneManager sceneManager;  // Gestor de escenas
        private StreamWriter logWriter;    // Escritor de log
        public const int SCREEN_WIDTH = 400;
        public const int SCREEN_HEIGHT = 400;
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
            bar = Content.Load<Texture2D>("bar");  
            startButton = Content.Load<Texture2D>("newGameButton");  
            playerTexture = Content.Load<Texture2D>("player");
            bartender = Content.Load<Texture2D>("barguy_front");
            door = Content.Load<Texture2D>("alfombra");
            mostrador = Content.Load<Texture2D>("mostrador");
            dialogSquare = Content.Load<Texture2D>("dialogSquare");

            string text = "Bienvenido al mundo del juego. Presiona 'Nueva Partida' para comenzar.";
            Vector2 dialogPosition = new Vector2(100, 200); // Posición del texto
            dialog = new Dialog("Bienvenida", text, dialogSquare, font, dialogPosition, 50);
            dialog.Draw(_spriteBatch);

            string path = Directory.GetCurrentDirectory();
            logWriter = new StreamWriter( path + "../../../../Log/Log.log", true);



            // Definición de las escenas
            sceneManager = new SceneManager();




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

            Scene introScene = new Scene("bar", bar, new List<Clue>(), sprites, doors, null, false);
            sceneManager.AddScene(introScene);
            sceneManager.ChangeScene("bar");
            // Definir las zonas de interacción del bar
            List<InteractionZone> interactionZones = new List<InteractionZone>
            {
                new InteractionZone(new Rectangle(150, 70, bartender.Width, bartender.Height+50), "talkToBartender"),
                new InteractionZone(new Rectangle(200, 400, door.Width, door.Height), "exitBar"),
                new InteractionZone(new Rectangle(50, 90, mostrador.Width, mostrador.Height), "inspectCounter")
            };

            // Asignar las zonas de interacción a la escena actual
            sceneManager.CurrentScene.InteractionZones = interactionZones;

            player = new Player("PlayerName", 200, 200, playerTexture, 3,0.1f);

            Scene gameScene = new Scene("game", null, new List<Clue>(), new List<Dictionary<string, object>>(), new List<Dictionary<string, object>>(), player, false);
            sceneManager.AddScene(gameScene);

            sceneManager.ChangeScene("bar");
        }


        protected override void Update(GameTime gameTime)
        {
            logWriter.WriteLine("Update");

            List<Rectangle> obstacles = new List<Rectangle>();

            foreach (var sprite in sceneManager.CurrentScene.sprites)
            {
                if (sprite.ContainsKey("position") && sprite.ContainsKey("texture"))
                {
                    Texture2D texture = (Texture2D)sprite["texture"];
                    Vector2 position = (Vector2)sprite["position"];
                    obstacles.Add(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height));
                }
            }



            // Llamar a Update del jugador pasando los obstáculos
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds, obstacles);

            // Verificar si se presiona Escape para salir
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                logWriter.Close();
                Exit();
            }

            // Actualizar la escena actual
            sceneManager.Update(gameTime);
            // Verificar interacciones
            foreach (var zone in sceneManager.CurrentScene.InteractionZones)
            {
                if (zone.Area.Intersects(player.Bounds))
                {
                    if(Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        HandleInteraction(zone.Action);
                        logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha interactuado con {zone.Action}.");
                        logWriter.Flush();
                    }   
                }
            }




            base.Update(gameTime);
        }
        private void HandleInteraction(string action)
        {
            switch (action)
            {
                case "talkToBartender":
                    dialog.SetText("Hola! Que te traigo?");  // Cambiar texto del diálogo
                    dialog.ShowLetterByLetter();
                    break;

                case "exitBar":
                    logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha salido del bar.");
                    logWriter.Flush();
                    dialog.SetText("Hola! Que te traigo?");  // Cambiar texto del diálogo
                    dialog.ShowLetterByLetter();
                    break;

                case "inspectCounter":
                    dialog.SetText("El mostrador está limpio, pero hay huellas de vasos.");  // Texto descriptivo
                    dialog.ShowLetterByLetter();
                    break;

                default:
                    break;
            }
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Dibujar la escena activa
            sceneManager.CurrentScene.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            dialog.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
