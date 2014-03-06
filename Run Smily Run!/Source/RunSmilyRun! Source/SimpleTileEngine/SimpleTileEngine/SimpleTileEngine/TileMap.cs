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
using System.IO;

namespace SimpleTileEngine
{
    public class TileMap
    {
        List<Texture2D> tiles;
        public int[,] tileMap;
        public List<Tile> solidTilesList = new List<Tile>();
        public List<Tile> tilesList = new List<Tile>();
        int tileWidth = 12;
        int tileHeight = 12;
        public static bool hasWon = false;
        public TileMap(string name, ContentManager content)
        {
            LoadTileTextures(content);
            LoadMapData(name,content);
        }

        void LoadTileTextures(ContentManager content)
        {
            tiles = new List<Texture2D>();
            tiles.Add(content.Load<Texture2D>("Textures\\Sky"));
            tiles.Add(content.Load<Texture2D>("Textures\\Grass"));
            tiles.Add(content.Load<Texture2D>("Textures\\Dirt"));
            tiles.Add(content.Load<Texture2D>("Textures\\Rock"));
            tiles.Add(content.Load<Texture2D>("Textures\\ERROR"));
            tiles.Add(content.Load<Texture2D>("Textures\\Start"));
            tiles.Add(content.Load<Texture2D>("Textures\\Finish"));
            
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            
            for (int y = 0; y < tileMap.GetLength(0); y++)
            {
                for (int x = 0; x < tileMap.GetLength(1); x++)
                {
                    
                    spriteBatch.Draw(tiles[tileMap[y, x]], new Vector2(x * tileWidth, y * tileHeight), Color.White);
                    
                }
            }
        }

        void LoadMapData(string name,ContentManager content)
        {
            string path = content.RootDirectory + @"\MapLayouts/" + name + ".txt";

            int width = 0;
            int height = File.ReadLines(path).Count();

            StreamReader sReader = new StreamReader(path);
            string line = sReader.ReadLine();
            string[] tileNo = line.Split(',');
            width = tileNo.Count();
            

            tileMap = new int[height, width];
            sReader.Close();

            sReader = new StreamReader(path);
            int counter = 0;
            for (int y = 0; y < height; y++)
            {
                
                line = sReader.ReadLine();
                tileNo = line.Split(',');
                counter++;
                for (int x = 0; x < width; x++)
                {
                    try
                    {
                        tileMap[y, x] = Convert.ToInt32(tileNo[x]);
                    }
                    catch(Exception e)
                    {
                        string f = e.Message;
                        tileMap[y, x] = 4;
                    }
                    if (Convert.ToInt32(tileNo[x]) > 6)
                        tileMap[y, x] = 4;

                    Tile tile = new Tile();
                    tile.tileID = counter;
                    tile.tilePosition = new Vector2(x * tileWidth, y * tileHeight);
                    tile.tileSize = new Point(tileWidth, tileHeight);
                    tile.tileRectangle = new Rectangle((int)tile.tilePosition.X, (int)tile.tilePosition.Y, tile.tileSize.X, tile.tileSize.Y);
                    tile.slowBoost = 0;
                    tile.tileType = Convert.ToInt32(tileNo[x]);
                    if (tileNo[x] == Convert.ToString(7) || tileNo[x] == Convert.ToString(8))
                        tile.noCollide = true;
                    else
                        tile.noCollide = false;
                                        
                    tilesList.Add(tile);
                                        
                    if (tile.tileType != 0)
                        solidTilesList.Add(tile);
                    
                }
            }
            sReader.Close();
        }
   

    
    }
}
