using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace MonoTycoon.Services
{
    public interface IContentManagerProvider
    {
        ContentManager Global { get; }

        ContentManager CurrentLevel { get; }

        ContentManager GetForLevel(string levelName);

        ContentManager Get(CM name);

        IEnumerable<string> GetAllIn(string directory);

        void Dispose(CM name);
    }
    
    public enum CM
    {
        Menu,
        Intro,
        Reboot,
        EndCutscene,
    }
}