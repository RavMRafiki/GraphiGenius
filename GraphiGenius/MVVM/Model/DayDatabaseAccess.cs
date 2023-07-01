using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    class DayDatabaseAccess : DatabaseAccess
    {
        public int employeers_per_shift(int id, int length)
        {
            DataTable dt = new DataTable();
            dt = dbConnect($"select StartHour, EndHour from Day where DepartmentId={id};");
            int endHour = Convert.ToInt32(dt.Rows[0]["EndHour"]);
            int startHour = Convert.ToInt32(dt.Rows[0]["StartHour"]);
            if (endHour == 0) endHour = 24;
            return (endHour - startHour) / length;
        }
    }
}
