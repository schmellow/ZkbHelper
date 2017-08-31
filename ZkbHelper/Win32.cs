using System;
using System.Runtime.InteropServices;

namespace ZkbHelper
{
    public static class Win32
    {
        /// <summary> 
        /// The WM_DRAWCLIPBOARD message notifies a clipboard viewer window that  
        /// the content of the clipboard has changed.  
        /// </summary> 
        public const int WM_DRAWCLIPBOARD = 0x0308;

        /// <summary> 
        /// A clipboard viewer window receives the WM_CHANGECBCHAIN message when  
        /// another window is removing itself from the clipboard viewer chain. 
        /// </summary> 
        public const int WM_CHANGECBCHAIN = 0x030D;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetClipboardOwner();
    }
}
