using DesktopOrqApp.ViewModels;
using DesktopOrqApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopOrqApp.HelperDI
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<HomePageViewModel>();
            services.AddSingleton<LoginPageViewModel>();
            services.AddSingleton<UserPrivilegeViewModel>();
            services.AddSingleton<AdminPrivilegeViewModel>();
            services.AddSingleton<AdminPrivilegeExtViewModel>();
            services.AddSingleton<AdminViewModel>();

            services.AddSingleton<MainWindow>();
            services.AddTransient<HomePageView>();
            services.AddTransient<LoginPageView>();    
            services.AddTransient<UserPrivilegeView>();
            services.AddTransient<AdminPrivilegeView>();
            services.AddTransient<AdminPrivilegeExtView>();
            services.AddTransient<AdminView>();

        }
    }
}
