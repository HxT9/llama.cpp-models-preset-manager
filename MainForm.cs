using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using llama.cpp_models_preset_manager.Models;
using System.ComponentModel;
using System.Windows.Forms;

namespace llama.cpp_models_preset_manager
{
    public partial class MainForm : Form
    {
        BindingList<AiModelDTO> aiModelBinding;
        BindingList<AiModelFlagDTO> aiModelFlagBinding;

        ContextMenuStrip contextMenuStripAiModel;
        ContextMenuStrip contextMenuStripAiModelFlag;

        private ComboBox flagSelectorComboBox;

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
            dataGridViewAiModel.CurrentCellChanged += DataGridViewAiModel_CurrentCellChanged;
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

        private void DataGridViewAiModel_CurrentCellChanged(object? sender, EventArgs e)
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
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Select GGUF";
                    dialog.Filter = "GGUF files (*.gguf)|*.gguf";
                    dialog.InitialDirectory = ServiceModel.Instance.GetKV("LastGGUFDirectory") ?? Environment.CurrentDirectory;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = dialog.FileName;

                        dataGridViewAiModel.Rows[e.RowIndex].Cells["Path"].Value = filePath;

                        if (string.IsNullOrWhiteSpace((string)dataGridViewAiModel.Rows[e.RowIndex].Cells["Name"].Value))
                            dataGridViewAiModel.Rows[e.RowIndex].Cells["Name"].Value = ModelScanner.getNameFromGGUFPath(filePath);

                        dataGridViewAiModel.NotifyCurrentCellDirty(true);

