namespace DeliveryTimeApi
{
    public static class Extensions
    {
        public static (int hours, int minutes) ParseHoursAndMinutes(string rawString)
        {
            var array = rawString.Split(":");

            return (int.Parse(array[0]), int.Parse(array[1]));
        }
    }
}
