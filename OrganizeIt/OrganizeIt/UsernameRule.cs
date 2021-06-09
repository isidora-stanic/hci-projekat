using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrganizeIt
{
    class UsernameRule: ValidationRule
    {
        public string MinimumCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value is null)
                return new ValidationResult(false, $"Datum se unosi u formatu datum\\mesec\\vreme");

            string charString = value as string;

            if(backend.Backend.LoadUsers().ContainsKey(charString))
                return new ValidationResult(false, $"Korisnik sa zadatim imenom već postoji");

            return new ValidationResult(true, null);
        }
    }
}
