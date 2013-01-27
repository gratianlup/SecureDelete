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
using EnvDTE;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.IO;

namespace PluginWizard {
    [ProgId("VSWizard.VSCustomWizardEngine")]
    public class Wizard : IWizard {
        private WizardForm form;

        #region IWizard Members

        public void BeforeOpeningFile(ProjectItem projectItem) {

        }

        public void ProjectFinishedGenerating(Project project) {

        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) {
            AddPlugin(projectItem);
        }

        public void RunFinished() {

        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, 
                               WizardRunKind runKind, object[] customParams) {
            form = new WizardForm();
            form.ShowDialog();
        }

        public bool ShouldAddProjectItem(string filePath) {
            if(form.DialogResult != DialogResult.OK) {
                return false;
            }

            return true;
        }

        #endregion

        private void AddPlugin(ProjectItem item) {
            string directory = Path.GetDirectoryName(item.ContainingProject.FullName);
            string source = ReplaceSource(SourceCode.Plugin, item);
            string name = item.Name;

            if(name.EndsWith(".cs")) {
                name = name.Substring(0, name.Length - 3);
            }

            // write the file
            StreamWriter writer = new StreamWriter(Path.Combine(directory, name + ".cs"));
            writer.Write(source);
            writer.Close();

            if(form.OptionsCheckbox.Checked) {
                string a, b, c;
                AddOptionsDialog(item, out a, out  b, out c);

                writer = new StreamWriter(Path.Combine(directory, name + "Dialog.cs"));
                writer.Write(a);
                writer.Close();
                ProjectItem parent = item.ContainingProject.ProjectItems.AddFromFile(Path.Combine(directory, name + "Dialog.cs"));

                writer = new StreamWriter(Path.Combine(directory, name + "Dialog.Designer.cs"));
                writer.Write(b);
                writer.Close();
                parent.ProjectItems.AddFromFile(Path.Combine(directory, name + "Dialog.Designer.cs"));

                writer = new StreamWriter(Path.Combine(directory, name + "Dialog.resx"));
                writer.Write(c);
                writer.Close();
                parent.ProjectItems.AddFromFile(Path.Combine(directory, name + "Dialog.resx"));
            }
        }

        private string ReplaceSource(string source, ProjectItem item) {
            source = source.Replace("%ProjectName%", item.ContainingProject.Name);
            source = source.Replace("%Guid%", form.GuidValue.Text);
            source = source.Replace("%Name%", form.NameValue.Text);
            source = source.Replace("%Description%", form.DescriptionValue.Text);
            source = source.Replace("%MajorVersion%", form.MajorVersionValue.Text);
            source = source.Replace("%MinorVersion%", form.MinorVersionValue.Text);
            source = source.Replace("%Author%", form.AuthorValue.Text);
            source = source.Replace("%PluginName%", form.NameValue.Text);

            if(form.OptionsCheckbox.Checked) {
                source = source.Replace("%OptionsDialogFields%", "private " + form.NameValue.Text + "Dialog dialog;");
                source = source.Replace("%HasOptionsDialog%", "true");
                source = source.Replace("%OptionsDialog%", "if(dialog == null)" + Environment.NewLine + 
                                        "\t\t\t{" + Environment.NewLine + "\t\t\t\tdialog = new " +
                                        form.NameValue.Text + "Dialog();" + Environment.NewLine + "\t\t\t}" +
                                        Environment.NewLine + Environment.NewLine + "\t\t\treturn dialog;");
            }
            else {
                source = source.Replace("%OptionsDialogFields%", "");
                source = source.Replace("%HasOptionsDialog%", "false");
                source = source.Replace("%OptionsDialog%", "return null;");
            }

            return source;
        }

        private void AddOptionsDialog(ProjectItem item, out string dialog, 
                                      out string dialogDesigner, out string dialogResources) {
            dialog = SourceCode.Dialog;
            dialog = dialog.Replace("%ProjectName%", item.ContainingProject.Name);
            dialog = dialog.Replace("%DialogName%", form.NameValue.Text + "Dialog");

            dialogDesigner = SourceCode.DialogDesigner;
            dialogDesigner = dialogDesigner.Replace("%ProjectName%", item.ContainingProject.Name);
            dialogDesigner = dialogDesigner.Replace("%DialogName%", form.NameValue.Text + "Dialog");

            dialogResources = SourceCode.DialogResource;
        }
    }
}
