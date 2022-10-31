using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowardAgarioStepTwo
{


    internal class Student
    {
        public float GPA;

        public string Name { get; set; }
        public int ID { get; private set; }

        public Student(string name, float gpa)
        {
            this.ID = 123456789;
            this.Name = name;
            this.GPA = gpa;
        }


    }
}