                        ServiceModel.Instance.SaveKV("LastGGUFDirectory", new FileInfo(filePath).Directory?.FullName ?? "");
                    }
                }
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
                    }
                    catch (Exception ex)
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

            dataGridViewAiModel.DataSource = aiModelBinding;

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
            dataGridViewAiModelFlag.KeyUp += DataGridViewAiModelFlag_KeyUp;

            {
                contextMenuStripAiModelFlag = new ContextMenuStrip();
                var deleteItem = new ToolStripMenuItem("Delete Selected Row");
                deleteItem.Click += ContextAiModelFlagDelete;
                var deleteAll = new ToolStripMenuItem("Delete All Flags");
                deleteAll.Click += ContextAiModelFlagDeleteAll;
                contextMenuStripAiModelFlag.Items.Add(deleteItem);
                contextMenuStripAiModelFlag.Items.Add(deleteAll);
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
                flagSelectorComboBox.Visible = true;
                flagSelectorComboBox.DataSource = ServiceModel.Instance.GetFlags();
                flagSelectorComboBox.DisplayMember = "Name";
                flagSelectorComboBox.ValueMember = "Name";
                flagSelectorComboBox.Tag = e.RowIndex;

                var cellRect = dataGridViewAiModelFlag.GetCellDisplayRectangle(dataGridViewAiModelFlag.Columns["Flag"].Index, e.RowIndex, true);

                flagSelectorComboBox.SetBounds(
                    cellRect.X,
                    cellRect.Y,
                    cellRect.Width,
                    flagSelectorComboBox.Height
                );

                flagSelectorComboBox.DroppedDown = true;
            }
            else if (dataGridViewAiModelFlag.Columns[e.ColumnIndex].Name == "Browse")
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Select File";
                    dialog.Filter = "All files (*.*)|*.*";

                    if (dataGridViewAiModel.CurrentRow != null)
                    {
                        string? path = (dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO)?.Path;
                        dialog.InitialDirectory = new FileInfo(path).Directory?.FullName ?? "";
                    }

                    if (string.IsNullOrWhiteSpace(dialog.InitialDirectory))
                        dialog.InitialDirectory = ServiceModel.Instance.GetKV("LastGGUFDirectory") ?? Environment.CurrentDirectory;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = dialog.FileName;

                        var cell = dataGridViewAiModelFlag.Rows[e.RowIndex].Cells["FlagValue"];
                        dataGridViewAiModelFlag.CurrentCell = cell;
                        dataGridViewAiModelFlag.NotifyCurrentCellDirty(true);
                        cell.Value = filePath;
                        dataGridViewAiModelFlag.EndEdit(DataGridViewDataErrorContexts.Commit);
                        dataGridViewAiModelFlag.NotifyCurrentCellDirty(false);
                    }
                }
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
                    if (f.Id == 0)
                        f.AiModelId = m.Id;

                    dataGridViewAiModelFlag.Rows[e.RowIndex].Cells["Flag"].ErrorText = null;
                    if (string.IsNullOrWhiteSpace(f.Flag))
                    {
                        dataGridViewAiModelFlag.Rows[e.RowIndex].Cells["Flag"].ErrorText = "Missing value";
                        return;
                    }

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

        private void DataGridViewAiModelFlag_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (dataGridViewAiModelFlag.CurrentCell != null)
                {
                    int i = 0;
                    DataGridViewCell nextCell = dataGridViewAiModelFlag.CurrentCell;

                    int nextColumnIncrement = e.KeyData.HasFlag(Keys.Shift) ? -1 : 1;

                    int nextCellIndex = 0;
                    int nextRowIndex = 0;

                    do
                    {
                        nextCellIndex = nextCell.ColumnIndex + nextColumnIncrement;
                        nextRowIndex = nextCell.RowIndex;

                        if (nextCellIndex < 0)
                        {
                            nextCellIndex = dataGridViewAiModelFlag.ColumnCount - 1;
                            nextRowIndex--;
                        }

                        if (nextCellIndex >= dataGridViewAiModelFlag.ColumnCount)
                        {
                            nextCellIndex = 0;
                            nextRowIndex++;
                        }

                        if (nextRowIndex >= 0 && nextRowIndex < dataGridViewAiModelFlag.RowCount)
                            nextCell = dataGridViewAiModelFlag
                                .Rows[nextRowIndex]
                                .Cells[nextCellIndex];
                    } while (nextRowIndex >= 0 && nextRowIndex < dataGridViewAiModelFlag.RowCount && nextCell.ReadOnly);

                    dataGridViewAiModelFlag.CurrentCell = nextCell;
                    e.Handled = true;
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

        private void ContextAiModelFlagDeleteAll(object? sender, EventArgs e)
        {
            if (dataGridViewAiModel.CurrentRow != null)
            {
                var item = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
                if (item == null) return;
                try
                {
                    ServiceModel.Instance.DeleteAllAiModelFlag(item);
                    aiModelFlagBinding.Clear();
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

            dataGridViewAiModelFlag.DataSource = aiModelFlagBinding;

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
                Width = 20,
                ReadOnly = true
            };
            dataGridViewAiModelFlag.Columns.Add(flagDropDown);

            flagSelectorComboBox = new ComboBox()
            {
                Visible = false,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            flagSelectorComboBox.SelectionChangeCommitted += FlagSelectorComboBox_SelectionChangeCommitted;
            flagSelectorComboBox.DropDownClosed += (s, e) => { flagSelectorComboBox.Visible = false; };
            dataGridViewAiModelFlag.Controls.Add(flagSelectorComboBox);

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
                Width = 30,
                ReadOnly = true
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

        private void FlagSelectorComboBox_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (flagSelectorComboBox.SelectedItem != null && flagSelectorComboBox.Tag is int rowIndex)
            {
                dataGridViewAiModelFlag.Focus();

                flagSelectorComboBox.Visible = false;

                var selectedFlag = flagSelectorComboBox.SelectedItem as FlagDTO;
                if (selectedFlag == null) return;

                if (dataGridViewAiModel.CurrentRow == null) return;
                var model = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
                if (model == null) return;

                var cell = dataGridViewAiModelFlag.Rows[rowIndex].Cells["Flag"];
                dataGridViewAiModelFlag.CurrentCell = cell;
                dataGridViewAiModelFlag.NotifyCurrentCellDirty(true);
                cell.Value = selectedFlag.Name;
                dataGridViewAiModelFlag.EndEdit(DataGridViewDataErrorContexts.Commit);
                dataGridViewAiModelFlag.NotifyCurrentCellDirty(false);
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

            ModelScanner.ScanAndAddModels(folder);
            LoadAiModels();
        }

        private void flagListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form f in fc)
            {
                if (f is FlagForm)
                {
                    f.BringToFront();
                    return;
                }
            }

            FlagForm flagForm = new FlagForm();
            flagForm.Show();
        }

        private void exportModelspresetConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewAiModelFlag.CurrentCell != null)
            {
                dataGridViewAiModelFlag.EndEdit();
                DataGridViewAiModelFlag_RowValidated(null, new DataGridViewCellEventArgs(dataGridViewAiModelFlag.CurrentCell.ColumnIndex, dataGridViewAiModelFlag.CurrentCell.RowIndex));
            }

            string? file = null;
            using (var dialog = new SaveFileDialog())
            {
                string? initialFile = ServiceModel.Instance.GetKV("ConfigFile");
                dialog.Filter = "Config file (*.ini)|*.ini|All files (*.*)|*.*";
                if (initialFile != null)
                {
                    if (File.Exists(initialFile))
                    {
                        var fi = new FileInfo(initialFile);
                        dialog.InitialDirectory = fi.Directory?.FullName;
                        dialog.FileName = fi.Name;
                    }
                    else
                    {
                        var dir = ServiceModel.Instance.GetKV("ConfigDirectory");
                        if (Directory.Exists(dir))
                            dialog.InitialDirectory = dir;
                        else
                            dialog.InitialDirectory = Environment.CurrentDirectory;
                        dialog.FileName = "llama-server-config.ini";
                    }
                }
                else
                {
                    dialog.InitialDirectory = Environment.CurrentDirectory;
                    dialog.FileName = "llama-server-config.ini";
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    file = dialog.FileName;
                    ServiceModel.Instance.SaveKV("ConfigFile", file ?? "");
                    ServiceModel.Instance.SaveKV("ConfigDirectory", new FileInfo(file ?? "").Directory?.FullName ?? "");
                }
            }

            if (string.IsNullOrWhiteSpace(file))
                return;

            ConfigExporter.Export(file);
        }
    }
}