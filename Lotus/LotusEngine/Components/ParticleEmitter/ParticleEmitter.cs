using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{
    /// <summary>
    /// Component to emmit particles
    /// </summary>
    public class ParticleEmitter : Component
    {
        #region Fields & Props

        private double timer = 0;
        private Random r = new Random();

        /// <summary>
        /// Specifies the max number of particles
        /// </summary>
        public int MaxNumberOfParticles { get; set; }

        /// <summary>
        /// The direction of the particles to fly. (in degree)
        /// </summary>
        public int EmmisionDirection { get; set; }

        /// <summary>
        /// The value of wich the EmmisionDirection vary.  (in degree)
        /// </summary>
        public int EmmisionSpreadAngle { get; set; }

        /// <summary>
        /// The area-radius of the emission point. ( 0 = spawn from a single location )
        /// </summary>
        public int SpawnRadius { get; set; }

        /// <summary>
        /// How many particles to emit pr seccond
        /// </summary>
        public int ParticlesPerSeccond { get; set; }

        /// <summary>
        /// Particle speed
        /// </summary>
        public FloatRange ParticleSpeed { get; set; }

        /// <summary>
        /// Particle rotation speed
        /// </summary>
        public FloatRange RotationSpeed { get; set; }

        /// <summary>
        /// Particle life (1000 life = 1 seccond)
        /// </summary>
        public IntRange ParticleLife { get; set; }

        /// <summary>
        /// Velocity of new particles to come.
        /// </summary>
        public Vector2 AddedVelocity { get; set; }

        /// <summary>
        /// Gravity
        /// </summary>
        public Vector2 Gravity { get; set; }

        /// <summary>
        /// List of particle textures (A random texture from the list, is selected when a particle is beeing created)
        /// </summary>
        public List<ParticleTexture> ParticleTextures { get; set; }

        /// <summary>
        /// Contains all the particles
        /// </summary>
        public List<Particle> Particles = new List<Particle>();

        /// <summary>
        /// The spawnPosition
        /// </summary>
        public Vector2 SpawnStartPostion;

        /// <summary>
        /// Particles are spawned randomly between SpawnStartPostion and SpawnEndPostion to form a line.
        /// </summary>
        public Vector2 SpawnEndPostion;

        /// <summary>
        /// Sets both SpawnStartPostion and SpawnEndPostion, and gets SpawnStartPostion.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return SpawnStartPostion;
            }
            set
            {
                SpawnEndPostion = value;
                SpawnStartPostion = value;
            }
        }

        #endregion


        public ParticleEmitter()
        {
            this.ParticleTextures = new List<ParticleTexture>();
            this.ParticleSpeed = new FloatRange(0, 50);
            this.RotationSpeed = new FloatRange(0, 0);
            this.ParticleLife = new IntRange(500, 1500);
            this.EmmisionDirection = 0;
            this.EmmisionSpreadAngle = 360;
            this.MaxNumberOfParticles = 6000;
            this.ParticlesPerSeccond = 2050;
            this.AddedVelocity = Vector2.zero;
            this.Position = Vector2.zero;
            this.SpawnEndPostion = Vector2.right * 200;
            this.SpawnRadius = 300;
            this.AddedVelocity = Vector2.down * -1;
            this.Gravity = Vector2.zero;
        }

        /// <summary>
        /// Adds a particle at a given location
        /// </summary>
        /// <param name="position"></param>
        private void addParticle(Vector2 position)
        {
            // TODO Behøves ikke at udregnes hver gang.
            double angleDir = EmmisionDirection;
            double angleSpread = EmmisionSpreadAngle / 2;
            double minAngle = angleDir - angleSpread;
            double maxAngle = angleDir + angleSpread;

            double randomAngle = r.NextDouble() * (maxAngle - minAngle) + minAngle;
            float randomSpeed = ((float)r.NextDouble() * (ParticleSpeed.Maximum - ParticleSpeed.Minimum) + ParticleSpeed.Minimum);

            Particle p = new Particle(
                position: new Vector2(position.x, position.y),
                velocity: Vector2.DirectionVector((float)randomAngle) * randomSpeed + AddedVelocity,
                gravity: Gravity / 40,
                particleTexture: ParticleTextures.Count() != 0 ? 
                        ParticleTextures[r.Next(0, ParticleTextures.Count())] : 
                        new ParticleTexture(
                            Textures.GetTexture("tempParticle"), 
                            Color.White, 
                            Color.FromArgb(0, 255, 255, 255), 2f, 00f),
                rotation: (float)r.NextDouble() * (RotationSpeed.Maximum - RotationSpeed.Minimum) + RotationSpeed.Minimum,
                life: r.Next(ParticleLife.Minimum, ParticleLife.Maximum)
            );

            Particles.Add(p);
        }

        public override void Update()
        {
            timer = Time.DeltaTimeMilliseconds;
            // AddParticles
            if (Particles.Count() < MaxNumberOfParticles)
            {
                // particles pr seccond:
                var pps = 1000f / ParticlesPerSeccond;

                while (timer > pps && Particles.Count() < MaxNumberOfParticles)
                {
                    timer -= pps;

                    Vector2 pos;
                    // Start somwehere between a and b..
                    if (SpawnStartPostion != SpawnEndPostion)
                    {
                        int randomDistance = r.Next(0, (int)Vector2.Distance(SpawnStartPostion, SpawnEndPostion));
                        float angle = Vector2.Angle(SpawnStartPostion, SpawnEndPostion);
                        pos = Vector2.DirectionVector(angle) * randomDistance + SpawnStartPostion;
                    }
                    else
                    {
                        //Start in one spat.
                        pos = SpawnStartPostion;
                    }
                    //Spawn-radius.
                    pos += Vector2.DirectionVector(r.Next(0, 360)) * r.Next(0, SpawnRadius);
                    addParticle(pos);
                }
            }

            // UpdateParticles
            foreach (Particle particle in Particles.ToList())
            {
                particle.Update();

                // Remove Particles
                if (particle.CurrentLife <= 0)
                    Particles.Remove(particle);
            }
        }


        public override void Draw()
        {
            Rendering.StartDrawing(this);

            foreach (Particle particle in Particles)
                particle.Draw();
        }
    }
}
