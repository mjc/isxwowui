using System;
using System.Collections.Generic;
using System.Text;
using ISXWoW;
using LavishVMAPI;
using InnerSpaceAPI;

namespace uiForms
{
    // Definitions matching www.wowwiki.com/Widget_API as closely as possible.
    // Get some bandages, your eyes will bleed.
    // Whatever assholio decided to NOT impliment multiple inheritance in C# needs to be shot repeatedly
    // Why can't I just do this in lua...
    // mjc: Send the box.
    class uiObject
    {
        public string name;
        internal ISXWoW.ISXWoW isxwow;

        public uiObject(string name)
        {
            this.name = name;
            isxwow = new ISXWoW.ISXWoW();
        }

        public uiObject()
        {
            isxwow = new ISXWoW.ISXWoW();
        }

        // Ugh. No typecasting, please. Augh. It burns.
        public void memberLua(string cmd)
        {
            memberLua(cmd, "");
        }

        public T memberLua<T>(string cmd)
        {
            return isxwow.WoWScript<T>(String.Format("{0}:{1}()", name, cmd));
        }

        public void memberLua(string cmd, string arg)
        {
            doLua(String.Format("{0}:{1}({2})", name, cmd, arg));
        }

        public void memberLua(string cmd, int arg)
        {
            memberLua(cmd, arg.ToString());
        }

        public void memberLua(string cmd, params string[] args)
        {
            doLua(String.Format("{0}:{1}({2})", name, cmd, String.Join(", ", args)));
        }

        internal List<string> getLua(string cmd)
        {
            return isxwow.WoWScript(cmd);
        }

        internal void doLua(string cmd)
        {
            echo("Lua: " + cmd);
            isxwow.WoWScriptExecute(cmd);
        }

        internal string q(string wrap)
        {
            if (wrap == "nil") return wrap;
            else return "'" + wrap + "'";
        }

        public override string ToString()
        {
            return this.name;
        }

        public void echo(string what)
        {
            InnerSpaceAPI.InnerSpace.Echo(what);
        }
    }

    class uiFontInstance : uiObject
    {
        public uiFontInstance(string name) : base(name)
        {
        }

        public void SetFont(string path, int height)
        {
            memberLua("SetFont", q(path), height.ToString());
        }

        public void SetJustifyH(string where)
        {
            memberLua("SetJustifyH", q(where));
        }

        public void SetJustifyV(string where)
        {
            memberLua("SetJustifyV", q(where));
        }

        public void SetFontObject(string what)
        {
            memberLua("SetFontObject", what);
        }
    }

    class uiRegion : uiObject
    {
        public string type, parent, inherits;

        public uiRegion(string name) : base(name)
        {
        }

        public void SetPoint(string point, uiRegion relativeTo, string relativePoint)
        {
            SetPoint(point, relativeTo.name, relativePoint);
        }

        public void SetPoint(string point, uiRegion relativeTo, string relativePoint, int x, int y)
        {
            SetPoint(point, relativeTo.name, relativePoint, x, y);
        }

        public void SetPoint(string point, string relativeTo, string relativePoint)
        {
            SetPoint(point, relativeTo, relativePoint, 0, 0);
        }

        public void SetPoint(string point, string relativeTo, string relativePoint, int x, int y)
        {
            memberLua("SetPoint", q(point), relativeTo, q(relativePoint), x.ToString(), y.ToString());
        }

        public void SetAllPoints(string what)
        {
            memberLua("SetAllPoints", what);
        }

        public void SetHeight(int height)
        {
            memberLua("SetHeight", height);
        }

        public void SetWidth(int width)
        {
            memberLua("SetWidth", width);
        }

        public void Show()
        {
            memberLua("Show");
        }

        public void Hide()
        {
            memberLua("Hide");
        }
    }

    class uiLayeredRegion : uiRegion
    {
        public string layer;
        public uiLayeredRegion(string name) : base(name) 
        {
        }

        public void SetVertexColor(double r, double g, double b)
        {
            memberLua("SetVertexColor", r.ToString(), g.ToString(), b.ToString());
        }

        public void SetVertexColor(double r, double g, double b, double a)
        {
            memberLua("SetVertexColor", r.ToString(), g.ToString(), b.ToString(), a.ToString());
        }
    }

