﻿using System;
using System.Windows.Forms;
using FilesSearching.Source;

namespace FilesSearching {
    public partial class MainForm : Form {
   
        private FolderBrowserDialog folderBrowserDialog;
        private FileSearcher fileSearcher;
        
       public MainForm() {
            InitializeComponent();
            unVisible();
            SetInitialValuesInTextBoxes();
            fileSearcher = new FileSearcher();
            fileSearcher.NewFileProcessed += NewFileProcessedMsg;
            fileSearcher.NewFileFound += NewFileFoundMsg;
        }

       public void unVisible()
       {
           this.stopButton.Visible = false;
       }

        private void SetInitialValuesInTextBoxes() {
            folderTextBox.Text = Properties.Settings.Default.Folder;
            fileTextBox.Text = Properties.Settings.Default.FileName;
            textInFileTextBox.Text = Properties.Settings.Default.FileContent;
        }

        private void SaveSettingsOfTextBoxes() {
            Properties.Settings.Default.Folder = folderTextBox.Text;
            Properties.Settings.Default.FileName = fileTextBox.Text;
            Properties.Settings.Default.FileContent = textInFileTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void MainFormClosedHandler(object sender, FormClosedEventArgs e) {
            SaveSettingsOfTextBoxes();
        }

        private void NewFileProcessedMsg(Object o, FileProcessed e) {
            qtyFilesLabel.Invoke(new Action<Int32>(num => qtyFilesLabel.Text = num.ToString()), ((FileSearcher)o).NumFiles);
            timeLabel.Invoke(new Action<String>(str => timeLabel.Text = str), ((FileSearcher)o).Time.ToString().Substring(0,11));
            currFileLabel.Invoke(new Action<String>(str => currFileLabel.Text = str), e.FileName);
        }

        private void NewFileFoundMsg(Object o, FileFound e) {
            treeView.Invoke(new Action<String>(str => AddInformationInTreeView(str)), e.FullName);
        }

        private void AddInformationInTreeView(String str) {
            if (treeView.Nodes.Count == 0) {
                WhatToDoWhenThereAreNoFoundFilesInTreeView(str);
            } else {
                WhatToDoIfAtLeastOneFoundFileExistsInTreeView(str);
            }
        }

        private void WhatToDoWhenThereAreNoFoundFilesInTreeView(String str) {
            String[] sArr = str.Split(new Char[] { '\\' });          
            TreeNode currItem = new TreeNode();
            if (sArr.Length > 0) {
                currItem.Text = sArr[0];
                treeView.Nodes.Add(currItem);
                currItem.ExpandAll();
            }
            for (Int32 i = 1; i < sArr.Length; i++) {
                TreeNode newTreeViewItem = new TreeNode();
                newTreeViewItem.Text = sArr[i];
                currItem.Nodes.Add(newTreeViewItem);
                currItem.ExpandAll();
                currItem = newTreeViewItem;                
            }
        }

        private void WhatToDoIfAtLeastOneFoundFileExistsInTreeView(String str) {
            String[] sArr = str.Split(new Char[] { '\\' });
            TreeNode currItem = null;
            foreach (TreeNode node in treeView.Nodes) { currItem = node; }
            for (Int32 i = 1; i < sArr.Length; i++) {
                Boolean isThisItemExist = false;
                foreach (TreeNode item in currItem.Nodes) {
                    if (item.Text == sArr[i]) {
                        isThisItemExist = true;
                        currItem = item;
                        break;
                    }
                }
                if (isThisItemExist) {
                    continue;
                }
                TreeNode newItem = new TreeNode();
                newItem.Text = sArr[i];
                currItem.Nodes.Add(newItem);
                currItem.ExpandAll();
                currItem = newItem;                
            }
        }

        public void Visible()
        {
            this.stopButton.Visible = true;
        }
        private void stopButtonClickHandler(object sender, EventArgs e) {
            fileSearcher.Stop();
            EmptyCurrFileLabel();
        }

        private async void startButtonClickHandler(object sender, EventArgs e) {
            if (folderTextBox.Text == "")
            {
                MessageBox.Show("Пожалуйста, выберете стартовую директорию для поиска", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            Visible();
            EmptyTreeView();
            SetSearchingParameters();
            await fileSearcher.StartSearching();
            EmptyCurrFileLabel();
        }

        private void EmptyTreeView() {
            treeView.Nodes.Clear();
        }

        private void SetSearchingParameters() {
            fileSearcher.Directory = folderTextBox.Text;
            fileSearcher.FilePattern = fileTextBox.Text;
            fileSearcher.TextPattern = textInFileTextBox.Text;
            fileSearcher.NumFiles = 0;
        }

        private void folderButtonClickHandler(object sender, EventArgs e) {
            WorkingWithFolderBrowserDialog();
        }

        private void WorkingWithFolderBrowserDialog() {
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.folderBrowserDialog.Description = "Задайте директорию для поиска файлов.";
            this.folderBrowserDialog.SelectedPath = folderTextBox.Text;
            DialogResult dialogResult = this.folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                folderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void EmptyCurrFileLabel() {
            currFileLabel.Text = String.Empty;
        }
    }
}
