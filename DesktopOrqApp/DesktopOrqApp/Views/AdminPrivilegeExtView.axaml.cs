using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopOrqApp.ViewModels;

namespace DesktopOrqApp;

public partial class AdminPrivilegeExtView : UserControl
{
    AdminPrivilegeExtViewModel _viewModel = new();
    public AdminPrivilegeExtView()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }
}