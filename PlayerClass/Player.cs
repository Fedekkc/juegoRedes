using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace juegoRedes.PlayerClass
{
    internal class Player
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

        // coordenadas
        private int x;
        private int y;

        public Player(string name, int x, int y, Texture2D texture, int framesPerRow, float frameTime, int totalRows)
        {
            // Constructor
            this.name = name;
            this.x = x;
            this.y = y;
            this.texture = texture;

            // Inicializa las animaciones
            walkUpAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 6); // Asumiendo que la primera fila es para caminar hacia arriba
            walkDownAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 4); // Asumiendo que la segunda fila es para caminar hacia abajo
            walkLeftAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 5); // Asumiendo que la tercera fila es para caminar hacia la izquierda
            walkRightAnim = new PlayerAnimation(texture, framesPerRow, frameTime, 7); // Asumiendo que la cuarta fila es para caminar hacia la derecha

            currentAnimation = walkDownAnim; // Animación inicial
        }

        public void movePlayer(string direction)
        {
            if (direction == "up")
            {
                currentAnimation = walkUpAnim;
                this.y -= 1;
            }
            else if (direction == "down")
            {
                currentAnimation = walkDownAnim;
                this.y += 1;
            }
            else if (direction == "left")
            {
                currentAnimation = walkLeftAnim;
                this.x -= 1;
            }
            else if (direction == "right")
            {
                currentAnimation = walkRightAnim;
                this.x += 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.x, this.y);
            currentAnimation.Draw(spriteBatch, position);
        }

        public void update(float deltaTime)
        {
            currentAnimation.update(deltaTime);
        }
    }
}
