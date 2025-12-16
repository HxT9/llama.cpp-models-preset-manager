using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using System.ComponentModel;

namespace llama.cpp_models_preset_manager
{
    public partial class MainForm : Form
    {
        BindingList<AiModelDTO> aiModelBinding;
        BindingList<AiModelFlagDTO> aiModelFlagBinding;

        ContextMenuStrip contextMenuStripAiModel;
        ContextMenuStrip contextMenuStripAiModelFlag;

        public MainForm()
        {
            InitializeComponent();

            Load += MainForm_Load;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            ThemeHelper.ApplyTheme(this);

            InitAiModelGrid();
            InitAiModelFlagGrid();
            LoadAiModels();
        }

        private void InitAiModelGrid()
        {
            dataGridViewAiModel.MultiSelect = false;
            dataGridViewAiModel.DataBindingComplete += DataGridViewAiModel_DataBindingComplete;
            dataGridViewAiModel.SelectionChanged += DataGridViewAiModel_SelectionChanged;
            dataGridViewAiModel.Resize += DataGridViewAiModel_Resize;
            dataGridViewAiModel.CellClick += DataGridViewAiModel_CellClick;
            dataGridViewAiModel.CellMouseDown += DataGridViewAiModel_CellMouseDown;
            dataGridViewAiModel.RowValidated += DataGridViewAiModel_RowValidated;

            {
                contextMenuStripAiModel = new ContextMenuStrip();
                var deleteItem = new ToolStripMenuItem("Delete Selected Row");
                deleteItem.Click += ContextAiModelDelete;
                contextMenuStripAiModel.Items.Add(deleteItem);
                dataGridViewAiModel.ContextMenuStrip = contextMenuStripAiModel;
            }
        }

        private void DataGridViewAiModel_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (aiModelBinding.Count > 0)
            {
                LoadAiModelFlags(aiModelBinding[0]);
            }
        }

