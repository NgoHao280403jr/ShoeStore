using QLBanGiay_Application;
using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Services;
using QLBanGiay_Application.View;
using QLBanGiay.Models.Models;
namespace YourWinFormsProject;

static class Program
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

        var dbContext = new QlShopBanGiayContext(); 
        var userRepository = new UserRepository(dbContext);
        var userService = new UserService(userRepository);

        Application.Run(new frm_Login(userService));
    }    
}