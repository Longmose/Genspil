using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Genspil
{
    public class Game
    {
        private string title;
        private string version;
        private string category;
        private int numberOfPlayersMin;
        private int numberOfPlayersMax;
        private string condition;
        private int amount;
        private double price;


        public string Title { get { return title; } set {  title = value; } }
        public string Version { get { return version; } set { version = value; } }
        public string Category { get { return category; } set { category = value; } }
        public int NumberOfPlayersMin { get {  return numberOfPlayersMin; } set {  numberOfPlayersMin = value; } }
        public int NumberOfPlayersMax { get {  return numberOfPlayersMax; } set { numberOfPlayersMax = value; } }
        public string Condition { get { return condition; } set { condition = value; } }
        public int Amount { get { return amount; } set { amount = value; } }
        public double Price { get { return price; } set { price = value; } }


        public Game( string title, string version, string category, int numberOfPlayersMin, int numberOfPlayersMax, string condition, int amount, double price )
        {
            this.title = title;
            this.version = version;
            this.category = category;
            this.numberOfPlayersMin = numberOfPlayersMin;
            this.numberOfPlayersMax = numberOfPlayersMax;
            this.condition = condition;
            this.amount = amount;
            this.price = price;
        }
        public string GetInfo()
        {
            return $"Titel: {Title}\nVersion: {Version}\nKategori: {Category}\nAntal Spillere: {NumberOfPlayersMin}-{NumberOfPlayersMax}\nStand: {Condition}\nAntal: {Amount}\nPris: {Price} kr";
        }


    }
}
