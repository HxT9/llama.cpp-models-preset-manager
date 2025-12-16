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
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModelFlag).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridViewAiModel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridViewAiModelFlag);
            splitContainer1.Size = new Size(1184, 761);
            splitContainer1.SplitterDistance = 745;
            splitContainer1.TabIndex = 0;
            // 
            // dataGridViewAiModel
            // 
            dataGridViewAiModel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAiModel.Dock = DockStyle.Fill;
            dataGridViewAiModel.Location = new Point(0, 0);
            dataGridViewAiModel.Name = "dataGridViewAiModel";
            dataGridViewAiModel.Size = new Size(745, 761);
            dataGridViewAiModel.TabIndex = 0;
            // 
            // dataGridViewAiModelFlag
            // 
            dataGridViewAiModelFlag.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAiModelFlag.Dock = DockStyle.Fill;
            dataGridViewAiModelFlag.Location = new Point(0, 0);
            dataGridViewAiModelFlag.Name = "dataGridViewAiModelFlag";
            dataGridViewAiModelFlag.Size = new Size(435, 761);
            dataGridViewAiModelFlag.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(splitContainer1);
            Name = "MainForm";
            Text = "llama.cpp models-preset manager";
            Load += MainForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModel).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAiModelFlag).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView dataGridViewAiModel;
        private DataGridView dataGridViewAiModelFlag;
    }
}
