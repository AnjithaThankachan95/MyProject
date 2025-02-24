using CustomerWinFormsApp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    static void Main()
    {
        // Build the configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base path to the application's directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load appsettings.json
            .Build();

        // Initialize WinForms application
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Pass the IConfiguration to the Form1 constructor
        Application.Run(new Form1(configuration));  // Passing the configuration here
    }
}
