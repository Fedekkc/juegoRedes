using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace juegoRedes.PlayerClass
{
    public class PlayerAnimation
    {
        private readonly Texture2D _texture;
        private readonly List<Rectangle> _sourceRectangles = new();
        private readonly int _frames;
        private int frame;
        private readonly float frameTime;
        private float _frameTimeLeft;
        private bool _active = true;
        private readonly int totalColumns = 4;

        public PlayerAnimation(Texture2D texture, int frames, float frameTime, int column)
        {
            _texture = texture;
            _frames = frames;
            this.frameTime = frameTime;

            // Tamaño de cada frame
            var width = texture.Width / totalColumns; // Total de columnas
            var height = texture.Height / 3; // Total de filas (una por dirección)

            // Calcular los rectángulos fuente para la columna específica
            for (var i = 0; i < frames; i++)
            {
                _sourceRectangles.Add(new Rectangle(column * width, i * height, width, height));
            }

            _frameTimeLeft = frameTime; // Inicializa el tiempo del primer frame
        }


        public Rectangle CurrentFrameRectangle => _sourceRectangles[frame];

        public void stop()
        {
            _active = false;
        }

        public void start()
        {
            _active = true;
        }

        public void reset()
        {
            frame = 0;
            _frameTimeLeft = frameTime;
        }

        public void update(float deltaTime)
        {
            if (!_active)
            {
                return;
            }
            _frameTimeLeft -= deltaTime;
            if (_frameTimeLeft <= 0)
            {
                frame++;
                if (frame >= _frames)
                {
                    frame = 0;
                }
                _frameTimeLeft = frameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool staticFrame = false)
        {
            Rectangle sourceRectangle;

            if (staticFrame)
            {
                // Usar el primer frame de la animación
                sourceRectangle = _sourceRectangles[0];
            }
            else
            {
                // Usar el frame actual de la animación
                sourceRectangle = _sourceRectangles[frame];
            }

            spriteBatch.Draw(_texture, position, sourceRectangle, Color.White);
        }

    }
}
