using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicagoInABox.Models.ViewModel
{
    public class AccountViewModel
    {
        public LoginModel LoginDetails{ get; set; }
        public RegisterModel RegistrationDetails { get; set; }
    }
}