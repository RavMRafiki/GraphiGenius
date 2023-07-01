using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    public class ScheduleRequest
    {
        public List<int>? employees;
        public List<int>? emp_workinghours;
        public int shifts_per_day;
        public int days_per_week;
        public int shift_length;
        public int emp_per_shift;
        public int monthId;
        public int year;
    }
}
