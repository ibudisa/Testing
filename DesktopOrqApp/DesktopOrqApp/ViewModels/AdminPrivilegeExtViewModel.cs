using CommunityToolkit.Mvvm.ComponentModel;
using DAL.Entities;
using DesktopOrqApp.Helper;
using DesktopOrqApp.Models;
using DesktopOrqApp.Services;
using ReactiveUI;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DesktopOrqApp.ViewModels
{
    public partial class AdminPrivilegeExtViewModel:ViewModelBase
    {
        private readonly IApiService _service;
        private List<User_WorkPosition> _userworkPositions = new List<User_WorkPosition>();
        public AdminPrivilegeExtViewModel(IApiService service)
        {
            _service = service;
            _userworkPositions = new ResponseHandle(_service).GetUserWP();
            UserWpositions = new ObservableCollection<User_WorkPosition>(_userworkPositions);

            AddUserWPositionCommand = ReactiveCommand.Create(AddUserWPosition);
        }

        public AdminPrivilegeExtViewModel() : this(RestService.For<IApiService>("https://localhost:50246"))
        { }
        public ObservableCollection<User_WorkPosition> UserWpositions { get; set; }

        [ObservableProperty]
        private string _newUWPUserId;

        [ObservableProperty]
        private string _newUWPPositionId;

        [ObservableProperty]
        private string _errorMessage = "";
        public ReactiveCommand<Unit, Unit> AddUserWPositionCommand { get; }

        public void AddUserWPosition()
        {
            try
            {
                var newUWP = new User_WorkPosition
                {
                    UserId = int.Parse(NewUWPUserId),
                    WorkPositionId = int.Parse(NewUWPPositionId) 

                };

                ResponseHandle responseHandle = new ResponseHandle(_service);

                bool added = responseHandle.AddUserWP(newUWP);
                if (added)
                {
                    int maxid = responseHandle.GetUWPMaxId();
                    newUWP.Id = maxid;
                    UserWpositions.Add(newUWP);

                    // Clear input fields after adding
                    NewUWPUserId = string.Empty;
                    NewUWPPositionId = String.Empty;
                }
                else
                {
                    ShowErrorMessage("Ids for User or WorkPosition are not found");
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding workposition: {ex.Message}");
            }
        }
		private void ShowErrorMessage(string message)
		{
			ErrorMessage = message;
		
		}
	}
}
