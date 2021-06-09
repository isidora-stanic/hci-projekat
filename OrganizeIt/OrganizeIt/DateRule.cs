using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrganizeIt
{
    class DateRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
                return new ValidationResult(false, $"Datum se unosi u formatu datum\\mesec\\vreme");
            string charString = value as string;
            try
            {
                DateTime oDate = Convert.ToDateTime(value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Datum se unosi u formatu datum/mesec/vreme");
            }

            return new ValidationResult(true, null);
        }
    }
}
