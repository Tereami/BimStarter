#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion

using System;
using System.Runtime.InteropServices;
using System.Security;


namespace BatchPrintYay
{
    public static class WinApi
    {
        #region Constants

        public const int PRINTER_ACCESS_USE = 0x00000008; // позволить выполнять базовые  задачи
        public const int PRINTER_ACCESS_ADMINISTER = 0x00000004; // позволить выполнять задачи на уровне администратора, такие как SetPrinter


        #endregion Constants

        #region Structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct PrinterDefaults
        {
            [MarshalAs(UnmanagedType.LPTStr)] public String pDatatype;
            public IntPtr pDevMode;
            [MarshalAs(UnmanagedType.I4)] public int DesiredAccess;
        };
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        internal struct Size
        {
            public Int32 width;
            public Int32 height;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct Rect
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct FormInfo1
        {
            public UInt32 Flags;
            public String pName;
            public Size Size;
            public Rect ImageableArea;
        };

        #endregion Structures

        #region Functions
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinter", SetLastError = true,
                     CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall),
                SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPTStr)]
            string printerName,
                    out IntPtr phPrinter,
                    ref PrinterDefaults pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true,
             CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool ClosePrinter(IntPtr phPrinter);


        [DllImport("winspool.Drv", EntryPoint = "AddFormW", SetLastError = true,
          CharSet = CharSet.Unicode, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool AddForm(
      IntPtr phPrinter, [MarshalAs(UnmanagedType.I4)] int level, ref FormInfo1 form);


        [DllImport("winspool.Drv", EntryPoint = "DeleteForm", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall),
       SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool DeleteForm(IntPtr phPrinter, [MarshalAs(UnmanagedType.LPTStr)] string pName);


        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = false,
             ExactSpelling = true, CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern Int32 GetLastError();

        #endregion Functions
    }
}

