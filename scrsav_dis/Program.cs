using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;

class Program
{
    [DllImport("kernel32.dll")]
    static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
        // Legacy flag, should not be used.
        // ES_USER_PRESENT   = 0x00000004,
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
    }

    [STAThread]
    static void Main(string[] args)
    {
        if (SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
        | EXECUTION_STATE.ES_DISPLAY_REQUIRED
        | EXECUTION_STATE.ES_SYSTEM_REQUIRED
        | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0) //Away mode for Windows >= Vista
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
        Application.Run(new Context());
    }
}

class Context : ApplicationContext
{
    NotifyIcon ico;
    ContextMenu menu;
    MenuItem item;

    public Context()
    {
        item = new MenuItem();
        item.Text = "Exit";
        item.Click += item_Click;
        menu = new ContextMenu();
        menu.MenuItems.Add(0, item);
        ico = new NotifyIcon();
        ico.Text = "Screen Saver Disabler";
        ico.Icon = scrsav_dis.Properties.Resources.Icon;
        ico.ContextMenu = menu;
        ico.Visible = true;
    }

    void item_Click(object sender, EventArgs e)
    {
        ico.Dispose();
        Application.Exit();
    }
}