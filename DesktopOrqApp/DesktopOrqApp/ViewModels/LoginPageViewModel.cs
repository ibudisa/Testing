using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using DesktopOrqApp.Services;
using DesktopOrqApp.Messages;
using Refit;
using Microsoft.AspNetCore.Mvc;
using DAL;
using System.Text.Json;

namespace DesktopOrqApp.ViewModels
{
    public partial class LoginPageViewModel:ViewModelBase
    {

        [ObservableProperty] private string _errorMessage = "";
        [ObservableProperty] private string _username = "";
        [ObservableProperty] private string _password = "";
       

        private readonly IApiService _loginService;
        private readonly IMessenger _messenger;

        public LoginPageViewModel(IApiService loginService, IMessenger messenger)
        {
            _loginService = loginService;
            _messenger = messenger;
          
        }

        // design only
        public LoginPageViewModel() : this(RestService.For<IApiService>("https://localhost:51873"), new WeakReferenceMessenger()) { }

        [RelayCommand]
        private async void Login()
        {
            ApiResponse<DataPosition> data = await _loginService.GetUserRoleData(Username, Password);
            if (!data.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid username or password";
                return;

            }

            var content = data.Content;
             ErrorMessage = "";
             _messenger.Send(new LoginSuccessMessage(content));
            
        
          
        }

    }
}
