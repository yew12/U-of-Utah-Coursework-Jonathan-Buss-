using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AgarioModels
{
    public class GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int ARGBColor { get; set; }
        public float Mass { get; set; }
        public long ID { get; set; }

        /// <summary>
        /// Game Object constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mass"></param>
        /// <param name="ARGBcolor"></param>
        /// <param name="id"></param>
        public GameObject (float x, float y, float mass, int ARGBcolor, long id)
        {
            this.X = x;
            this.Y = y;
            this.Mass = mass;
            this.ARGBColor = ARGBcolor;
            this.ID = id;
        }
    }
}
