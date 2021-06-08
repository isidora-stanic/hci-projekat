using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrganizeIt
{
    class MinimumCharacterRule : ValidationRule
    {
        
        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string charString = value as string;
            /*if (backend.Backend.LoadUsers().ContainsKey(charString))
                return new ValidationResult(false, $"Korisnicko ime vec postoji");*/
            if (charString is null)
                return new ValidationResult(false, $"User atleast {MinimumCharacters} characters.");
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"User atleast {MinimumCharacters} characters.");

            return new ValidationResult(true, null);
        }
    }
  
}
