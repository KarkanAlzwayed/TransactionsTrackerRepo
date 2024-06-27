using Microsoft.Maui;
using TransactionsTracker; // Replace with your namespace
using SQLite;

namespace TransactionsTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CreateTableIfNotExists()
        {
            // Connect to local SQLite database
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "transactions.db");
            // Create the table if it doesn't exist
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Transactions (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Category TEXT NOT NULL,
                                        Amount REAL NOT NULL
                                    );";

                conn.Execute(createTableQuery);
            }
        }

        private void OnSubmitTransactionClicked(object sender, EventArgs e)
        {
            string category = CategoryEntry.Text;

            try
            {
                double amount = double.Parse(AmountEntry.Text);

                // Connect to local SQLite database (replace path if needed)
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "transactions.db");

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {
                        // Insert new transaction data
                        Transaction newTransaction = new Transaction { Category = category, Amount = amount };
                        conn.Insert(newTransaction);
                    }

                    // Display success message (optional)
                    DisplayAlert("Success", "Transaction added!", "OK");
                }
                catch (Exception ex)
                {
                    // Handle exception related to database connection
                    Console.WriteLine("Error connecting to database: " + ex.Message);
                    DisplayAlert("Error", "Failed to add transaction. Please check the logs for details.", "OK");
                }
            }
            catch (FormatException)
            {
                // Handle the exception (e.g., display an error message to the user)
                DisplayAlert("Error", "Invalid amount entered. Please enter a numeric value.", "OK");
            }
        }
    }

    // Class representing a transaction (replace with your desired properties)
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
    }
}
