// Copyright (c) 2007 Gratian Lup. All rights reserved.
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
using Microsoft.ManagementConsole;
using SecureDeleteWinForms;
using SecureDeleteWinForms.Modules;
using System.ComponentModel;

namespace SecureDeleteMMC {
    [RunInstaller(true)]
    public class SecureDeleteInstaller : SnapInInstaller {

    }

    [SnapInSettings("{8ef070dc-85e1-46fa-96aa-50433a19b48d}", DisplayName = "SecureDelete", Description = "SecureDelete")]
    public class SecureDeleteSnapin : SnapIn {
        public SecureDeleteSnapin() {
            this.RootNode = new SecureDeleteScopeNode();
            this.RootNode.Children.Add(new WipeScopeNode());
            this.RootNode.Children.Add(new ScheduleScopeNode());
            this.RootNode.Children.Add(new ReportScopeNode());

            // get module images
            this.SmallImages.Add(WipeModule.ModuleImage);
            this.SmallImages.Add(ScheduleModule.ModuleImage);
            this.SmallImages.Add(ReportModule.ModuleImage);

            this.RootNode.Children[0].ImageIndex = 0;
            this.RootNode.Children[1].ImageIndex = 1;
            this.RootNode.Children[2].ImageIndex = 2;
            this.RootNode.Children[0].SelectedImageIndex = 0;
            this.RootNode.Children[1].SelectedImageIndex = 1;
            this.RootNode.Children[2].SelectedImageIndex = 2;

            // get all other images
            HostControl.BuildImageIndex(this);
        }
    }


    public class SecureDeleteScopeNode : ScopeNode {
        public SecureDeleteScopeNode()
            : base() {
            this.DisplayName = "SecureDelete";
            FormViewDescription view = new FormViewDescription(typeof(SummaryControl));
            view.DisplayName = "Wipe";
            view.ViewType = typeof(SummaryHost);

            this.ViewDescriptions.Add(view);
            this.ViewDescriptions.DefaultIndex = 0;
        }

        protected override void OnRefresh(AsyncStatus status) {
            base.OnRefresh(status);
        }
    }


    public class SummaryHost : FormView {
        private SummaryControl control;

        protected override void OnShow() {
            if(Control != null) {
                ((SummaryControl)Control).UpdateData();
            }

            base.OnShow();
        }
    }

    public class SecureDeleteHost : FormView {
        private HostControl control;

        public SecureDeleteHost()
            : base() {
        }

        protected override void OnShow() {
            if(Control != null) {
                switch(this.ScopeNode.DisplayName) {
                    case "SecureDelete": {
                        ((HostControl)Control).InitInterface(WipeModule.WipeModuleName);
                        break;
                    }
                    case "Wipe": {
                        ((HostControl)Control).InitInterface(WipeModule.WipeModuleName);
                        break;
                    }
                    case "Schedule": {
                        ((HostControl)Control).InitInterface(ScheduleModule.ScheduleModuleName);
                        break;
                    }
                    case "Reports": {
                        ((HostControl)Control).InitInterface(ReportModule.ReportModuleName);
                        break;
                    }
                    default: {
                        ((HostControl)Control).InitInterface(ReportModule.ReportModuleName);
                        break;
                    }
                }
            }

            base.OnShow();
        }

        protected override void OnInitialize(AsyncStatus status) {
            base.OnInitialize(status);
        }
    }


    public class WipeScopeNode : ScopeNode {
        public WipeScopeNode()
            : base() {
            this.DisplayName = "Wipe";

            FormViewDescription view = new FormViewDescription(typeof(HostControl));
            view.DisplayName = "Wipe";
            view.ViewType = typeof(SecureDeleteHost);

            this.ViewDescriptions.Add(view);
            this.ViewDescriptions.DefaultIndex = 0;
        }
    }


    public class ScheduleScopeNode : ScopeNode {
        public ScheduleScopeNode()
            : base() {
            this.DisplayName = "Schedule";

            FormViewDescription view = new FormViewDescription(typeof(HostControl));
            view.DisplayName = "Schedule";
            view.ViewType = typeof(SecureDeleteHost);

            this.ViewDescriptions.Add(view);
            this.ViewDescriptions.DefaultIndex = 0;
        }
    }

    public class ReportChildScopeNode : ScopeNode {
        public ReportChildScopeNode(string name)
            : base() {
            this.DisplayName = name;

            FormViewDescription view = new FormViewDescription(typeof(HostControl));
            view.DisplayName = name;
            view.ViewType = typeof(SecureDeleteHost);

            this.ViewDescriptions.Add(view);
            this.ViewDescriptions.DefaultIndex = 0;
        }
    }

    public class ReportScopeNode : ScopeNode {
        public ReportScopeNode()
            : base() {
            this.DisplayName = "Reports";

            FormViewDescription view = new FormViewDescription(typeof(HostControl));
            view.DisplayName = "Reports";
            view.ViewType = typeof(SecureDeleteHost);

            this.ViewDescriptions.Add(view);
            this.ViewDescriptions.DefaultIndex = 0;
        }
    }
}
