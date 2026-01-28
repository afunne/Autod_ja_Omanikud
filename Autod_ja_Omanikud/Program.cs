namespace Autod_ja_Omanikud
{
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
        Start:
            using (var login = new Forms.LoginForm())
            {
                var result = login.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                if (login.LaunchMinesweeper)
                {
                    using var mines = new Forms.MinesweeperForm();
                    Application.Run(mines);
                    if (mines.BackToLoginRequested)
                    {
                        goto Start; // return to login after a win if requested
                    }
                }
                else if (login.AuthenticatedUser != null)
                {
                    Application.Run(new Form1(login.AuthenticatedUser));
                }
            }
        }
    }
}