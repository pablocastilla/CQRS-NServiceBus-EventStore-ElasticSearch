using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Controllers.DTOs
{
    public class NewClientDTO
    {
        public string id { get; set; }

        public string name { get; set; }

        public double initialDeposit { get; set; }
    }
}