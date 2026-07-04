namespace BookBuddy.API.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) //this base means -> "Call the constructor of the parent i.e (Exception) class and give it this message."
        {

        }



    }
}
