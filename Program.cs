﻿using System.Runtime.CompilerServices;

namespace Genspil
{
    public enum Stand
    {
        Perfekt = 1,
        God,
        Middel,
        Slidt,
        Dårlig,
        Elendig
    }
        


    internal class Program
    {
        static void Main(string[] args)
        {
            // Opretter en instans af GameManagement til at håndtere spiloperationer
            GameManagement gameManagement = new GameManagement();
            bool programRunning = true;

            // Løkke til at håndtere brugrinput og navigere programmet
            while (programRunning == true)
            {
                // Brugeren vælger hvad de vil i programmet
                Console.WriteLine("Vælg hvad du ønsker at gøre: ");
                Console.WriteLine("1. Tilføj et eller flere spil til lagerlisten");
                Console.WriteLine("2. Slet spil fra lagerlisten");
                Console.WriteLine("3. Rediger eksisterende spil på lagerlisten");
                Console.WriteLine("4. Se Lagerliste");
                Console.WriteLine("0. Luk programmet");

                
                if (int.TryParse(Console.ReadLine(), out int userDirection))
                {

                    switch (userDirection)
                    {
                        // Tilføj Spil
                        case 1:
                            Console.Clear();
                            gameManagement.AddGames();
                            break;

                        // Slet spil
                        case 2:
                            Console.Clear();
                            gameManagement.RemoveGames();
                            break;

                        // Rediger Spil
                        case 3:
                            Console.Clear();
                            gameManagement.EditGames();
                            break;

                        // Se og Sorter Spil
                        case 4:
                            Console.Clear();
                            gameManagement.ViewGames();
                            break;

                        // Stop og luk programmet
                        case 0:
                            Console.Clear();
                            Console.WriteLine("Lukker Programmet. Tak for nu! :)");
                            programRunning = false;
                            break;

                        default:
                            Console.WriteLine("Invalid Input. Try again");
                            break;
                    }
                    if (programRunning == true)
                    {
                        Console.WriteLine("\nTryk på en vilkårlig tast for at fortsætte");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ugyldigt input, indtast venligst kun tal");
                }
            }
            Console.ReadLine();
        }

        
    }
}
