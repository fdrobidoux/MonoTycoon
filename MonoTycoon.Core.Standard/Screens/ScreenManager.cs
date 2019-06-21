using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Screens
{
    public interface IScreenManager
    {
        /// <summary>
        /// This boolean is set when the Initialize() method is called.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// The current active screen.
        /// </summary>
        Screen ActiveScreen { get; }

        /// <summary>
        /// Add a screen to the top of the stack, if the manager is initialized, but the screen isn't, initialize it.
        /// Also set it's manager to this.
        /// </summary>
        /// <param name="screen">The screen to add.</param>
        void Push(Screen screen);

        /// <summary>
        /// Get the top of the screen stack. The most active screen.
        /// </summary>
        /// <returns>The active screen.</returns>
        Screen Peek();

        /// <summary>
        /// Remove a screen from the screen stack.
        /// </summary>
        /// <returns>The removed screen.</returns>
        Screen Pop();
    }

	/// <summary>
	/// This class is a collection/stack of active screens in the game.<br />
	/// A game should have one screen manager and control the active screens by 
	/// adding and removing screens from this manager.
	/// 
	/// <para>The top screen of the stack will be updated and the top screens that are 
	/// translucent will be drawn. The first screen on the stack that isn't translucent
	/// will stop the drawing.</para>
	/// 
	/// <para><em>In Game:</em><br />
	///	<code>public ScreenManager Screens { get; private set; }</code>
	/// </para>
	/// 
	/// <para><em>In Game.Initialize():</em><br />
	/// <code>this.Components.Add( this.Screens = new ScreenManager(this, new StartScreen(this)) );</code>
	/// </para>
	/// 
	/// </summary>
	/// By Koen Bollen, 2011
	public class ScreenManager : DrawableGameComponent, IScreenManager
    {
        public bool Initialized { get; private set; }

        /// <summary>
        /// This is the list of active screens in the game.
        /// </summary>
        private Stack<Screen> screens;

        public Screen ActiveScreen
        {
            get { return this.Peek(); }
        }

        public ScreenManager(Game game, Screen start)
            : base(game)
        {
            this.screens = new Stack<Screen>();
            if (start != null)
                this.Push(start);
            this.Initialized = false;
        }

        /// <summary>
        /// Initialize all screens in the active screen list.
        /// </summary>
        public override void Initialize()
        {
            foreach (Screen s in this.screens)
                s.Initialize();
            this.Initialized = true;
            base.Initialize();
        }

        /// <summary>
        /// Only update the top of the screen stack.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.screens.Peek().Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw all visible screens. A screen that is not translucent will stop the iteration of screens.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            var visible = new List<Screen>();
            foreach (Screen s in this.screens)
            {
                visible.Add(s);
                if (!s.Translucent) 
                    break;
            }

            // Draw from back to front:
            for (int i = visible.Count - 1; i >= 0; i--)
                visible[i].Draw(gameTime);

            base.Draw(gameTime);
        }

        public void Push(Screen screen)
        {
            screen.Manager = this;
            if (!screen.Initialized && this.Initialized)
                screen.Initialize();
            this.ActiveScreen?.Deactivated();
            this.screens.Push(screen);
        }

		/// <inheritdoc />
		public Screen Peek()
        {
            return this.screens.Count >= 1 ? this.screens.Peek() : null;
        }

        public Screen Pop()
        {
            if (this.screens.Count < 1)
                return null;
            Screen prev = this.screens.Pop();
            if (this.ActiveScreen != null)
                this.ActiveScreen.Activated();
            return prev;
        }
    }
}