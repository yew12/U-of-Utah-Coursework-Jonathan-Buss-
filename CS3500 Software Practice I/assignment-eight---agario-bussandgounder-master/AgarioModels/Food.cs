using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgarioModels
{
    /// <summary>
    /// Food 
    /// </summary>
    public class Food : GameObject
    {

        /// <summary>
        /// Food constructor, similar to our player sub-class but doesn't include 
        /// name
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mass"></param>
        /// <param name="ARGBcolor"></param>
        /// <param name="ID"></param>
        public Food(float X, float Y, float mass, int ARGBcolor, long ID) : base(X, Y, mass, ARGBcolor, ID)
        {
        }

    }
}
