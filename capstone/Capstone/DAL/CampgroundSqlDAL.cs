using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;
using System.Configuration;

namespace Capstone.DAL
{

    public class CampgroundSqlDAL
    {
        #region Data Access Languages
        //string connectionString = ConfigurationManager.ConnectionStrings["Campground"].ConnectionString;
        private const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
        private const string SQL_GetParksAlphabetically = "select * from park order by name asc";
        private const string SQL_GetParkInfo = "select * from park where park.name = @parkName";
        private const string SQL_GetCamgroundsByPark = "SELECT * FROM campground JOIN park ON park.park_id = campground.park_id" +
        " WHERE  park.name = @parkName;";
        

        private const string SQL_GetReservationsByCampground = "select * from reservation JOIN site " +
        "ON site.site_id = reservation.site_id JOIN campground ON site.campground_id = campground.campground_id " +
        "WHERE campground.name = @campground";

        private const string SQL_GetCampgroundAvailability = "SELECT * FROM site join campground on campground.campground_id = site.campground_id " +
            "WHERE campground.name = @campgroundName AND site.site_id NOT IN (SELECT site.site_id FROM reservation Join site ON site.site_id = reservation.site_id " +
            "JOIN campground ON campground.campground_id = site.campground_id WHERE campground.name = @campgroundName and (@startDate between reservation.from_date and reservation.to_date) " +
            "or (@endDate between reservation.from_date and reservation.to_date) or(reservation.from_date between @startDate and @endDate) or " +
            "(reservation.to_date between @startDate and @endDate)  ) order by site.site_id";

        private const string SQL_GetParkAvailability = "SELECT TOP 5 site.* FROM site JOIN campground ON campground.campground_id = site.campground_id " +
            "JOIN park ON park.park_id = campground.park_id WHERE park.name = @parkName AND site.site_id NOT IN (SELECT site.site_id FROM reservation " +
            "join site ON reservation.site_id = site.site_id join campground ON campground.campground_id = site.campground_id join park on campground.park_id = park.park_id " +
            "WHERE park.name = @parkName AND  ((@startDate between reservation.from_date and reservation.to_date) OR (@endDate between reservation.from_date and reservation.to_date)))";

        private const string SQL_InsertReservation = "insert into reservation(site_id, name, from_date, to_date) VALUES (@siteID, @name, @startDate, @endDate)";

        private const string SQL_GetOpeningsForNext30Days = "SELECT site.* FROM site join campground " +
        "ON campground.campground_id = site.campground_id join park on campground.park_id = park.park_id " +
        "WHERE park.name = @campgroundName and site.site_id IN (SELECT site.site_number " +
        "FROM reservation WHERE ((@startDate between reservation.from_date and reservation.to_date) " +
        "OR (@endDate between reservation.from_date and reservation.to_date)))";

        private const string SQL_AdvancedSearchWithoutRV = "select * from site where max_occupancy >= @numOfGuests " +
        "and accessible = @wheelchairAccessible and utilities = @utilitiesHookup";

        private const string SQL_AdvancedSearchWithRV = "select * from site where max_occupancy >= @numOfGuests" +
            " and max_rv_length >= @rvlength and accessible = @wheelchairAccessible and utilities = @utilitiesHookup";
        private const string getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";

        #endregion 

        public Park GetParkInfo(string parkName)
        {
            Park park = new Park();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParkInfo, conn);
                    cmd.Parameters.AddWithValue("@parkName", parkName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        park = GetParkFromReader(reader);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return park;
        }

        public Campground GetCampgroundInfo(string campgroundName)
        {
            const string SQL_GetCampgroundByName = "SELECT * FROM campground WHERE campground.name = @campgroundName";
            Campground campground = new Campground();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCampgroundByName, conn);
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        campground = GetCampgroundFromReader(reader);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return campground;
        }

        public Dictionary<int, Park> GetAllParksAlphabetically()
        {
            Dictionary<int,Park> parks = new Dictionary<int, Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParksAlphabetically, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int key = 1;
                    while (reader.Read())
                    {
                        Park item = GetParkFromReader(reader);
                        parks.Add(key, item);
                        key++;
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return parks;

        }

        //As a user of the system, I need the ability to select a park that
        //my customer is visiting and see a list of all campgrounds for that available park.
        public Dictionary<int, Campground> GetAllCampgroundsInPark(string parkName)
        {
            Dictionary<int, Campground> campgrounds = new Dictionary<int, Campground>();
            int count = 1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCamgroundsByPark, conn);
                    cmd.Parameters.AddWithValue("@parkname", parkName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground item = GetCampgroundFromReader(reader);
                        campgrounds.Add(count, item);
                        count++;
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return campgrounds;

        }

        //As a user of the system, I need the ability to select a campground and
        //search for date availability so that I can make a reservation.
        public List<Site> GetCampgroundAvailability(string campgroundName, DateTime startDate, DateTime endDate)
        {
            List<Site> sites = new List<Site>();
            

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCampgroundAvailability, conn);
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site item = GetSitesFromReader(reader);
                        sites.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            Campground campground = GetCampgroundInfo(campgroundName);

            bool inSeason = Between(startDate.Month, endDate.Month, campground.Open_From_MM, campground.Open_To_MM);

            if (!inSeason)
            {
                sites.Clear();
            }

            return sites;

        }


