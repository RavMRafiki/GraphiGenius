using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employee { get; set;} = new HashSet<Employee>();
    }
}
