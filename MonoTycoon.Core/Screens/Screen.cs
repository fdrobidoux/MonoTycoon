using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoTycoon.Core.Screens
{
    /// <summary>
    /// This is a screen that can be added to the ScreenManager. Extend it and add components 
    /// to it in the Initialize() method. You can also override the Update() and Draw() method.
    /// </summary>
    public class Screen : DrawableGameComponent
    {
        /// <summary>
        /// This member tells if this screen is initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Set this member to true if this screen doesn't cover the entire screen.
        /// </summary>
        public bool Translucent { get; set; }

        /// <summary>
        /// A reference to the ScreenManager.
        /// </summary>
        public ScreenManager Manager { get; internal set; }

        /// <summary>
        /// The GameComponentCollection, add components for this screen.
        /// </summary>
        public GameComponentCollection Components { get; protected set; }

        public Screen(Game game) : base(game)
        {
            Components = new GameComponentCollection();
            Translucent = false;
        }

        /// <summary>
        /// This method is called when this screen is back at the top of the stack.
        /// </summary>
        public virtual void Activated()
        {
        }

        /// <summary>
        /// This method is called when a screen is deactivated by an other screen.
        /// </summary>
        public virtual void Deactivated()
        {
        }

        /// <summary>
        /// Initialize every Component of this screen.
        /// </summary>
        public override void Initialize()
        {
            foreach (GameComponent gc in this.Components)
                gc.Initialize();
            Initialized = true;
            base.Initialize();
        }

        /// <summary>
        /// Update every Enabled Component of this screen.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Major credits to Nils Dijk:
            foreach (IUpdateable gc in Components.OfType<IUpdateable>().Where(x => x.Enabled).OrderBy(x => x.UpdateOrder))
                gc.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw every Visible Component of this screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Major credits to Nils Dijk:
            foreach (IDrawable gc in Components.OfType<IDrawable>().Where(x => x.Visible).OrderBy(x => x.DrawOrder))
            {
                gc.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            Components.Clear();
            base.Dispose(disposing);
        }
    }
}