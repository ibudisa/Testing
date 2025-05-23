using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DesktopOrqApp.Models;
using DesktopOrqApp.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopOrqApp.ViewModels
{
    public partial class AdminViewModel : ViewModelBase
    {
        private readonly IApiService _service;
        
        public AdminViewModel(IApiService service)
        {
            _service = service;
            CurrentPage=new AdminPrivilegeViewModel(_service);
            Items = new ObservableCollection<ListItemTemplate>(_datatemplates);

            SelectedListItem = Items.First(vm => vm.ModelType == typeof(AdminPrivilegeViewModel));

        }
        public AdminViewModel() : this(RestService.For<IApiService>("https://localhost:50246"))
        { }

        private readonly List<ListItemTemplate> _datatemplates =
        [
         new ListItemTemplate(typeof(AdminPrivilegeViewModel), "Admin1Regular", "CRUD WorkPositions"),
         new ListItemTemplate(typeof(AdminPrivilegeExtViewModel), "Admin2Regular", "Add Users to workpositions")

        ];

        [ObservableProperty]
        private bool _isPaneOpen=true;


        [ObservableProperty]
        private ViewModelBase _currentPage;

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


    }
}
