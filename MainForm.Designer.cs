namespace llama.cpp_models_preset_manager
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            dataGridViewAiModel = new DataGridView();
            dataGridViewAiModelFlag = new DataGridView();
            menuStrip1 = new MenuStrip();
            actionsToolStripMenuItem = new ToolStripMenuItem();
            lookupFromFolderToolStripMenuItem = new ToolStripMenuItem();
            resetModelsToolStripMenuItem = new ToolStripMenuItem();
            flagListToolStripMenuItem = new ToolStripMenuItem();
            exportModelspresetConfigToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModelFlag).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridViewAiModel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridViewAiModelFlag);
            splitContainer1.Size = new Size(1184, 737);
            splitContainer1.SplitterDistance = 745;
            splitContainer1.TabIndex = 0;
            // 
            // dataGridViewAiModel
            // 
            dataGridViewAiModel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAiModel.Dock = DockStyle.Fill;
            dataGridViewAiModel.Location = new Point(0, 0);
            dataGridViewAiModel.Name = "dataGridViewAiModel";
            dataGridViewAiModel.Size = new Size(745, 737);
            dataGridViewAiModel.TabIndex = 0;
            // 
            // dataGridViewAiModelFlag
            // 
            dataGridViewAiModelFlag.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAiModelFlag.Dock = DockStyle.Fill;
            dataGridViewAiModelFlag.Location = new Point(0, 0);
            dataGridViewAiModelFlag.Name = "dataGridViewAiModelFlag";
            dataGridViewAiModelFlag.Size = new Size(435, 737);
            dataGridViewAiModelFlag.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { actionsToolStripMenuItem, flagListToolStripMenuItem, exportModelspresetConfigToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1184, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            actionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lookupFromFolderToolStripMenuItem, resetModelsToolStripMenuItem });
            actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            actionsToolStripMenuItem.Size = new Size(59, 20);
            actionsToolStripMenuItem.Text = "Actions";
            // 
            // lookupFromFolderToolStripMenuItem
            // 
            lookupFromFolderToolStripMenuItem.Name = "lookupFromFolderToolStripMenuItem";
            lookupFromFolderToolStripMenuItem.Size = new Size(180, 22);
            lookupFromFolderToolStripMenuItem.Text = "Lookup from folder";
            // 
            // resetModelsToolStripMenuItem
            // 
            resetModelsToolStripMenuItem.Name = "resetModelsToolStripMenuItem";
            resetModelsToolStripMenuItem.Size = new Size(180, 22);
            resetModelsToolStripMenuItem.Text = "Reset models";
            resetModelsToolStripMenuItem.Click += resetModelsToolStripMenuItem_Click;
            // 
            // flagListToolStripMenuItem
            // 
            flagListToolStripMenuItem.Name = "flagListToolStripMenuItem";
            flagListToolStripMenuItem.Size = new Size(59, 20);
            flagListToolStripMenuItem.Text = "Flag list";
            // 
            // exportModelspresetConfigToolStripMenuItem
            // 
            exportModelspresetConfigToolStripMenuItem.Name = "exportModelspresetConfigToolStripMenuItem";
            exportModelspresetConfigToolStripMenuItem.Size = new Size(169, 20);
            exportModelspresetConfigToolStripMenuItem.Text = "Export models-preset config";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "llama.cpp models-preset manager";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModel).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModelFlag).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView dataGridViewAiModel;
        private DataGridView dataGridViewAiModelFlag;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem actionsToolStripMenuItem;
        private ToolStripMenuItem lookupFromFolderToolStripMenuItem;
        private ToolStripMenuItem resetModelsToolStripMenuItem;
        private ToolStripMenuItem flagListToolStripMenuItem;
        private ToolStripMenuItem exportModelspresetConfigToolStripMenuItem;
    }
}
