using Capstone;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capstone
{
    public class ProgramCLI
    {
        #region Command Constants
        private const string command_Cancel = "0";
        private const string command_SelectAcadia = "1";
        private const string command_SelectArches = "2";
        private const string command_SelectCuyahoga = "3";
        private const string command_Quit = "q";
        private const string command_ViewCampgrounds = "1";
        private const string command_SearchReservations = "2";
        private const string command_AdvanceSearchReservations = "2";

        private const string command_ReturnToPreviousScreen = "3";
        private const string command_SearchForAvailableReservations = "1";
        private const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
        #endregion End Of Commands

        private Dictionary<int, string> Parks = new Dictionary<int, string>()
        {
            {1, "Acadia" },
            {2, "Arches" },
            {3, "Cuyahoga Valley" }
        };
        private Dictionary<int, string> Months = new Dictionary<int, string>()
        {
            {1,"January" },
            {2, "February" },
            {3,"March" },
            {4, "April" },
            {5, "May" },
            {6, "June" },
            {7, "July" },
            {8, "August" },
            {9, "September" },
            {10, "October" },
            {11, "November" },
            {12, "December" }
        };

        public void RunCLI()
        {
            int numAttempt = 0;
            do
            {
                Console.Clear();
                MainMenu();

                CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
                Dictionary<int, Park> parks = campgroundDAL.GetAllParksAlphabetically();


                if (numAttempt > 0)
                {
                    Console.WriteLine("The command provided was not a valid, please try again.");
                }

                ConsoleKeyInfo userInput = Console.ReadKey();
                string command = userInput.KeyChar.ToString();

                
                int.TryParse(command, out int parkKey);
                bool isValidUserParkChoice = parks.ContainsKey(parkKey);

                if (isValidUserParkChoice)
                {
                    bool isTurningoff = false;

                    while (!isTurningoff)
                    {
                        int park = int.Parse(command);
                        isTurningoff = GetParkInfo(park);
                    }
                }
                else if (command.ToLower() == command_Quit)
                {
                    return;
                }
                numAttempt++;
            } while (true);
        }

        #region Menus

        private void MainMenu()
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            Dictionary<int, Park> parks = campgroundDAL.GetAllParksAlphabetically();

            Console.WriteLine("Select a park for further details");
            Console.WriteLine();
            Console.WriteLine(String.Format("").PadRight(35, '='));
            foreach (KeyValuePair<int, Park> park in parks)
            {
                Console.WriteLine($"{park.Key}) {park.Value.Name}");
            }

            Console.WriteLine("Q) Quit");
            Console.WriteLine();

        }

        private void PrintParkInfoMenu()
        {
            Console.WriteLine("Select a Command");
            Console.WriteLine(String.Format("").PadRight(30, '-'));
            Console.WriteLine(" 1) View Campgrounds");
            Console.WriteLine(" 2) Search for Reservations");
            Console.WriteLine(" 3) View Reservations For Next 30 days");
            Console.WriteLine(" 4) Return to Previous Screen");
            Console.WriteLine();

        }

        private void PrintCampgroundMenu()
        {
            Console.WriteLine("Select a campground for further Details");
            Console.WriteLine(String.Format("").PadRight(50, '-'));
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Advanced Search for Available Reservation");
            Console.WriteLine("3) Return to Previous Screen");
            Console.WriteLine();

        }

        #endregion end of menus

        #region CrudCLIMethods()
        private bool GetParkInfo(int parkDictionaryKey)
        {
            bool returnToPrevious = false;
             

            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

            Park park = campgroundDAL.GetParkInfo(Parks[parkDictionaryKey]);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"{park.Name} National Park");
            Console.WriteLine(String.Format("").PadRight(30, '='));
            Console.WriteLine(String.Format("Location:").PadRight(19) + park.Location);
            Console.WriteLine(String.Format("Established:").PadRight(19) + park.Establish_Date.ToShortDateString());
            Console.WriteLine(String.Format("Area:").PadRight(19) + park.Area + " sq km");
            Console.WriteLine(String.Format("Annual Visitors:").PadRight(19) + park.Visitors);

            Console.WriteLine();

            PrintParkInfoMenu();
            ConsoleKeyInfo userInput = Console.ReadKey();
            string command = userInput.KeyChar.ToString();

            const string command_PreviousScreen = "4";
            const string command_Get30DaysOfReservations = "3";

            switch (command)
            {
                case command_ViewCampgrounds:
                    GetCampgrounds(Parks[parkDictionaryKey]);
                    break;
                case command_SearchReservations:
                    GetParkWideAvailability(park.Name);
                    break;
                case command_Get30DaysOfReservations:
                    GetAllReservationsFor30DaysByPark(park.Name);
                    break;
                case command_PreviousScreen:
                    returnToPrevious = true;
                    break;
                default:
                    Console.WriteLine("The command provided was not a valid, please try again.");
                    command = Console.ReadKey().ToString();
                    break;
            }
            return returnToPrevious;
        }

        private bool GetCampgrounds(string parkName)
        {
            bool returnToPrevious = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"{parkName} National Park Campgrounds");
            
            Dictionary<int, Campground> campgrounds = GetAndPrintCampgrounds(parkName);

            
            Console.WriteLine();
            PrintCampgroundMenu();

            ConsoleKeyInfo userInput = Console.ReadKey();
            string command = userInput.KeyChar.ToString();
            Console.Write("\b");

            switch (command)
            {
                case command_SearchForAvailableReservations:
                    Console.Clear();
                    
                    GetAndPrintCampgrounds(parkName);
                    Console.WriteLine();
                    GetCampgroundAvailability(campgrounds);
                    break;
                case command_AdvanceSearchReservations:
                    GetCampgroundAvailabilityAdvanced(campgrounds);
                    break;
                case command_ReturnToPreviousScreen:
                    returnToPrevious = true;
                    break;
                default:
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                    command = Console.ReadKey().ToString();
                    break;
            }
            return returnToPrevious;
        }

        private void GetCampgroundAvailability(Dictionary<int, Campground> campgrounds)
        {

            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            int campground = CLIHelper.GetInteger("Which Campground (enter 0 to cancel):");
            bool returnToPrevious = campground == int.Parse(command_Cancel);
            //Return to previous screen if user enters 0
            if (returnToPrevious)
            {
                return;
            }

            while (!campgrounds.ContainsKey(campground))
            {
                campground = campground = CLIHelper.GetInteger("Invalid choice. Please pick a campground Site number from available list:");
            };

            bool stillBooking = false;
            do
            {
                DateTime startDate = CLIHelper.GetDateTime("Enter start date (YYYY/MM/DD):");
                DateTime endDate = CLIHelper.GetDateTime("Enter end date (YYYY/MM/DD):");

                var availableSites = campgroundDAL.GetCampgroundAvailability(campgrounds[campground].Name, startDate, endDate);

                if (availableSites.Count > 0)
                {
                    List<int> availableSiteNumbers = new List<int>();

                    int totalReservDays = (int)(endDate - startDate).TotalDays;

                    Console.Clear();
                    Console.WriteLine("Results Matching Your Search Criteria");
                    Console.WriteLine(String.Format("").PadRight(30, '='));
                    Console.WriteLine("{0,3}{1,12}{2,13}{3,14}{4,9}{5,10}{6,7}", "Campground", "Site No.", "Max Occup.", "Accessible?", "RV Len", "Utility", "Cost");
                    foreach (var site in availableSites)
                    {
                        double cost = campgrounds[campground].Daily_Fee * totalReservDays;
                        availableSiteNumbers.Add(site.SiteNumber);

                        Console.WriteLine(campgrounds[campground].Name.PadRight(17) + site.SiteNumber.ToString().PadRight(12) + site.MaxOccupancy.ToString().PadRight(13) +
                            site.WheelchairAccess.PadRight(11) + site.MaxRVLength.PadRight(10) + site.UtilityHookups.PadRight(9) + cost);
                    }

                    bool reservationSuccessful = BookReservation(availableSiteNumbers, startDate, endDate);
                    if (reservationSuccessful)
                    {
                        stillBooking = false;
                    }
                }
                else
                {
                    Console.WriteLine("Sorry, there are no sites available in the specified date range.");
                    stillBooking = CLIHelper.GetBoolFromYesOrNo("Would you like to enter another date range?");
                }
            } while (stillBooking);
            

            
        }
        private void GetCampgroundAvailabilityAdvanced(Dictionary<int, Campground> campgrounds)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            int campground = campground = CLIHelper.GetInteger("Which Campground (enter 0 to cancel):");
            bool returnToPrevious = campground == int.Parse(command_Cancel);
            //Return to previous screen if user enters 0
            if (returnToPrevious)
            {
                return;
            }

            while (!campgrounds.ContainsKey(campground))
            {
                campground  = CLIHelper.GetInteger("Invalid choice. Please pick a campground number from available list:");
            };

            DateTime startDate = CLIHelper.GetDateTime("Enter start date (YYYY/MM/DD):");
            DateTime endDate = CLIHelper.GetDateTime("Enter end date (YYYY/MM/DD):");

            int numberOfGuests = CLIHelper.GetInteger("How many guests?");

            bool wheelChairAccessible = CLIHelper.GetBoolFromYesOrNo("Wheel chair accessible (y or n)?");

            int rvLength = CLIHelper.GetInteger("RV length: ");

            bool utilityHookup = CLIHelper.GetBoolFromYesOrNo("Utility hookups (y or n)?");

            bool stillBooking = false;

            do
            {
                
                List<Site> availableSites = campgroundDAL.GetCampgroundAvailabilityAdvancedSearch(campgrounds[campground].Name, startDate, endDate, 
                    numberOfGuests, wheelChairAccessible, rvLength, utilityHookup);

                if (availableSites.Count > 0)
                {
                    Console.Clear();
                    Console.WriteLine("Results Matching Your Search Criteria");
                    Console.WriteLine();
                    Console.WriteLine("{0,3}{1,12}{2,13}{3,14}{4,9}{5,10}{6,7}", "Campground", "Site No.", "Max Occup.", "Accessible?", "RV Len", "Utility", "Cost");
                    List<int> availableSiteNumbers = new List<int>();

                    int totalReservDays = (int)(endDate - startDate).TotalDays;

                    foreach (var site in availableSites)
                    {
                        availableSiteNumbers.Add(site.SiteNumber);
                        double cost = totalReservDays * campgrounds[campground].Daily_Fee;

                        Console.WriteLine(campgrounds[campground].Name.PadRight(17) + site.SiteNumber.ToString().PadRight(12) + site.MaxOccupancy.ToString().PadRight(13) +
                            site.WheelchairAccess.PadRight(11) + site.MaxRVLength.PadRight(10) + site.UtilityHookups.PadRight(9) + cost);
                    }

                    BookReservation(availableSiteNumbers, startDate, endDate);
                }
                else
                {
                    Console.WriteLine("Sorry, there are no sites available in the specified date range.");
                    stillBooking = CLIHelper.GetBoolFromYesOrNo("Would you like to enter another date range (y or n)?");

                    if (stillBooking)
                    {
                        startDate = CLIHelper.GetDateTime("Enter start date (YYYY/MM/DD):");
                        endDate = CLIHelper.GetDateTime("Enter end date (YYYY/MM/DD):");
                    }
                    
                }
            } while (stillBooking);

        }

        private void GetParkWideAvailability(string parkName)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            Dictionary<int,Campground> campgrounds = campgroundDAL.GetAllCampgroundsInPark(parkName);

            Console.Clear();
            DateTime startDate = CLIHelper.GetDateTime("What is the arrival date (YYYY/MM/DD)?:");
            DateTime endDate = CLIHelper.GetDateTime("What is the departure date (YYYY/MM/DD)?:");

            List<Site> availableSites = campgroundDAL.GetParkAvailability(parkName, startDate, endDate ); 

            if (availableSites.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("Results Matching Your Search Criteria");
                Console.WriteLine("{0,13}{1,12}{2,13}{3,14}{4,9}{5,10}{6,7}", "Campground", "Site No.", "Max Occup.", "Accessible?", "RV Len", "Utility", "Cost");
                List<int> availableSiteNumbers = new List<int>();

                int totalReservDays = (int)(endDate - startDate).TotalDays;

                foreach (var site in availableSites)
                {
                    Campground campground = campgrounds.Where(i => i.Value.CampgroundId == site.CampgroundID).First().Value;
                    availableSiteNumbers.Add(site.SiteNumber);
                    double cost = totalReservDays * campground.Daily_Fee;

                    Console.WriteLine(campground.Name.PadRight(13) + site.SiteNumber.ToString().PadRight(12) + site.MaxOccupancy.ToString().PadRight(13) +
                        site.WheelchairAccess.PadRight(19) + site.MaxRVLength.PadRight(9) + site.UtilityHookups.PadRight(10) + cost);
                }

                BookReservation(availableSiteNumbers, startDate, endDate);

                
            }
            else
            {
                Console.WriteLine("Sorry, there are no sites available in the specified date range.");
                bool stillBooking = CLIHelper.GetBoolFromYesOrNo("Would you like to enter another date range?");

            }
        }

        private bool BookReservation(List<int> availableSiteNumbers, DateTime startDate, DateTime endDate)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

            bool successfullyBooked = false; 

            Console.WriteLine();
            int siteToReserve = CLIHelper.GetInteger("Which site should be reserved(enter 0 to cancel) ?");

            //Return to previous screen if user enters 0
            if (siteToReserve == int.Parse(command_Cancel))
            {
                return successfullyBooked;
            }

            while (!(availableSiteNumbers.Contains(siteToReserve)))
            {
                siteToReserve = CLIHelper.GetInteger("Invalid site choice. Which site should be reserved(enter 0 to cancel)?");
            }

            string reservationName = CLIHelper.GetString("What name should the reservation be made under?");

            int? reservationId = null;
            reservationId = campgroundDAL.BookReservation(reservationName, siteToReserve, startDate, endDate);

            if (reservationId.HasValue)
            {
                

                bool returnToPrevious = false;
                do
                {
                    Console.WriteLine();
                    Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}");

                    Console.WriteLine();
                    Console.WriteLine("Press (Enter) Return to previous screen");
                    char userInput = Console.ReadKey().KeyChar;

                    returnToPrevious = userInput == (char)Keys.Return ? true : false;

                } while (!returnToPrevious);
                successfullyBooked = true;

            }
            else
            {
                Console.WriteLine("Reservation attempt failed.");
                Console.WriteLine("Would you like to enter another date?: ");
                Console.ReadKey();
            }
            return successfullyBooked;
        }

        

        private Dictionary<int, Campground> GetAndPrintCampgrounds(string parkName)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            Dictionary<int, Campground> campgrounds = campgroundDAL.GetAllCampgroundsInPark(parkName);

            //int campgroundNameLength

            Console.WriteLine();
            Console.WriteLine("{0, -6}{1,-35}{2,-12}{3,-12}{4,-14}", "", "Name", "Open", "Close", "Daily Fee");
            Console.WriteLine(String.Format("").PadRight(80, '='));

            foreach (KeyValuePair<int, Campground> campground in campgrounds)
            {

                Console.WriteLine('#' + campground.Key.ToString().PadRight(5) + campground.Value.Name.PadRight(35) + Months[campground.Value.Open_From_MM].PadRight(12) + 
                    Months[campground.Value.Open_To_MM].PadRight(12) + String.Format("{0:c}", campground.Value.Daily_Fee).PadRight(14));
            }
            return campgrounds;
        }


        private void GetAllReservationsFor30DaysByPark(string parkName)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            List<Reservation> reservations = campgroundDAL.GetAllReservationsForNext30Days(parkName);

            bool returnToPrevious = false;
            do
            {
                Console.Clear();
                Console.WriteLine("{0,-18}{1,-10}{2, -30}{3,-13}{4,-13}", "Confirmation ID", "Site", "Name", "From-Date", "To-Date");
                Console.WriteLine(String.Format("").PadRight(80, '='));

                foreach (var reserv in reservations)
                {
                    Console.WriteLine(reserv.ReservationID.ToString().PadRight(18) + reserv.SiteID.ToString().PadRight(10) + 
                        reserv.Name.PadRight(30) + reserv.FromDate.ToShortDateString().PadRight(13) + reserv.ToDate.ToShortDateString().PadRight(13));
                }

                Console.WriteLine();
                Console.WriteLine("Press (Enter) to return to previous screen");
                char userInput = Console.ReadKey().KeyChar;

                returnToPrevious = userInput == (char)Keys.Return ? true : false;

            } while (!returnToPrevious);

        }
        #endregion
    }
}
