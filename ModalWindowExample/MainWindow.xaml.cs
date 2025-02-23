using Microsoft.UI.Xaml;

namespace ModalWindowExample;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        ModalWindow window = new ModalWindow();
        window.Activate();
    }
}
