using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace llama.cpp_models_preset_manager
{
    public partial class FlagForm : Form
    {
        private BindingList<FlagDTO> flagBinding;
        private ContextMenuStrip contextMenuStripFlag;

        public FlagForm()
        {
            InitializeComponent();
            Load += FlagForm_Load;
        }

        private void FlagForm_Load(object? sender, EventArgs e)
        {
            ThemeHelper.ApplyTheme(this);
            InitFlagGrid();
        }

        private void InitFlagGrid()
        {
            dataGridViewFlags.AutoGenerateColumns = false;
            dataGridViewFlags.MultiSelect = false;

            // Events
            dataGridViewFlags.RowValidated += DataGridViewFlags_RowValidated;
            dataGridViewFlags.Resize += DataGridViewFlags_Resize;
            dataGridViewFlags.CellMouseDown += DataGridViewFlags_CellMouseDown;

            // Context Menu
            contextMenuStripFlag = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("Delete Selected Row");
            deleteItem.Click += ContextFlagDelete;
            contextMenuStripFlag.Items.Add(deleteItem);
            dataGridViewFlags.ContextMenuStrip = contextMenuStripFlag;

            LoadFlags();
        }

        private void LoadFlags()
        {
            dataGridViewFlags.SuspendLayout();
            dataGridViewFlags.Columns.Clear();

            try
            {
                flagBinding = new BindingList<FlagDTO>(ServiceModel.Instance.GetFlags());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                flagBinding = new BindingList<FlagDTO>();
            }

            dataGridViewFlags.DataSource = flagBinding;

            dataGridViewFlags.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Flag",
                DataPropertyName = "Name"
            });

            dataGridViewFlags.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description"
            });

            ResizeColumns();
            dataGridViewFlags.ResumeLayout(true);
        }

        private void ResizeColumns()
        {
            try
            {
                dataGridViewFlags.Columns["Name"].Width = dataGridViewFlags.ClientSize.Width * 4 / 10;
                dataGridViewFlags.Columns["Description"].Width = dataGridViewFlags.ClientSize.Width * 6 / 10;
            }
            catch { }
        }

        private void DataGridViewFlags_Resize(object? sender, EventArgs e)
        {
            ResizeColumns();
        }

        private void DataGridViewFlags_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewFlags.Rows[e.RowIndex].IsNewRow) return;

            FlagDTO? f = dataGridViewFlags.Rows[e.RowIndex].DataBoundItem as FlagDTO;
            if (f != null)
            {
                dataGridViewFlags.Rows[e.RowIndex].Cells["Name"].ErrorText = null;
                if (string.IsNullOrWhiteSpace(f.Name))
                {
                    dataGridViewFlags.Rows[e.RowIndex].Cells["Name"].ErrorText = "Missing value";
                    return;
                }

                try
                {
                    ServiceModel.Instance.SaveFlag(f);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.InnerException);
                    ServiceModel.Instance.UndoChanges();
                    flagBinding.Remove(f);
                }
            }
        }

        private void DataGridViewFlags_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridViewFlags.ClearSelection();
                dataGridViewFlags.Rows[e.RowIndex].Selected = true;
                dataGridViewFlags.CurrentCell = dataGridViewFlags.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void ContextFlagDelete(object? sender, EventArgs e)
        {
            if (dataGridViewFlags.CurrentRow != null)
            {
                var item = dataGridViewFlags.CurrentRow.DataBoundItem as FlagDTO;
                if (item == null) return;
                try
                {
                    ServiceModel.Instance.DeleteFlag(item);
                    flagBinding.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void deleteAllSavedFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all models configuration?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ServiceModel.Instance.DeleteAllFlag();
                flagBinding.Clear();
            }
        }
    }
}
