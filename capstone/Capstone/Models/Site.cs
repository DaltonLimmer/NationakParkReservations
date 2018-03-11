using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        private string _wheelChairAccess = "0";
        private string _maxRVLength = "0";
        private string _utility = "0";
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public string WheelchairAccess
        {
            get
            {
                string yesOrNo = "No";
                if (_wheelChairAccess == "1")
                {
                    return "Yes";
                }
                return yesOrNo;
            }
            set
            {
                _wheelChairAccess = value;
            }
        }
        public string MaxRVLength
        {
            get
            {
                string lengthString = "N/A";

                if (int.Parse(_maxRVLength) > 0)
                {
                    lengthString = _maxRVLength;
                }

                return lengthString;
            }
            set
            {
                _wheelChairAccess = value;
            } 
        }
        public string UtilityHookups
        {
            get
            {
                string availability = "N/A";

                if (_utility == "1")
                {
                    availability = "Yes";
                }

                return availability;
            }
            set
            {
                _utility = value;
            }
        }

    }
}