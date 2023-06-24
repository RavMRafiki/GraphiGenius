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
    class WindowStateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool editEmployee = true;
        public bool EditEmployee
        {
            get { return editEmployee; }
            private set
            {
                if (value)
                {
                    editDepartment = false;
                    editSettings = false;
                }
                editEmployee = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditEmployee)));
            }
        }
        private bool editDepartment = false;
        public bool EditDepartment
        {
            get { return editDepartment; }
            private set
            {
                if (value)
                {
                    editEmployee= false;
                    editSettings= false;
                }
                editDepartment = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditDepartment)));
            }
        }
        private bool editSettings = false;
        public bool EditSettings
        {
            get { return editSettings; }
            private set
            {
                if (value)
                {
                    editEmployee= false;
                    editDepartment = false;
                }
                editSettings = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditSettings)));
            }
        }
    }
}
