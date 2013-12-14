using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// Defines behaviour which can be attached to a GameObject.
    /// </summary>
    public abstract class Component
    {
        #region Properties
        /// <summary>
        /// The GameObject this Component belongs to.
        /// </summary>
        public GameObject gameObject { get; internal set; }

        /// <summary>
        /// The name of the GameObject which this Component belongs to.
        /// </summary>
        public string name { get { return gameObject.name; } set { gameObject.name = value; } }

        /// <summary>
        /// The Renderer of the GameObject which this Component belongs to.
        /// </summary>
        public Renderer renderer { get { return gameObject.renderer; } }

        /// <summary>
        /// The Transform of the GameObject which this Component belongs to.
        /// </summary>
        public Transform transform { get { return gameObject.transform; } }

        /// <summary>
        /// The Collider of the GameObject which this Component belongs to.
        /// </summary>
        public Collider collider { get { return gameObject.collider; } }

        /// <summary>
        /// The Scene of the GameObject which this Component belongs to.
        /// </summary>
        internal Scene homeScene { get { return gameObject.homeScene; } }
        #endregion

        /// <summary>
        /// Whether this Component has been destroyed.
        /// </summary>
        public bool isDestroyed { get; private set; }
        
        /// <summary>
        /// Destroy this Component.
        /// </summary>
        public void Destroy()
        {
            if (!isDestroyed)
            {
                gameObject.RemoveComponent(this);
                OnDestroy();
                isDestroyed = true;
            }
            else
                Debug.LogError("This component has already been destroyed!");
        }

        #region Virtual Methods
        /// <summary>
        /// Is called when the Component is first created.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Is called on the Component every frame.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Is called on the Component during every Draw phase.
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// Is called on the Component during the DrawGUI phase.
        /// </summary>
        public virtual void DrawGUI() { }

        /// <summary>
        /// Is called on the Component just before it is destroyed.
        /// </summary>
        public virtual void OnDestroy() { }

        /// <summary>
        /// Is called on the Component when a Collider on the Component's GameObject starts colliding with another Collider.
        /// </summary>
        /// <param name="other">The Collider which the Component's GameObject is colliding with.</param>
        public virtual void OnCollisionEnter(Collider other) { }

        /// <summary>
        /// Is called on the Component every frame while a Collider on the Component's GameObject is colliding with another Collider.
        /// </summary>
        /// <param name="other">The Collider which the Component's GameObject is colliding with.</param>
        public virtual void OnCollisionStay(Collider other) { }

        /// <summary>
        /// Is called on the Component when a Collider on the Component's GameObject stops colliding with another Collider.
        /// </summary>
        /// <param name="other">The Collider which the Component's GameObject stops colliding with.</param>
        public virtual void OnCollisionExit(Collider other) { }
        #endregion
    }
}