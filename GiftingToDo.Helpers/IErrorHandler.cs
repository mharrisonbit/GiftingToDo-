using System;

namespace GiftingToDo.Helpers
{
    public interface IErrorHandler
    {
        void PrintErrorMessage(Exception exception);
    }
}