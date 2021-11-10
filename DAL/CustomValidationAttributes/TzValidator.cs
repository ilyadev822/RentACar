using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CustomValidationAttributes
{
    public class TzValidator : ValidationAttribute

    {
        public override bool IsValid(object value)
        {
            if(value != null)
            {
                string strID = value.ToString();
                int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
                int count = 0;

                if (strID == null)
                    return false;

                strID = strID.PadLeft(9, '0');

                for (int i = 0; i < 9; i++)
                {
                    int num = Int32.Parse(strID.Substring(i, 1)) * id_12_digits[i];

                    if (num > 9)
                        num = (num / 10) + (num % 10);

                    count += num;
                }

                return (count % 10 == 0);
            }
            else
            {
                return false;
            }
            
        }
    }
}
