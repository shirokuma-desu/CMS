using System.Globalization;
using System.Text;

namespace Client.Helper
{
    public class TimeHelper
    {
        public static int ToUnixTimeStamp(DateTime datetime)
        {
            return (int)(datetime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public static string GetTimeLeft(string deadline)
        {
            DateTime parsedDeadline = DateTime.Parse(deadline);
            TimeSpan timeLeft = parsedDeadline - DateTime.Now;

            // Check if the deadline has already passed
            if(timeLeft.TotalSeconds < 0)
            {
                return "Deadline has passed";
            }

            // Format the time left as a string
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} days, ", timeLeft.Days);
            sb.AppendFormat("{0} hours, ", timeLeft.Hours);
            sb.AppendFormat("{0} minutes, ", timeLeft.Minutes);
            sb.AppendFormat("{0} seconds", timeLeft.Seconds);

            return sb.ToString();
        }

        public static string ConvertToDateFormat(string input)
        {
            DateTime parsedDate = DateTime.ParseExact(input, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("dd/MM/yyyy");
            return formattedDate;
        }
        public static DateTime ConvertStringToDateTime(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
