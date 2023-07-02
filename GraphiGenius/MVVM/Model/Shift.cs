using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace GraphiGenius.MVVM.Model
{
    class Shift
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int IndexOfShift { get; set; }
        public int DayInMonth { get; set; }
        public double ShiftLengthDay { get; set; }
        public int StartHourDay { get; set; }
        public int StartMinuteDay { get; set; }
        public int EndHourDay { get; set; }
        public int EndMinuteDay { get; set; }
        public int ShiftsDay { get; set; }
        public string gName { get; set; }

        //Only for adding to DB
        public int GraphId { get; set; }
        public int DayId { get; set; }


        
    }
}