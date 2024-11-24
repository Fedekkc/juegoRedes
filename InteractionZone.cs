using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;




public class InteractionZone
{
    public Rectangle Area { get; set; }
    public string Action { get; set; }  // Acción asociada (por ejemplo, "talk", "openDoor", etc.)

    public InteractionZone(Rectangle area, string action)
    {
        Area = area;
        Action = action;
    }
}
