using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Kursach.Models
{
    public class UserModel
    {
        [Name("login")]
        public string login { get; set; }
        [Name("email")]
        public string email { get; set; }
        [Name("password")]
        public string password { get; set; }
        [Name("role")]
        public string role { get; set; }
        [Name("familia")]
        public string familia { get; set; }
        [Name("ima")]
        public string ima { get; set; }
        [Name("otchestvo")]
        public string otchestvo { get; set; }

        string fio = "f";
        [Ignore]
        public string Fio
        {
            get
            {
                if(otchestvo.Length > 0)
                    return fio = familia+" " + ima[0] + ". " + otchestvo[0] + ".";
                else
                {
                    return fio = familia + " " + ima[0] + ". ";
                }
            }
        }
    }
}
