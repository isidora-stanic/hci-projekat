using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrganizeIt
{
    class MinimumCharacterRule: ValidationRule
    {

        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string charString = value as string;
            /*if (backend.Backend.LoadUsers().ContainsKey(charString))
                return new ValidationResult(false, $"Korisnicko ime vec postoji");*/
            if (charString == null)
                return new ValidationResult(false, $"Polje mora da sadrži minimum {MinimumCharacters} karakter");
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Polje mora da sadrži minimum {MinimumCharacters} karakter");

            return new ValidationResult(true, null);
        }
    }

}
