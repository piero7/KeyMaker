using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyReader
{
 
    public class Class1
    {
    }

    public class Key
    {
        public string KeyString { get; set; }

        public string Salt { get; set; }

        public bool IsUseDays { get; set; }

        public DateTime LastDate { get; set; }

        public int LastDays { get; set; }

        public string CustomerName { get; set; }

        
    }
}
