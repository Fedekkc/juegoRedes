using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace juegoRedes
{
    public class Clue
    {
        private string name;
        private string description;
        private string time_found;
        private string place_found;
        private string tool_found;
        private Rectangle area;
        private Vector2 position;
        private Texture2D texture;

        public string Name => name;
        public string Description => description;
        public string Time_found => time_found;
        public string Place_found => place_found;
        public string Tool_found => tool_found;

        public Texture2D Texture => texture;
        
        public Vector2 Position => position;
        






        public Clue(string name, Texture2D texture, Vector2 position, string description, string time_found, string place_found, string tool_found, Rectangle area)
        {
            this.name = name;

            this.description = description;
            this.time_found = time_found;
            this.place_found = place_found;
            this.tool_found = tool_found;
            this.area = area;
            this.position = position;
            this.texture = texture;
        }

        public Rectangle Area => area;

         

    }

}
