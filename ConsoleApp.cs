using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FInalOOPproject
{
     public class ConsoleApp
    {
        private readonly FileService _fileService;
        private EcoTracker? _tracker;
        private string? _currentUser;
        private bool _isRunning = true;

        public ConsoleApp()
        {
            _fileService = new FileService();
        }

        public void Run()
        {
            Console.Title = "Sustainable Lifestyle Tracker";
            Welcome();

            while (_isRunning)
            {
                if (_currentUser == null)
                    LoginMenu();
                else
                    MainMenu();
            }
        }

        private void Welcome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    _____           __       _               __  __  
   / ___/__  ______/ /____ _ (_)___  ____ _ / /_ / /__  
   \__ \/ / / / ___/ __/ __ `/ / __ \/ __ `/ __ \/ / _ \
  ___/ / /_/ (__  ) /_/ /_/ / / / / / /_/ / /_/ / /  __/
 /____/\__,_/____/\__/\__,_/_/_/ /_/\__,_/_.___/_/\___/  
    
     __   _ ____           __        __  
    / /  (_) __/__  _____/ /___  __/ /__  
   / /  / / /_/ _ \/ ___/ __/ / / / / _ \
  / /___/ / __/  __(__  ) /_/ /_/ / /  __/
 /_____/_/_/  \___/____/\__/\__, /_/\___/  
                             /____/        
  _______  ______  ________ __ __  _______
 /_  __/ |/ / __ \/ ____/ //_// / / / ___/
  / /  |   / /_/ / /   / ,<  / /_/ / __/  
 / /  /   / _, _/ /___/ /| |/ __  / /___  
/_/  /_/|/_/ |_|\____/_/ |_/_/ /_/_____/  
");

            Console.WriteLine("\n     --- Track your impact. Save the planet. ---\n");
            Console.ResetColor();
        }
        private void LoginMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║                ACCESS CONTROL                 ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "1.) Login"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "2.) Register"));
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "Q.) Quit"));
            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.Write("\n  Choice: ");
            var choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "1": LoginFlow(); break;
                case "2": RegisterFlow(); break;
                case "Q": _isRunning = false; break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  Invalid choice.\n");
                    Console.ResetColor();
                    break;
            }
        }

        private void LoginFlow()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            if (_fileService.Login(username, password))
            {
                _currentUser = username;
                _tracker = new EcoTracker(username, _fileService);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"\nWelcome back, {username}!\n");
                Console.ResetColor();
            }
            else Console.WriteLine("Invalid credentials.\n");
        }

        private void RegisterFlow()
        {
            Console.Clear();
            Console.Write("Choose a username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Choose a password: ");
            string password = Console.ReadLine() ?? "";

            if (_fileService.Register(username, password))
                Console.WriteLine("Registered successfully! You can now log in.\n");
            else
                Console.WriteLine("Username already exists.\n");
        }

        private void MainMenu()
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  User: {_currentUser}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;

            // The width remains 45 for consistency
            Console.WriteLine("\n  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║                  MAIN MENU                    ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "1.) Add Activity"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "2.) View All Activities"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "3.) View Points Summary (All Time)"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "4.) View Monthly Progress"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "5.) Manage Activities (Edit/Delete)"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "6.) Search Activities"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "7.) Clear All Activities"));
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "8.) Logout"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "9.) Delete Account"));
            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");

            Console.ResetColor();
            Console.Write("\n  Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddActivityFlow(); break;
                case "2": ViewActivities(); break;
                case "3": ViewSummary(); break;
                case "4": ViewMonthlySummaryFlow(); break;
                case "5": ManageActivitiesFlow(); break;
                case "6": SearchActivitiesFlow(); break;
                case "7":
                    _tracker?.ClearAll();
                    Console.WriteLine("  All activities cleared.\n");
                    Console.WriteLine("  Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case "8": Logout(); break;
                case "9": DeleteAccountFlow(); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  Invalid option.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    break;
            }
        }

        // --- CREATE FLOW ---
        private void AddActivityFlow()
        {
            Console.Clear(); // Clear screen to focus on the task

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n  ** LOG NEW ACTIVITY **");
            Console.Write("  Enter the date (YYYY-MM-DD) or leave blank for today: ");
            string dateInput = Console.ReadLine() ?? "";

            // Try to parse the input date, default to DateTime.Now if invalid or empty
            DateTime activityDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(dateInput) && !DateTime.TryParse(dateInput, out activityDate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Invalid date format. Using today's date instead.");
                Console.ResetColor();
                activityDate = DateTime.Now;
            }

            // Display Menu
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Logging activity for: {activityDate:yyyy-MM-dd}");
            Console.WriteLine("  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║                 CATEGORY                      ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "1.) Recycling"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "2.) Energy"));
            Console.WriteLine(String.Format("  ║ {0,-45} ║", "3.) Transport"));
            Console.WriteLine("  ║                                               ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.Write("\n  Select category: ");
            string choice = Console.ReadLine() ?? "";

            if (_tracker == null) return;

            switch (choice)
            {
                case "1":
                    Console.Write("  What did you recycle?: ");
                    string item = Console.ReadLine() ?? "Unknown";
                    // Pass the custom date
                    _tracker.AddActivity(new RecyclingActivity(activityDate, item));
                    Console.WriteLine($"  -> Added Recycling Activity: {item} on {activityDate:yyyy-MM-dd}\n");
                    break;
                case "2":
                    Console.Write("  What energy-saving action?: ");
                    string action = Console.ReadLine() ?? "Unknown";
                    // Pass the custom date
                    _tracker.AddActivity(new EnergyActivity(activityDate, action));
                    Console.WriteLine($"  -> Added Energy Activity: {action} on {activityDate:yyyy-MM-dd}\n");
                    break;
                case "3":
                    Console.Write("  What mode of transport?: ");
                    string mode = Console.ReadLine() ?? "Unknown";
                    Console.Write("  Distance in km: ");
                    double.TryParse(Console.ReadLine(), out double dist);
                    // Pass the custom date
                    _tracker.AddActivity(new TransportActivity(activityDate, mode, dist));
                    Console.WriteLine($"  -> Added Transport Activity: {mode} on {activityDate:yyyy-MM-dd}\n");
                    break;
                default:
                    Console.WriteLine("  Invalid category.\n");
                    break;
            }

            Console.WriteLine("  Press Enter to continue...");
            Console.ReadLine();
        }

        // --- READ FLOW (All) ---
        private void ViewActivities()
        {
            if (_tracker == null || !_tracker.Activities.Any())
            {
                Console.WriteLine("\nNo activities yet.\n");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("\n*** All Logged Activities ***");
            foreach (var act in _tracker.Activities.OrderByDescending(a => a.Date)) // Ordered by date
                Console.WriteLine(act);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        // --- MONTHLY SUMMARY FLOW ---
        private void ViewMonthlySummaryFlow()
        {
            if (_tracker == null) return;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n*** MONTHLY PROGRESS SUMMARY ***");
            Console.Write("Enter the **Year and Month** to summarize (YYYY-MM, e.g., 2025-12): ");
            Console.ResetColor();

            if (!DateTime.TryParse(Console.ReadLine() + "-01", out DateTime targetDate))
            {
                Console.WriteLine("Invalid date format. Returning to main menu.");
                Thread.Sleep(1500);
                return;
            }

            // Calculate the summary
            var summary = _tracker.MonthlySummary(targetDate.Year, targetDate.Month);
            int totalPoints = summary.Sum(kv => kv.Value);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine($"  ║        PROGRESS FOR {targetDate:MMMM yyyy}       ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");

            if (totalPoints == 0)
            {
                Console.WriteLine(String.Format("  ║ {0,-45} ║", "No activities logged for this month."));
            }
            else
            {
                // Breakdown Section
                Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "CATEGORY BREAKDOWN", "POINTS"));
                Console.WriteLine("  ║ --------------------------------------------- ║");

                foreach (var kv in summary)
                {
                    Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", kv.Key, "+" + kv.Value));
                }

                Console.WriteLine("  ║                                                ║");
                Console.WriteLine("  ╠═══════════════════════════════════════════════╣");

                // Total
                Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "TOTAL MONTHLY POINTS", totalPoints));
            }

            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        // --- SEARCH FLOW ---
        private void SearchActivitiesFlow()
        {
            if (_tracker == null) return;

            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*** SEARCH ACTIVITIES ***");
            Console.Write("Enter Keyword: ");
            Console.ResetColor();

            string keyword = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Search cancelled.");
                Thread.Sleep(1500);
                return;
            }

            var results = _tracker.SearchActivities(keyword);

            Console.WriteLine($"\n--- Found {results.Count} Activities Matching '{keyword}' ---");
            if (results.Any())
            {
                foreach (var act in results)
                    Console.WriteLine(act);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("No matching activities found.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        // --- UPDATE / DELETE FLOW ---
        private void ManageActivitiesFlow()
        {
            // You might want to view activities before managing them
            ViewActivities();

            if (_tracker == null || !_tracker.Activities.Any()) return;

            Console.Write("Enter the Short ID (first 8 chars) of the activity to manage: ");
            string shortId = Console.ReadLine() ?? "";

            var activity = _tracker.GetActivityByShortId(shortId);

            if (activity == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Activity not found.\n");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"\nSelected: {activity.Category} on {activity.Date:d}");

            Console.WriteLine("D) Delete");
            Console.WriteLine("C) Cancel");
            Console.Write("Choice: ");

            string choice = Console.ReadLine()?.ToUpper() ?? "";

            if (choice == "D")
            {
                if (_tracker!.DeleteActivity(activity))
                    Console.WriteLine("Activity deleted successfully.\n");
                else
                    Console.WriteLine("Error deleting activity.\n");
            }
            else if (choice == "C")
            {
                Console.WriteLine("\nAction cancelled. Returning to main menu.\n");

            }
            else
            {
                Console.WriteLine("Invalid choice. Returning to main menu.\n");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void ViewSummary()
        {
            if (_tracker == null) return;

            Console.Clear();

            // 1. Calculate Data
            int total = _tracker.TotalPoints();
            var breakdown = _tracker.PointsByCategory();

            // 2. Determine Badge
            string badge = "Seedling";
            if (total >= 100) badge = "Community Guardian";
            else if (total >= 50) badge = "Eco Hero";
            else if (total >= 20) badge = "Growing Green";

            // 3. Display The "Statement" Table
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║             POINTS SUMMARY (ALL TIME)         ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine("  ║                                               ║");

            // Breakdown Section
            Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "CATEGORY BREAKDOWN", "POINTS"));
            Console.WriteLine("  ║ --------------------------------------------- ║");

            foreach (var kv in breakdown)
            {
                Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", kv.Key, "+" + kv.Value));
            }

            // If no activities yet, show a blank line to keep shape
            if (!breakdown.Any())
            {
                Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "No activities yet", "0"));
            }

            Console.WriteLine("  ║                                               ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");

            // Totals Section
            Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "TOTAL ECO POINTS", total));
            Console.WriteLine(String.Format("  ║ {0,-30} {1,13} ║", "CURRENT BADGE", badge));

            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n  Press Enter to return...");
            Console.ReadLine();
        }
        private void DeleteAccountFlow()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║             CRITICAL WARNING                  ║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════╣");
            Console.WriteLine("  ║  This action is permanent and cannot be undone║");
            Console.WriteLine("  ║  All activities and login data will be lost.  ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");

            Console.Write($"\n  Type 'CONFIRM DELETE' to proceed with deletion: ");
            Console.ResetColor();

            string confirmation = Console.ReadLine() ?? "";

            if (confirmation.Equals("CONFIRM DELETE", StringComparison.Ordinal))
            {
                if (_fileService.DeleteUserAccount(_currentUser!)) //using ! because _currentUser is known to be set here
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  Account successfully deleted. Thank you for tracking your lifestyle.");
                    Console.ResetColor();
                    _isRunning = false; //Goes back to login menu
                    _currentUser = null;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  Error during deletion. Files might be locked or corrupt.");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("\n  Deletion cancelled. Returning to main menu.");
                Thread.Sleep(1500);
            }
        }
        private void Logout()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Goodbye, {_currentUser}!\n");
            Console.ResetColor();
            _currentUser = null;
            _tracker = null;
        }
    }
}
