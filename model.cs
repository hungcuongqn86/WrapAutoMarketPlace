using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrapAutoMarketPlace
{
    class model
    {
    }

    public class HardDrive
    {
        public string Model { get; set; }
        public string InterfaceType { get; set; }
        public string Caption { get; set; }
        public string SerialNo { get; set; }
    }

    public class ResBody
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
