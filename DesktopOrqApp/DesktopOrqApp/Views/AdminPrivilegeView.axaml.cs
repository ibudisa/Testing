using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopOrqApp.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace DesktopOrqApp;

public partial class AdminPrivilegeView : UserControl
{
    AdminPrivilegeViewModel _viewModel = new();
    public AdminPrivilegeView()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }
}