        private void DataGridViewAiModel_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridViewAiModel.CurrentRow != null)
            {
                AiModelDTO? m = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
                if (m != null)
                {
                    LoadAiModelFlags(m);
                    return;
                }
            }
            LoadAiModelFlags(null);
        }

        private void DataGridViewAiModel_Resize(object? sender, EventArgs e)
        {
            ResizeAiModelColumns();
        }

        private void DataGridViewAiModel_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridViewAiModel.Columns[e.ColumnIndex].Name == "Browse")
            {

            }
        }

        private void DataGridViewAiModel_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridViewAiModel.ClearSelection();
                dataGridViewAiModel.Rows[e.RowIndex].Selected = true;
                dataGridViewAiModel.CurrentCell = dataGridViewAiModel.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void DataGridViewAiModel_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewAiModel.Rows[e.RowIndex].IsNewRow) return;

            AiModelDTO? m = dataGridViewAiModel.Rows[e.RowIndex].DataBoundItem as AiModelDTO;
            if (m != null)
                try
                {
                    dataGridViewAiModel.Rows[e.RowIndex].Cells["Name"].ErrorText = null;
                    if (string.IsNullOrWhiteSpace(m.Name))
                    {
                        dataGridViewAiModel.Rows[e.RowIndex].Cells["Name"].ErrorText = "Missing value";
                        return;
                    }

                    dataGridViewAiModel.Rows[e.RowIndex].Cells["Path"].ErrorText = null;
                    if (string.IsNullOrWhiteSpace(m.Path))
                    {
                        dataGridViewAiModel.Rows[e.RowIndex].Cells["Path"].ErrorText = "Missing value";
                        return;
                    }

                    try
                    {
                        ServiceModel.Instance.SaveAiModel(m);
                        dataGridViewAiModelFlag.AllowUserToAddRows = true;
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.InnerException);
                        ServiceModel.Instance.UndoChanges();
                        aiModelBinding.Remove(m);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.InnerException);
                }
        }

        private void ContextAiModelDelete(object? sender, EventArgs e)
        {
            if (dataGridViewAiModel.CurrentRow != null)
            {
                var item = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
                if (item == null) return;
                try
                {
                    ServiceModel.Instance.DeleteAiModel(item);
                    aiModelBinding.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LoadAiModels(int selectedRow = 0)
        {
            dataGridViewAiModel.SuspendLayout();

            dataGridViewAiModel.AutoGenerateColumns = false;
            dataGridViewAiModel.Columns.Clear();

            try
            {
                aiModelBinding = new BindingList<AiModelDTO>(ServiceModel.Instance.GetAiModels());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                aiModelBinding = new BindingList<AiModelDTO>();
            }

            dataGridViewAiModel.CellValueChanged -= DataGridViewAiModel_RowValidated;
            dataGridViewAiModel.DataSource = aiModelBinding;
            dataGridViewAiModel.CellValueChanged += DataGridViewAiModel_RowValidated;

            dataGridViewAiModel.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
            });

            dataGridViewAiModel.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Path",
                HeaderText = "Path",
                DataPropertyName = "Path",
            });

            var browseColumn = new DataGridViewButtonColumn
            {
                Name = "Browse",
                HeaderText = "",
                Text = "…",
                UseColumnTextForButtonValue = true,
                Width = 30
            };
            dataGridViewAiModel.Columns.Add(browseColumn);

            ResizeAiModelColumns();

            dataGridViewAiModel.ResumeLayout(true);

            if (dataGridViewAiModel.Rows.Count > 0)
            {
                if (selectedRow > dataGridViewAiModel.Rows.Count - 1) selectedRow = 0;
                LoadAiModelFlags(dataGridViewAiModel.Rows[selectedRow].DataBoundItem as AiModelDTO);
            }
        }

        private void ResizeAiModelColumns()
        {
            try
            {
                dataGridViewAiModel.Columns["Name"].Width = dataGridViewAiModel.ClientSize.Width * 4 / 10;
                dataGridViewAiModel.Columns["Path"].Width = dataGridViewAiModel.ClientSize.Width * 5 / 10;
                dataGridViewAiModel.Columns["Browse"].Width = Math.Min(30, dataGridViewAiModel.ClientSize.Width * 1 / 10);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void InitAiModelFlagGrid()
        {
            dataGridViewAiModelFlag.MultiSelect = false;
            dataGridViewAiModelFlag.Resize += DataGridViewAiModelFlag_Resize;
            dataGridViewAiModelFlag.CellClick += DataGridViewAiModelFlag_CellClick;
            dataGridViewAiModelFlag.CellMouseDown += DataGridViewAiModelFlag_CellMouseDown;
            dataGridViewAiModelFlag.RowValidated += DataGridViewAiModelFlag_RowValidated;

            {
                contextMenuStripAiModelFlag = new ContextMenuStrip();
                var deleteItem = new ToolStripMenuItem("Delete Selected Row");
                deleteItem.Click += ContextAiModelFlagDelete;
                contextMenuStripAiModelFlag.Items.Add(deleteItem);
                dataGridViewAiModelFlag.ContextMenuStrip = contextMenuStripAiModelFlag;
            }

            LoadAiModelFlags(null);
        }

        private void DataGridViewAiModelFlag_Resize(object? sender, EventArgs e)
        {
            ResizeAiModelFlagColumns();
        }

        private void DataGridViewAiModelFlag_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridViewAiModelFlag.Columns[e.ColumnIndex].Name == "FlagDropDown")
            {

            }
            else if (dataGridViewAiModelFlag.Columns[e.ColumnIndex].Name == "Browse")
            {

            }
        }

        private void DataGridViewAiModelFlag_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridViewAiModelFlag.ClearSelection();
                dataGridViewAiModelFlag.Rows[e.RowIndex].Selected = true;
                dataGridViewAiModelFlag.CurrentCell = dataGridViewAiModelFlag.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void DataGridViewAiModelFlag_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridViewAiModelFlag.Rows[e.RowIndex].IsNewRow) return;

            AiModelFlagDTO? f = dataGridViewAiModelFlag.Rows[e.RowIndex].DataBoundItem as AiModelFlagDTO;
            if (f != null && dataGridViewAiModel.CurrentRow != null)
            {
                AiModelDTO? m = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
                if (m != null && m.Id > 0)
                {
                    if (f.AiModelId == 0)
                        f.AiModelId = m.Id;

                    if (f.AiModelId == m.Id)
                        try
                        {
                            ServiceModel.Instance.SaveAiModelFlag(f);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            ServiceModel.Instance.UndoChanges();
                            aiModelFlagBinding.Remove(f);
                        }
                }
            }
        }

        private void ContextAiModelFlagDelete(object? sender, EventArgs e)
        {
            if (dataGridViewAiModelFlag.CurrentRow != null)
            {
                var item = dataGridViewAiModelFlag.CurrentRow.DataBoundItem as AiModelFlagDTO;
                if (item == null) return;
                try
                {
                    ServiceModel.Instance.DeleteAiModelFlag(item);
                    aiModelFlagBinding.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LoadAiModelFlags(AiModelDTO? m)
        {
            dataGridViewAiModelFlag.SuspendLayout();

            dataGridViewAiModelFlag.AutoGenerateColumns = false;
            dataGridViewAiModelFlag.Columns.Clear();

            dataGridViewAiModelFlag.AllowUserToAddRows = false;
            if (m != null && m.Id > 0)
                try
                {
                    dataGridViewAiModelFlag.AllowUserToAddRows = true;
                    aiModelFlagBinding = new BindingList<AiModelFlagDTO>(ServiceModel.Instance.GetAiModelFlags(m));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    aiModelFlagBinding = new BindingList<AiModelFlagDTO>();
                }
            else
                aiModelFlagBinding = new BindingList<AiModelFlagDTO>();

            dataGridViewAiModelFlag.CellValueChanged -= DataGridViewAiModelFlag_RowValidated;
            dataGridViewAiModelFlag.DataSource = aiModelFlagBinding;
            dataGridViewAiModelFlag.CellValueChanged += DataGridViewAiModelFlag_RowValidated;

            dataGridViewAiModelFlag.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Flag",
                HeaderText = "Flag",
                DataPropertyName = "Flag",
            });

            var flagDropDown = new DataGridViewButtonColumn
            {
                Name = "FlagDropDown",
                HeaderText = "",
                Text = "•",
                UseColumnTextForButtonValue = true,
                Width = 20
            };
            dataGridViewAiModelFlag.Columns.Add(flagDropDown);

            dataGridViewAiModelFlag.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FlagValue",
                HeaderText = "Value",
                DataPropertyName = "FlagValue",
            });

            var browseColumn = new DataGridViewButtonColumn
            {
                Name = "Browse",
                HeaderText = "",
                Text = "…",
                UseColumnTextForButtonValue = true,
                Width = 30
            };
            dataGridViewAiModelFlag.Columns.Add(browseColumn);

            ResizeAiModelFlagColumns();

            dataGridViewAiModelFlag.ResumeLayout(true);
        }

        private void ResizeAiModelFlagColumns()
        {
            try
            {
                dataGridViewAiModelFlag.Columns["Flag"].Width = dataGridViewAiModelFlag.ClientSize.Width * 4 / 10;
                dataGridViewAiModelFlag.Columns["FlagDropDown"].Width = Math.Min(30, dataGridViewAiModelFlag.ClientSize.Width * 1 / 10);
                dataGridViewAiModelFlag.Columns["FlagValue"].Width = dataGridViewAiModelFlag.ClientSize.Width * 4 / 10;
                dataGridViewAiModelFlag.Columns["Browse"].Width = Math.Min(30, dataGridViewAiModelFlag.ClientSize.Width * 1 / 10);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message); 
            }
        }

        private void resetModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all models configuration?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ServiceModel.Instance.DeleteAllAiModel();
                aiModelBinding.Clear();
            }
        }

        private void lookupFromFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string? folder = null;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Folder";
                dialog.UseDescriptionForTitle = true;
                dialog.SelectedPath = ServiceModel.Instance.GetKV("GGUFScanDirectory") ?? Environment.CurrentDirectory;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folder = dialog.SelectedPath;
                    ServiceModel.Instance.SaveKV("GGUFScanDirectory", folder);
                }
            }

            if (string.IsNullOrWhiteSpace(folder))
                return;

            List<AiModelDTO> models = ModelScanner.ScanAndAddModels(folder);
            models.ForEach(m => aiModelBinding.Add(m));
        }
    }
}