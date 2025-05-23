using DesktopOrqApp.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using DesktopOrqApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopOrqApp.Helper;
using DAL.Entities;
using Refit;
using System.Linq.Expressions;


namespace DesktopOrqApp.ViewModels
{
    public partial class AdminPrivilegeViewModel:ViewModelBase
    {
        private readonly IApiService _service;
        private List<WorkPositionModel> _workPositionsmodel=new List<WorkPositionModel>();
        private List<WorkPosition> _workPositions = new List<WorkPosition>();

        public AdminPrivilegeViewModel(IApiService service)
        {
            _service = service;
            _workPositions = new ResponseHandle(_service).GetWP();
            _workPositionsmodel=MapperData.MapToAll(_workPositions);
            Wpositions = new ObservableCollection<WorkPositionModel>(_workPositionsmodel);
           

            AddWPositionCommand = ReactiveCommand.Create(AddWPosition);
            DeleteSelectedWPositionCommand = ReactiveCommand.Create(DeleteSelectedWPosition);
            UpdateWPositionCommand= ReactiveCommand.Create(UpdateWPosition);
        }

        public AdminPrivilegeViewModel():this(RestService.For<IApiService>("https://localhost:50246"))
        { }
        public ObservableCollection<WorkPositionModel> Wpositions { get; set; }

        [ObservableProperty]
        private string _newWPositionName;

        [ObservableProperty]
        private string _newWPositionDescription;

        [ObservableProperty]
        private bool _isOpenError;

        [ObservableProperty]
        private string _errorMessage;
     

        public ReactiveCommand<Unit, Unit> AddWPositionCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedWPositionCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateWPositionCommand { get; }


        public void AddWPosition()
        {
            try
            {
                var newWP = new WorkPositionModel
                {
                    Name = NewWPositionName ?? "New position",
                    Description = NewWPositionDescription ?? "New description"
                   
                };
              

                WorkPosition pos=MapperData.MapFrom(newWP);

                ResponseHandle responseHandle = new ResponseHandle(_service);

                bool added=responseHandle.AddWP(pos);

                int maxid = responseHandle.GetWPMaxId();

                newWP.Id = maxid;

                Wpositions.Add(newWP);

                // Clear input fields after adding
                NewWPositionName = string.Empty;
                NewWPositionDescription = String.Empty;
              

                IsOpenError = false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error adding workposition: {ex.Message}");
            }
        }

        public void DeleteSelectedWPosition()
        {
            try
            {
                var wpositionToRemove = Wpositions.FirstOrDefault(p => p.IsSelected);
                if (wpositionToRemove is null)
                {
                    ShowErrorMessage("Please select a work position.");
                    return;
                }

                Wpositions.Remove(wpositionToRemove);
                int id = wpositionToRemove.Id;
                ResponseHandle responseHandle = new ResponseHandle(_service);

                bool deleted = responseHandle.DeleteWP(id);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error deleting workposition: {ex.Message}");
            }
	   }

        public void UpdateWPosition()
        {
            try
            {
                var positionToUpdate = Wpositions.FirstOrDefault(p => p.IsSelected);
                if (positionToUpdate is null)
                {
                    ShowErrorMessage("Please select a work position.");
                    return;
                }
                WorkPosition pos = MapperData.MapFrom(positionToUpdate);

                ResponseHandle responseHandle = new ResponseHandle(_service);

                bool updated = responseHandle.UpdateWP(pos);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error updating workposition: {ex.Message}");
            }
		}

        private void ShowErrorMessage(string message)
        {
            ErrorMessage = message;
            IsOpenError = true;
        }
    }
}
