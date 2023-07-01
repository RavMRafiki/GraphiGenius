using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    public class GenerateShift
    {
        private Model.DepartmentsDatabaseAccess _departmentsDatabaseAccess = new();
        private Model.EmployeeDatabaseAccess _employeeDatabaseAccess = new();
        private Model.ShiftDatabaseAccess _shiftDatabaseAccess = new();
        private Model.DayDatabaseAccess _daydatabaseaccess = new();
        public async Task generate_shift(string graphiName, int monthNumber)
        {
            List<List<int>> ints = _departmentsDatabaseAccess.DepartmentInfo();
            for (int i = 0; i < ints.Count; i++)
            {
                
                Graphi grafik = new Graphi
                {
                    Name = graphiName,
                    Month = monthNumber,
                    Year = 2023
                };
                _shiftDatabaseAccess.addGraphi(grafik);
                var emp_temp = _employeeDatabaseAccess.loadEmployees(ints[i][3]);
                ScheduleRequest requestData = new ScheduleRequest
                {
                    employees = new List<int>(emp_temp),
                    emp_workinghours = _employeeDatabaseAccess.loadEmployeesWorkingHours(new List<int>(emp_temp)),
                    shifts_per_day = ints[i][0],
                    days_per_week = ints[i][1],
                    shift_length = ints[i][2],
                    emp_per_shift = _daydatabaseaccess.employeers_per_shift(ints[i][3], ints[i][2]),
                    monthId = grafik.Month,
                    year = grafik.Year,
                };

                
                var scheduleArray = await SendToApi.send_to_api(requestData);
                for (int x = 0; x < scheduleArray.work_schedule.Count; x++)
                {
                    for (int y = 0; y < scheduleArray.work_schedule[x].Count; y++)
                    {
                        for (int z = 0; z < scheduleArray.work_schedule[x][y].Count; z++)
                        {
                            Shift shift = new Shift
                            {
                                EmployeeId = scheduleArray.work_schedule[x][y][z],
                                DayId = x,
                                IndexOfShift = y,
                                DayInMonth = x + (7 - ints[i][1]) * (x / 7),
                                GraphId = _shiftDatabaseAccess.loadGraphi(grafik.Name).Id
                            };
                            _shiftDatabaseAccess.addShift(shift);
                        }
                    }
                }
            }
        }
    }
}
