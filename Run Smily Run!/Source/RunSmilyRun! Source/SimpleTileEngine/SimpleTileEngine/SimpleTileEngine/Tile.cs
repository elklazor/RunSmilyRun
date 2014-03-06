using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SimpleTileEngine
{
    public class Tile
    {
        public int tileID { get; set; }
        public Vector2 tilePosition { get; set; }
        public Rectangle tileRectangle { get; set; }
        public Point tileSize{ get; set; }
        public bool noCollide { get; set; }
        public int slowBoost { get; set; }
        public string tileName { get; set; }
        public int tileType { get; set; }
        
    }
}
