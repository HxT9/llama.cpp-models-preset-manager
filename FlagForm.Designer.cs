namespace llama.cpp_models_preset_manager
{
    partial class FlagForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewFlags = new DataGridView();
            menuStrip1 = new MenuStrip();
            deleteAllSavedFlagsToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFlags).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewFlags
            // 
            dataGridViewFlags.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFlags.Dock = DockStyle.Fill;
            dataGridViewFlags.Location = new Point(0, 24);
            dataGridViewFlags.Name = "dataGridViewFlags";
            dataGridViewFlags.Size = new Size(800, 426);
            dataGridViewFlags.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { deleteAllSavedFlagsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // deleteAllSavedFlagsToolStripMenuItem
            // 
            deleteAllSavedFlagsToolStripMenuItem.Name = "deleteAllSavedFlagsToolStripMenuItem";
            deleteAllSavedFlagsToolStripMenuItem.Size = new Size(128, 20);
            deleteAllSavedFlagsToolStripMenuItem.Text = "Delete all saved flags";
            deleteAllSavedFlagsToolStripMenuItem.Click += deleteAllSavedFlagsToolStripMenuItem_Click;
            // 
            // FlagForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridViewFlags);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FlagForm";
            Text = "Manage Flags";
            ((System.ComponentModel.ISupportInitialize)dataGridViewFlags).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewFlags;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem deleteAllSavedFlagsToolStripMenuItem;
    }
}
