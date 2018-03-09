using Capstone;
using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class ProgramCLI
    {
        private const string command_Cancel = "0";
        private const string command_SelectAcadia = "1";
        private const string command_SelectArches = "2";
        private const string command_SelectCuyahoga = "3";
        private const string command_Quit = "q";
        private const string command_ViewCampgrounds = "1";
        private const string command_SearchReservations = "2";
        private const string command_ReturnToPreviousScreen = "3";
        private const string command_SearchForAvailableReservations = "1";
        private const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
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
            PrintMainMenu();

            while (true)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();
                string command = userInput.KeyChar.ToString();

                switch (command)
                {
                    case command_SelectAcadia:
                        int park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_SelectArches:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_SelectCuyahoga:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        break;
                    case command_Quit:
                        park = int.Parse(command);
                        GetParkInfo(park);
                        return;
                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
            }
        }

        //Menus
        private void PrintMainMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) Acadia");
            Console.WriteLine(" 2) Arches");
            Console.WriteLine(" 3) Cuyahoga National Valley Park");
            Console.WriteLine();

            Console.WriteLine(" Q) Quit");
            Console.WriteLine();

        }

        private void PrintParkInfoMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) View Campgrounds");
            Console.WriteLine(" 2) Search for Reservations");
            Console.WriteLine(" 3) Return to Previous Screen");
            Console.WriteLine();

        }

        private void PrintCampgroundMenu()
        {
            Console.WriteLine("Select a Park for further Details");
            Console.WriteLine(" 1) Search for Available Reservation");
            Console.WriteLine(" 2) Return to Previous Screen");
            Console.WriteLine();

        }

      

        // Search
        private void GetAllParks()
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

        }

        //Get Park Info
        private void GetParkInfo(int parkDictionaryKey)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();

            Park park = campgroundDAL.GetParkInfo(Parks[parkDictionaryKey]);

            Console.Clear();
            Console.WriteLine($"{park.Name}");
            Console.WriteLine($"Location: {park.Location}");
            Console.WriteLine($"Established: {park.Establish_Date.ToShortDateString()}");
            Console.WriteLine($"Area: {park.Area} sq km");
            Console.WriteLine($"Annual Visitors: {park.Visitors}");

            Console.WriteLine();

            PrintParkInfoMenu();
            ConsoleKeyInfo userInput = Console.ReadKey();
            string command = userInput.KeyChar.ToString();


            switch (command)
            {
                case command_ViewCampgrounds:
                    GetCampgrounds(Parks[parkDictionaryKey]);
                    break;
                case command_SearchReservations:
                    GetCampgroundAvailability(Parks[parkDictionaryKey]);
                    break;
                case command_ReturnToPreviousScreen:

                    break;
                default:
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                    command = Console.ReadKey().ToString();
                    break;
            }



        }

        private void GetCampgrounds(string parkName)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            Dictionary<int, Campground> campgrounds = campgroundDAL.GetAllCampgroundsInPark(parkName);

            Console.Clear();
            Console.WriteLine();
            foreach (KeyValuePair<int, Campground> campground in campgrounds)
            {
                Console.WriteLine($"#{campground.Key} {campground.Value.Name} {Months[campground.Value.Open_From_MM]} {Months[campground.Value.Open_To_MM]} {campground.Value.Daily_Fee}");
            }

            Console.WriteLine();
            PrintCampgroundMenu();
            string command = Console.ReadKey().ToString();

            switch (command)
            {
                case command_SearchForAvailableReservations:
                    //Search Availabilities
                    GetCampgrounds(parkName);
                    Console.WriteLine();
                    GetCampgroundAvailability(campgrounds);
                    break;
                case command_ReturnToPreviousScreen:

                    break;
                default:
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                    command = Console.ReadKey().ToString();
                    break;
            }
        }

        private void GetCampgroundAvailability(Dictionary<int, Campground> campgrounds)
        {
            CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL();
            int campground = CLIHelper.GetInteger("Which Campground:");
            DateTime startDate = CLIHelper.GetDateTime("Enter start date:");
            DateTime endDate = CLIHelper.GetDateTime("Enter end date:");

            campgroundDAL.CampgroundAvailability(campgrounds[campground].Name, startDate, endDate);

        }


        ////Availabilities
        //private void GetAvailableCampgroundReservations()
        //{

        //}

        private void GetParkCampsiteAdvancedSearch()
        {

        }



        //Booking
        private void BookReservation()
        {

        }

        private void SearchParkForMadeReservations()
        {

        }

    }
}
