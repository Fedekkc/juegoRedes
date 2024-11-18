using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using juegoRedes.PlayerClass;



namespace juegoRedes.Stages
{
    internal class IntroScene : Scene
    {
        public IntroScene
            (string uid, 
            Texture2D background, 
            List<Clue> clues, 
            List<Dictionary<string, object>> sprites, 
            List<Dictionary<string, object>> doors, 
            PlayerClass.Player player, 
            bool isLocked) : base(uid, background, clues, sprites, doors, player, isLocked) // base es el constructor de la clase padre Scene
        {





        }
    }   

}
