using System;

namespace Wittel_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            string endFlag;
            int whileFlag1 = 0;
            int whileFlag2 = 0;
            BotController bot = new BotController();

            bot.Connect();

            // While loop to keep applicaiton alive for stream. Can only be killed thorugh command window. Or by killing the command window.
            while(whileFlag1 != 1)
            {
                whileFlag2 = 0;
                endFlag = Console.ReadLine();
                if (endFlag.Equals("x") || endFlag.Equals("X"))
                {
                    while (whileFlag2 != 1)
                    {
                        Console.WriteLine("Are you sure you want to kill Wittel_Bot? (y/n)");
                        endFlag = Console.ReadLine();
                        if (endFlag.Equals("y") || endFlag.Equals("Y"))
                        {
                            whileFlag1 = 1;
                            whileFlag2 = 1;
                        }
                        else if (endFlag.Equals("n") || endFlag.Equals("N"))
                        {
                            Console.WriteLine("Well watch what you're doing then! Please enter 'x' when you're ready to stop the program.");
                            whileFlag2 = 1;
                        }
                        else
                        {
                            Console.WriteLine("Not a valid response. This is a life we're talking about here! Try again.");
                        }
                    }
                    
                }
            }
            
            bot.Disconnect();
        }
    }
}

/*
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Wittel_Bot;

namespace Sicae_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> Exit = new List<string> { "x", "exit" };
            List<string> Yes = new List<string> { "y", "yes" };
            List<string> No = new List<string> { "n", "no" };

            Console.WriteLine("Welcome to Sicae-Bot!");
            Console.Write("Initialization in progress");

            BotController Bot = new BotController();

            Dots();

            Console.WriteLine("Complete!");

            Bot.Connect();

            Thread.Sleep(3000);
            Console.WriteLine("Beginning main loop!");
            Thread.Sleep(2000);

            bool Loop = true;

            while (Loop)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                Console.WriteLine("You are at the Main Menu:");
                Console.WriteLine();
                Console.Write("Type '");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Exit");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("' to terminate the program.");
                Console.WriteLine("\n");
                Console.Write(">>>");


                Console.ForegroundColor = ConsoleColor.Red;
                var Read = Console.ReadLine().Trim().ToLower();

                if (Exit.Contains(Read))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Are you sure you want to EXIT?");
                    Console.WriteLine("\n");
                    Console.Write(">>>");
                    Console.ForegroundColor = ConsoleColor.Red;

                    if (Yes.Contains(Console.ReadLine().Trim().ToLower()))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Very well...Shutting Down");
                        Loop = false;
                        Dots();
                    }
                }
                else
                {
                    switch (Read)
                    {
                        default:
                            Console.Clear();
                            Console.Write(Read);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" is not a valid selection! Returning to Main Menu!");
                            Dots();
                            break;
                    }
                }
            }

            Bot.Disconnect();
        }

        public static void Dots()
        {
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
        }
    }
}
*/