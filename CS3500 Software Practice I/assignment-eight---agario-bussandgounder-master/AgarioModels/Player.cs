using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AgarioModels
{

    public class Player : GameObject
    {
        // turn to properties
        Vector2 vectorLocation;
        float playerMass;
        int playerARGBColor;
        long playerID;
        string playerName;

        /// <summary>
        /// Similar to our Food sub-class, but adds a name variable 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mass"></param>
        /// <param name="ARGBcolor"></param>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        public Player(float X, float Y, float mass, int ARGBcolor, long ID, string name) : base(X, Y, mass, ARGBcolor, ID)
        {
            this.Name = name;
        }

        /// <summary>
        /// name property
        /// </summary>
        public string Name { get; set; }

    }
}
