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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DebugUtils.Debugger;
using System.IO;
using SecureDelete;

namespace SecureDeleteWinForms {
    public partial class WipeMethods : Form {
        public WipeMethods() {
            InitializeComponent();
        }

        #region Fields

        private bool optionsVisible;
        private WipeMethod activeMethod;
        private int activeMethodIndex;
        private bool methodModified;
        private WipeStepBase activeStep;
        private int activeStepIndex;
        private bool loadingSteps;

        #endregion

        #region Properties

        private WipeMethodManager _methodManager;
        public WipeMethodManager MethodManager {
            get { return _methodManager; }
            set { _methodManager = value; }
        }

        private int _selectedMethod = -1;
        public int SelectedMethod {
            get { return _selectedMethod; }
            set { _selectedMethod = value; }
        }

        private bool _showSelected;
        public bool ShowSelected {
            get { return _showSelected; }
            set { _showSelected = value; SelectButton.Enabled = _showSelected; }
        }

        private SDOptions _options;
        public SDOptions Options {
            get { return _options; }
            set { _options = value; }
        }

        #endregion

        #region Private methods

        private void LoadMethods() {
            Debug.AssertNotNull(_methodManager, "Method manager not set");
            MethodList.Items.Clear();

            for(int i = 0; i < _methodManager.Methods.Count; i++) {
                ListViewItem item = new ListViewItem();

                item.Text = ((int)(i + 1)).ToString();
                item.SubItems.Add(_methodManager.Methods[i].Name);
                item.SubItems.Add(_methodManager.Methods[i].Steps.Count.ToString());

                if(i == _selectedMethod) {
                    item.ImageIndex = 0;
                }

                MethodList.Items.Add(item);
            }

            UpdateMethodCountLabel();
        }

        private void SelectMethod(int index) {
            // deselect
            if(_selectedMethod >= 0 && _selectedMethod < MethodList.Items.Count) {
                MethodList.Items[_selectedMethod].ImageIndex = -1;
            }

            _selectedMethod = index;
            MethodList.Items[_selectedMethod].ImageIndex = 0;
        }

        private void ShowOptionsPanel() {
            splitContainer1.Panel2Collapsed = false;
            optionsVisible = true;

            toolStrip1.Enabled = false;
            MethodList.Enabled = false;
        }

