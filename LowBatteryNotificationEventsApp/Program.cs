using System;

namespace LowBatteryNotificationEventsApp
{
    internal class Program
    {
        static void Main()
        {
            Battery battery = new Battery(60);
            BatteryMonitor monitor = new BatteryMonitor();

            battery.BatteryLow += monitor.OnBatteryLow;
            battery.BatteryCritical += monitor.OnBatteryCritical;

            Console.WriteLine("Simulating battery drain...");
            while (battery.Level > 0)
            {
                Console.WriteLine("Press any key to drain 10% battery...");
                Console.ReadKey();
                battery.DrainBattery(10);
            }

            Console.WriteLine("Battery is empty. Device shutting down.");

        }
    }
    public class Battery
    {
        public event EventHandler BatteryLow;
        public event EventHandler BatteryCritical;

        private int level;

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                if (level <= 10)
                {
                    OnBatteryCritical();
                }
                else if (level <= 30)
                {
                    OnBatteryLow();
                }
            }
        }
        public Battery(int lvl)
        {
            level = lvl;   
        }
        public void DrainBattery(int amount)
        {
            Level = Math.Max(0, level - amount); // Obniż poziom baterii, ale nie poniżej 0
            Console.WriteLine($"Battery level: {level}%");
        }
        protected virtual void OnBatteryLow()
        {
            BatteryLow?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnBatteryCritical()
        {
            BatteryCritical?.Invoke(this, EventArgs.Empty);
        }
    }
    public class BatteryMonitor
    {
        public void OnBatteryLow(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: Battery level is low! Please charge soon.");
            Console.ResetColor();
        }
        public void OnBatteryCritical(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Critical Warning: Battery level is critically low! Shutting down soon.");
            Console.ResetColor();
        }
    }
}
