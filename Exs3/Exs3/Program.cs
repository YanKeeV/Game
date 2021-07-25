using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Exs3
{
    class Program
    {
        static void Main(string[] args)
        {  
            if(args.Count() < 3)
            {
                Console.WriteLine("Invalid number of arguments, minimum three \nExample: Exs3 rock paper scissors");return;
            }
            if (args.Count()%2 == 0)
            {
                Console.WriteLine("Enter an odd number of arguments"); return;
            }

            byte[] key = new byte[16];
            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(key);
            string encoded = BitConverter.ToString(key).Replace("-", string.Empty);

            Random rnd = new Random();
            var compMove = rnd.Next(1, args.Count()+1).ToString();
            int compMoveNum = Convert.ToInt32(compMove) - 1;
            var answer = args[compMoveNum];

            var hmac = new HMACSHA256(key);
            var answerBytes = Encoding.UTF8.GetBytes(answer);
            var hash = hmac.ComputeHash(answerBytes);

            string hashString = BitConverter.ToString(hash).Replace("-", string.Empty);

            Console.WriteLine("HMAC: " + hashString + "\nAvailable moves:");
            bool flag = true;
            String userMove = "";
            while (flag)
            {
                for (int i = 0; i < args.Count(); i++)
                {
                    Console.WriteLine(i + 1 + " - " + args[i]);
                }
                Console.WriteLine("0 - exit");
                Console.WriteLine("Enter your move: ");
                userMove = Console.ReadLine();
                for(int i = 0; i <= args.Count(); i++)
                {
                    if(userMove == i.ToString())
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if(userMove == "0")
            {
                Console.WriteLine("Program ended");
                return;
            }
            int userMoveNum = Convert.ToInt32(userMove) - 1;
            Console.WriteLine("Your move:" + args[userMoveNum]);
            Console.WriteLine("Computer move:" + args[compMoveNum]);
            int moveNum = args.Count() / 2;

            int[] winMoves = new int[moveNum];
            int wtf = userMoveNum;
            for(int i = 0; i < moveNum; i++)
            {
                wtf++;
                if (wtf >= args.Count())
                {
                    wtf = 0;
                }
                winMoves[i] = wtf;
            }
            if(userMoveNum - compMoveNum == 0)
            {
                Console.WriteLine("Draw!");
                Console.WriteLine("HMAC key: " + encoded);
                return;
            }
            if(winMoves.Contains(compMoveNum))
            {
                Console.WriteLine("You win!");
                Console.WriteLine("HMAC key: " + encoded);
                return;
            }
            else
            {
                Console.WriteLine("Computer win!");
                Console.WriteLine("HMAC key: " + encoded);
                return;
            }
        }
    }
}
