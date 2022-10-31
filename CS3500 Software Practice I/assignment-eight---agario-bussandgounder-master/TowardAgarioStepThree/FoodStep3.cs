using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TowardAgarioStepThree
{
    internal class FoodStep3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ARGBcolor { get; set; }
        public int Mass { get; set; }

        public String toString()
        {
            return $"X: {this.X}, Y: {this.Y}, ARGBColor: {this.ARGBcolor}, Mass: {this.Mass}";
        }
    }


}
