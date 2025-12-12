using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FInalOOPproject
{

    public class EcoTracker
    {
        private readonly List<Activity> _activities = new List<Activity>();
        private readonly string _username;
        private readonly FileService _fileService;

        // Activities are now sorted by date in the ViewActivities method of ConsoleApp
        public IReadOnlyList<Activity> Activities => _activities.AsReadOnly();

        public EcoTracker(string username, FileService fileService)
        {
            _username = username;
            _fileService = fileService;
            _fileService.LoadActivities(_username, _activities);
        }

        //  CREATE 
        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
            _fileService.SaveActivities(_username, _activities);
        }

        // READ/FIND 
        public Activity? GetActivityByShortId(string shortId)
        {
            return _activities.FirstOrDefault(a =>
                a.Id.ToString().Substring(0, 8).Equals(shortId, StringComparison.OrdinalIgnoreCase));
        }

        // SEARCH METHOD
        /// <summary>
        /// Filters activities based on a keyword match in Category, Note, or specific subclass fields.
        /// </summary>
        /// <param name="keyword">The string to search for.</param>
        /// <returns>A list of matching activities.</returns>
        public List<Activity> SearchActivities(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Activity>();

            string search = keyword.Trim().ToLower();

            return _activities.Where(a =>
            {
                // 1. Check common fields
                if (a.Category.ToLower().Contains(search)) return true;
                if (a.Note.ToLower().Contains(search)) return true;

                // 2. Check subclass-specific fields
                if (a is RecyclingActivity r && r.Item.ToLower().Contains(search)) return true;
                if (a is EnergyActivity e && e.Action.ToLower().Contains(search)) return true;
                if (a is TransportActivity t && t.Mode.ToLower().Contains(search)) return true;

                return false;
            }).ToList();
        }


        //  UPDATE 
        public void UpdateActivityNote(Activity activity, string newNote)
        {
            activity.Note = newNote;
            _fileService.SaveActivities(_username, _activities);
        }

        //  DELETE 
        public bool DeleteActivity(Activity activity)
        {
            bool removed = _activities.Remove(activity);
            if (removed)
            {
                _fileService.SaveActivities(_username, _activities);
            }
            return removed;
        }

        public void ClearAll()
        {
            _activities.Clear();
            _fileService.SaveActivities(_username, _activities);
        }

        // CALCULATIONS
        public int TotalPoints() => _activities.Sum(a => a.Points);

        public IDictionary<string, int> PointsByCategory() =>
            _activities.GroupBy(a => a.Category)
                        .ToDictionary(g => g.Key, g => g.Sum(a => a.Points));

        // MONTHLY SUMMARY METHOD
        public IDictionary<string, int> MonthlySummary(int year, int month)
        {
            return _activities
                // Filter activities for the target year and month
                .Where(a => a.Date.Year == year && a.Date.Month == month)
                // Group the filtered activities by category
                .GroupBy(a => a.Category)
                // Create a dictionary mapping the category name to the sum of points for that month
                .ToDictionary(g => g.Key, g => g.Sum(a => a.Points));
        }
    }
}
