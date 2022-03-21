namespace TennisPlanner.Server.Models
{
    public class HourRangeSelector
    {
        private int min=8;
        private int max=22;

        public int Min {
            get => min;
            set {
                min = value != 24 ? value : 23;
                if (min >= max)
                {
                    max = min + 1;
                }
            }
        }

        public int Max { 
            get => max;
            set 
            { 
                max = value != 0 ? value : 1;
                if(min >= max)
                {
                    min = max - 1;
                }
            } 
        }
    }
}
