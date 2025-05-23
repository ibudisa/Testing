using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopOrqApp.ViewModels;

namespace DesktopOrqApp;

public partial class AdminView : UserControl
{
    AdminViewModel _viewModel = new();
    public AdminView()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }
}