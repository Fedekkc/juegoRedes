﻿using juegoRedes.PlayerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace juegoRedes.Stages
{
    public class Scene
    {
        // Escenas a renderizar a lo largo del juego
        public string uid;
        public Texture2D background;
        private List<Clue> clues;

        // Array con los sprites y sus ubicaciones
        public List<Dictionary<string, object>> sprites;

        // Variables para manejar las puertas de la escena
        public List<Dictionary<string, object>> doors;

        private PlayerClass.Player player;
        private bool isLocked = true;
        private bool isActive = false;
        public List<InteractionZone> InteractionZones { get; set; } = new List<InteractionZone>();
        public List<Rectangle> limits = new List<Rectangle>();


        // Constructor con parámetros opcionales
        public Scene(
            string uid,
            Texture2D background,
            List<Clue> clues,
            List<Dictionary<string, object>> sprites,
            List<Dictionary<string, object>> doors,
            PlayerClass.Player player,
            bool isLocked,
            List<InteractionZone> interactionZones,
            List<Rectangle> limits
        )
        {
            this.uid = uid;
            this.background = background;
            this.clues = clues;
            this.sprites = sprites;
            this.doors = doors;
            this.player = player;
            this.isLocked = isLocked;
            this.isActive = false;
            this.InteractionZones = interactionZones;
            this.limits = limits;
        }

        // Métodos para obtener las propiedades
        public string getUid() => uid;
        public Texture2D getBackground() => background;
        public List<Clue> GetClues() => clues;
        public List<Dictionary<string, object>> GetSprites() => sprites;
        public string Uid => uid;
        public Texture2D Background => background;


        public void OnSceneStart()
        {
            if (this.Uid == "bar" || this.Uid == "crimeScene")
            {
                player.X = 200;
                player.Y = 350;
                player.Direction = "up";
            } else if (this.Uid == "casa")
            {
                player.X = 200;
                player.Y = 320;
                player.Direction = "up";
            } else if (this.Uid == "cancha")
            {
                player.X = 290;
                player.Y = 100;
                player.Direction = "down";
            } 
            
            isActive = true;
        } 


        // Método para dibujar la escena
        public void Draw(SpriteBatch spriteBatch)
        {
            // Dibuja el fondo
            try
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                // Dibuja los sprites
                foreach (var sprite in sprites)
                {
                    if (sprite.ContainsKey("texture") && sprite.ContainsKey("position"))
                    {
                        Texture2D texture = (Texture2D)sprite["texture"];
                        Vector2 position = (Vector2)sprite["position"];
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }

                // Dibuja las puertas
                foreach (var door in doors)
                {
                    if (door.ContainsKey("texture") && door.ContainsKey("position"))
                    {
                        Texture2D doorTexture = (Texture2D)door["texture"];
                        Vector2 doorPosition = (Vector2)door["position"];
                        spriteBatch.Draw(doorTexture, doorPosition, Color.White);
                    }
                }

                foreach (var clue in clues)
                {
                    spriteBatch.Draw(clue.Texture, clue.Position, Color.White);
                    

                }

            } catch (Exception e)
            {
                
                   Console.WriteLine(e);

            }

        }

        public Rectangle GetSpriteBounds(Dictionary<string, object> sprite)
        {
            if (sprite.ContainsKey("texture") && sprite.ContainsKey("position"))
            {
                Texture2D texture = (Texture2D)sprite["texture"];
                Vector2 position = (Vector2)sprite["position"];
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
            
            return Rectangle.Empty;
        }

        public void CheckCollisions(Player player)
        {
            Rectangle playerBounds = player.Bounds;

            foreach (var sprite in sprites)
            {
                Rectangle spriteBounds = GetSpriteBounds(sprite);

                // Si los rectángulos del jugador y el sprite se superponen
                if (playerBounds.Intersects(spriteBounds))
                {
                    Console.WriteLine($"Colisión detectada con sprite en posición {spriteBounds.Location}");
                    HandleCollision(player, sprite); // Maneja la colisión (ver siguiente paso)
                }
            }
        }

        private void HandleCollision(Player player, Dictionary<string, object> sprite)
        {
          
            if (sprite.ContainsKey("name") && sprite["name"].ToString() == "bartender")
            {
                Console.WriteLine("Colisión con el bartender. ¡Inicia diálogo!");
            }
        }


        // Método para actualizar la escena
        public void Update(GameTime gameTime)
        {
            // Si la escena no está bloqueada, actualizamos los sprites y las puertas
            if (!isLocked)
            {
                
                foreach (Dictionary<string, object> sprite in sprites)
                {
                    
                }

                
                foreach (Dictionary<string, object> door in doors)
                {
                    
                }
            }
        }
    }
}
