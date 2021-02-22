using System;

namespace CommonClassLib
{
    public interface IDisplayMessage
    { 
        void DisplyText(string value);
    }
    public class DisplayMessage : IDisplayMessage
    {
        public void DisplyText(string value)
        {
            Console.WriteLine(value);
        }
    }
}
