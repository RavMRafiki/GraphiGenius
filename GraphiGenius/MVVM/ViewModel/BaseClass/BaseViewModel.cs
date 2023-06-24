using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.ViewModel.BaseClass
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(params string[] properties)
        {
            if (PropertyChanged != null)
                foreach (var property in properties)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
