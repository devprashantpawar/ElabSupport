﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElabSupport.Models
{
    public class ExcelData
    {
        public DateTime Date { get; set; }
        public int holiday { get; set; }
        public int Interface { get; set; }
        public List<string> SupportPersons { get; set; }
        
    }
}