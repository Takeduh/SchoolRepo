using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Transaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // Income or Expense
    public string Category { get; set; }
    public DateTime Date { get; set; }

    public Transaction(string description, decimal amount, string type, string category, DateTime date)
    {
        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
    }
}

public class BudgetTracker
{
    // Private list to store transactions
    private List<Transaction> transactions = new List<Transaction>();
    private const string filePath = "transactions.txt";

    // Public property to access transactions
    public List<Transaction> Transactions
    {
        get { return transactions; }
    }

    // Add a new transaction
    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
        SaveTransactionsToFile();
    }

    // Delete a transaction by description
    public bool DeleteTransaction(string description)
    {
        var transactionToDelete = transactions.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
        if (transactionToDelete != null)
        {
            transactions.Remove(transactionToDelete);
            SaveTransactionsToFile();
            return true;
        }
        return false;
    }

    // Calculate total income
    public decimal GetTotalIncome()
    {
        return transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
    }

    // Calculate total expenses
    public decimal GetTotalExpenses()
    {
        return transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
    }

    // Calculate net savings
    public decimal GetNetSavings()
    {
        return GetTotalIncome() - GetTotalExpenses();
    }

    // Get analytics by category
    public void GetCategoryAnalytics()
    {
        var categorySummary = transactions
            .Where(t => t.Type == "Expense")
            .GroupBy(t => t.Category)
            .Select(group => new
            {
                Category = group.Key,
                TotalSpent = group.Sum(t => t.Amount)
            });

        foreach (var category in categorySummary)
        {
            Console.WriteLine($"{category.Category}: ${category.TotalSpent}");
        }
    }

    // Sort transactions by date
    public void SortTransactionsByDate()
    {
        transactions = transactions.OrderBy(t => t.Date).ToList();
    }

    // Sort transactions by amount
    public void SortTransactionsByAmount()
    {
        transactions = transactions.OrderBy(t => t.Amount).ToList();
    }

    // Show a simple text-based bar chart of spending
    public void ShowSpendingGraph()
    {
        var categorySummary = transactions
            .Where(t => t.Type == "Expense")
            .GroupBy(t => t.Category)
            .Select(group => new
            {
                Category = group.Key,
                TotalSpent = group.Sum(t => t.Amount)
            });

        foreach (var category in categorySummary)
        {
            Console.WriteLine($"{category.Category}: " + new string('*', (int)(category.TotalSpent / 10)));
        }
    }

    // Load transactions from file
    public void LoadTransactionsFromFile()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length == 5)
                {
                    string description = parts[0];
                    decimal amount = decimal.Parse(parts[1]);
                    string type = parts[2];
                    string category = parts[3];
                    DateTime date = DateTime.Parse(parts[4]);

                    transactions.Add(new Transaction(description, amount, type, category, date));
                }
            }
        }
    }

    // Save transactions to file
    private void SaveTransactionsToFile()
    {
        try
        {
            var lines = transactions.Select(t => $"{t.Description},{t.Amount},{t.Type},{t.Category},{t.Date.ToString("yyyy-MM-dd")}");
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving transactions: " + ex.Message);
        }
    }
}

public class Program
{
    static void Main()
    {
        BudgetTracker budgetTracker = new BudgetTracker();
        budgetTracker.LoadTransactionsFromFile();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Personal Budget Tracker");
            Console.WriteLine("1. Add Transaction");
            Console.WriteLine("2. Delete Transaction");
            Console.WriteLine("3. View Analytics");
            Console.WriteLine("4. View Spending Graph");
            Console.WriteLine("5. View Sorted Transactions");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTransaction(budgetTracker);
                    break;
                case "2":
                    DeleteTransaction(budgetTracker);
                    break;
                case "3":
                    ViewAnalytics(budgetTracker);
                    break;
                case "4":
                    budgetTracker.ShowSpendingGraph();
                    break;
                case "5":
                    SortTransactions(budgetTracker);
                    break;
                case "6":
                    Console.WriteLine("Exiting. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddTransaction(BudgetTracker budgetTracker)
    {
        try
        {
            Console.Clear();
            Console.Write("Enter transaction description: ");
            string description = Console.ReadLine();

            Console.Write("Enter amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Enter transaction type (Income/Expense): ");
            string type = Console.ReadLine().Trim();

            if (type != "Income" && type != "Expense")
            {
                Console.WriteLine("Invalid transaction type.");
                return;
            }

            Console.Write("Enter category (e.g., Food, Transportation): ");
            string category = Console.ReadLine();

            Console.Write("Enter date (YYYY-MM-DD): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Transaction transaction = new Transaction(description, amount, type, category, date);
            budgetTracker.AddTransaction(transaction);
            Console.WriteLine("Transaction added successfully!\n");

            // Wait for the user to press a key before returning to the main menu
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void DeleteTransaction(BudgetTracker budgetTracker)
    {
        Console.Clear();
        Console.Write("Enter the description of the transaction to delete: ");
        string description = Console.ReadLine();

        bool deleted = budgetTracker.DeleteTransaction(description);

        if (deleted)
        {
            Console.WriteLine("Transaction deleted successfully!\n");
        }
        else
        {
            Console.WriteLine("Transaction not found.\n");
        }

        // Wait for the user to press a key before returning to the main menu
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    static void ViewAnalytics(BudgetTracker budgetTracker)
    {
        Console.Clear();
        Console.WriteLine("Total Income: $" + budgetTracker.GetTotalIncome());
        Console.WriteLine("Total Expenses: $" + budgetTracker.GetTotalExpenses());
        Console.WriteLine("Net Savings: $" + budgetTracker.GetNetSavings());
        Console.WriteLine("\nCategory Analytics:");
        budgetTracker.GetCategoryAnalytics();

        // Wait for the user to press a key before returning to the main menu
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    static void SortTransactions(BudgetTracker budgetTracker)
    {
        Console.Clear();
        Console.WriteLine("Sort Transactions by:");
        Console.WriteLine("1. Date");
        Console.WriteLine("2. Amount");
        string sortChoice = Console.ReadLine();

        if (sortChoice == "1")
        {
            budgetTracker.SortTransactionsByDate();
            Console.WriteLine("Transactions sorted by date.");
        }
        else if (sortChoice == "2")
        {
            budgetTracker.SortTransactionsByAmount();
            Console.WriteLine("Transactions sorted by amount.");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

        Console.WriteLine("\nTransactions:");
        foreach (var transaction in budgetTracker.Transactions)
        {
            Console.WriteLine($"{transaction.Date.ToShortDateString()} | {transaction.Description} | {transaction.Amount:C} | {transaction.Category}");
        }

        // Wait for the user to press a key before returning to the main menu
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }
}
