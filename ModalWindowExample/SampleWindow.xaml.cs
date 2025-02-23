using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModalWindowExample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModalWindow : Window
    {
        private AppWindow appWindow;

        public ModalWindow()
        {
            this.InitializeComponent();

            appWindow = GetAppWindowForCurrentWindow();

            var presenter = OverlappedPresenter.CreateForDialog();

            SetOwner(appWindow);
            presenter.IsModal = true;
            appWindow.SetPresenter(presenter);
            appWindow.Show();

            Closed += ModalWindow_Closed;
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }

        private void SetOwner(AppWindow childAppWindow)
        {
            // Get HWND of the main window
            // The main window can be retrieved from App.xaml.cs if it's set as a static property.
            IntPtr parentHwnd = WindowNative.GetWindowHandle(App.m_window);

            // Get HWND of the AppWindow
            IntPtr childHwnd = Win32Interop.GetWindowFromWindowId(childAppWindow.Id);

            // Attempt to set the owner window (parent), falling back to 32-bit if needed.
            try
            {
                SetWindowLongPtr64(childHwnd, -8, parentHwnd); // -8 = GWLP_HWNDPARENT
            }
            catch
            {
                SetWindowLongPtr32(childHwnd, -8, parentHwnd); // -8 = GWLP_HWNDPARENT
            }
        }

        // Import the 32-bit version of SetWindowLong for modifying window properties.
        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // Import the 64-bit version of SetWindowLongPtr for modifying window properties.
        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private void ModalWindow_Closed(object sender, WindowEventArgs args)
        {
            App.m_window.Activate();
        }
    }
}
