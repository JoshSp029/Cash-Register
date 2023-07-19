using System;
using System.Collections.Generic;

namespace ChangeCalculatorApp
{
    public class ChangeCalculator
    {
        // Dictionary to store the denominations for each country
        private Dictionary<string, List<double>> currencyDenominations;

        public ChangeCalculator(string country)
        {
            // Initialize the currency denominations based on the provided country
            currencyDenominations = new Dictionary<string, List<double>>
            {
                { "US", new List<double> { 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 } },
                { "Mexico", new List<double> { 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 } },
                // Add more countries and their denominations here if needed
            };

            // Set the currency based on the provided country
            if (!currencyDenominations.ContainsKey(country))
            {
                throw new ArgumentException("Invalid country specified. Country not supported.");
            }
            CurrentCountryCurrency = country;
        }

        public string CurrentCountryCurrency { get; private set; }

        public Dictionary<double, int> CalculateChange(double price, List<double> payment)
        {
            // Check if the payment provided is sufficient to cover the price
            double totalPayment = 0;
            foreach (var coin in payment)
            {
                totalPayment += coin;
            }

            if (totalPayment < price)
            {
                throw new ArgumentException("Insufficient payment. The total payment should be greater than or equal to the price.");
            }

            // Calculate the change amount
            double changeAmount = totalPayment - price;
            Dictionary<double, int> changeDictionary = new Dictionary<double, int>();

            // Loop through the denominations in reverse order (largest to smallest) and calculate the number of each denomination needed
            foreach (var denomination in currencyDenominations[CurrentCountryCurrency])
            {
                int numDenomination = (int)(changeAmount / denomination);
                if (numDenomination > 0)
                {
                    changeDictionary.Add(denomination, numDenomination);
                    changeAmount -= numDenomination * denomination;
                }
            }

            return changeDictionary;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Change Calculator App!");
            Console.WriteLine("------------------------------------");

            // Example usage
            ChangeCalculator changeCalculator = new ChangeCalculator("US");

            double price = 10.50;
            List<double> payment = new List<double> { 5.00, 1.00 };

            try
            {
                Dictionary<double, int> change = changeCalculator.CalculateChange(price, payment);

                Console.WriteLine("\nChange breakdown:");
                foreach (var denomination in change)
                {
                    Console.WriteLine($"Denomination: {denomination.Key} - Count: {denomination.Value}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nThank you for using the Change Calculator App!");
        }
    }
}
