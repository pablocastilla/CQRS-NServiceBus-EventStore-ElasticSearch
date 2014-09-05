using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Controllers.DTOs
{
    public class UpdateClientDTO
    {
        public string id { get; set; }

        public double quantity { get; set; }

        public string inATM { get; set; }
    }
}