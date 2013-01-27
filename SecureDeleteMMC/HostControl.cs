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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.ManagementConsole;
using SecureDeleteWinForms;
using SecureDeleteWinForms.Modules;

namespace SecureDeleteMMC {
    public partial class HostControl : UserControl, IFormViewControl {
        private FormView view;
        private static Dictionary<string, int> imageIndex;
        private Dictionary<string, ActionsPaneItem[]> actionCache;

        public HostControl() {
            InitializeComponent();
            actionCache = new Dictionary<string, ActionsPaneItem[]>();
            this.Dock = DockStyle.Fill;
        }

        public void InitInterface(string moduleName) {
            InitializeComponent();
            Interface.ChangeModule(moduleName);
        }

        public void ModuleChangedHandler(object sender, EventArgs e) {
            if(Interface.ActiveModule == null) {
                return;
            }

            if(Interface.ActiveModule.ModuleName == WipeModule.WipeModuleName) {
                HandleWipeModule();
            }
            else if(Interface.ActiveModule.ModuleName == ScheduleModule.ScheduleModuleName) {
                HandleScheduleModule();
            }
            else if(Interface.ActiveModule.ModuleName == ReportModule.ReportModuleName) {
                HandleReportModule();
            }
        }

        public static void BuildImageIndex(NamespaceSnapInBase snapin) {
            // wipe module
            imageIndex = new Dictionary<string, int>();
            Dictionary<string, Image> images = WipeModule.ActionImages;

            foreach(KeyValuePair<string, Image> kvp in images) {
                imageIndex.Add(kvp.Key, snapin.SmallImages.Count);
                snapin.SmallImages.Add(kvp.Value);
            }

            // schedule module
            images = ScheduleModule.ActionImages;

            foreach(KeyValuePair<string, Image> kvp in images) {
                imageIndex.Add(kvp.Key, snapin.SmallImages.Count);
                snapin.SmallImages.Add(kvp.Value);
            }
        }

        public static ModuleAction[] GetReportListItems() {
            if(Interface == null) {
                return null;
            }

            foreach(IModule module in Interface.Modules) {
                if(module.ModuleName == ReportModule.ReportModuleName) {
                    if(module.ActionManager != null && module.ActionManager.Actions != null) {
                        List<ModuleAction> actions = new List<ModuleAction>();

                        foreach(KeyValuePair<string, ModuleAction> action in module.ActionManager.Actions) {
                            if(action.Value.Type == ActionType.ListAction) {
                                actions.Add(action.Value);
                            }
                        }

                        return actions.ToArray();
                    }

                    break;
                }
            }
            
            return null;
        }

        public void ActionStateChanged(object sender, EventArgs e) {
            if(sender is ModuleActionManager) {
                ModuleActionManager manager = (ModuleActionManager)sender;

                foreach(KeyValuePair<string, ModuleAction> kvp in manager.Actions) {
                    if(kvp.Value.Tag is Microsoft.ManagementConsole.Action) {
                        UpdateAction((Microsoft.ManagementConsole.Action)kvp.Value.Tag, kvp.Value);
                    }
                }
            }
        }

        private void UpdateAction(Microsoft.ManagementConsole.Action action, ModuleAction moduleAction) {
            action.Enabled = moduleAction.IsEnabled();
        }

        private void AddAction(ModuleAction moduleAction, string name) {
            Microsoft.ManagementConsole.Action action = new Microsoft.ManagementConsole.Action();
            action.DisplayName = name;
            moduleAction.Tag = action;
            action.Tag = moduleAction;
            action.Triggered += ActionTriggeredHandler;

            // set image
            if(imageIndex.ContainsKey(moduleAction.ImageName)) {
                action.ImageIndex = imageIndex[moduleAction.ImageName];
            }

            UpdateAction(action, moduleAction);
            view.ActionsPaneItems.Add(action);
        }

