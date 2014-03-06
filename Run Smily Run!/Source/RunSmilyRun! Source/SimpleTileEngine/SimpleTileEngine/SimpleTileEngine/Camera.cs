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
    class Camera
    {
        protected float zoom;
        public Matrix transform;
        public Vector2 position{ get; set; }
        protected float rotation { get; set; }
        float speed = 1f;

        public Camera()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public void MoveTo(Vector2 newPosition)
        {
            position = newPosition;
        }
       
        public void Move(Vector2 amount)
        {
            Vector2 locCamPos = position;
            position = new Vector2((locCamPos.X += amount.X), amount.Y);
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0))
                * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1))
                * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        

        }

        public void Input(Camera camera)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.O))
                camera.Zoom += 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                camera.Zoom -= 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                camera.Move(new Vector2(0, -speed));
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                camera.Move(new Vector2(0, speed));
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                camera.Move(new Vector2(-speed, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                camera.Move(new Vector2(speed, 0));
        
        }
    }
}
