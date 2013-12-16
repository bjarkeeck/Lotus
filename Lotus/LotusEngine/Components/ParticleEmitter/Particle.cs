using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{
    /// <summary>
    /// Reprecents a single particle
    /// </summary>
    public class Particle
    {
        public int Life { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public ParticleTexture ParticleTexture { get; set; }
        public float RotationSpeed { get; set; }
        public Vector2 Gravity { get; set; }
        public int CurrentLife { get; set; }
        public float CurrentRotation { get; set; }

        public Particle(Vector2 position, Vector2 velocity, Vector2 gravity, ParticleTexture particleTexture, float rotation, int life)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.ParticleTexture = particleTexture;
            this.RotationSpeed = rotation;
            this.Life = life;
            this.Gravity = gravity;
            CurrentLife = life;
            if (particleTexture.Rotate)
                this.CurrentRotation = rotation;
        }

        public void Update()
        {
            if (CurrentLife > 0)
            {
                if (ParticleTexture.Rotate)
                    CurrentRotation += RotationSpeed;

                //Gravity
                Velocity += Gravity;

                //Velociy
                Position += Velocity * Time.DeltaTime;

                //Drain Life
                CurrentLife -= (int)Time.DeltaTimeMilliseconds;
            }
        }

        public void Draw()
        {
            //Rendering.DrawTexture(Textures.GetTexture("tempParticle"), Vector2.zero, Color.White);
            Rendering.DrawTexture(
                ParticleTexture.Texture,
                Position,
                ParticleTexture.Color((int)Life, CurrentLife),
                ParticleTexture.Scale((int)Life, CurrentLife),
                CurrentRotation,
                ParticleTexture.Texture.Origin,
                TextureFlip.None,
                ParticleTexture.DrawAdditive
                );
        }
    }
}
