using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSelectCommond
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            HandleRunningInstance(_temp);
         
            SendKeys.Send("A");
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        public static class MouseHook

        {

            public static event EventHandler MouseAction = delegate { };

            public static void Start()
            {
                _hookID = SetHook(_proc);

            }

            public static void stop()
            {
                UnhookWindowsHookEx(_hookID);
            }
            private static LowLevelMouseProc _proc = HookCallback;
            private static IntPtr _hookID = IntPtr.Zero;

            private static IntPtr SetHook(LowLevelMouseProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc,
                      GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

            private static IntPtr HookCallback(
              int nCode, IntPtr wParam, IntPtr lParam)
            {

                //按下左鍵
                if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    MouseAction(null, new EventArgs());
                    //x,y座標
                    Console.WriteLine((MouseMessages)wParam + "," + hookStruct.pt.x.ToString() + "," + hookStruct.pt.y.ToString());
                }

                if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    MouseAction(null, new EventArgs());
                    Console.WriteLine((MouseMessages)wParam + "," + hookStruct.pt.x.ToString() + "," + hookStruct.pt.y.ToString());
                }

                



                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            private const int WH_MOUSE_LL = 14;

            private enum MouseMessages
            {
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_MOUSEMOVE = 0x0200,
                WM_MOUSEWHEEL = 0x020A,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int x;
                public int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct MSLLHOOKSTRUCT
            {
                public POINT pt;
                public uint mouseData;
                public uint flags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook,
              LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
              IntPtr wParam, IntPtr lParam);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
            
        }
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        private const uint GA_ROOT = 2;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MouseEnter += new System.EventHandler(this.MouseEnters);


        }
        Process _temp;
        private void MouseEnters(object sender, EventArgs e)
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {

                    if (activatedHandle == clsProcess.MainWindowHandle)
                    {
                        string processName = clsProcess.ProcessName;
                        Console.WriteLine(processName);
                        #region 移動最後一個Foucs的視窗
                        //IntPtr mainWindow = clsProcess.MainWindowHandle;
                        //IntPtr newPos = new IntPtr(-1);
                        //SetWindowPos(mainWindow, new IntPtr(0), 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);
                        _temp = clsProcess;
                        #endregion



                        //Point pt = Cursor.Position;
                        //IntPtr wnd = WindowFromPoint(pt.X, pt.Y);
                        //IntPtr mainWnd = GetAncestor(wnd, GA_ROOT);
                        //POINT PT;
                        //PT.X = pt.X;
                        //PT.Y = pt.Y;
                        ////ScreenToClient(mainWnd, ref PT);
                        //Console.WriteLine(String.Format("({0}, {1})", PT.X.ToString(), PT.Y.ToString()));
                    }
                }
                
            }
            catch { }
            // Update the mouse event label to indicate the MouseLeave event occurred.
            Console.WriteLine( sender.GetType().ToString() + ": MouseEnter");
        }
        private const int WS_SHOWNORMAL = 1;
        public static void HandleRunningInstance(Process instance)
        {
            // 相同時透過ShowWindowAsync還原，以及SetForegroundWindow將程式至於前景
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            SetForegroundWindow(instance.MainWindowHandle);
        }

        private void Btnpaste_Click(object sender, EventArgs e)
        {
            HandleRunningInstance(_temp);

            SendKeys.Send("A");
        }
        string FolderPath;
        string FilePath;
        private void BtnNew_Click(object sender, EventArgs e)
        {
            string FolderPath = System.IO.Directory.GetCurrentDirectory();
            FilePath = labtxtPath.Text;
            //如果檔案存在，重新寫入
            if (File.Exists(FilePath))
            {

            }
            else
            {
            //如果檔案不存在，新增檔案
            }
            using (FileStream fs = File.Create(FilePath))
            {
                
            }
        }
    }
}
