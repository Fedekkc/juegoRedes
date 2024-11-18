using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace juegoRedes.Stages
{
    public class SceneManager
    {
        private List<Scene> scenes;

        public SceneManager()
        {
            scenes = new List<Scene>();
            CurrentScene = null;
        }

        // Añadir una escena a la lista
        public void AddScene(Scene scene)
        {
            scenes.Add(scene);
        }

        // Cambiar la escena actual
        public void ChangeScene(string sceneUid)
        {
            Scene newScene = scenes.Find(scene => scene.getUid() == sceneUid);
            if (newScene != null)
            {
                CurrentScene = newScene;
                CurrentScene.OnSceneStart();  // Llamada cuando empieza la escena
            }
        }

        // Actualizar la escena actual
        public void Update(GameTime gameTime)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Update(gameTime);  // Actualiza la escena
            }
        }

        // Dibujar la escena actual
        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentScene != null)
            {
                try
                {
                    Console.WriteLine("lautaro puta2");
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    // Dibujar el fondo de la escena
                    spriteBatch.Draw(CurrentScene.getBackground(), Vector2.Zero, Color.White);

                    // Dibujar los demás elementos de la escena
                    // Asegúrate de que CurrentScene.Draw no llame a Begin() nuevamente
                    Console.WriteLine("lautaro puta");
                    CurrentScene.Draw(spriteBatch);

                    spriteBatch.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine("lautaro puta3");
                    Console.WriteLine(e);
                }

            }
        }

        // Método para obtener la escena actual
        public Scene CurrentScene { get; private set; }
    }
}
