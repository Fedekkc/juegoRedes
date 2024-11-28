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
        private Texture2D casa;
        private Texture2D cancha;
        private Texture2D taburete;
        private Texture2D clue;
        private Texture2D mapTexture;
        private Texture2D pointer;
        private Player player;
        private Map map;
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
            clue = Content.Load<Texture2D>("clue");
            mapTexture = Content.Load<Texture2D>("map");
            pointer = Content.Load<Texture2D>("pointer");
            casa = Content.Load<Texture2D>("home2x");
            cancha = Content.Load<Texture2D>("cancha2x");

            player = new Player("BetaTester", 200, 200, playerTexture, 3, 0.1f);
            ConfigureScenes();

            List<InteractionZone> localizaciones = new List<InteractionZone>
            {
                new InteractionZone(new Rectangle(26, 83, 74, 35), "cancha"),
                new InteractionZone(new Rectangle(310, 33, 42, 56), "crimeScene"),
                new InteractionZone(new Rectangle(250, 140, 47, 50), "bar"),
                new InteractionZone(new Rectangle(232, 294, 62, 65), "casa")

            };

            map = new Map(mapTexture, font, new Vector2(50, 50), localizaciones, sceneManager);
            


            string text = "Bienvenido al mundo del juego. Presiona 'Nueva Partida' para comenzar.";
            Vector2 dialogPosition = new Vector2(100, 200); // Posición del texto
            dialog = new Dialog("Bienvenida", text, dialogSquare, font, dialogPosition, 50);

            string path = Directory.GetCurrentDirectory();
            logWriter = new StreamWriter(path + "../../../../Log/Log.log", true);

            // Configuración de escenas y elementos de juego
            

            
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
                new List<Clue>
                {
                    new Clue("Billetera", clue, new Vector2(200, 200), "Billetera encontrada en el Piso", "12:00", "Bar", "Lupa", new Rectangle(200, 200, clue.Width, clue.Height)),
                                      
                },
                sprites,
                doors,
                player,
                false,
                new List<InteractionZone>
                {
                    new InteractionZone(new Rectangle(150, 70, bartender.Width, bartender.Height + 50), "talkToBartender"),
                    new InteractionZone(new Rectangle(200, 370, door.Width, door.Height), "exit"),
                    new InteractionZone(new Rectangle(50, 90, mostrador.Width, mostrador.Height), "inspectCounter"),
                },
                new List<Rectangle>
                {
                    new Rectangle(300, 34, 50, 35), // Pared derecha
                    new Rectangle(360, 0, 50, 461), // Borde derecho
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
                player,
                false,
                new List<InteractionZone>(),
                new List<Rectangle>
                {
                    new Rectangle(300, 34, 50, 55)

                }
            );
            scenes.Add(crimeScene);

            Scene canchaScene = new Scene(
                "cancha",
                cancha,
                new List<Clue>(),
                new List<Dictionary<string, object>>(),
                new List<Dictionary<string, object>>(),
                player,
                false,
                new List<InteractionZone>
                {
                    new InteractionZone(new Rectangle(292, 50, 40, 40), "exit"),
                },
                new List<Rectangle>
                {
                    new Rectangle(365, 0, 50, 400),
                    new Rectangle(0, 0, 290, 95),
                    new Rectangle(0, 0, 123, 275),
                    new Rectangle(0, 0, 28, 400),
                    new Rectangle(290, 0, 100, 45),
                    new Rectangle(335, 45, 40, 50),
                    new Rectangle(0, 366, 400, 50),

                }
            );  
            scenes.Add(canchaScene);

            Scene casaScene = new Scene(
                "casa",
                casa,
                new List<Clue>(),
                new List<Dictionary<string, object>>(),
                new List<Dictionary<string, object>>(),
                player,
                false,
                new List<InteractionZone>(),
                new List<Rectangle>
                {
                    new Rectangle(350, 0, 50, 400),
                    new Rectangle(0, 0, 400, 90),
                    new Rectangle(0, 0, 50, 400),
                    new Rectangle(50, 315, 100, 85),
                    new Rectangle(150, 361, 105, 20),
                    new Rectangle(255, 315, 105, 80),

                    
                }
                );
            scenes.Add(casaScene);


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

            bool MJustPressed = currentKeyboardState.IsKeyDown(Keys.M) &&
                                    !previousKeyboardState.IsKeyDown(Keys.M);

            List<Rectangle> obstacles = new List<Rectangle>();
            foreach (var sprite in sceneManager.CurrentScene.sprites)
            {
                if (sprite.ContainsKey("position") && sprite.ContainsKey("texture"))
                {
                    Texture2D texture = (Texture2D)sprite["texture"];
                    Vector2 position = (Vector2)sprite["position"];
                    obstacles.Add(new Rectangle((int)position.X+10 , (int)position.Y, texture.Width-20 , texture.Height -30));
                }
            }

            if (!map.IsVisible())
            {
                player.Update((float)gameTime.ElapsedGameTime.TotalSeconds, obstacles, limits: sceneManager.CurrentScene.limits);

            }
            else
            {
                map.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                logWriter.WriteLine($"[{DateTime.Now}] - El jugador se ha movido a  {sceneManager.CurrentScene.Uid} " );
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                logWriter.Close();
                Exit();
            }

            if(EJustPressed)
            {
                
                foreach( var clue in sceneManager.CurrentScene.GetClues())
                {
                    logWriter.WriteLine(clue.Area);
                    logWriter.WriteLine(player.Bounds);

                    if (clue.Area.Intersects(player.Bounds))
                    {
                        dialog.SetText($"Prueba encontrada {clue.Name}\n{clue.Description}");
                        logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha encontrado la pista {clue.Name}.");
                        dialog.ShowLetterByLetter();
                        player.isInteracting = true;
                        break;
                    }
                }
            }

            if(MJustPressed)
            {
                if (map.IsVisible())
                {
                    logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha cerrado el mapa.");
                    map.Hide();
                }
                else
                {
                    logWriter.WriteLine($"[{DateTime.Now}] - El jugador ha abierto el mapa.");
                    map.Show();
                }
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

                case "exit":
                    //mostrar el mapa
                    logWriter.WriteLine($"[{DateTime.Now}] El mapa no deberia ser visible: {map.IsVisible()}");
                    player.setPosition(new Vector2(200, 200));
                    if (!map.IsVisible())
                    {
                        map.Show();
                        logWriter.WriteLine($"[{DateTime.Now}] ESTO DEBERIA SER TRUE: {map.IsVisible()} ");

                    }
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
            map.Draw(_spriteBatch, player.Position, pointer);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
