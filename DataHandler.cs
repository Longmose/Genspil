using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Genspil
{
    public class DataHandler
    {
        private string filePath;

        public DataHandler(string filePath)
        {
            this.filePath = filePath;
        } //"G:\Visual Studio Projects\Genspil\bin\Debug\net8.0\spildata.txt" for Eske

        public void SaveGames(List<Game> games, bool append = true)
        {
            try
            {
                // Åbn filen i enten Append eller Create tilstand afhængigt af append-parameteren
                using (StreamWriter writer = new StreamWriter(filePath, append))
                {
                    foreach (var game in games)
                    {
                        writer.WriteLine(game.GetInfo());
                        writer.WriteLine(); // Skriv en tom linje mellem hvert spil
                    }
                }
                Console.WriteLine("Spil data er gemt i filen.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved gemning af spil data: {ex.Message}");
            }
        }

        public void OverwriteGames(List<Game> games)
        {
            try
            {
                // Åbn filen i tilstand Create for at overskrive den eksisterende data
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (var game in games)
                    {
                        writer.WriteLine(game.GetInfo());
                        writer.WriteLine(); // Skriv en tom linje mellem hvert spil
                    }
                }
                Console.WriteLine("Spil data er gemt i filen.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved gemning af spil data: {ex.Message}");
            }
        }

        public List<Game> ReadGames()
        {
            List<Game> games = new List<Game>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            string title = ExtractValue(line, "Titel:").Trim();
                            string version = ExtractValue(reader.ReadLine(), "Version:").Trim();
                            string category = ExtractValue(reader.ReadLine(), "Kategori:").Trim();
                            string playersRange = ExtractValue(reader.ReadLine(), "Antal Spillere:").Trim();
                            int numberOfPlayersMin = GetMinPlayers(playersRange);
                            int numberOfPlayersMax = GetMaxPlayers(playersRange);
                            string conditionStr = ExtractValue(reader.ReadLine(), "Stand:").Trim();
                            Stand condition = (Stand)Enum.Parse(typeof(Stand), conditionStr);
                            int amount = int.Parse(ExtractValue(reader.ReadLine(), "Antal:"));
                            double price = double.Parse(ExtractValue(reader.ReadLine(), "Pris:").TrimEnd('k', 'r'));

                            Game game = new Game(title, version, category, numberOfPlayersMin, numberOfPlayersMax, condition, amount, price);
                            games.Add(game);

                            // Læs den tomme linje, der adskiller hvert spil
                            reader.ReadLine(); 
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Fejl ved behandling af linjen: {ex.Message}");
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af spil data: {ex.Message}");
            }

            return games;
        }
        private string ExtractValue(string line, string key)
        {
            int startIndex = line.IndexOf(key);
            if (startIndex == -1)
            {
                throw new FormatException($"'{key}' blev ikke fundet i linjen: {line}");
            }
            return line.Substring(startIndex + key.Length);
        }

        private int GetMinPlayers(string playersRange)
        {
            string[] range = playersRange.Split('-');
            return int.Parse(range[0]);
        }

        private int GetMaxPlayers(string playersRange)
        {
            string[] range = playersRange.Split('-');
            return int.Parse(range[1]);
        }

        // Skriv læste spil til konsollen
        public void ReadAndPrintGames()
        {
            List<Game> games = ReadGames();

            if (games.Count == 0)
            {
                Console.WriteLine("Der er ingen spil i filen.");
                return;
            }

            Console.WriteLine("Gemte spil i filen:\n");

            foreach (var game in games)
            {
                string info = game.GetInfo();
                // Fjern '\t' fra oplysningerne, før de bliver udskrevet
                info = info.Replace("\t", "");
                Console.WriteLine(info);
                Console.WriteLine(); // Skriv en tom linje mellem hvert spil
            }
        }

        // Slet spil metode
        public void RemoveGame(string gameTitleToRemove, int quantity = 0)
        {
            List<Game> games = ReadGames();
            var gameToRemove = games.FirstOrDefault(g => g.Title.Equals(gameTitleToRemove, StringComparison.OrdinalIgnoreCase));

            if (gameToRemove != null)
            {
                if (quantity == 0 || gameToRemove.Amount <= quantity)
                {
                    games.Remove(gameToRemove);
                }
                else
                {
                    gameToRemove.Amount -= quantity;
                }

                OverwriteGames(games);
            }
            else
            {
                Console.WriteLine($"Spillet med titlen \"{gameTitleToRemove}\" blev ikke fundet i filen.");
            }
        }
    }
}

