using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphiGenius.MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private int currentDepartment = 0;
        public int CurrentDepartment
        {
            get { return currentDepartment; }
            set
            {
                currentDepartment = value;
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDepartment)));
            }
        }
        private List<string> departments = new List<string>();
        public List<string> Departments
        {
            get
            { return departments; }
        }

        private int currentEmployee = 0;
        public int CurrentEmployee
        {
            get { return currentEmployee; }
            set
            {
                currentEmployee = value;
                //zgłoszenie zmiany wartości tej własności
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentEmployee)));
            }
        }
        private List<string> employees = new List<string>();
        public List<string> Employees
        {
            get
            { return employees; }
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
                        generateGrahic();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
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
        private void generateGrahic()
        {
            throw new NotImplementedException();
        }
        private void addEmployee()
        {
            throw new NotImplementedException();

        }
        private void addDepartment()
        {
            throw new NotImplementedException();

        }

    }
}
