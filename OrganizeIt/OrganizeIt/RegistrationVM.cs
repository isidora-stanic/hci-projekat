using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizeIt
{
    class RegistrationVM : ObservableObject
    {

        private class SaveCommandObject : ICommand
        {
            private readonly Saveable _target;

            public SaveCommandObject(Saveable target)
            {
                _target = target;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _target.SaveCommand();
            }
        }

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        public RegistrationVM(Saveable ancestor)
        {
            _saveCommand = new SaveCommandObject(ancestor);
        }

        public RegistrationVM() {}


        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                OnPropertyChanged(ref _username, value);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                OnPropertyChanged(ref _name, value);
            }
        }

        private string _lastname;
        public string Lastname
        {
            get { return _lastname; }
            set
            {
                OnPropertyChanged(ref _lastname, value);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                OnPropertyChanged(ref _city, value);
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                OnPropertyChanged(ref _address, value);
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                OnPropertyChanged(ref _phone, value);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                OnPropertyChanged(ref _email, value);
            }
        }

        private string _data;
        public string BirthData
        {
            get { return _data; }
            set
            {
                OnPropertyChanged(ref _data, value);
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                OnPropertyChanged(ref _password, value);
            }
        }
    }
}
