using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsKirtasiye.Models
{
    public class ProfileModel
    {
        public DB.Members members { get; set; }
        public List<DB.Addresses> Addresses { get; set; }
    }
}