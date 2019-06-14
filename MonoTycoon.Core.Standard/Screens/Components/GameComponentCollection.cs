using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MonoTycoon.Core.Standard.Screens.Components
{
	public class GameComponentCollection : Collection<IGameComponent>
	{
		//
		// Summary:
		//     /// Event that is triggered when a Microsoft.Xna.Framework.GameComponent is added
		//     /// to this Microsoft.Xna.Framework.GameComponentCollection. ///
		/// <summary>
		/// Event that is triggered when a Microsoft.Xna.Framework.GameComponent is added
		/// to this GameComponentCollection.
		/// </summary>
		public event EventHandler<GameComponentCollectionEventArgs> ComponentAdded;

		/// <summary>
		/// Event that is triggered when a Microsoft.Xna.Framework.GameComponent is removed
		/// from this GameComponentCollection.
		/// </summary>
		public event EventHandler<GameComponentCollectionEventArgs> ComponentRemoved;

		//
		// Summary:
		//     /// Removes every Microsoft.Xna.Framework.GameComponent from this Microsoft.Xna.Framework.GameComponentCollection.
		//     /// Triggers Microsoft.Xna.Framework.GameComponentCollection.OnComponentRemoved(Microsoft.Xna.Framework.GameComponentCollectionEventArgs)
		//     once for each Microsoft.Xna.Framework.GameComponent removed. ///
		protected override void ClearItems()
		{
			for (int i = 0; i < base.Count; i++)
			{
				OnComponentRemoved(new GameComponentCollectionEventArgs(base[i]));
			}
			base.ClearItems();
		}

		protected override void InsertItem(int index, IGameComponent item)
		{
			if (IndexOf(item) != -1)
			{
				throw new ArgumentException("Cannot Add Same Component Multiple Times");
			}
			base.InsertItem(index, item);
			if (item != null)
			{
				OnComponentAdded(new GameComponentCollectionEventArgs(item));
			}
		}

		private void OnComponentAdded(GameComponentCollectionEventArgs eventArgs)
		{
			ComponentAdded?.Invoke(this, eventArgs);
		}

		private void OnComponentRemoved(GameComponentCollectionEventArgs eventArgs)
		{
			ComponentRemoved?.Invoke(this, eventArgs);
		}

		protected override void RemoveItem(int index)
		{
			IGameComponent gameComponent = base[index];
			base.RemoveItem(index);
			if (gameComponent != null)
			{
				OnComponentRemoved(new GameComponentCollectionEventArgs(gameComponent));
			}
		}

		protected override void SetItem(int index, IGameComponent item)
		{
			throw new NotSupportedException();
		}
	}
}
