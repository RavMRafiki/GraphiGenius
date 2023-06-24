using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HourSalary { get; set; }
        public int WorkingHours { get; set; }
    }
}
