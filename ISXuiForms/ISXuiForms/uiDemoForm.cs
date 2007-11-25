using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ISXWoW;
using LavishVMAPI;
using InnerSpaceAPI;
using ISXWoWUI;

namespace ISXuiDemoForm
{
    public partial class uiDemoForm : Form
    {
        public uiDemoForm()
        {
            InitializeComponent();
        }

        // Create our exit function
        private int ExitDemo(string[] args)
        {
            InnerSpaceAPI.InnerSpace.Echo("uiForms demo exiting (In-game click).");
            Application.Exit();
            return 1;
        }

        private void buttonNewFrame_Click(object sender, EventArgs e)
        {
            LavishScriptAPI.LavishScript.Commands.AddCommand("uiFdemoExit", new LavishScriptAPI.Delegates.CommandTarget(ExitDemo));
            Frame.Lock();

            // Create a blank form, setting it's exit function to call ours.
            uiForm ui = new uiForm("uiFormsTest", "uiForms Demo", "ISCommand('uiFdemoExit')");

            // Create our logo
            uiTexture logotex = ui.frameui.CreateTexture(ui.frame + "LogoTex");
            logotex.SetTexture(@"'Interface\\PVPFrame\\Icons\\PVP-Banner-Emblem-81.blp'");
            logotex.SetPoint("CENTER", ui.frameui, "CENTER");
            logotex.SetVertexColor(0.4, 0.5, 0.9);

            // Resize the form to fit
            ui.height = 150;

            Frame.Unlock();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            InnerSpace.Echo("uiForms demo exiting.");
            Application.Exit();
        }

        ~uiDemoForm() {
            LavishScriptAPI.LavishScript.Commands.RemoveCommand("uiFdemoExit");
        }
    }
}