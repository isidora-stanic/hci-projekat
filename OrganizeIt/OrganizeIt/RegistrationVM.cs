using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeIt
{
    class RegistrationVM : ObservableObject
    {
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
    }
}
