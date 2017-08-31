using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZkbHelper.Logging;

// clipboard code courtesy of
// https://blogs.msdn.microsoft.com/codefx/2012/03/07/sample-of-mar-7th-monitor-windows-clipboard-changes-in-wpf/

namespace ZkbHelper.Views
{
    public partial class MainWindow : Window
    {
        private const string LOG_TARGET_SCREEN = "screen";
        private IntPtr _nextViewer;
        private HwndSource _hwndSource;
        private bool _isLaunch = true;

        public MainWindow()
        {
            InitializeComponent();
            if (!Configuration.LogToScreen)
                SizeToContent = SizeToContent.Height;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Configuration.LogToScreen)
            {
                Logger.Instance.SetTarget(LOG_TARGET_SCREEN, new TextBoxLoggingTarget(tbLog));
            }
            else
            {
                rowLog.Height = new GridLength(0);
                MinHeight = ActualHeight - rowSearch.ActualHeight;
                MaxHeight = ActualHeight - rowSearch.ActualHeight;
            }
            toggleListener.IsChecked = Configuration.EnableListenerOnStartup;

            WindowInteropHelper wih = new WindowInteropHelper(this);
            _hwndSource = HwndSource.FromHwnd(wih.Handle);
            _hwndSource.AddHook(WinProc);
            _nextViewer = Win32.SetClipboardViewer(_hwndSource.Handle);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.Instance.ClearTarget(LOG_TARGET_SCREEN);
            Win32.ChangeClipboardChain(_hwndSource.Handle, _nextViewer);
            _nextViewer = IntPtr.Zero;
            _hwndSource.RemoveHook(WinProc);
        }

        private void tbSearchQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Task.Run(() => SearchCharacter());
            else if (e.Key == Key.Escape)
                tbSearchQuery.Text = "";
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == _nextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it. 
                        _nextViewer = lParam;
                    }
                    else if (_nextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer. 
                        Win32.SendMessage(_nextViewer, msg, wParam, lParam);
                    }
                    break;
                case Win32.WM_DRAWCLIPBOARD:
                    if(_isLaunch) // ignore first event on app startup
                    {
                        _isLaunch = false;
                        break;
                    }
                    if (toggleListener.IsChecked != true)
                        break;
                    // eve does not own the clipboard
                    // hence, if we can't get the owner, event probably did originate from eve
                    // (although other cases are possible, like adobe acrobat reader)
                    var ownerHwnd = Win32.GetClipboardOwner();
                    if (ownerHwnd == IntPtr.Zero)
                    {
                        // some clipped texts have \n instead of \r\n
                        var data = Clipboard.GetText()
                            .Replace("\r", "")
                            .Replace("\n", ",");
                        tbSearchQuery.Text = data;
                        Task.Run(() => SearchCharacter());
                    }
                    // pass the message to the next viewer. 
                    Win32.SendMessage(_nextViewer, msg, wParam, lParam);
                    break;
            }
            return IntPtr.Zero;
        }

        // called from clipboard hook, or when enter is pressed in character name field
        private void SearchCharacter()
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                try
                {
                    if (string.IsNullOrEmpty(tbSearchQuery.Text))
                        return;
                    var characterNames = tbSearchQuery.Text.Split(',');
                    foreach (var name in characterNames)
                    {
                        var id = ESI.GetCharacterIdString(name);
                        if (string.IsNullOrEmpty(id))
                            continue;
                        var url = string.Format("https://zkillboard.com/character/{0}/", id);
                        Logger.Instance.Write("Url: " + url);
                        Process.Start(url);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.Write(ex.ToString());
                }
            }));
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearchQuery.Text = "";
            tbSearchQuery.Focus();
        }
    }
}
