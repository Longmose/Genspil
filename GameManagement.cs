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
            bool keepDeleting = true;
            while (keepDeleting)
            {
                Console.Clear();

                DataHandler dataHandler = new DataHandler("spildata.txt");
                // Viser en liste over alle spil
                dataHandler.ReadAndPrintGames();

                // Bed brugeren om at indtaste titlen på det spil, de ønsker at fjerne
                Console.WriteLine("Indtast titlen på spillet, du ønsker at slette (Tast 'q' for at stoppe):");
                string gameTitleToRemove = Console.ReadLine();

                if (gameTitleToRemove.ToLower() == "q")
                {
                    keepDeleting = false;
                    continue;
                }

                var games = dataHandler.ReadGames();
                // Find alle spil, der matcher den givne titel
                var matchingGames = games.Where(g => g.Title.Equals(gameTitleToRemove, StringComparison.OrdinalIgnoreCase)).ToList();

                if (matchingGames.Count > 0)
                {
                    if (matchingGames.Count == 1)
                    {
                        // Håndter fjernelse, hvis der kun er ét matchende spil
                        HandleGameRemoval(matchingGames.First(), dataHandler, games);
                    }
                    else
                    {
                        // Hvis der er flere spil med samme titel, vis mulighederne
                        Console.Clear();
                        Console.WriteLine("Flere spil fundet med denne titel. Vælg et spil:");
                        for (int i = 0; i < matchingGames.Count; i++)
                        {
                            Console.WriteLine($"\n{i + 1}: {matchingGames[i].GetInfo()}");
                        }

                        // Bed brugeren vælge et specifikt spil
                        Console.WriteLine("\nIndtast nummeret på det spil, du vil slette:");
                        if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex >= 1 && gameIndex <= matchingGames.Count)
                        {
                            HandleGameRemoval(matchingGames[gameIndex - 1], dataHandler, games);
                        }
                        else
                        {
                            Console.WriteLine("Ugyldigt nummer indtastet.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Spillet med titlen \"{gameTitleToRemove}\" blev ikke fundet.");
                }

                Console.WriteLine("\nTryk på en vilkårlig tast for at fortsætte");
                Console.ReadKey();
            }
        }

        // Hjælpemetode til at håndtere fjernelse eller reduktion af et spil
        private void HandleGameRemoval(Game gameToRemove, DataHandler dataHandler, List<Game> allGames)
        {
            Console.Clear();
            Console.WriteLine("Information om valgte spil:");
            Console.WriteLine(gameToRemove.GetInfo());

            // Spørger brugeren, om de vil fjerne spillet helt eller reducere antallet
            Console.WriteLine("\nVil du fjerne spillet helt (tast 'f') eller reducere antallet (tast 'r')?");
            char choice = Console.ReadKey().KeyChar;

            if (choice == 'f')
            {
                allGames.Remove(gameToRemove);
                dataHandler.OverwriteGames(allGames);
                Console.WriteLine($"\nSpillet {gameToRemove.Title} er fjernet helt.");
            }
            else if (choice == 'r')
            {
                Console.WriteLine("\nHvor mange enheder vil du fjerne?");
                if (int.TryParse(Console.ReadLine(), out int quantityToRemove) && quantityToRemove > 0)
                {
                    if (quantityToRemove >= gameToRemove.Amount)
                    {
                        allGames.Remove(gameToRemove);
                        Console.WriteLine($"Alle enheder af {gameToRemove.Title} er fjernet.");
                    }
                    else
                    {
                        gameToRemove.Amount -= quantityToRemove;
                        if (gameToRemove.Amount <= 0)
                        {
                            allGames.Remove(gameToRemove);
                            Console.WriteLine($"Spillet {gameToRemove.Title} er fjernet helt, da antallet blev reduceret til 0 eller derunder.");
                        }
                        else
                        {
                            Console.WriteLine($"{quantityToRemove} enheder af {gameToRemove.Title} er fjernet. Resterende antal: {gameToRemove.Amount}");
                        }
                        dataHandler.OverwriteGames(allGames);
                    }
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


        // Metode til at redigere spil
        public void EditGames()
        {
            DataHandler dataHandler = new DataHandler("spildata.txt");
            var games = dataHandler.ReadGames(); // Indlæser alle spil fra datafilen

            // Viser en liste over alle spil
            dataHandler.ReadAndPrintGames();

            // Bed brugeren om at indtaste titlen på det spil, de ønsker at redigere
            Console.WriteLine("Indtast titlen på det spil, du ønsker at redigere:");
            string titleToEdit = Console.ReadLine();

            // Find alle spil, der matcher den indtastede titel
            var matchingGames = games.Where(g => g.Title.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase)).ToList();

            Game gameToEdit = null;

            // Håndterer scenarier, hvor der er flere spil med samme titel
            if (matchingGames.Count > 1)
            {
                Console.Clear();
                Console.WriteLine("Flere spil fundet med denne titel. Vælg et spil:");
                for (int i = 0; i < matchingGames.Count; i++)
                {
                    Console.WriteLine($"\n{i + 1}: {matchingGames[i].GetInfo()}");
                }
                Console.WriteLine("\nIndtast nummeret på det spil, du vil redigere:");
                if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex >= 1 && gameIndex <= matchingGames.Count)
                {
                    gameToEdit = matchingGames[gameIndex - 1];
                }
            }
            else if (matchingGames.Count == 1)
            {
                // Hvis der kun findes et spil med den indtastede titel
                gameToEdit = matchingGames.First();
            }

            // Hvis et spil er valgt, tillad redigering af dettes detaljer
            if (gameToEdit != null)
            {
                EditGameDetails(gameToEdit);
                dataHandler.OverwriteGames(games); // Gem de opdaterede spil til datafilen
            }
            else
            {
                Console.WriteLine("Spil ikke fundet eller ugyldigt valg.");
            }
        }

        // Hjælpemetode til at redigere detaljer for et specifikt spil
        private void EditGameDetails(Game game)
        {
            Console.Clear();
            Console.WriteLine("Redigerer spil:");
            Console.WriteLine(game.GetInfo());

            // Tillad brugeren at indtaste ny titel, men kun opdater hvis input ikke er tomt
            Console.Write("\nNy titel (lad være tom for at beholde nuværende): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle)) game.Title = newTitle;

            // Tillad brugeren at indtaste ny version, men kun opdater hvis input ikke er tomt
            Console.Write("Ny Version (lad være tom for at beholde nuværende): ");
            var newVersion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newVersion)) game.Version = newVersion;

            // Tillad brugeren at indtaste ny kategori, men kun opdater hvis input ikke er tomt
            Console.Write("Ny Kategori (lad være tom for at beholde nuværende): ");
            var newCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCategory)) game.Category = newCategory;

            string userInput; // Variabel til at holde brugerinput

            // Rediger minimum antal spillere
            Console.Write("Nyt minimum antal spillere (lad være tom for at beholde nuværende): ");
            userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int newMinPlayers) && newMinPlayers > 0)
            {
                game.NumberOfPlayersMin = newMinPlayers;
            }
            else if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Ugyldigt input. Beholder nuværende værdi.");
            }

            // Rediger maksimum antal spillere
            Console.Write("Nyt maksimum antal spillere (lad være tom for at beholde nuværende): ");
            userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int newMaxPlayers) && newMaxPlayers > 0)
            {
                game.NumberOfPlayersMax = newMaxPlayers;
            }
            else if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Ugyldigt input. Beholder nuværende værdi.");
            }

            // Rediger stand
            Console.WriteLine("Ny stand:\n1: Perfekt\n2: God\n3: Middel\n4: Slidt\n5: Dårlig\n6: Elendig ");
            userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int newCondition) && Enum.IsDefined(typeof(Stand), newCondition))
            {
                game.Condition = (Stand)newCondition;
            }
            else if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Ugyldigt input. Beholder nuværende stand.");
            }

            // Rediger antal
            Console.Write("Nyt antal (lad være tom for at beholde nuværende): ");
            userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int newAmount) && newAmount >= 0)
            {
                game.Amount = newAmount;
            }
            else if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Ugyldigt input. Beholder nuværende antal.");
            }

            // Rediger pris
            Console.Write("Ny pris (lad være tom for at beholde nuværende): ");
            userInput = Console.ReadLine();
            if (double.TryParse(userInput, out double newPrice) && newPrice >= 0)
            {
                game.Price = newPrice;
            }
            else if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Ugyldigt input. Beholder nuværende pris.");
            }

            Console.Clear();
            Console.WriteLine("Spil opdateret:");
            Console.WriteLine(game.GetInfo());
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
