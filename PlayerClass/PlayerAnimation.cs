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
        private readonly int totalRows = 8;

        public PlayerAnimation(Texture2D texture, int frames, float frameTime, int row)
        {
            _texture = texture;
            _frames = frames;
            this.frameTime = frameTime;
            var width = texture.Width / frames;
            var height = texture.Height / totalRows; // totalRows es el número total de filas en sprites.png
            for (var i = 0; i < frames; i++)
            {
                _sourceRectangles.Add(new Rectangle(i * width, row * height, width, height));
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, position, _sourceRectangles[frame], Color.White);
        }
    }
}
