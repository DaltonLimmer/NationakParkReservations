using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        private string _wheelChairAccess;
        private string _maxRVLength;
        private string _utility;
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public string WheelchairAccess
        {
            get
            {
                if (_wheelChairAccess == "true")
                {
                    _wheelChairAccess = "Yes";
                }
                else
                {
                    _wheelChairAccess = "N/A";
                }
                return _wheelChairAccess;
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
                if (_maxRVLength == "true")
                {
                    _maxRVLength = "Yes";
                }
                else
                {
                    _maxRVLength = "N/A";
                }

                return _maxRVLength;
            }
            set
            {
                _maxRVLength = value;
            } 
        }
        public string UtilityHookups
        {
            get
            {

                if (_utility == "true")
                {
                    _utility = "Yes";
                }
                else
                {
                    _utility = "N/A";
                }

                return _utility;
            }
            set
            {
                _utility = value;
            }
        }

    }
}