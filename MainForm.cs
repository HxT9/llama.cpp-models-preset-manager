using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using System.ComponentModel;

namespace llama.cpp_models_preset_manager
{
    public partial class MainForm : Form
    {
        BindingList<AiModelDTO> aiModelBinding;
        BindingList<AiModelFlagDTO> aiModelFlagBinding;

        public MainForm()
        {
            InitializeComponent();
            DatabaseManager.init();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ThemeHelper.ApplyTheme(this);

            initAiModelGrid();
            initAiModelFlagGrid();
        }

        private void initAiModelGrid()
        {
            dataGridViewAiModel.MultiSelect = false;
            dataGridViewAiModel.SelectionChanged += DataGridViewAiModel_SelectionChanged;
            dataGridViewAiModel.Resize += DataGridViewAiModel_Resize;
            dataGridViewAiModel.CellClick += DataGridViewAiModel_CellClick;
            dataGridViewAiModel.RowValidated += DataGridViewAiModel_RowValidated;

            using (var contextMenu = new ContextMenuStrip())
            {
                var deleteItem = new ToolStripMenuItem("Delete Selected Row");
                deleteItem.Click += (s, e) => DatabaseService.DeleteAiModel((AiModelDTO)dataGridViewAiModel.CurrentRow.DataBoundItem);
                dataGridViewAiModel.ContextMenuStrip = contextMenu;
            }

            LoadAiModels();
        }

        private void DataGridViewAiModel_SelectionChanged(object? sender, EventArgs e)
        {
            AiModelDTO? m = dataGridViewAiModel.CurrentRow.DataBoundItem as AiModelDTO;
            if (m != null)
            {
                LoadAiModelFlags(m);
            }
        }

        private void DataGridViewAiModel_Resize(object? sender, EventArgs e)
        {
            resizeAiModelColumns();
        }

        private void DataGridViewAiModel_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewAiModel.Columns[e.ColumnIndex].Name == "Browse")
            {

            }
        }

        private void DataGridViewAiModel_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            AiModelDTO? m = dataGridViewAiModel.Rows[e.RowIndex].DataBoundItem as AiModelDTO;
            if (m != null) 
                DatabaseService.SaveAiModel(m);
        }

        private void LoadAiModels()
        {
            dataGridViewAiModel.AutoGenerateColumns = false;

            aiModelBinding = new BindingList<AiModelDTO>(DatabaseService.GetAiModels());
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

            resizeAiModelColumns();
        }

        private void resizeAiModelColumns()
        {
            dataGridViewAiModel.Columns["Name"].Width = dataGridViewAiModel.Width * 4 / 10;
            dataGridViewAiModel.Columns["Path"].Width = dataGridViewAiModel.Width * 5 / 10;
            dataGridViewAiModel.Columns["Browse"].Width = Math.Min(30, dataGridViewAiModel.Width * 1 / 10);
        }

        private void initAiModelFlagGrid()
        {
            dataGridViewAiModelFlag.MultiSelect = false;
            dataGridViewAiModelFlag.Resize += DataGridViewAiModelFlag_Resize;
            dataGridViewAiModelFlag.CellClick += DataGridViewAiModelFlag_CellClick;

            using (var contextMenu = new ContextMenuStrip())
            {
                var deleteItem = new ToolStripMenuItem("Delete Selected Row");
                deleteItem.Click += (s, e) => DatabaseService.DeleteAiModelFlag((AiModelFlagDTO)dataGridViewAiModelFlag.CurrentRow.DataBoundItem);
                dataGridViewAiModelFlag.ContextMenuStrip = contextMenu;
            }
        }

        private void DataGridViewAiModelFlag_Resize(object? sender, EventArgs e)
        {
            resizeAiModelFlagColumns();
        }

        private void DataGridViewAiModelFlag_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewAiModelFlag.Columns[e.ColumnIndex].Name == "FlagDropDown")
            {

            }else if (dataGridViewAiModelFlag.Columns[e.ColumnIndex].Name == "Browse")
            {

            }
        }

        private void LoadAiModelFlags(AiModelDTO m)
        {
            dataGridViewAiModelFlag.AutoGenerateColumns = false;

            aiModelFlagBinding = new BindingList<AiModelFlagDTO>(DatabaseService.GetAiModelFlags(m));
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

            resizeAiModelFlagColumns();
        }

        private void resizeAiModelFlagColumns()
        {
            dataGridViewAiModel.Columns["Flag"].Width = dataGridViewAiModel.Width * 4 / 10;
            dataGridViewAiModel.Columns["FlagDropDown"].Width = Math.Min(30, dataGridViewAiModel.Width * 1 / 10);
            dataGridViewAiModel.Columns["FlagValue"].Width = dataGridViewAiModel.Width * 4 / 10;
            dataGridViewAiModel.Columns["Browse"].Width = Math.Min(30, dataGridViewAiModel.Width * 1 / 10);
        }
    }
}
