// Copyright (c) Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor
// may "SecureDelete" appear in their names without prior written
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using SecureDelete;
using System.Windows.Forms;
using System.Drawing;

namespace SecureDeleteWinForms.Modules {
    public enum MenuAction {
        ExitApplication, Options, Help, About, ImportExportSettings
    }

    public delegate void ModuleStatusDelegate(IModule module, string status);
    public delegate void MenuActionDelegeate(IModule module, MenuAction action);

    public interface IModule {
        string ModuleName { get; }
        Control ParentControl { get; set; }
        SDOptions Options { get; set; }
        event ModuleStatusDelegate OnStatusChanged;
        event MenuActionDelegeate OnMenuAction;
        void Activated();
        MenuStrip Menu { get; }
        ITool ChangeTool(ToolType type, bool setSize);
        void ChangeToolHeaderText(string text);
        void ChangeToolHeaderIcon(Image icon);
        ModuleActionManager ActionManager { get; }
    }

    public class ModuleActionManager {
        #region Constructor

        public ModuleActionManager() {
            _actions = new Dictionary<string, ModuleAction>();
        }

        #endregion

        private Dictionary<string, ModuleAction> _actions;
        public Dictionary<string, ModuleAction> Actions {
            get { return _actions; }
        }

        public event EventHandler OnStateChanged;

        public void StateChanged() {
            if(OnStateChanged != null) {
                OnStateChanged(this, null);
            }
        }
    }

    public delegate bool ActionEnabledDelegate();

    public enum ActionType {
        PanelAction, ListAction
    }

    public class ModuleAction {
        #region Constructor

        public ModuleAction(ActionType type, string name, string imageName, 
                            ActionEnabledDelegate enabledDelegate, EventHandler onClick) {
            _type = type;
            _name = name;
            _imageName = imageName;
            IsEnabled = enabledDelegate;
            _children = new List<ModuleAction>();

            if(onClick != null) {
                OnClick += onClick;
            }
        }

        #endregion

        private ActionType _type;
        public ActionType Type {
            get { return _type; }
            set { _type = value; }
        }

        private string _name;
        public string Name {
            get { return _name; }
        }

        private string _imageName;
        public string ImageName {
            get { return _imageName; }
            set { _imageName = value; }
        }

        private object _tag;
        public object Tag {
            get { return _tag; }
            set { _tag = value; }
        }

        private List<ModuleAction> _children;
        public List<ModuleAction> Children {
            get { return _children; }
            set { _children = value; }
        }

        public ActionEnabledDelegate IsEnabled;
        public event EventHandler OnClick;

        public void Trigger() {
            if(OnClick != null) {
                OnClick(this, null);
            }
        }
    }
}