        public int BookReservation(string personName, int siteNumber, DateTime startDate, DateTime endDate)
        {
            int lastId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd;
                    cmd = new SqlCommand(SQL_InsertReservation + getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@siteID", siteNumber);
                    cmd.Parameters.AddWithValue("@name", personName);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    lastId = (int)cmd.ExecuteScalar();

                }
            }
            catch (SqlException)
            {

                throw;
            }
            return lastId;
        }

        //As a user of the system, I want the ability to select a park and search for campsite
        //availability across the entire park so that I can make a reservation.
        public List<Site> GetParkAvailability(string parkName, DateTime startDate, DateTime endDate)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParkAvailability, conn);
                    cmd.Parameters.AddWithValue("@parkName", parkName);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site item = GetSitesFromReader(reader);
                        sites.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            if (true)
            {

            }

            return sites;
        }

        ////BONUS: As a user of the system, I would like the ability to see a list
        ////of all upcoming reservations within the next 30 days for a selected national park.
        public List<Site> SearchParkForMadeReservations(string parkName)
        {
            List<Site> sites = new List<Site>();
            DateTime date = DateTime.Now;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetOpeningsForNext30Days, conn);
                    cmd.Parameters.AddWithValue("@parkName", parkName);
                    cmd.Parameters.AddWithValue("@startDate", date);
                    cmd.Parameters.AddWithValue("@endDate", date.AddDays(30));

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site item = GetSitesFromReader(reader);
                        sites.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return sites;

        }

        ////Provide an advanced search functionality allowing users to indicate any
        ////requirements they have for maximum occupancy, requires wheelchair 
        ////accessible site, an rv and its length if required, and if a utility hookup is necessary.
        public List<Site> AdvancedSearchWithoutRV(int numberOfGuests, bool wheelchairAccessible, bool utilityHookupRequired)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AdvancedSearchWithoutRV, conn);
                    cmd.Parameters.AddWithValue("@numOfGuests", numberOfGuests);
                    cmd.Parameters.AddWithValue("@wheelchairAccessible", wheelchairAccessible);
                    cmd.Parameters.AddWithValue("@utilitiesHookup", utilityHookupRequired);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site item = GetSitesFromReader(reader);
                        sites.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return sites;

        }

        public List<Site> GetCampgroundAvailabilityAdvancedSearch(string parkName, DateTime startDate, DateTime endDate, int numberOfGuests, bool wheelchairAccessible, int rvlength, bool utilityHookupRequired)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AdvancedSearchWithRV, conn);
                    cmd.Parameters.AddWithValue("@numOfGuests", numberOfGuests);
                    cmd.Parameters.AddWithValue("@wheelchairAccessible", wheelchairAccessible);
                    cmd.Parameters.AddWithValue("@rvlength", rvlength);
                    cmd.Parameters.AddWithValue("@utilitiesHookup", utilityHookupRequired);
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site item = GetSitesFromReader(reader);
                        sites.Add(item);
                    }

                }
            }
            catch (SqlException)
            {

                throw;
            }

            return sites;

        }



        #region From Reader() methods
        private Park GetParkFromReader(SqlDataReader reader)
        {
            Park item = new Park
            {
                ParkID = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Location = Convert.ToString(reader["location"]),
                Establish_Date = Convert.ToDateTime(reader["establish_date"]),
                Area = Convert.ToString(reader["area"]),
                Visitors = Convert.ToInt32(reader["visitors"]),
                Description = Convert.ToString(reader["description"]),

            };

            return item;

        }

        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            if (true)
            {

            }
            Campground item = new Campground
            {
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                ParkId = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Open_From_MM = Convert.ToInt32(reader["open_from_mm"]),
                Open_To_MM = Convert.ToInt32(reader["open_to_mm"]),
                Daily_Fee = Convert.ToDouble(reader["daily_fee"]),

            };

            return item;

        }

        private Reservation GetReservationsFromReader(SqlDataReader reader)
        {
            Reservation item = new Reservation
            {
                ReservationID = Convert.ToInt32(reader["reservation_id"]),
                SiteID = Convert.ToInt32(reader["site_id"]),
                Name = Convert.ToString(reader["name"]),
                FromDate = Convert.ToDateTime(reader["from_date"]),
                ToDate = Convert.ToDateTime(reader["to_date"]),
                CreateDate = Convert.ToDateTime(reader["create_date"]),

            };

            return item;

        }

        private Site GetSitesFromReader(SqlDataReader reader)
        {
            Site item = new Site
            {
                SiteID = Convert.ToInt32(reader["site_id"]),
                CampgroundID = Convert.ToInt32(reader["campground_id"]),
                SiteNumber = Convert.ToInt32(reader["site_number"]),
                MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                WheelchairAccess = Convert.ToString(reader["accessible"]),
                MaxRVLength = Convert.ToString(reader["max_rv_length"]),
                UtilityHookups = Convert.ToString(reader["utilities"]),

            };

            return item;

        }

        private static bool Between(int reservStartMonth, int reservEndMonth, int parkOpenFromMonth, int parkOpenToMonth)
        {
            bool inclusive = (parkOpenFromMonth <= reservStartMonth && reservStartMonth <= parkOpenToMonth) &&
                (parkOpenFromMonth <= reservEndMonth && reservEndMonth <= parkOpenToMonth);

            return inclusive;
        }


        #endregion
    }
}