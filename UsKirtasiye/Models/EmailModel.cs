using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsKirtasiye.Models
{
    public class EmailModel
    {
        public string FromEmailAdress { get; set; }

        public string ToEmailAdress { get; set; }

        public string Subject { get; set; }

        public string HtmlContent { get; set; }
    }
}