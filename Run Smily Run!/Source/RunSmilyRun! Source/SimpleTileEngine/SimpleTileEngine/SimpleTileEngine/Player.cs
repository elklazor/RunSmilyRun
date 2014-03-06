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
    class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle rectangle;
        
        private bool hasJumped = true;

        public Vector2 Position
        {
            get { return position; }
            set { position = Position; }
        }

        public Player(Vector2 playerPos)
        {
            position = playerPos;
        }

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("Player");
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            Input(gameTime);
            if(velocity.Y < 10)
                velocity.Y += 0.4f;


        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                velocity.X = 2f;

            else if (Keyboard.GetState().IsKeyDown(Keys.A))
                velocity.X = -2f;
            else
                velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasJumped)
            {
                position.Y -= 2f;
                velocity.Y = -4f;
                hasJumped = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                position = new Vector2(0, 40);
        }

        public bool Collision(Rectangle newRectangle,int id)
        {
            int check = 0;
            if(rectangle.TouchTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
                if (id == 6)
                    check++;
                
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 4;
                if (id == 6)
                    check++;
                
            }

            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 5;
                if (id == 6)
                    check++;
                
            }

            if (rectangle.TouchBottomOf(newRectangle))
            {
                velocity.Y = 1f;
                if(id == 6)
                    check++;
                
            }
            if (check > 0)
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.Wheat);
        }

        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
        }
    }
}
