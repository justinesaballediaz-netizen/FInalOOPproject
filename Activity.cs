using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FInalOOPproject
{
    // Abstraction base class
    public abstract class Activity
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public abstract string Category { get; }
        public abstract int Points { get; }

        protected Activity(DateTime date, string note = "")
        {
            Date = date;
            Note = note;
        }

        public override string ToString()
        {
            return $"[{Id.ToString().Substring(0, 8)}] {Date:yyyy-MM-dd} | {Category,-12} | +{Points,2} pts | {Note}";
        }
    }

    public class RecyclingActivity : Activity
    {
        public string Item { get; set; }

        public RecyclingActivity(DateTime date, string item, string note = "") : base(date, note)
        {
            Item = item;
        }

        public override string Category => "Recycling";

        public override int Points
        {
            get
            {
                if (Item.ToLower().Contains("electronics")) return 12;
                if (Item.ToLower().Contains("glass")) return 8;
                if (Item.ToLower().Contains("plastic")) return 6;
                if (Item.ToLower().Contains("paper")) return 5;
                if (Item.ToLower().Contains("can")) return 6;
                return 4;
            }
        }

        public override string ToString() => base.ToString() + $" (Item: {Item})";
    }

    public class EnergyActivity : Activity
    {
        public string Action { get; set; }

        public EnergyActivity(DateTime date, string action, string note = "") : base(date, note)
        {
            Action = action;
        }

        public override string Category => "Energy";

        public override int Points
        {
            get
            {
                if (Action.ToLower().Contains("unplug")) return 5;
                if (Action.ToLower().Contains("led")) return 8;
                if (Action.ToLower().Contains("shower")) return 4;
                if (Action.ToLower().Contains("solar")) return 12;
                return 3;
            }
        }

        public override string ToString() => base.ToString() + $" (Action: {Action})";
    }

    public class TransportActivity : Activity
    {
        public string Mode { get; set; }
        public double DistanceKm { get; set; }

        public TransportActivity(DateTime date, string mode, double distanceKm = 0, string note = "") : base(date, note)
        {
            Mode = mode;
            DistanceKm = Math.Round(distanceKm, 2);
        }

        public override string Category => "Transport";

        public override int Points
        {
            get
            {
                if (Mode.ToLower().Contains("walk")) return (int)Math.Min(20, 5 + DistanceKm / 4);
                if (Mode.ToLower().Contains("bike")) return (int)Math.Min(20, 6 + DistanceKm / 5);
                if (Mode.ToLower().Contains("public")) return (int)Math.Min(15, 4 + DistanceKm / 10);
                if (Mode.ToLower().Contains("carpool")) return 6;
                if (Mode.ToLower().Contains("electric")) return 7;
                return 3;
            }
        }

        public override string ToString() => base.ToString() + $" (Mode: {Mode}, {DistanceKm} km)";
    }
}
