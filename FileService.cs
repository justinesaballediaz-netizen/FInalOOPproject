using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FInalOOPproject
{
    public class FileService
    {
        private const string UserFile = "users.txt";
        private const string DataFolder = "data";

        public FileService()
        {
            // Ensure data directory exists upon service creation
            Directory.CreateDirectory(DataFolder);
        }

        public bool Register(string username, string password)
        {
            if (File.Exists(UserFile))
            {
                var users = File.ReadAllLines(UserFile);
                if (users.Any(u => u.StartsWith(username + ":")))
                    return false;
            }

            File.AppendAllText(UserFile, $"{username}:{password}\n");
            File.Create($"{DataFolder}/{username}_activities.txt").Close();
            return true;
        }

        public bool Login(string username, string password)
        {
            if (!File.Exists(UserFile)) return false;
            return File.ReadAllLines(UserFile)
                       .Any(u => u == $"{username}:{password}");
        }

        public void SaveActivities(string username, IEnumerable<Activity> activities)
        {
            string file = $"{DataFolder}/{username}_activities.txt";
            using var writer = new StreamWriter(file);

            // IMPORTANT: Order by date when saving to ensure chronological integrity if file is viewed directly
            foreach (var a in activities.OrderBy(a => a.Date))
                writer.WriteLine($"{a.Id},{a.Date},{a.Category},{a.Note},{GetExtraData(a)}");
        }

        // Helper to save subclass specific data flatly
        private string GetExtraData(Activity a)
        {
            if (a is RecyclingActivity r) return r.Item;
            if (a is EnergyActivity e) return e.Action;
            if (a is TransportActivity t) return $"{t.Mode}|{t.DistanceKm}";
            return "";
        }

        public void LoadActivities(string username, List<Activity> activities)
        {
            string file = $"{DataFolder}/{username}_activities.txt";
            if (!File.Exists(file)) return;

            foreach (var line in File.ReadAllLines(file))
            {
                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    // The Date stored in the file is a full DateTime string, e.g., "12/4/2025 12:00:00 AM"
                    DateTime.TryParse(parts[1], out DateTime date);
                    string cat = parts[2];
                    string note = parts[3];
                    string extra = parts[4];

                    if (cat == "Recycling") activities.Add(new RecyclingActivity(date, extra, note));
                    else if (cat == "Energy") activities.Add(new EnergyActivity(date, extra, note));
                    else if (cat == "Transport")
                    {
                        var tParts = extra.Split('|');
                        if (tParts.Length == 2)
                        {
                            double.TryParse(tParts[1], out double dist);
                            activities.Add(new TransportActivity(date, tParts[0], dist, note));
                        }
                    }
                }
            }
        }

        public bool DeleteUserAccount(string username)
        {
            // 1. Delete the activity data file
            string dataFilePath = $"{DataFolder}/{username}_activities.txt";
            if (File.Exists(dataFilePath))
            {
                File.Delete(dataFilePath);
            }

            // 2. Remove the user from the login file
            if (!File.Exists(UserFile)) return false;

            try
            {
                // Read all lines, filter out the line that starts with the current username, and rewrite the file with the remaining lines.
                var users = File.ReadAllLines(UserFile)
                                 .Where(u => !u.StartsWith(username + ":"))
                                 .ToList();

                File.WriteAllLines(UserFile, users);
                return true;
            }
            catch (Exception)
            {
                // Handle potential file access errors gracefully
                return false;
            }
        }
    }
}
