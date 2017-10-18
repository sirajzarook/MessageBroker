using Dropship.Ofs.Processor.Helpers;
using System;

namespace Dropship.Ofs.Processor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayIntroductionMessages();

            ApplicationMode applicationMode;
            if (!ArgsProcessor.TryGetApplicationMode(args, out applicationMode, out string errorMessage))
            {
                Console.WriteLine(errorMessage);
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Starting in mode:  {applicationMode.ToName()}  ");
                Console.WriteLine(String.Empty);

                //TODO: Remove all of TEST mode. Only for debug
                if (applicationMode == ApplicationMode.TEST)
                {
                    var application = new Startup(applicationMode);
                    application.ProduceFakeTestMessage();
                }
                else
                {
                    var application = new Startup(applicationMode);
                    application.Start();
                }
                Console.ReadLine();
            }
        }

        private static void DisplayIntroductionMessages()
        {
            Console.WriteLine("---------------------------------------- ");
            Console.WriteLine("Welcome in Dropship Ofs Processor Vendor Plugin ");
            Console.WriteLine("________________________________________ ");
            Console.WriteLine(String.Empty);
            Console.WriteLine("Please select the application mode. On application startup args");
            Console.WriteLine("Available mode params: ");
            Console.WriteLine($"- {ApplicationMode.CHECK_IN.ToName()} ");
            Console.WriteLine($"- {ApplicationMode.SEND_ORDER.ToName()} ");
            Console.WriteLine($"- {ApplicationMode.UPDATE_ADDRESS.ToName()} ");
            Console.WriteLine("________________________________________ ");
            Console.WriteLine(String.Empty);
        }


    }
}
