﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace juegoRedes.PlayerClass
{
    public class Player
    {
        // Atributos

        private string name;
        private string[] tools = new string[3];
        private Clue[] clues = new Clue[10];
        private Texture2D texture;

        public PlayerAnimation walkUpAnim;
        public PlayerAnimation walkDownAnim;
        public PlayerAnimation walkLeftAnim;
        public PlayerAnimation walkRightAnim;

        private PlayerAnimation currentAnimation;
        private string currentDirection;

        // coordenadas
        private int x;
        private int y;

        public Player(string name, int x, int y, Texture2D texture, int framesPerRow, float frameTime = 0.08f)
        {
            // Constructor
            this.name = name;
            this.x = x;
            this.y = y;
            this.texture = texture;


            // Inicializa las animaciones
            walkUpAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 1); // Asumiendo que la primera fila es para caminar hacia arriba
            walkDownAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 0); // Asumiendo que la segunda fila es para caminar hacia abajo
            walkLeftAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 2); // Asumiendo que la tercera fila es para caminar hacia la izquierda
            walkRightAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 3); // Asumiendo que la cuarta fila es para caminar hacia la derecha

            currentAnimation = walkDownAnim; // Animación inicial
            currentDirection = "down"; // Dirección inicial
        }

        public void movePlayer(string direction, int speed = 3, int screenWidth = 400, int screenHeight = 400, List<Rectangle> obstacles = null)
        {
            Rectangle futureBounds = this.Bounds; // Usamos los bounds actuales del jugador

            if (direction == "up")
            {
                futureBounds.Y -= speed; // Calcula la nueva posición futura
                currentAnimation = walkUpAnim;
            }
            else if (direction == "down")
            {
                futureBounds.Y += speed; // Calcula la nueva posición futura
                currentAnimation = walkDownAnim;
            }
            else if (direction == "left")
            {
                futureBounds.X -= speed; // Calcula la nueva posición futura
                currentAnimation = walkLeftAnim;
            }
            else if (direction == "right")
            {
                futureBounds.X += speed; // Calcula la nueva posición futura
                currentAnimation = walkRightAnim;
            }

            // Si hay obstáculos y el jugador está colisionando, no se mueve
            if (obstacles != null && !IsCollidingWithObstacles(futureBounds, obstacles))
            {
                // Solo actualizamos la posición si no colisiona
                this.x = futureBounds.X;
                this.y = futureBounds.Y;
            }

            // Limita la posición del jugador a no salir de la pantalla
            this.x = Math.Clamp(this.x, 0, screenWidth - currentAnimation.CurrentFrameRectangle.Width);
            this.y = Math.Clamp(this.y, 0, screenHeight - currentAnimation.CurrentFrameRectangle.Height);
        }

        private bool IsCollidingWithObstacles(Rectangle futureBounds, List<Rectangle> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                if (futureBounds.Intersects(obstacle)) // Si colisiona con cualquier obstáculo
                {
                    // excluir la colisión con el background
                    if (obstacle.Width == 400 && obstacle.Height == 400)
                    {
                        continue;
                    }

                    return true;
                }
            }
            return false; // No hay colisión
        }

        public void ResetPosition()
        {
            // Restablece la posición del jugador
            this.x = 0;
            this.y = 0;
        }

        public bool isColliding(Rectangle other)
        {
            return Bounds.Intersects(other);
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)x, (int)y, texture.Width, texture.Height);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.x, this.y);

            // Si el jugador está quieto, mostrar el primer frame
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                // Mantener el primer frame de la animación de la dirección actual
                currentAnimation.Draw(spriteBatch, position, staticFrame: true);
            }
            else
            {
                // Animación normal en movimiento
                currentAnimation.Draw(spriteBatch, position);
            }
        }

        public void Update(float deltaTime, List<Rectangle> obstacles)
        {
            Console.WriteLine("Update player");
            currentAnimation.update(deltaTime);

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                movePlayer("up", obstacles: obstacles);
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                movePlayer("down", obstacles: obstacles);
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                movePlayer("left", obstacles: obstacles);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                movePlayer("right", obstacles: obstacles);
            }
        }


    }
}
