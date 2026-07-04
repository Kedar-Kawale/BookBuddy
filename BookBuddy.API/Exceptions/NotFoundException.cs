namespace BookBuddy.API.Exceptions
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string message) : base(message)  //this base means -> "Call the constructor of the parent i.e (Exception) class and give it this message."

        {

        }

    }
}
