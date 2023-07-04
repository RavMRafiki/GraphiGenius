using GraphiGenius.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;


namespace GraphiGenius.MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public WindowStateViewModel WindowStateViewModel = new();
        private Model.DepartmentsDatabaseAccess _departmentsDatabaseAccess = new();
        private Model.EmployeeDatabaseAccess _employeeDatabaseAccess= new();
        private Model.ShiftDatabaseAccess _shiftDatabaseAccess = new();
        private Model.Employee _employeeForm = new();
        private Model.Department _departmentForm = new();
        private Model.DayDatabaseAccess _daydatabaseaccess = new();
        private Model.GenerateShift _generateshift = new();
        public MainViewModel()
        {
            try
            {
            _reloadDepartments();
            _reloadEmployees();
            }
            catch { }
        }  
        private int[] departmentsIds;
        private void _reloadDepartments()
        {
            Departments = new();
            departmentsIds = _departmentsDatabaseAccess.loadDepartments();
            for (int i =0;i<departmentsIds.Length;i++)
            {
                Departments.Add(_departmentsDatabaseAccess.departmentName(departmentsIds[i]));
            }
        }
        private int[] employeesIds = new int[0];
        private void _reloadEmployees()
        {
            Employees.Clear();
            if(currentDepartmentIndex != -1)
            {
                if (departmentsIds.Length > 0)
                {
                    employeesIds = _employeeDatabaseAccess.loadEmployees(departmentsIds[currentDepartmentIndex]);
                }
                else return;
            }
            
            for (int i = 0; i < employeesIds.Length; i++)
            {
                Employees.Add(_employeeDatabaseAccess.employeeName(employeesIds[i]));
            }

        }
        private int currentDepartmentIndex = -1;
        public int CurrentDepartmentIndex
        {
            get { return currentDepartmentIndex; }
            set
            {
                EditDepartment = true;
                currentDepartmentIndex = value;
                _reloadEmployees();
                loadDepartment();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDepartmentIndex)));
            }
        }
        private ObservableCollection<string> departments = new ObservableCollection<string>();
        public ObservableCollection<string> Departments
        {
            get
            { return departments; }
            set
            {
                departments = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Departments)));
            }
        }
        private string departmentNameForm;
        public string DepartmentNameForm
        {
            get { return departmentNameForm; }
            set
            {
                departmentNameForm = value;
                _departmentForm.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DepartmentNameForm)));

            }
        }
        private ObservableCollection<int> shiftsForm;
        public ObservableCollection<int> ShiftsForm
        {
            get { return shiftsForm; }
            set
            {
                shiftsForm = value;
                _departmentForm.Shifts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftsForm)));
            }
        }
        private ObservableCollection<int> shiftLenghtsForm;
        public ObservableCollection<int> ShiftLenghtsForm
        {
            get { return shiftLenghtsForm; }
            set
            {
                shiftLenghtsForm = value;
                _departmentForm.ShiftLengths = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShiftLenghtsForm)));
            }
        }
        private ObservableCollection<int> startHoursForm;
        public ObservableCollection<int> StartHoursForm
        {
            get { return startHoursForm; }
            set
            {
                startHoursForm = value;
                _departmentForm.StartHours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartHoursForm)));
            }
        }
        private ObservableCollection<int> startMinutesForm;
        public ObservableCollection<int> StartMinutesForm
        {
            get { return startMinutesForm; }
            set
            {
                startMinutesForm = value;
                _departmentForm.StartMinutes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartMinutesForm)));
            }
        }
        private ObservableCollection<int> endHoursForm;
        public ObservableCollection<int> EndHoursForm
        {
            get { return endHoursForm; }
            set
            {
                endHoursForm = value;
                _departmentForm.EndHours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndHoursForm)));
            }
        }
        private ObservableCollection<int> endMinutesForm;
        public ObservableCollection<int> EndMinutesForm
        {
            get { return endMinutesForm; }
            set
            {
                endMinutesForm = value;
                _departmentForm.EndMinutes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndMinutesForm)));
            }
        }

        private int currentEmployeeIndex = -1;
        public int CurrentEmployeeIndex
        {
            get { return currentEmployeeIndex; }
            set
            {
                if (value != -1)
                {
                    EditEmployee= true;
                }
                currentEmployeeIndex = value;
                loadEmployee();
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentEmployeeIndex)));
            }
        }
        private string employeeNameForm;
        public string EmployeeNameForm
        {
            get { return employeeNameForm; }
            set
            {
                employeeNameForm = value;
                _employeeForm.Name= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeNameForm)));

            }
        }
        private string employeeWorkingHoursForm;
        public string EmployeeWorkingHoursForm
        {
            get { return employeeWorkingHoursForm; }
            set { employeeWorkingHoursForm = value;
                _employeeForm.WorkingHours = Int32.Parse(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeWorkingHoursForm)));
            }
        }
        private ObservableCollection<string> employees = new ObservableCollection<string>();
        public ObservableCollection<string> Employees
        {
            get
            { return employees; }
            set
            {
                employees = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
            }
        }
        #region EditDepartments
        private ICommand _addDepartment;


        public ICommand AddDepartment
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _addDepartment ?? (_addDepartment = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        addDepartment();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private ICommand _saveDepartment;
        public ICommand SaveDepartment
        {
            get
            {
                return _saveDepartment ?? (_saveDepartment = new BaseClass.RelayCommand(
                (p) => {
                    saveDepartment();
                }
                ,
            p => true)
                );
            }
        }
        private ICommand _deleteDepartment;
        public ICommand DeleteDepartment
        {
            get
            {
                return _deleteDepartment ?? (_deleteDepartment = new BaseClass.RelayCommand(
                (p) => {
                    deleteDepartment();
                }
                ,
            p => true)
                );
            }
        }
        private void loadDepartment()
        {
            if(currentDepartmentIndex != -1)
            {
                _departmentForm = _departmentsDatabaseAccess.loadDepartment(departmentsIds[currentDepartmentIndex]);
            }
            //MessageBox.Show(messageBoxText: _departmentForm.Name);
            DepartmentNameForm = _departmentForm.Name;
            ShiftsForm= _departmentForm.Shifts;
            ShiftLenghtsForm = _departmentForm.ShiftLengths;
            StartHoursForm= _departmentForm.StartHours;
            StartMinutesForm= _departmentForm.StartMinutes;
            EndHoursForm= _departmentForm.EndHours;
            EndMinutesForm= _departmentForm.EndMinutes;
        }
        private void saveDepartment()
        {
            _departmentsDatabaseAccess.editDepartment(_departmentForm);
            _reloadDepartments();
        }
        private void deleteDepartment()
        {
            _departmentsDatabaseAccess.deleteDepartment(_departmentForm.Id);
            _reloadDepartments();
        }
        private void addDepartment()
        {
            _departmentsDatabaseAccess.addDepartment();
            _reloadDepartments();
            EditDepartment = true;
            CurrentDepartmentIndex= departmentsIds.Length - 1;

        }
        #endregion
        #region EditEmployees
        private ICommand _addEmployee;


        public ICommand AddEmployee
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _addEmployee ?? (_addEmployee = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        addEmployee();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private ICommand _saveEmployee;
        public ICommand SaveEmployee
        {
            get
            {
                return _saveEmployee ?? (_saveEmployee = new BaseClass.RelayCommand(
                (p) => {
                saveEmployee();
            }
                ,
            p => true)
                );
            }
        }
        private ICommand _deleteEmployee;
        public ICommand DeleteEmployee
        {
            get
            {
                return _deleteEmployee ?? (_deleteEmployee = new BaseClass.RelayCommand(
                (p) => {
                    deleteEmployee();
                }
                ,
            p => true)
                );
            }
        }
        private void loadEmployee()
        {
            if(currentEmployeeIndex != -1)
            {
                _employeeForm = _employeeDatabaseAccess.loadEmployee(employeesIds[currentEmployeeIndex]);
            }

            EmployeeNameForm = _employeeForm.Name;
            EmployeeWorkingHoursForm = _employeeForm.WorkingHours.ToString();
        }
        private void saveEmployee()
        {
            _employeeDatabaseAccess.editEmployee(_employeeForm);
            _reloadEmployees();
        }
        private void deleteEmployee()
        {
            _employeeDatabaseAccess.deleteEmployee(_employeeForm.Id);
            _reloadEmployees();

        }

        private void addEmployee()
        {
            _employeeDatabaseAccess.addEmployee(departmentsIds[currentDepartmentIndex]);
            _reloadEmployees();
            EditEmployee = true;
            CurrentEmployeeIndex = employeesIds.Length -1;

        }
        #endregion
        #region Generate
        private string generateNameForm;
        public string GenerateNameForm
        {
            get { return generateNameForm; }
            set
            {
                generateNameForm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GenerateNameForm)));

            }
        }
        private string generateMonthForm;
        public string GenerateMonthForm
        {
            get { return generateMonthForm; }
            set
            {
                generateMonthForm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GenerateMonthForm)));

            }
        }
        private string generateYearForm;
        public string GenerateYearForm
        {
            get { return generateYearForm; }
            set
            {
                generateYearForm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GenerateYearForm)));

            }
        }
        private ICommand _generatesettings;

        public ICommand GenerateSettings
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _generatesettings ?? (_generatesettings = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        generateSettings();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private ICommand _generate;

        public ICommand Generate
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _generate ?? (_generate = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        generate();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
        private void generateSettings()
        {
            EditSettings = true;
        }
        private int _monthNumber;
        public int MonthNumber
        {
            get => _monthNumber;
            set
            {
                _monthNumber = Convert.ToInt32(value);
            }
        }
        private string _graphiName;
        public string GraphiName
        {
            get => _graphiName;
            set
            {
                _graphiName = value;
            }
        }

        public string GenerateScheduleTable(List<Shift> shifts)
        {
            StringBuilder tableBuilder = new StringBuilder();

            Graphi graphi = _shiftDatabaseAccess.loadGraphi(shifts[0].gName); // Pobierz obiekt Graphi

            int year = graphi.Year; // Pobierz rok z obiektu Graphi
            int month = graphi.Month; // Pobierz miesiąc z obiektu Graphi

            int days = DateTime.DaysInMonth(year, month);

            var departments = shifts.Select(s => new { DepartmentName = s.DepartmentName, DepartmentId = s.DepartmentId, DepartmentShifts  = s.ShiftsDay, DepartmentSh = s.StartHourDay, DepartmentSm = s.StartMinuteDay, DepartmentWt = s.ShiftLengthDay})
                                    .Distinct()
                                    .ToList();

            foreach (var department in departments)
            {
                // Pobierz listę pracowników dla danego departamentu
                int[] employeeIds = _employeeDatabaseAccess.loadEmployees(department.DepartmentId);

                // Pobierz maksymalny dzień w miesiącu dla danego departamentu
                int maxDayInMonth = shifts.Where(s => s.DepartmentId == department.DepartmentId)
                                          .Max(s => s.DayInMonth);

                // Generowanie tabeli dla departamentu
                tableBuilder.AppendLine("<h1>" + department.DepartmentName + "</h1>");
                tableBuilder.AppendLine("<table class='schedule-table'>");
                tableBuilder.AppendLine("<tr><th colspan='2'></th>");

                // Dodaj nazwy dni tygodnia
                string[] dayNames = { "ND","Pon", "Wt", "Śr", "Czw", "Pt", "So"  };
                for (int j = 0; j < maxDayInMonth; j++)
                {
                    DateTime date = new DateTime(year, month, j+1);
                    string dayName = dayNames[(int)date.DayOfWeek];
                    tableBuilder.AppendLine("<th>" + dayName + "</th>");
                }

                tableBuilder.AppendLine("</tr>");

                tableBuilder.AppendLine("<tr><th colspan='2'></th>");

                for (int i = 0; i < maxDayInMonth; i++)
                {
                    tableBuilder.AppendLine("<td id='day" + i + "' class='days' colspan='1'>" + (i + 1) + "</td>");
                }

                tableBuilder.AppendLine("</tr>");

                // Wypełnij tabelę danymi pracowników i ich zmian
                for (int i = 0; i < employeeIds.Length; i++)
                {
                    tableBuilder.AppendLine("<tr> <th class='days' colspan='2'>" + _employeeDatabaseAccess.employeeName(employeeIds[i]) + "</th>");

                    double totalHours = 0; // Suma godzin dla danego pracownika

                    for (int j = 0; j < maxDayInMonth; j++)
                    {
                        double shiftNumber=0;
                       
                        bool check = false;
                        foreach (var shift in shifts)
                        {
                            if (shift.DayInMonth == j + 1 && shift.EmployeeId == employeeIds[i] && shift.gName == shifts[0].gName)
                            {

                                if (shiftNumber==0)
                                {
                                    shiftNumber = shift.IndexOfShift + 1;
                                }
                                else
                                {
                                    shiftNumber = shiftNumber + (shift.IndexOfShift) / 10 + 1.1;
                                }
                               
                                check = true;
                            }
                        }

                        if (check == false)
                        {
                            tableBuilder.AppendLine("<td id='prac" + i + "dzien" + j + "' class='changes'></td>");
                        }
                        else
                        {
                            tableBuilder.AppendLine("<td id='prac" + i + "dzien" + j + "' class='changes'>" + shiftNumber + "</td>");
                        }

                        if (j == maxDayInMonth - 1)
                        {
                            // Oblicz sumę godzin
                            double shiftHoursSum = shifts.Where(s => s.DayInMonth <= maxDayInMonth &&
                                                                  s.EmployeeId == employeeIds[i] &&
                                                                  s.gName == shifts[0].gName) 
                                                      .Sum(s => (s.ShiftLengthDay)/(s.ShiftsDay));
                            totalHours += shiftHoursSum;
                            
                            tableBuilder.AppendLine("<td id='prach" + i + "' class='changes'>" + totalHours + "</td>");
                        }
                    }

                    tableBuilder.AppendLine("</tr>");


                }

                tableBuilder.AppendLine("</tr></table>");

                tableBuilder.AppendLine("<h2 class='table-header'>" + department.DepartmentName + " - Godziny zmian:</h2>");
                tableBuilder.AppendLine("<table class='schedule-table'>");

                // Nagłówki dla dni tygodnia
                tableBuilder.AppendLine("<tr>");
                tableBuilder.AppendLine("<th></th>");
                for (int j = 0; j < 7; j++)
                {
                    int dayIndex = (j + 1) % 7; // Poprawka: Zmieniamy indeksy dni tygodnia, aby rozpoczynały się od niedzieli (0 - niedziela, 1 - poniedziałek, itd.)
                    tableBuilder.AppendLine("<th class='day-header'>" + dayNames[dayIndex] + "</th>");
                }
                tableBuilder.AppendLine("</tr>");

                // Wypełnij tabelę zakresami godzinowymi zmian

                double endh = 0;
                double endm = 0;

                for (int shiftIndex = 0; shiftIndex < 3; shiftIndex++)
                {
                    tableBuilder.AppendLine("<tr>");
                    tableBuilder.AppendLine("<th class='shift-header'>Zmiana " + (shiftIndex + 1) + "</th>");

                    double starth = department.DepartmentSh + endh;
                    double startm = department.DepartmentSm + endm;
                    endh = endh + department.DepartmentWt;
                    for (int dayIndex = 0; dayIndex < 7; dayIndex++)
                    {
                        int correctedDayIndex = (dayIndex + 1) % 7;
                        Shift shift = shifts.FirstOrDefault(s => s.DepartmentId == department.DepartmentId &&
                                                                                          s.DaysId == correctedDayIndex &&
                                                                                          s.IndexOfShift == shiftIndex);
                        // Poprawka: Zmieniamy indeksy dni tygodnia, aby rozpoczynały się od niedzieli (0 - niedziela, 1 - poniedziałek, itd.)
                        /*
                                                Shift shift = shifts.FirstOrDefault(s => s.DepartmentId == department.DepartmentId &&
                                                                                          s.DaysId == correctedDayIndex  &&
                                                                                          s.IndexOfShift == shiftIndex);

                                                string shiftHours = shift != null ? shift.StartHourDay.ToString("00") + ":" + shift.StartMinuteDay.ToString("00") + " - " +
                                                                                   shift.EndHourDay.ToString("00") + ":" + shift.EndMinuteDay.ToString("00") : "";
                        */

                        if (shift != null)
                        {

                            string shiftHours = starth.ToString("00") + ":" + startm.ToString("00") + " - " +
                                                                endh.ToString("00") + ":" + endm.ToString("00");
                            tableBuilder.AppendLine("<td class='shift-hours'>" + shiftHours + "</td>");
                        }
                        else
                        {



                            tableBuilder.AppendLine("<td class='shift-hours'></td>");
                        }
                    }

                    tableBuilder.AppendLine("</tr>");
                }

                tableBuilder.AppendLine("</table>");
            }

            return tableBuilder.ToString();
        }


        private async Task generate()

        {
            int year = Convert.ToInt32(GenerateYearForm);
           // await _generateshift.generate_shift(GraphiName, MonthNumber, year);

            ShiftDatabaseAccess shiftDatabaseAccess = new ShiftDatabaseAccess();
            List<Shift> shifts = shiftDatabaseAccess.loadShifts();

            string generatehtml = @"<!DOCTYPE html>
                                <html lang=""pl"">
                                <head>

                                    <meta charset=""utf-8"">
                                    <title>Pole Minowe</title>
                                    <meta name=""keywords"" content=""javascript, jQuery, game, gwent, memory"">
                                    <meta name=""author"" content=""BB"">
                                    
                                    <meta http-equiv=""X-Ua-Compatible"" content=""IE=edge,chrome=1"">


                                    <link rel=""stylesheet"" href=""grafmain.css"">


                                     <link href=""https://fonts.googleapis.com/css2?family=Rubik+Beastly&display=swap"" rel=""stylesheet""> 

                                    
                                    
                                    <!--[if lt IE 9]>
                                    <script src=""//cdnjs.cloudflare.com/ajax/libs/html5shiv/3.7.3/html5shiv.min.js""></script>
                                    <![endif]-->




                                                                       <style>

                                    body {
                                        margin: 0;
                                        background-color: #272537;
                                        color: #fafafa;
                                        font-family: ""Lobster"", sans-serif;
                                        font-size: 18px;
                                        text-align: center;
                                    }

                                    #board {
                                        background-color: #3c3b52;
                                        justify-content: center;
                                        margin: auto;
                                        height: auto;
                                        width: auto;
                                    }
 .table-header {
        font-size: 18px;
        font-weight: bold;
        margin-top: 20px;
    }

    .schedule-table {
        border-collapse: collapse;
        width: 100%;
        margin-bottom: 20px;
    }

    .day-header {
        font-weight: bold;
        text-align: center;
        padding: 8px;
    }

    .shift-header {
        font-weight: bold;
        padding: 8px;
    }

    .shift-hours {
        text-align: center;
        padding: 8px;
    }
/* Główna tabela harmonogramu */
.schedule-table {
  width: 100%;
  border-collapse: collapse;
}

.schedule-table th,
.schedule-table td {
  padding: 10px;
  text-align: center;
  border: 1px solid #ccc;
}

.schedule-table th {
  background-color: #272537;
}

.schedule-table .day-header {
  font-weight: bold;
}

.schedule-table .shift-header {
  font-weight: bold;
  background-color: #272537;
}

.schedule-table .shift-hours {
  font-weight: normal;
}

/* Tabela z grafikiem */
.schedule-table-graph {
  width: 100%;
  border-collapse: collapse;
}

.schedule-table-graph th,
.schedule-table-graph td {
  padding: 5px;
  text-align: center;
  border: 1px solid #ccc;
}

.schedule-table-graph th {
  background-color: #272537;
}

.schedule-table-graph .days {
  font-weight: bold;
}

.schedule-table-graph .changes {
  font-weight: normal;
}
                                    #monthname {
                                        width: 100px;
                                    }

                                    a:link {
                                        color: #fafafa;
                                        text-decoration: none;
                                    }

                                    a:visited {
                                        color: #fafafa;
                                    }

                                    a:active {
                                        color: #fafafa;
                                    }

                                    a:hover {
                                        color: #e9b64a;
                                    }

                                    h1 {
                                        font-size: 64px;
                                        font-weight: 200;
                                        text-align: center;
                                        letter-spacing: 5px;
                                        margin-top: 20px;
                                        margin-bottom: 0px;
                                        color: #c4c2d1;
                                    }

                                    .blad {
                                        background-color: rgb(92, 50, 170);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                        border-bottom: 1px solid #dadada;
                                    }

                                    .blad2 {
                                        background-color: rgb(255, 168, 55);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                        border-bottom: 1px solid #dadada;
                                    }

                                    .days {
                                        border: 1px solid #e9b64a;
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                    }

                                    .weekendchanges {
                                        border: 1px solid #e9b64a;
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        background-color: rgb(197, 195, 70);
                                    }

                                    .urlop {
                                        background-color: plum;
                                    }

                                    .changes {
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                    }

                                    table, th, td {
                                        border-collapse: collapse;
                                    }

                                    .dniowka, .nocka, .changes, .urlop, .check, .weekendchanges {
                                        border: 1px solid #dadada;
                                        cursor: pointer;
                                        color: #fafafa;
                                        font-weight: 400;
                                    }

                                    .check {
                                        background-color: rgb(170, 22, 22);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                    }

                                    .cor {
                                        background-color: rgb(24, 201, 92);
                                        margin: 0px;
                                        width: 30px;
                                        height: 40px;
                                        cursor: cell;
                                    }

                                    .box {
                                        cursor: pointer;
                                        border: 1px solid #dadada;
                                        border-collapse: collapse;
                                    }
                                    </style>


                                </head>

                                <body>

                                    <header>
                                    
                                        <h1>Grafik pracy</h1>
                                    
                                    
                                    </header>
                                    
                                    <main>
                                        
                                        <div id=""board"">
                                            " + GenerateScheduleTable(shifts) + @"
                                        </div>
                                        
                                    </main>
                                    


                                </body>

                                </html>";

            WebBrowserWindow webBrowserWindow = new();
            webBrowserWindow.Show();
            webBrowserWindow.webBrowser1.NavigateToString(generatehtml);
        }

        #endregion
            #region ChooseView
        public void selectedEmployeeChanged()
        {
            EditEmployee= true;
        }
        public void selectedDepartmentChanged()
        {
            EditDepartment = true;
        }
        private bool editEmployee = true;
        public bool EditEmployee
        {
            get { return editEmployee; }
            private set
            {
                editEmployee = value;
                if (value)
                {
                    EditDepartment = false;
                    EditSettings = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditEmployee)));
            }
        }
        private bool editDepartment = false;
        public bool EditDepartment
        {
            get { return editDepartment; }
            private set
            {
                editDepartment = value;
                if (value)
                {
                    EditEmployee = false;
                    EditSettings = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditDepartment)));
            }
        }
        private bool editSettings = false;
        public bool EditSettings
        {
            get { return editSettings; }
            private set
            {
                editSettings = value;
                if (value)
                {
                    EditEmployee = false;
                    EditDepartment = false;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditSettings)));
            }
        }
        #endregion
    }
}
