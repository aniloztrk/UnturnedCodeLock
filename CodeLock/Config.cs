using CodeLock.Models;
using Rocket.API;
using System.Collections.Generic;

namespace CodeLock
{
    public class Config : IRocketPluginConfiguration
    {
        public List<Door> Doors = new List<Door>();
        public void LoadDefaults()
        {
            Doors = new List<Door>();
        }   
    }
}
