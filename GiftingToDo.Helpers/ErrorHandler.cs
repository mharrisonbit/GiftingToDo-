using System;
namespace GiftingToDo.Helpers
{
    public class ErrorHandler : IErrorHandler
    {
        /// <summary>
        /// This is going to print the exception message that is sent to it
        /// </summary>
        /// <param name="exception"></param>
        public void PrintErrorMessage(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
