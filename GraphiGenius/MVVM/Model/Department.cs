using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<int> Shifts { get; set; }
        public ObservableCollection<int> ShiftLengths { get; set; }
        public ObservableCollection<int> StartHours { get; set; }
        public ObservableCollection<int> StartMinutes { get; set; }
        public ObservableCollection<int> EndHours { get; set; }
        public ObservableCollection<int> EndMinutes { get; set; }
        public ICollection<Employee> Employee { get; private set;} = new ObservableCollection<Employee>();
    }
}
