using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DesktopOrqApp.Services;
using DesktopOrqApp.ViewModels;
using DesktopOrqApp.Views;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using DesktopOrqApp.HelperDI;



namespace DesktopOrqApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var locator = new ViewLocator();
            DataTemplates.Add(locator);

            var services = new ServiceCollection();
            DependencyInjection.AddApplication(services);
           

            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            services.AddRefitClient<IApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:50246"));

            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetRequiredService<MainWindowViewModel>();

            //var vm = new MainWindowViewModel();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow(vm);
               
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }

        //[Singleton(typeof(MainWindowViewModel))]
        //[Transient(typeof(HomePageViewModel))]
        //[Singleton(typeof(LoginPageViewModel))]
        //[Singleton(typeof(UserPrivilegeViewModel))]
        //[Singleton(typeof(AdminPrivilegeViewModel))]
        //[SuppressMessage("CommunityToolkit.Extensions.DependencyInjection.SourceGenerators.InvalidServiceRegistrationAnalyzer", "TKEXDI0004:Duplicate service type registration")]
        //internal static partial void ConfigureViewModels(IServiceCollection services);

        //[Singleton(typeof(MainWindow))]
        //[Transient(typeof(HomePageView))]
        //[Transient(typeof(LoginPageView))]
        //[Transient(typeof(UserPrivilegeView))]
        //[Transient(typeof(AdminPrivilegeView))]
        //internal static partial void ConfigureViews(IServiceCollection services);
    }
}