    class uiFontString : uiLayeredRegion
    {
        public uiFontInstance font;
        public uiFontString(string nname, string nlayer, string ninherits) : base(nname)
        {
            layer = nlayer;
            inherits = ninherits;
            font = new uiFontInstance(nname);
        }

        public void SetText(string text)
        {
            memberLua("SetText", q(text));
        }
    }

    class uiTexture : uiLayeredRegion
    {
        public uiTexture(string name) : base(name)
        {

        }

        public void SetTexture(string path) 
        {
            memberLua("SetTexture", path);
        }

        public void SetBlendMode(string mode)
        {
            memberLua("SetBlendMode", q(mode));
        }

        public void SetTexture(double r, double g, double b, double a) 
        {
            memberLua("SetTexture", r.ToString(), g.ToString(), b.ToString(), a.ToString());
        }

        public void SetTexture(double r, double g, double b)
        {
            memberLua("SetTexture", r.ToString(), g.ToString(), b.ToString());
        }

        public void SetGradient(string orientation, double minR, double minG, double minB, double maxR, double maxG, double maxB)
        {
            memberLua("SetGradient", q(orientation), minR.ToString(), minG.ToString(), minB.ToString(), maxR.ToString(), maxG.ToString(), maxB.ToString());
        }

        public void SetGradientAlpha(string orientation, double minR, double minG, double minB, double minA, double maxR, double maxG, double maxB, double maxA)
        {
            memberLua("SetGradientAlpha", q(orientation), minR.ToString(), minG.ToString(), minB.ToString(), minA.ToString(), maxR.ToString(), maxG.ToString(), maxB.ToString(), maxA.ToString());
        }

        public void SetTexCord(double minX, double maxX, double minY, double maxY)
        {
            memberLua("SetTexCord", minX.ToString(), maxX.ToString(), minY.ToString(), maxY.ToString());
        }

        public void SetTexCord(double ULx, double ULy, double LLx, double LLy, double URx, double URy, double LRx, double LRy)
        {
            memberLua("SetTexCord", ULx.ToString(), ULy.ToString(), LLx.ToString(), LLy.ToString(), URx.ToString(), URy.ToString(), LRx.ToString(), LRy.ToString());
        }
    }

    class uiFrame : uiRegion
    {
        public uiFrame(string type, string name, string parent) : base(name)
        {
            doLua("CreateFrame('" + type + "', '" + name + "', " + parent + ")");
        }

        public uiFrame(string type, string name, string parent, string inherits) : base(name)
        {
            this.inherits = inherits;
            this.parent = parent;
            this.type = type;
            doLua("CreateFrame('"+type+"', '"+name+"', "+parent+", "+q(inherits)+")");
        }

        public void SetFrameStrata(string strata)
        {
            memberLua("SetFrameStrata", q(strata));
        }

        public void SetFrameLevel(int level)
        {
            memberLua("SetFrameLevel", level);
        }

        public int GetFrameLevel()
        {
            return memberLua<int>("GetFrameLevel");
        }

        public void SetMovable(int movable)
        {
            memberLua("SetMovable", movable);
        }

        public void EnableMouse(int enabled)
        {
            memberLua("EnableMouse", enabled);
        }

        public void RegisterForDrag(string button)
        {
            memberLua("RegisterForDrag", q(button));
        }

        public void SetScript(string script, string lua)
        {
            memberLua("SetScript", q(script), "function() " + lua + " end");
        }

        public void SetScript(string script, string luaformat, string var)
        {
            SetScript(script, String.Format(luaformat, var));
        }

        public uiFontString CreateFontString(string name)
        {
            return CreateFontString(name, "nil", "nil");
        }

        public uiFontString CreateFontString(string name, string layer)
        {
            return CreateFontString(name, layer, "nil");
        }

        public uiFontString CreateFontString(string name, string layer, string inherits)
        {
            memberLua("CreateFontString", q(name), q(layer), q(inherits));
            return new uiFontString(name, layer, inherits);
        }

        public uiTexture CreateTexture(string name)
        {
            return CreateTexture(name, "nil", "nil");
        }

