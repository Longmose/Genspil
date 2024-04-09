using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genspil
{
    public class GameManagement
    {
        // Metode til håndtering af tilføjelse af spil
        public void AddGames()
        {
            List<Game> games = new List<Game>();
            // Indtastning af Spil-info
            while (true)
            {
                
                Console.WriteLine("Indtast information for det nye spil, eller 'q' for at afslutte og gemme de indtastede spil i listen:");

                Console.Write("Titel: ");
                string title = Console.ReadLine();
                //Giver mulighed for at stoppe med at indtaste info og bryde ud af loopet
                if (title.ToLower() == "q")
                    break;

                Console.Write("Version: ");
                string version = Console.ReadLine();

                Console.Write("Kategori: ");
                string category = Console.ReadLine();

                int numberOfPlayersMin, numberOfPlayersMax, amount;
                double price;
                Stand condition;

                // Exception handling for ugyldige input
                try
                {
                    Console.Write("Antal spillere (min): ");
                    numberOfPlayersMin = int.Parse(Console.ReadLine());

                    Console.Write("Antal spillere (max): ");
                    numberOfPlayersMax = int.Parse(Console.ReadLine());

                    Console.WriteLine("Spillets tilstand:\n1: Perfekt\n2: God\n3: Middel\n4: Slidt\n5: Dårlig\n6: Elendig ");
                    condition = (Stand)int.Parse(Console.ReadLine());

                    Console.Write("Antal: ");
                    amount = int.Parse(Console.ReadLine());

                    Console.Write("Pris: ");
                    price = double.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ugyldigt input. Indtast venligst kun tal for antal spillere, antal og pris.");
                    continue; // Start loopet forfra
                }


                // Opret nyt Game objekt med brugerinput og gem det i listen
                Game newGame = new Game(title, version, category, numberOfPlayersMin, numberOfPlayersMax, condition, amount, price);
                games.Add(newGame);

                Console.Clear();

                Console.WriteLine($"{title} oprettet og gemt i listen.");
            }
            Console.Clear();

            //Gemmer i .txt fil
            DataHandler dataHandler = new DataHandler("spildata.txt");
            dataHandler.SaveGames(games);

            // Udskriv info om alle gemte spil
            Console.WriteLine("\nGemte spil:");
            foreach (var game in games)
            {
                Console.WriteLine(game.GetInfo());
                Console.WriteLine();
            }
        }

        // Metode til at fjerne spil
        public void RemoveGames()
        {
            // Loop for at tillade gentagne sletninger, indtil brugeren beslutter at stoppe
            bool keepDeleting = true;
            while (keepDeleting)
            {
                Console.Clear();

                // Opret en instans af DataHandler for at arbejde med spildata
                DataHandler dataHandler = new DataHandler("spildata.txt");
                // Viser en liste over alle spil
                dataHandler.ReadAndPrintGames();

                // Bed brugeren om at indtaste titlen på det spil, de ønsker at fjerne eller 'q' for at afslutte
                Console.WriteLine("Indtast titlen på spillet, du ønsker at slette (Tast 'q' for at stoppe):");
                string gameTitleToRemove = Console.ReadLine();

                // Håndterer afslutningsbetingelse
                if (gameTitleToRemove.ToLower() == "q")
                {
                    keepDeleting = false;
                    continue;
                }

                // Henter listen over spil og finder det specifikke spil baseret på titlen
                var games = dataHandler.ReadGames();
                var gameToRemove = games.FirstOrDefault(g => g.Title.Equals(gameTitleToRemove, StringComparison.OrdinalIgnoreCase));

                // Tjekker om spillet findes i listen
                if (gameToRemove != null)
                {
                    Console.Clear();
                    Console.WriteLine("Information om valgte spil:");
                    // Viser detaljeret information om det valgte spil
                    Console.WriteLine(gameToRemove.GetInfo());
                    Console.WriteLine("\nVil du fjerne spillet helt (tast 'f') eller reducere antallet (tast 'r')?");
                    char choice = Console.ReadKey().KeyChar;

                    // Behandler fjernelse af spil helt
                    if (choice == 'f')
                    {
                        games.Remove(gameToRemove);
                        dataHandler.OverwriteGames(games);
                        Console.WriteLine($"\nSpillet {gameTitleToRemove} er fjernet helt.");
                    }
                    // Behandler reduktion af antal spil
                    else if (choice == 'r')
                    {
                        Console.WriteLine("\nHvor mange enheder vil du fjerne?");
                        if (int.TryParse(Console.ReadLine(), out int quantityToRemove) && quantityToRemove > 0)
                        {
                            // Reducerer antallet af spil eller fjerner spillet helt, hvis det nye antal er 0 eller mindre
                            if (quantityToRemove >= gameToRemove.Amount)
                            {
                                games.Remove(gameToRemove);
                                Console.WriteLine($"Alle enheder af {gameTitleToRemove} er fjernet.");
                            }
                            else
                            {
                                gameToRemove.Amount -= quantityToRemove;
                                if (gameToRemove.Amount <= 0)
                                {
                                    games.Remove(gameToRemove);
                                    Console.WriteLine($"Spillet {gameTitleToRemove} er fjernet helt, da antallet blev reduceret til 0 eller derunder.");
                                }
                                else
                                {
                                    Console.WriteLine($"{quantityToRemove} enheder af {gameTitleToRemove} er fjernet. Resterende antal: {gameToRemove.Amount}");
                                }
                            }
                            dataHandler.OverwriteGames(games);
                        }
                        else
                        {
                            Console.WriteLine("Ugyldigt antal indtastet.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nUgyldigt valg.");
                    }
                }
                else
                {
                    // Informerer brugeren, hvis det indtastede spil ikke findes
                    Console.WriteLine($"Spillet med titlen \"{gameTitleToRemove}\" blev ikke fundet.");
                }

                // Giver brugeren en chance for at se resultatet og fortsætter eller afslutter loopen
                Console.WriteLine("\nTryk på en vilkårlig tast for at fortsætte");
                Console.ReadKey();
            }
        }


        // Metode til at redigere spil
        public void EditGames()
        {

        }

        // Metode til at vise og sortere lagerlisten
        public void ViewGames()
        {
            DataHandler datahandler = new DataHandler("spildata.txt");
            var games = datahandler.ReadGames();

            Console.WriteLine("Vælg sortering:");
            Console.WriteLine("1. Titel");
            Console.WriteLine("2. Version");
            Console.WriteLine("3. Kategori");
            Console.WriteLine("4. Antal spillere (Min)");
            Console.WriteLine("5. Antal spillere (Max)");
            Console.WriteLine("6. Stand");
            Console.WriteLine("7. Antal");
            Console.WriteLine("8. Pris");

            int choice = int.Parse(Console.ReadLine()); // Husk at tilføje fejlhåndtering her

            switch (choice)
            {
                case 1:
                    games = games.OrderBy(g => g.Title).ToList();
                    break;
                case 2:
                    games = games.OrderBy(g => g.Version).ToList();
                    break;
                case 3:
                    games = games.OrderBy(g => g.Category).ToList();
                    break;
                case 4:
                    games = games.OrderBy(g => g.NumberOfPlayersMin).ToList();
                    break;
                case 5:
                    games = games.OrderBy(g => g.NumberOfPlayersMax).ToList();
                    break;
                case 6:
                    games = games.OrderBy(g => g.Condition).ToList();
                    break;
                case 7:
                    games = games.OrderBy(g => g.Amount).ToList();
                    break;
                case 8:
                    games = games.OrderBy(g => g.Price).ToList();
                    break;
            }

            Console.Clear();

            foreach (var game in games)
            {
                Console.WriteLine(game.GetInfo());
                Console.WriteLine();
            }
        }
    }
}
