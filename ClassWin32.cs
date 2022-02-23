using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LC_Service
{
    public class ClassWin32
    {
        private const int HWND_TOP = 0;
        //private const int HWND_BOTTOM = 1;
        private const int HWND_TOPMOST = -1;
        //private const int HWND_NOTOPMOST = -2;

        private class SWP
        {
            public static readonly int
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;
        }
        
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        // 도 = 256Hz
        // 레 = 도 * 9/8 = 288Hz
        // 미 = 레 * 10/9 = 320Hz
        // 파 = 미 * 16/15 = 341.3Hz
        // 솔 = 파 * 9/8 = 384Hz
        // 라 = 솔 * 10/9 = 426.6Hz
        // 시 = 라 * 9/8 = 480Hz
        // 도 = 시 * 16/15 = 512Hz (= 처음 도의 2배)
        // 2배 = 높은음, 1/2배 = 낮은음
        [DllImport("KERNEL32.DLL")]
        public static extern void Beep(int freq, int duration);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static bool CheckIsConnectedInternet()
        {
            return InternetGetConnectedState(out _, 0);
        }

        public static bool SetExplorerTopMost(IntPtr hWnd, int pX, int pY, int Width, int Height)
        {
            return SetWindowPos(hWnd, HWND_TOP, pX, pY, Width, Height, SWP.SHOWWINDOW);
        }

        public static bool SetPopupWindowTopMost(IntPtr hWnd, int pX, int pY, int Width, int Height)
        {
            return SetWindowPos(hWnd, HWND_TOPMOST, pX, pY, Width, Height, SWP.NOMOVE | SWP.NOSIZE | SWP.SHOWWINDOW);
        }
    }
}