        public uiTexture CreateTexture(string name, string layer)
        {
            return CreateTexture(name, layer, "nil");
        }

        public uiTexture CreateTexture(string name, string layer, string inherits)
        {
            memberLua("CreateTexture", q(name), q(layer), q(inherits));
            return new uiTexture(name);
        }

        public void SetHitRectInsets(int left, int right, int top, int bottom)
        {
            memberLua("SetHitRectInsets", left.ToString(), right.ToString(), top.ToString(), bottom.ToString());
        }

        public void SetBackdrop(string backdrop)
        {
            memberLua("SetBackdrop", backdrop);
        }

        public void SetBackdropColor(double r, double g, double b)
        {
            memberLua("SetBackdropColor", r.ToString(), g.ToString(), b.ToString());
        }

        public void SetBackdropColor(double r, double g, double b, double a)
        {
            memberLua("SetBackdropColor", r.ToString(), g.ToString(), b.ToString(), a.ToString());
        }

        public void SetBackdropBorderColor(double r, double g, double b)
        {
            memberLua("SetBackdropBorderColor", r.ToString(), g.ToString(), b.ToString());
        }

        public void SetBackdropBorderColor(double r, double g, double b, double a)
        {
            memberLua("SetBackdropBorderColor", r.ToString(), g.ToString(), b.ToString(), a.ToString());
        }
    }

    class uiButton : uiFrame
    {
        public uiFontInstance font;
        public uiButton(string type, string name, string parent) : base(type, name, parent)
        {
            font = new uiFontInstance(name);
        }

        public uiButton(string type, string name, string parent, string inherits) : base(type, name, parent, inherits)
        {
            font = new uiFontInstance(name);
        }

        public void RegisterForClicks(params string[] buttons)
        {
            for ( int i=0 ; i <= buttons.Length ; i++ )
                buttons[i] = q(buttons[i]);
            memberLua("RegisterForClicks", String.Join(", ", buttons));
        }

        public void SetNormalTexture(string texture)
        {
            memberLua("SetNormalTexture", texture);
        }

        public void SetPushedTexture(string texture)
        {
            memberLua("SetPushedTexture", texture);
        }

        public void SetHighlightTexture(string texture)
        {
            memberLua("SetHighlightTexture", texture);
        }

        public void SetText(string text)
        {
            memberLua("SetText", q(text));
        }

        public void SetTextColor(double r, double g, double b)
        {
            memberLua("SetTextColor", r.ToString(), g.ToString(), b.ToString());
        }

        public void SetButtonState(string state, bool locked)
        {
            memberLua("SetButtonState", state, locked.ToString());
        }

        public void SetButtonState(string state)
        {
            memberLua("SetButtonState", state);
        }

        public void Enable()
        {
            memberLua("Enable");
        }

        public void Disable()
        {
            memberLua("Disable");
        }
    }

    class uiForm
    {
        static int forms;
        string title, name, frame;
        uiFrame titleui, frameui;
        uiButton close, shade;
        uiFontString titletext;

        public uiForm(string name, string label, string exitlua)
        {
            this.name = name;
            this.safe();
            this.buildForm(label, exitlua);
        }

        public uiForm(string name, string label)
        {
            this.name = name;
            this.safe();
            this.buildForm(label, "");
        }

