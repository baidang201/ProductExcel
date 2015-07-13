using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signalway.CommThemes.Modules
{
    public class timeZone
    {
        public int ID { get; set; }
        public string ZoneName { get; set; }

        public timeZone()
        {
            ID = -1;
            ZoneName = String.Empty;
        }

        public timeZone(int id, string zname)
        {
            ID = id;
            ZoneName = zname;
        }

        public override string ToString()
        {
            return ZoneName;
        }
    }
}
