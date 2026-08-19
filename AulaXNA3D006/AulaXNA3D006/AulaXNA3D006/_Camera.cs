using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AulaXNA3D006
{
    public class _Camera
    {
        Matrix view;
        Matrix projection;
        Matrix rotation;

        Vector3 position;
        Vector3 target;
        Vector3 up;
        Vector3 forward, right;

        float speed = 10;

        float angleY = 0;
        float speedY = 100;

        public _Camera()
        {
            this.position = Vector3.Backward * 20;
            this.target = Vector3.Zero;
            this.up = Vector3.Up;
            this.SetupView(this.position, this.target, this.up);

            this.SetupProjection();
        }

        public void SetupView(Vector3 position, Vector3 target, Vector3 up)
        {
            this.view = Matrix.CreateLookAt(position, target, up);
        }

        public void SetupProjection()
        {
            _Screen screen = _Screen.GetInstance();

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  screen.GetWidth() / (float)screen.GetHeight(),
                                                                  0.001f,
                                                                  1000);
        }

        public Matrix GetView()
        {
            return this.view;
        }

        public Matrix GetProjection()
        {
            return this.projection;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Rotation(gameTime, keyState, deltaTime);

            this.rotation = Matrix.CreateRotationY(MathHelper.ToRadians(this.angleY));
            this.forward = this.rotation.Forward;
            this.right = this.rotation.Right;

            this.Translation(gameTime, keyState, deltaTime);

            this.view = Matrix.Identity;
            this.view *= this.rotation;
            this.view *= Matrix.CreateTranslation(this.position);
            this.view = Matrix.Invert(this.view);
        }

        private void Rotation(GameTime gameTime, KeyboardState ks, float deltaTime)
        {
            if (ks.IsKeyDown(Keys.Q)) this.angleY += this.speedY * deltaTime;
            if (ks.IsKeyDown(Keys.E)) this.angleY -= this.speedY * deltaTime;
        }

        private void Translation(GameTime gameTime, KeyboardState ks, float deltaTime)
        {
            if (ks.IsKeyDown(Keys.W)) this.position += this.forward * deltaTime * this.speed;
            if (ks.IsKeyDown(Keys.S)) this.position -= forward * deltaTime * this.speed;
            if (ks.IsKeyDown(Keys.D)) this.position += right * deltaTime * this.speed;
            if (ks.IsKeyDown(Keys.A)) this.position -= right * deltaTime * this.speed;
        }
    }
}
