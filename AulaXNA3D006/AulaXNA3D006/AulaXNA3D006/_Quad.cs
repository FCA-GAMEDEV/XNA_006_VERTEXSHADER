using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AulaXNA3D006
{
    public class _Quad
    {
        Game game;
        GraphicsDevice device;
        Matrix world;
        int row, column;
        VertexPositionTexture[] verts;
        VertexBuffer vBuffer;
        int[] indices;
        IndexBuffer iBuffer;
        Effect effect;
        Texture2D texture;
        float time;
        float speed = 5;

        public _Quad(GraphicsDevice device, Game game, string textureName, string effectName)
        {
            this.game = game;
            this.device = device;
            this.world = Matrix.Identity;
            this.world *= Matrix.CreateRotationY(MathHelper.ToRadians(45));

            this.row = 150;
            this.column = 200;

            this.verts = new VertexPositionTexture[this.row * this.column];

            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    this.verts[i * this.column + j] = new VertexPositionTexture(new Vector3((j - this.column / 2f) / 10f, (-i + this.row / 2f) / 10f, 0),
                                                                                new Vector2(j / (float)(this.column - 1), i / (float)(this.row - 1)));
                }
            }            

            this.vBuffer = new VertexBuffer(this.device,
                                           typeof(VertexPositionTexture),
                                           this.verts.Length,
                                           BufferUsage.None);
            this.vBuffer.SetData<VertexPositionTexture>(this.verts);

            this.indices = new int[(this.row - 1) * (this.column - 1) * 2 * 3];

            int k = 0;
            for (int i = 0; i < this.row - 1; i++)
            {
                for (int j = 0; j < this.column - 1; j++)
                {
                    this.indices[k++] = (int)(i       * this.column +  j);      // v0
                    this.indices[k++] = (int)(i       * this.column + (j + 1)); // v1
                    this.indices[k++] = (int)((i + 1) * this.column +  j);      // v2

                    this.indices[k++] = (int)( i      * this.column + (j + 1)); // v1
                    this.indices[k++] = (int)((i + 1) * this.column + (j + 1)); // v3
                    this.indices[k++] = (int)((i + 1) * this.column +  j);      // v2
                }
            }

            this.iBuffer = new IndexBuffer(this.device,
                                           IndexElementSize.ThirtyTwoBits,
                                           this.indices.Length,
                                           BufferUsage.None);
            this.iBuffer.SetData<int>(this.indices);

            this.effect = this.game.Content.Load<Effect>(effectName);

            this.texture = this.game.Content.Load<Texture2D>(textureName);
        }

        public void Update(GameTime gameTime)
        {
            this.time += gameTime.ElapsedGameTime.Milliseconds / 1000f * this.speed;
        }

        public virtual void Draw(_Camera camera)
        {
            this.device.SetVertexBuffer(this.vBuffer);

            this.effect.CurrentTechnique = this.effect.Techniques["Technique1"];
            this.effect.Parameters["World"].SetValue(this.world);
            this.effect.Parameters["View"].SetValue(camera.GetView());
            this.effect.Parameters["Projection"].SetValue(camera.GetProjection());
            this.effect.Parameters["flagTexture"].SetValue(this.texture);
            this.effect.Parameters["time"].SetValue(this.time);
                        
            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
                                                                             this.verts,
                                                                             0, 
                                                                             this.verts.Length,
                                                                             this.indices,
                                                                             0,
                                                                             this.indices.Length / 3);
            }
        }
    }
}
