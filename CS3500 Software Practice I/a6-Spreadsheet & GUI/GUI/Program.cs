using SpreadsheetGrid_Core;

namespace GUI
{
    /// <summary>
    /// Author:    Jim St. Germain & University of Utah CS Department
    /// </summary>
    class SpreadsheetWindow : ApplicationContext
    {
        /// <summary>
        ///  Number of open forms
        /// </summary>
        private int formCount = 0;

        /// <summary>
        ///  Singleton ApplicationContext
        /// </summary>
        private static SpreadsheetWindow appContext;

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetWindow getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetWindow();
            }
            return appContext;
        }

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetWindow()
        {
        }

        /// <summary>
        /// Build another GUI Window
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // Assign an EVENT handler to take an action when the GUI is closed 
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

        /// <summary>
        /// Simple method for getting the form count so I can display the window number that is open
        /// </summary>
        /// <returns></returns>
        public int getFormCount()
        {
            return formCount+1;
        }

    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //Working on checking windows being open
            SpreadsheetWindow appContext = SpreadsheetWindow.getAppContext();
            appContext.RunForm(new SpreadsheetGUI());
             Application.Run(appContext);

            // multiple windows open

            
        }
    }
}