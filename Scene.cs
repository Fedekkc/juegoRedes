using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace juegoRedes
{
    internal class Scene
    {
        //Escenas a renderizar a lo largo del juego
        private string uid;
        private Texture2D background;
        private List<Clue> clues;
        //Defino un array con los sprites que se van a renderizar en la escena, sus ubicaciones y sus texturas
        private List<Dictionary<string, object>> sprites;

        //Defino una variable que va a handlear las puertas de la escena
        private List<Dictionary<string, object>> doors;

        private PlayerClass.Player player;
        private Boolean isLocked = true;




        //Generamos un constructor con parametros opcionales
        public Scene(string uid, Texture2D background, List<Clue> clues, List<Dictionary<string, object>> sprites, List<Dictionary<string, object>> doors, PlayerClass.Player player, bool isLocked)
        {
            this.uid = uid;
            this.background = background;
            this.clues = clues;
            this.sprites = sprites;
            this.doors = doors;
            this.player = player;
            this.isLocked = isLocked;
        }




        public string getUid()
        {
            return this.uid;
        }
    
        public Texture2D getBackground()
        {
            return this.background;
        }

        public List<Clue> getClues()
        {
            return this.clues;
        }

        public List<Dictionary<string, object>> getSprites()
        {
            return this.sprites;
        }



    }

    
}
