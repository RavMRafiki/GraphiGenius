using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphiGenius.MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _stopGame;
        public ICommand StopGame
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return _stopGame ?? (_stopGame = new BaseClass.RelayCommand(
                    //co wykonuje polecenie
                    (p) => {
                        thisfunction();
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }

        private void thisfunction()
        {
            throw new NotImplementedException();
        }
    }
}