        private void HandleWipeModule() {
            view.ActionsPaneItems.Clear();

            if(Interface.ActiveModule.ActionManager != null) {
                ModuleActionManager manager = Interface.ActiveModule.ActionManager;

                if(actionCache.ContainsKey(Interface.ActiveModule.ModuleName)) {
                    view.ActionsPaneItems.AddRange(actionCache[Interface.ActiveModule.ModuleName]);
                }
                else {
                    TryAddAction("New", "New", manager.Actions);
                    TryAddAction("Open", "Open", manager.Actions);
                    TryAddAction("Save", "Save", manager.Actions);
                    TryAddAction("SaveAs", "Save As", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("File", "Insert File", manager.Actions);
                    TryAddAction("Folder", "Insert Folder", manager.Actions);
                    TryAddAction("Drive", "Insert Drive", manager.Actions);
                    TryAddAction("Plugin", "Insert Plugin", manager.Actions);
                    TryAddAction("Search", "Search Files", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("Edit", "Edit", manager.Actions);
                    TryAddAction("RemoveSelected", "Remove Selected", manager.Actions);
                    TryAddAction("RemoveAll", "Remove All", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("Start", "Start Wipe", manager.Actions);
                    TryAddAction("Stop", "Stop Wipe", manager.Actions);
                    TryAddAction("Options", "Options", manager.Actions);
                    actionCache.Add(Interface.ActiveModule.ModuleName, view.ActionsPaneItems.ToArray());
                }

                if(Interface.Parent != null) {
                    Interface.ActiveModule.ActionManager.OnStateChanged -= ((HostControl)Interface.Parent).ActionStateChanged;
                }
                Interface.ActiveModule.ActionManager.OnStateChanged += ActionStateChanged;
            }
        }

        private void TryAddAction(string name, string displayName, Dictionary<string, ModuleAction> actions) {
            if(actions.ContainsKey(name)) {
                AddAction(actions[name], displayName);
            }
        }

        private void HandleScheduleModule() {
            view.ActionsPaneItems.Clear();

            if(Interface.ActiveModule.ActionManager != null) {
                ModuleActionManager manager = Interface.ActiveModule.ActionManager;

                if(actionCache.ContainsKey(Interface.ActiveModule.ModuleName)) {
                    view.ActionsPaneItems.AddRange(actionCache[Interface.ActiveModule.ModuleName]);
                }
                else {
                    TryAddAction("New", "New Task", manager.Actions);
                    TryAddAction("Remove", "Remove Task", manager.Actions);
                    TryAddAction("Edit", "Task Options", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("Start", "Force Start", manager.Actions);
                    TryAddAction("Stop", "Stop", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("Import", "Import", manager.Actions);
                    TryAddAction("Export", "Export", manager.Actions);
                    TryAddAction("RemoveAll", "Remove All", manager.Actions);
                    actionCache.Add(Interface.ActiveModule.ModuleName, view.ActionsPaneItems.ToArray());
                }

                if(Interface.Parent != null) {
                    Interface.ActiveModule.ActionManager.OnStateChanged -= ((HostControl)Interface.Parent).ActionStateChanged;
                }
                Interface.ActiveModule.ActionManager.OnStateChanged += ActionStateChanged;
            }
        }

        private void HandleReportModule() {
            view.ActionsPaneItems.Clear();

            if(Interface.ActiveModule.ActionManager != null) {
                ModuleActionManager manager = Interface.ActiveModule.ActionManager;

                if(actionCache.ContainsKey(Interface.ActiveModule.ModuleName)) {
                    view.ActionsPaneItems.AddRange(actionCache[Interface.ActiveModule.ModuleName]);
                }
                else {
                    TryAddAction("View", "View", manager.Actions);
                    TryAddAction("RemoveSelected", "Remove Selected", manager.Actions);
                    TryAddAction("RemoveAll", "Remove All", manager.Actions);
                    view.ActionsPaneItems.Add(new ActionSeparator());

                    TryAddAction("Search", "Search", manager.Actions);
                    Interface.ActiveModule.ActionManager.OnStateChanged += ActionStateChanged;
                    actionCache.Add(Interface.ActiveModule.ModuleName, view.ActionsPaneItems.ToArray());
                }

                if(Interface.Parent != null) {
                    Interface.ActiveModule.ActionManager.OnStateChanged -= ((HostControl)Interface.Parent).ActionStateChanged;
                }

                Interface.ActiveModule.ActionManager.OnStateChanged -= ActionStateChanged;
                Interface.ActiveModule.ActionManager.OnStateChanged += ActionStateChanged;
            }
        }

        private void ActionTriggeredHandler(object sender, ActionEventArgs e) {
            if(e.Action.Tag != null) {
                ModuleAction action = (ModuleAction)e.Action.Tag;
                action.Trigger();
            }
        }

        #region IFormViewControl Members

        public void Initialize(FormView formView) {
            Interface.LoadOptions();

            if(Interface.Parent != null) {
                Interface.OnModuleChanged -= ((HostControl)Interface.Parent).ModuleChangedHandler;
            }

            Interface.OnModuleChanged += ModuleChangedHandler;
            view = formView;
        }

        #endregion
    }
}

