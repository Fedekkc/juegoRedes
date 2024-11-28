using juegoRedes.Stages;
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace juegoRedes
{
    internal class Map
    {
        private Texture2D mapTexture; // Textura del mapa
        private Vector2 position; // Posición del mapa en pantalla
        private bool isVisible; // Indica si el mapa está visible
        private SpriteFont font; // Fuente para escribir texto en el mapa
        private Vector2 markerPosition = new Vector2(200, 200); // Posición del marcador en el mapa
        private List<InteractionZone> interactionZones = new List<InteractionZone>(); // Lista de zonas de interacción
        private SceneManager sceneManager; // Administrador de escenas
        


        public Map(Texture2D texture, SpriteFont font, Vector2 position, List<InteractionZone> interactionZones, SceneManager sceneManager)
        {
            this.mapTexture = texture;
            this.position = position;
            this.isVisible = false;
            this.font = font;
            this.interactionZones = interactionZones;
            this.sceneManager = sceneManager;
        }
        



        public void Show()
        {
            isVisible = true;
        }


        public void Hide()
        {
            isVisible = false;
        }


        public bool IsVisible()
        {
            return isVisible;
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)markerPosition.X, (int)markerPosition.Y, 16, 16);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition, Texture2D pointer)
        {
            if (isVisible)
            {
                spriteBatch.Draw(mapTexture, new Vector2(0,0), Color.White);

               spriteBatch.Draw(pointer, markerPosition, Color.White);
            }
        }

        public void moveMarker( string direction)
        {
            switch (direction)
            {
                case "up":
                    markerPosition.Y -= 5;
                    break;
                case "down":
                    markerPosition.Y += 5;
                    break;
                case "left":
                    markerPosition.X -= 5;
                    break;
                case "right":
                    markerPosition.X += 5;
                    break;
            }
         



        }

        public void checkInteractionZones(Vector2 markerPosition)
        {
            foreach (InteractionZone zone in interactionZones)
            {
                if (zone.Area.Intersects(Bounds))
                {
                    sceneManager.ChangeScene(zone.Action);                   
                    this.Hide();


                }
                
            }
        }

        public void Update(float deltaTime)
        {
           
            if (isVisible)
            {
                KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    moveMarker("up");
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    moveMarker("down");
                }
                else if (keyboardState.IsKeyDown(Keys.Left))
                {
                    moveMarker("left");
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    moveMarker("right");
                }
                
                markerPosition.X = MathHelper.Clamp(markerPosition.X, 0, 385);
                markerPosition.Y = MathHelper.Clamp(markerPosition.Y, 0, 385);

                if(keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space))
                {
                    checkInteractionZones(markerPosition);
                }





            }


        }
    }
}
