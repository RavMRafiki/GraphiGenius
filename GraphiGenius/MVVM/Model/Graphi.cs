using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class Graphi
    {
        public Graphi(string _name = "", int _month = 0,int _year = 2023) 
        {
            this.Name = _name;
            this.Month = _month;
            this.Year = _year; 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
