using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using System.Threading.Tasks;

namespace juegoRedes
{
    internal class Dialog
    {
        private string title;        // Título del diálogo
        private string text;         // Texto completo del diálogo
        private string displayedText = ""; // Texto que se mostrará progresivamente
        private SpriteFont font;     // Fuente para dibujar el texto
        private Vector2 position;    // Posición en la pantalla
        private bool isDisplaying;   // Indica si el texto se está mostrando actualmente
        private int charIndex;       // Índice del carácter actual
        private int delay;           // Retardo entre cada letra en milisegundos
        private Texture2D dialogSquare;
        private bool isVisible;        

        public Dialog(string title, string text, Texture2D square, SpriteFont font, Vector2 position, int delay = 50, Texture2D dialogSquare = null)
        {
            this.title = title;
            this.text = text;
            this.font = font;
            this.position = position;
            this.delay = delay;
            this.isDisplaying = false;
            this.charIndex = 0;
            this.dialogSquare = square;

        }

        // Lógica para mostrar el texto letra por letra
        public async void ShowLetterByLetter()
        {

            isDisplaying = true;
            displayedText = "";
            charIndex = 0;
            isVisible = true;

            while (charIndex < text.Length)
            {
                displayedText += text[charIndex];
                charIndex++;
                await Task.Delay(delay); // Espera entre cada letra
            }

            isDisplaying = false; // Termina el efecto
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void Hide()
        {
            displayedText = "";
            isVisible = false; // Cambiar la visibilidad del diálogo.
            charIndex = 0; // Reiniciar el índice de caracteres.
            isDisplaying = false; // Detener cualquier visualización activa.
        }

        public bool IsVisible()
        {
            return isVisible;
        }


        // Método para dibujar el texto
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(dialogSquare, new Vector2(51, 340), Color.White);
                spriteBatch.DrawString(font, displayedText, new Vector2(65, 350), Color.Black);
            }
        }

    }
}
