using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class Shift
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int IndexOfShift { get; set; }
        public int DayInMonth { get; set; }
        public double ShiftLengthDay { get; set; }
        public int StartHourDay { get; set; }
        public int StartMinuteDay { get; set; }
        public int EndHourDay { get; set; }
        public int EndMinuteDay { get; set; }
        public int ShiftsDay { get; set; }
        //Only for adding to DB
        public int GraphId { get; set; }
        public int DayId { get; set; }
    }
}
