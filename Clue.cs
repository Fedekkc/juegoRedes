using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegoRedes
{
    public class Clue
    {
        private string name;
        private string description;
        private string time_found;
        private string place_found;
        private string tool_found;



        public Clue(string name, string description, string time_found, string place_found, string tool_found)
        {
            this.name = name;
            this.description = description;
            this.time_found = time_found;
            this.place_found = place_found;
            this.tool_found = tool_found;
        }

    }

}
