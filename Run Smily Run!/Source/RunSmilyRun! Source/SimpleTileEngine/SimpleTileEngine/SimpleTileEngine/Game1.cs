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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileMap tileMap;
        Player player;
        SpriteFont spriteFont;
        Camera camera = new Camera();
 public gameState gState = gameState.MainMenu;
        Texture2D menuTexture;
        Texture2D cursorTexture;
        Texture2D spikesTexture;
        Texture2D deathTexture;
        Texture2D noButtonTexture;
        Texture2D yesButtonTexure;
        Texture2D winScreenTexture;
        Vector2 startPositionPlayer;

        SoundEffect soundClick;
        SoundEffectInstance soundClickInstance;
        SoundEffect soundLoose;
        SoundEffectInstance soundLooseInstance;
        SoundEffect winSound;

        MouseState mouseState = new MouseState();
        Rectangle startRect = new Rectangle(320, 310, 50, 30);
        Rectangle scoreRect = new Rectangle(310, 360, 70, 30);
        Rectangle exitRect = new Rectangle(360, 420, 70, 30);
        Rectangle yesRect = new Rectangle(590, 250, 200, 80);
        Rectangle noRect = new Rectangle(590, 340, 200, 80);
        Rectangle yesRect2 = new Rectangle(590, 50, 200, 80);
        Rectangle noRect2 = new Rectangle(590, 140, 200, 80);
        

        float counter = 0;
        string num = "4";
        int count = 0;
        int timeCounter = 0;
        
        float elapsedLocCount = 0;

        public enum gameState
        {
            MainMenu,
            InGame,
            DeathScreen,
            PreGame,
            WinScreen
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            
            
            base.Initialize(); 
        }

        protected override void LoadContent()
        {
            
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileMap = new TileMap("map", Content);
            foreach (var item in tileMap.tilesList)
            {
                if (item.tileType == 5)
                {
                    startPositionPlayer = new Vector2(item.tilePosition.X, item.tilePosition.Y - 12);
                    break;
                } 
            }
            player = new Player(startPositionPlayer);
            player.Load(Content);
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            Texture2D playerText = Content.Load<Texture2D>("Player");
            cursorTexture = Content.Load<Texture2D>("Cursor");
            menuTexture = Content.Load<Texture2D>("TitleMenu");
            spikesTexture = Content.Load<Texture2D>("Textures\\Spikes");
            deathTexture = Content.Load<Texture2D>("DeathScreen");
            yesButtonTexure = Content.Load<Texture2D>("TryAgain");
            noButtonTexture = Content.Load<Texture2D>("NoMain");
            winScreenTexture = Content.Load<Texture2D>("WinScreen");

            soundClick = Content.Load<SoundEffect>("Audio\\Click");
            soundLoose = Content.Load<SoundEffect>("Audio\\Loose");
            winSound = Content.Load<SoundEffect>("Audio\\Win");
            soundLooseInstance = soundLoose.CreateInstance();
            soundClickInstance = soundClick.CreateInstance();
            
            camera.position = new Vector2(0,0);
        }

        protected override void Update(GameTime gameTime)
        {
           
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, 8, 8);
            
            if (gState == gameState.InGame)
            {
                camera.Input(camera);
                camera.Move(new Vector2(2f, player.Position.Y));
                player.Update(gameTime);

                foreach (var item in tileMap.solidTilesList)
                {
                    bool hasWon = player.Collision(item.tileRectangle,item.tileType);
                    if (hasWon)
                    {
                        gState = gameState.WinScreen;
                        winSound.Play();
                    }   
                    
                }

                if (player.Position.X <= camera.position.X - 380)
                {
                    soundLoose.Play();

                    gState = gameState.DeathScreen;
                    
                }
                elapsedLocCount += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedLocCount >= 1000)
                {
                    elapsedLocCount = 0;
                    timeCounter++;
                }
            }
            #region timer
            else if(gState == gameState.PreGame)
            {
                counter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (counter >= 1000)
                {
                    count++;
                    if (count == 1)
                        num = "3";
                    if (count == 2)
                        num = "2";
                    if (count == 3)
                        num = "1";
                    if (count == 4)
                    {
                        gState = gameState.InGame;
                        count = 0;
                        num = "4";
                    }
                    counter = 0f;
                }
            }
            #endregion

            
            CheckButtonPress();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            string posString = mouseState.X.ToString() + " " + mouseState.Y.ToString();
            #region InGame
            if (gState == gameState.InGame)
            {
                GraphicsDevice.Clear(Color.SkyBlue);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.get_transformation(this.GraphicsDevice));
                tileMap.DrawTiles(spriteBatch);
                player.Draw(spriteBatch);
                spriteBatch.Draw(spikesTexture, new Vector2(camera.position.X - 400, camera.position.Y - 240), Color.White);
                spriteBatch.DrawString(spriteFont, timeCounter.ToString(), new Vector2(camera.position.X, camera.position.Y - 150), Color.Purple);
                spriteBatch.End();
            }
            #endregion
            #region MainMenu
            else if (gState == gameState.MainMenu)
            {
                
                GraphicsDevice.Clear(Color.Cornsilk);
                spriteBatch.Begin();
                spriteBatch.Draw(menuTexture,Vector2.Zero,Color.White);
                spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
                spriteBatch.End();
            }
            #endregion
            #region PreGame
            else if (gState == gameState.PreGame)
            {
                GraphicsDevice.Clear(Color.SkyBlue);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.get_transformation(this.GraphicsDevice));
                tileMap.DrawTiles(spriteBatch);
                player.Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont,"Get ready: "+ num, camera.position, Color.Snow);
                player.Draw(spriteBatch);
                spriteBatch.Draw(spikesTexture, new Vector2(camera.position.X - 400,camera.position.Y - 240) , Color.White);
                spriteBatch.End();

            }
            #endregion
            #region DeathScreen
            else if (gState == gameState.DeathScreen)
            {
                GraphicsDevice.Clear(Color.Plum);
                spriteBatch.Begin();
                spriteBatch.Draw(deathTexture, Vector2.Zero, Color.White);
                spriteBatch.Draw(yesButtonTexure, new Vector2(590, 250), Color.White);
                spriteBatch.Draw(noButtonTexture, new Vector2(590, 340), Color.White);
                spriteBatch.DrawString(spriteFont, "Congratulations!", new Vector2(250, 35), Color.White);
                spriteBatch.DrawString(spriteFont,  "You managed to die in: " + timeCounter.ToString() + " Seconds.", new Vector2(125, 70), Color.White);
                spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
                spriteBatch.End();
            }
            #endregion
            #region WinScreen
            else if (gState == gameState.WinScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(winScreenTexture, Vector2.Zero, Color.White);
                spriteBatch.Draw(yesButtonTexure, new Vector2(590, 50), Color.White);
                spriteBatch.Draw(noButtonTexture, new Vector2(590, 140), Color.White);
                spriteBatch.DrawString(spriteFont,"You managed to finish in: "+timeCounter.ToString()+" Seconds", new Vector2(130, 400),new Color(255,0,220));
                spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X, mouseState.Y), Color.White);
                spriteBatch.End();
            }
            #endregion
            base.Draw(gameTime);
        }

        public void CheckButtonPress()
        {
            mouseState = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Y, 20, 20);

            if (mouseRect.Intersects(exitRect) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.MainMenu)
            {
                soundClick.Play();
                this.Exit();
                
            }

            if (mouseRect.Intersects(startRect) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.MainMenu)
            {
                soundClick.Play();
                gState = gameState.PreGame;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (gState == gameState.InGame || gState == gameState.DeathScreen || gState == gameState.WinScreen)
                {
                    gState = gameState.MainMenu;

                    camera.position = startPositionPlayer;
                    player.SetPosition(startPositionPlayer);
                    timeCounter = 0;
                    counter = 0;
                    num = "4";
                    count = 0;
                    elapsedLocCount = 0;
                
                }
                
            }
            

            if (mouseRect.Intersects(yesRect) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.DeathScreen)
            {
                camera.position = startPositionPlayer;
                player.SetPosition(startPositionPlayer);
                timeCounter = 0;
                counter = 0;
                num = "4";
                count = 0;
                elapsedLocCount = 0;
                
                soundClick.Play();
                gState = gameState.PreGame;
            }
            if (mouseRect.Intersects(noRect) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.DeathScreen)
            {
                camera.position = startPositionPlayer;
                player.SetPosition(startPositionPlayer);
                timeCounter = 0;
                counter = 0;
                num = "4";
                count = 0;
                elapsedLocCount = 0;
                soundClick.Play();
                gState = gameState.MainMenu;
            }
            if (mouseRect.Intersects(yesRect2) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.WinScreen)
            {
                camera.position = startPositionPlayer;
                player.SetPosition(startPositionPlayer);
                timeCounter = 0;
                counter = 0;
                num = "4";
                count = 0;
                elapsedLocCount = 0;

                soundClick.Play();
                gState = gameState.PreGame;
            }
            if (mouseRect.Intersects(noRect2) && mouseState.LeftButton == ButtonState.Pressed && gState == gameState.WinScreen)
            {
                camera.position = startPositionPlayer;
                player.SetPosition(startPositionPlayer);
                timeCounter = 0;
                counter = 0;
                num = "4";
                count = 0;
                elapsedLocCount = 0;
                soundClick.Play();
                gState = gameState.MainMenu;
                
            }
            
        }

       
    }
}