        private void HideOptionsPanel() {
            if(methodModified) {
                DialogResult result = MessageBox.Show("\"" + activeMethod.Name + "\" wipe method not saved. Do you want to save the changes ?",
                                                      "SecureDelete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                                                      MessageBoxDefaultButton.Button1);

                if(result == DialogResult.Yes) {
                    SaveChages();
                }
                else if(result == DialogResult.Cancel) {
                    return;
                }
            }

            splitContainer1.Panel2Collapsed = true;
            optionsVisible = false;

            toolStrip1.Enabled = true;
            MethodList.Enabled = true;
        }

        private void SaveChages() {
            Debug.AssertNotNull(activeMethod, "ActiveMethod not set");

            if(methodModified == false) {
                HideOptionsPanel();
                return;
            }

            // validate
            if(activeMethod.ValidateMethod() == false) {
                MessageBox.Show("Wipe method not valid. Probably no wipe steps added.", "SecureDelete", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _methodManager.Methods[activeMethodIndex] = activeMethod;
            activeMethod = (WipeMethod)activeMethod.Clone();
            _methodManager.SaveMethod(activeMethod);
            UpdateMethod(activeMethodIndex);

            methodModified = false;
            HandleMethodModified();
            HideOptionsPanel();
        }

        private void HandleMethodModified() {
            SaveMethodButton.Enabled = methodModified;
        }

        private void UpdateMethod(int index) {
            Debug.Assert(index >= 0 && index < MethodList.Items.Count, "Index out of range");

            MethodList.Items[index].SubItems[0].Text = ((int)(index + 1)).ToString();
            MethodList.Items[index].SubItems[1].Text = _methodManager.Methods[index].Name;
            MethodList.Items[index].SubItems[2].Text = _methodManager.Methods[index].Steps.Count.ToString();
        }

        private void ShowMethodOptions(int index) {
            ShowOptionsPanel();
            activeMethod = (WipeMethod)_methodManager.Methods[index].Clone();
            activeMethodIndex = index;
            methodModified = false;

            MethodNameLabel.Text = activeMethod.Name + " method steps";
            MethodNameLabel2.Text = activeMethod.Name + " method settings";
            NameTextbox.Text = activeMethod.Name;
            ShuffleLastTextbox.Maximum = activeMethod.Steps.Count;
            CheckCheckbox.Checked = activeMethod.CheckWipe;

            PopulateStepList();
            ShuffleCheckbox.Checked = activeMethod.Shuffle;
            SetShuffleMinMax();

            if(ShuffleCheckbox.Checked) {
                ShuffleFirstTextbox.Value = activeMethod.ShuffleFirst + 1;
                ShuffleLastTextbox.Value = activeMethod.ShuffleLast + 1;
            }

            if(StepList.Items.Count > 0) {
                SelectStep(0, true);
            }

            methodModified = false;
            HandleMethodModified();
        }

        private void PopulateStepList() {
            // load the steps
            StepList.Items.Clear();

            for(int i = 0; i < activeMethod.Steps.Count; i++) {
                StepList.Items.Add(GetStepName(i));
            }
        }

        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            ShuffleFirstTextbox.Enabled = ShuffleLastTextbox.Enabled = ShuffleCheckbox.Checked;
            activeMethod.Shuffle = ShuffleCheckbox.Checked;

            methodModified = true;
            HandleMethodModified();
        }

        private void WipeMethods_Load(object sender, EventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.LoadPosition(this);
            LoadMethods();
            splitContainer1.Panel2Collapsed = true;
        }

        private void EditSelectedMethod() {
            if(MethodList.SelectedIndices.Count > 0) {
                ShowMethodOptions(MethodList.SelectedIndices[0]);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            EditSelectedMethod();
        }

        private void StepList_SelectedIndexChanged(object sender, EventArgs e) {
            if(loadingSteps == false) {
                if(StepList.SelectedIndices.Count > 0) {
                    SelectStep(StepList.SelectedIndices[0], false);
                }
            }
        }

        private void SelectStep(int index, bool select) {
            Debug.AssertNotNull(activeMethod, "ActiveMethod not set");

            activeStep = activeMethod.Steps[index];
            activeStepIndex = index;
            loadingSteps = true;

            if(select) {
                StepList.SelectedIndices.Clear();
                StepList.SelectedIndices.Add(index);
            }

            PatternTextbox.Enabled = false;
            PatternTextbox.Text = string.Empty;
            UncheckOptionboxes();

            // disable complement on first position
            ComplementOptionbox.Enabled = index > 0;

            if(activeStep.Type == WipeStepType.Pattern) {
                PatternOptionbox.Checked = true;
                byte[] pattern = ((PatternWipeStep)activeStep).Pattern;

                PatternTextbox.Text = string.Empty;
                PatternTextbox.Enabled = true;
                if(pattern != null && pattern.Length > 0) {
                    for(int i = 0; i < pattern.Length; i++) {
                        PatternTextbox.Text += pattern[i].ToString() + (i < (pattern.Length - 1) ? " " : string.Empty);
                    }
                }
            }
            else if(activeStep.Type == WipeStepType.Random) {
                RandomOptionbox.Checked = true;
            }
            else if(activeStep.Type == WipeStepType.RandomByte) {
                RandombyteOptionbox.Checked = true;
            }
            else {
                ComplementOptionbox.Checked = true;
            }

            loadingSteps = false;
        }

        private void toolStripButton6_Click(object sender, EventArgs e) {
            HideOptionsPanel();
        }

        private void button1_Click(object sender, EventArgs e) {
            HideOptionsPanel();
        }

        private void SaveMethodButton_Click(object sender, EventArgs e) {
            SaveChages();
        }

        private void NameTextbox_TextChanged(object sender, EventArgs e) {

        }

        private void CheckCheckbox_CheckedChanged(object sender, EventArgs e) {
            activeMethod.CheckWipe = CheckCheckbox.Checked;

            methodModified = true;
            HandleMethodModified();
        }

        private void UncheckOptionboxes() {
            PatternOptionbox.Checked = false;
            RandombyteOptionbox.Checked = false;
            RandomOptionbox.Checked = false;
            ComplementOptionbox.Checked = false;
        }

        private void PatternOptionbox_CheckedChanged(object sender, EventArgs e) {
            if(PatternOptionbox.Checked == true) {
                if(loadingSteps == true) {
                    return;
                }

                RandombyteOptionbox.Checked = false;
                RandomOptionbox.Checked = false;
                ComplementOptionbox.Checked = false;

                activeStep = new PatternWipeStep(activeStep.Number);
                activeMethod.Steps[activeStepIndex] = activeStep;
                UpdateStepName(activeStepIndex);
                PatternTextbox.Enabled = true;
                methodModified = true;
                HandleMethodModified();
            }
            else {
                PatternTextbox.Enabled = false;
            }
        }

        private void PatternTextbox_Validating(object sender, CancelEventArgs e) {
            if(activeStep is PatternWipeStep) {
                PatternWipeStep step = activeStep as PatternWipeStep;

                string[] components = PatternTextbox.Text.Split(' ');
                byte[] pattern;

                if(components.Length == 0) {
                    step.Pattern = null;
                    return;
                }

                pattern = new byte[components.Length];
                for(int i = 0; i < components.Length; i++) {
                    if(byte.TryParse(components[i], out pattern[i]) == false) {
                        PatternTextbox.Focus();
                        PatternTextbox.SelectAll();
                        ErrorTooltip.Show("Invalid pattern entered.", PatternTextbox, 3000);
                        e.Cancel = true;
                        return;
                    }
                }

                step.Pattern = pattern;
            }
        }

        private void NameTextbox_Validating(object sender, CancelEventArgs e) {
            if(NameTextbox.Text.Trim() == string.Empty) {
                PatternTextbox.Focus();
                PatternTextbox.SelectAll();
                ErrorTooltip.Show("Invalid name entered.", PatternTextbox, 3000);
                e.Cancel = true;
            }
            else {
                activeMethod.Name = NameTextbox.Text.Trim();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            AddStep();
        }

        private void AddStep() {
            Debug.AssertNotNull(activeMethod, "Active method not set");
            activeMethod.Steps.Add(new RandomWipeStep(activeMethod.Steps.Count));
            PopulateStepList();

            // select the added step
            SelectStep(activeMethod.Steps.Count - 1, true);
            methodModified = true;
            HandleMethodModified();
            SetShuffleMinMax();
        }

        private void RandomOptionbox_CheckedChanged(object sender, EventArgs e) {
            if(loadingSteps == true) {
                return;
            }

            PatternOptionbox.Checked = false;
            RandombyteOptionbox.Checked = false;
            ComplementOptionbox.Checked = false;

            activeStep = new RandomWipeStep(activeStep.Number);
            activeMethod.Steps[activeStepIndex] = activeStep;
            UpdateStepName(activeStepIndex);
            methodModified = true;
            HandleMethodModified();
        }

        private void RandombyteOptionbox_CheckedChanged(object sender, EventArgs e) {
            if(loadingSteps == true) {
                return;
            }

            PatternOptionbox.Checked = false;
            RandomOptionbox.Checked = false;
            ComplementOptionbox.Checked = false;

            activeStep = new RandomByteStep(activeStep.Number);
            activeMethod.Steps[activeStepIndex] = activeStep;
            UpdateStepName(activeStepIndex);
            methodModified = true;
            HandleMethodModified();
        }

        private void ComplementOptionbox_CheckedChanged(object sender, EventArgs e) {
            if(loadingSteps == true) {
                return;
            }

            PatternOptionbox.Checked = false;
            RandombyteOptionbox.Checked = false;
            RandomOptionbox.Checked = false;

            activeStep = new ComplementStep(activeStep.Number);
            activeMethod.Steps[activeStepIndex] = activeStep;
            UpdateStepName(activeStepIndex);
            methodModified = true;
            HandleMethodModified();
        }

        private void toolStripSplitButton5_ButtonClick(object sender, EventArgs e) {
            RemoveSelectedSteps();
        }

        private void RemoveSelectedSteps() {
            while(StepList.SelectedIndices.Count > 0) {
                activeMethod.Steps.RemoveAt(StepList.SelectedIndices[0]);
                StepList.Items.RemoveAt(StepList.SelectedIndices[0]);
            }

            SetStepNumbers();
            methodModified = true;
            HandleMethodModified();
            PopulateStepList();

            if(StepList.Items.Count > 0) {
                SelectStep(0, true);
            }

            SetShuffleMinMax();
        }

        private void RemoveAllSteps() {
            activeMethod.Steps.Clear();
            StepList.Items.Clear();
            SetShuffleMinMax();
        }

        private void SetStepNumbers() {
            for(int i = 0; i < activeMethod.Steps.Count; i++) {
                activeMethod.Steps[i].Number = i;
            }
        }

        private void removeSelectedToolStripMenuItem2_Click(object sender, EventArgs e) {
            RemoveSelectedSteps();
        }

        private string GetStepName(int index) {
            string name = ((int)(index + 1)).ToString() + " (";

            switch(activeMethod.Steps[index].Type) {
                case WipeStepType.Pattern: {
                        name += "Pattern)";
                        break;
                    }
                case WipeStepType.Random: {
                        name += "Random)";
                        break;
                    }
                case WipeStepType.RandomByte: {
                        name += "Random Byte)";
                        break;
                    }
                case WipeStepType.Complement: {
                        name += "Complement)";
                        break;
                    }
            }

            return name;
        }

        private void UpdateStepName(int index) {
            StepList.Items[index] = GetStepName(index);
        }

        private void toolStripButton7_Click(object sender, EventArgs e) {
            if(StepList.SelectedIndices.Count == 1) {
                int index = StepList.SelectedIndices[0];

                if(index > 0) {
                    WipeStepBase a = activeMethod.Steps[index];
                    WipeStepBase b = activeMethod.Steps[index - 1];

                    a.Number = index - 1;
                    b.Number = index;

                    activeMethod.Steps[index - 1] = a;
                    activeMethod.Steps[index] = b;

                    UpdateStepName(index - 1);
                    UpdateStepName(index);
                    SelectStep(index - 1, true);
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e) {
            if(StepList.SelectedIndices.Count == 1) {
                int index = StepList.SelectedIndices[0];

                if(index < StepList.Items.Count - 1) {
                    WipeStepBase a = activeMethod.Steps[index];
                    WipeStepBase b = activeMethod.Steps[index + 1];

                    a.Number = index + 1;
                    b.Number = index;

                    activeMethod.Steps[index + 1] = a;
                    activeMethod.Steps[index] = b;

                    UpdateStepName(index + 1);
                    UpdateStepName(index);
                    SelectStep(index + 1, true);
                }
            }
        }

        private void SetShuffleMinMax() {
            ShuffleFirstTextbox.Minimum = 1;
            ShuffleFirstTextbox.Maximum = StepList.Items.Count;

            ShuffleLastTextbox.Minimum = 1;
            ShuffleLastTextbox.Maximum = StepList.Items.Count;

            ShuffleCheckbox.Enabled = ShuffleLastTextbox.Maximum >= 2;
            ShuffleCheckbox.Checked = ShuffleCheckbox.Checked && ShuffleCheckbox.Enabled;
        }

        private void ShuffleFirstTextbox_Validating(object sender, CancelEventArgs e) {
            if(ShuffleFirstTextbox.Value <= ShuffleLastTextbox.Value) {
                activeMethod.ShuffleFirst = (int)ShuffleFirstTextbox.Value - 1;
                methodModified = true;
                HandleMethodModified();
            }
            else {
                ShuffleFirstTextbox.Focus();
                ErrorTooltip.Show("Invalid number. Shuffle First must be smaller than Shuffle Last.", 
                                  ShuffleFirstTextbox, 3000);
                e.Cancel = true;
            }
        }

        private void ShuffleLastTextbox_Validating(object sender, CancelEventArgs e) {
            if(ShuffleLastTextbox.Value >= ShuffleFirstTextbox.Value) {
                activeMethod.ShuffleLast = (int)ShuffleLastTextbox.Value - 1;
                methodModified = true;
                HandleMethodModified();
            }
            else {
                ShuffleLastTextbox.Focus();
                ErrorTooltip.Show("Invalid number. Shuffle Last must be greater than Shuffle First.", 
                                  ShuffleLastTextbox, 3000);
                e.Cancel = true;
            }
        }

        private void removeAllToolStripMenuItem2_Click(object sender, EventArgs e) {
            RemoveAllSteps();
        }

        private void removeSelectedToolStripMenuItem1_Click(object sender, EventArgs e) {
            RemoveSelectedMethods();
        }

        private void RemoveSelectedMethods() {
            while(MethodList.SelectedIndices.Count > 0) {
                _methodManager.RemoveMethod(_methodManager.Methods[MethodList.SelectedIndices[0]], true);
                MethodList.Items.RemoveAt(MethodList.SelectedIndices[0]);
            }

            LoadMethods();
            if(MethodList.Items.Count == 0) {
                _selectedMethod = -1;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            AddWipeMethod();
        }

        private void AddWipeMethod() {
            activeMethod = _methodManager.CreateMethod();
            _methodManager.SaveMethod(activeMethod);
            activeMethodIndex = _methodManager.Methods.Count - 1;
            LoadMethods();
            MethodList.SelectedIndices.Add(activeMethodIndex);
            ShowMethodOptions(activeMethodIndex);
            UpdateMethodCountLabel();
        }

        private void UpdateMethodCountLabel() {
            MethodCountLabel.Text = "Wipe Methods: " + MethodList.Items.Count.ToString();
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e) {
            RemoveSelectedMethods();
        }

        private void removeAllToolStripMenuItem1_Click(object sender, EventArgs e) {
            RemoveAllMethods();
        }

        private void RemoveAllMethods() {
            while(_methodManager.Methods.Count > 0) {
                _methodManager.RemoveMethod(_methodManager.Methods[0], true);
                _methodManager.Methods.RemoveAt(0);
            }

            MethodList.Items.Clear();
            UpdateMethodCountLabel();
            _selectedMethod = -1;
        }

        private void SelectMethod() {
            if(MethodList.SelectedIndices.Count > 0) {
                SelectMethod(MethodList.SelectedIndices[0]);
                this.Close();
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e) {
            SelectMethod();
        }

        private void MethodList_SelectedIndexChanged(object sender, EventArgs e) {
            SelectButton.Enabled = _showSelected && MethodList.SelectedIndices.Count > 0;
        }

        private void MethodList_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(MethodList.SelectedIndices.Count > 0) {
                if(_showSelected) {
                    SelectMethod(MethodList.SelectedIndices[0]);
                    this.Close();
                }
                else {
                    ShowMethodOptions(MethodList.SelectedIndices[0]);
                }
            }
        }

        private void ExportMethods() {
            ImportExport dialog = new ImportExport();
            dialog.EnterExportMode(ImportExport.ExporPanelTab.Methods);

            dialog.Options = _options;
            dialog.ShowDialog();
        }

        private void ImportMethods() {
            ImportExport dialog = new ImportExport();
            dialog.EnterImportMode();

            dialog.Options = _options;
            dialog.ShowDialog();
            LoadMethods();
        }

        private void WipeMethods_FormClosing(object sender, FormClosingEventArgs e) {
            SecureDeleteWinForms.Properties.Settings.Default.PositionManager.SavePosition(this);

            if(optionsVisible == true) {
                HideOptionsPanel();
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void toolStripButton9_ButtonClick(object sender, EventArgs e) {
            ImportMethods();
        }

        private void selectMethodToolStripMenuItem_Click(object sender, EventArgs e) {
            SelectMethod();
        }

        private void editMethodToolStripMenuItem_Click(object sender, EventArgs e) {
            EditSelectedMethod();
        }

        private void newMethodToolStripMenuItem_Click(object sender, EventArgs e) {
            AddWipeMethod();
        }

        private void selectedMethodsToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveSelectedMethods();
        }

        private void allMethodsToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveAllMethods();
        }

        private void importMethodsToolStripMenuItem1_Click(object sender, EventArgs e) {
        }

        private void importMethodsToolStripMenuItem2_Click(object sender, EventArgs e) {
            ImportMethods();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            for(int i = 0; i < MethodList.Items.Count; i++) {
                MethodList.Items[i].Selected = true;
            }
        }

        private void toolStripButton9_Click_2(object sender, EventArgs e) {

        }

        private void toolStripSplitButton4_Click(object sender, EventArgs e) {
            ExportMethods();
        }
    }
}
