using System;

using Microsoft.Owin.Hosting;

namespace PartyServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Listening on {baseAddress}");
                Console.ReadLine();
            }
        }
    }
}