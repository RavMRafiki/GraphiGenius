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
        public int MondayShifts { get; set; }
        public double MondayShiftLength { get; set; }
        public int TuesdayShifts { get; set; }
        public double TuesdayShiftLength { get; set; }
        public int WednesdayShifts { get; set; }
        public double WednesdayShiftLength { get; set; }
        public int ThursdayShifts { get; set; }
        public double ThursdayShiftLength { get; set; }
        public int FridayShifts { get; set; }
        public double FridayShiftLength { get; set; }
        public int SaturdayShifts { get; set; }
        public double SaturdayShiftLength { get; set; }
        public int SundayShifts { get; set; }
        public double SundayShiftLength { get; set; }


        public ICollection<Employee> Employee { get; private set;} = new ObservableCollection<Employee>();
    }
}