        private void buildForm(string label, string exitlua)
        {
            uiForm.forms++;
            this.frame = "uiFormsFrame" + uiForm.forms;
            this.title = this.frame + "Title";

            titleui = new uiFrame("Frame", title, "UIParent");
            titletext = titleui.CreateFontString(title+"Text", "ARTWORK", "GameFontNormalLarge");//(this.title, this.title + "Text", "ARTWORK", "GameFontNormalLarge");
            frameui = new uiFrame("Frame", frame, title);
            shade = new uiButton("Button", title + "Shade", title);
            close = new uiButton("Button", title + "Close", title);


            titleui.SetFrameStrata("DIALOG");
            titleui.SetHeight(26);
            titleui.SetWidth(200);
            titleui.SetMovable(1);
            titleui.EnableMouse(1);
            titleui.SetFrameLevel(6);
            titleui.SetPoint("CENTER", "UIParent", "CENTER");
            titleui.RegisterForDrag("LeftButton");
            titleui.SetScript("OnDragStart", title+":StartMoving()");
            titleui.SetScript("OnDragStop", title+":StopMovingOrSizing()");

            titletext.SetAllPoints(title);
            titletext.font.SetJustifyH("LEFT");
            titletext.SetText("   " + label + "");
            titletext.SetVertexColor(1, 1, 1);

            frameui.SetPoint("TOPLEFT", title, "BOTTOMLEFT", 0, 9);
            frameui.SetPoint("TOPRIGHT", title, "BOTTOMRIGHT", 0, 9);
            frameui.SetHeight(100);
            frameui.SetFrameLevel(titleui.GetFrameLevel() - 1);

            close.SetNormalTexture(@"'Interface\\Buttons\\UI-Panel-MinimizeButton-Up'");
            close.SetPushedTexture(@"'Interface\\Buttons\\UI-Panel-MinimizeButton-Down'");
            close.SetHighlightTexture(@"'Interface\\Buttons\\UI-Panel-MinimizeButton-Highlight'");
            close.SetPoint("TOPRIGHT", title, "TOPRIGHT", 3, 3);
            close.SetWidth(32);
            close.SetHeight(32);
            close.SetFrameLevel(titleui.GetFrameLevel() + 1);
            close.SetHitRectInsets(5, 5, 5, 5);
            close.SetScript("OnClick", exitlua+" "+title+":Hide()", frame);
            close.Show();

            //shade.SetNormalTexture(@"'Interface\\Buttons\\UI-MinusButton-Up.blp'");
            //shade.SetPushedTexture(@"'Interface\\Buttons\\UI-MinusButton-Down.blp'");
            shade.SetHighlightTexture(@"'Interface\\Buttons\\UI-PlusButton-Hilight.blp'");
            shade.SetText("-");
            shade.SetTextColor(.8, .7, .1);
            shade.SetPoint("RIGHT", close.name, "LEFT", 6, 0);
            shade.SetWidth(24);
            shade.SetHeight(24);
            shade.SetFrameLevel(titleui.GetFrameLevel() + 1);
            shade.SetHitRectInsets(-5, 5, 0, 0);
            shade.SetScript("Onclick", "if {0}:IsVisible() then {0}:Hide() else {0}:Show() end", frame);
            shade.Show();
            

            // Skin
            skin(titleui);
            skin(frameui);

            // Show
            titleui.Show();
        }

        public void skin(uiFrame frame)
        {
            frame.SetBackdrop(@"{bgFile = 'Interface\\ChatFrame\\ChatFrameBackground', tile = true, tileSize = 16,	edgeFile = 'Interface\\Tooltips\\UI-Tooltip-Border', edgeSize = 13,	insets = {left = 3, right = 3, top = 3, bottom = 3}}");
            frame.SetBackdropBorderColor(.5, .5, .5, 1);
            frame.SetBackdropColor(0, 0, 0, .9);
            uiTexture fade = frame.CreateTexture(frame.name + "Fade", "BORDER");
            fade.SetTexture(@"'Interface\\ChatFrame\\ChatFrameBackground'");
            fade.SetBlendMode("ADD");
            fade.SetGradientAlpha("VERTICAL", .1, .1, .1, 0, .3, .3, .2, 1);
            fade.SetPoint("TOPLEFT", frame, "TOPLEFT", 3, -3);
            fade.SetPoint("BOTTOMRIGHT", frame, "BOTTOMRIGHT", -3, 4);
        }

        public void Hide()
        {
            frameui.Hide();
        }

        public void echo(string what)
        {
            InnerSpaceAPI.InnerSpace.Echo("uiForms: "+what);
        }

        public void doLua(string luacmd)
        {
            ISXWoW.ISXWoW isxwow = new ISXWoW.ISXWoW();
            isxwow.WoWScriptExecute(luacmd);
        }

        private bool safe()
        {
            bool locked = Frame.IsLocked;
            if (!locked)
                this.echo("Frame not locked! ["+this.name+"] IS Performance will suffer if you do not call Frame.Lock() before ui operations.");
            return locked;
        }
    }
}
