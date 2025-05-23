using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Linq;
using DesktopOrqApp.Messages;
using DesktopOrqApp.Services;
using DesktopOrqApp.Views;
using Refit;
using CommunityToolkit.Mvvm.Input;
using DesktopOrqApp.Models;


namespace DesktopOrqApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IApiService service,IMessenger messenger)
        {
            _service = service;
            messenger.Register<MainWindowViewModel, LoginSuccessMessage>(this, (_, message) =>
            {
                CompactSize = 0;
                IsPaneOpen = false;

                if (message.Value.UserRoleInfo.Equals("User"))
                {
                    CurrentPage = new UserPrivilegeViewModel(message.Value);
                }
                else
                {
                   
                    CurrentPage = new AdminViewModel(_service);
                }
            });

            Items = new ObservableCollection<ListItemTemplate>(_templates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(HomePageViewModel));
        }

        private readonly List<ListItemTemplate> _templates =
        [
         new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular", "Home"),
         new ListItemTemplate(typeof(LoginPageViewModel), "LockRegular", "Login Form")
        
        ];
        private readonly IApiService _service;
        public MainWindowViewModel() : this(RestService.For<IApiService>("https://localhost:50246"),new WeakReferenceMessenger()) { }

        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private int _compactSize = 60;

        [ObservableProperty]
        private ViewModelBase _currentPage = new HomePageViewModel();

        public ObservableCollection<ListItemTemplate> Items { get; }

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;



        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ViewModelBase vmb) return;

            CurrentPage = vmb;
        }


        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}
