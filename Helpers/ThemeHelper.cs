using System.Drawing;
using System.Windows.Forms;

namespace llama.cpp_models_preset_manager.Helpers
{
    public static class ThemeHelper
    {
        public static void ApplyTheme(Form form)
        {
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            form.BackColor = Color.FromArgb(236, 239, 241);

            foreach (Control control in form.Controls)
            {
                ApplyThemeToControl(control);
            }
        }

        private static void ApplyThemeToControl(Control control)
        {
            if (control is DataGridView dgv)
            {
                StyleDataGridView(dgv);
            }
            else if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.BackColor = Color.FromArgb(236, 239, 241);
            }
            else if (control is SplitContainer split)
            {
                split.BackColor = Color.White;
                ApplyThemeToControl(split.Panel1);
                ApplyThemeToControl(split.Panel2);
            }
            else if (control is Panel panel)
            {
                foreach (Control child in panel.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
            
            if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
        }

        private static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(236, 239, 241);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(69, 90, 100);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(236, 239, 241);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(69, 90, 100);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(236, 239, 241);
            dgv.ColumnHeadersHeight = 40;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(96, 125, 139);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(207, 216, 220);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(236, 239, 241);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(38, 50, 56);
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            dgv.RowHeadersVisible = false;
            dgv.GridColor = Color.FromArgb(207, 216, 220);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col is DataGridViewButtonColumn btnCol)
                {
                    btnCol.FlatStyle = FlatStyle.Flat;
                    btnCol.DefaultCellStyle.BackColor = Color.FromArgb(236, 239, 241);
                    btnCol.DefaultCellStyle.ForeColor = Color.FromArgb(38, 50, 56);
                    btnCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(96, 125, 139);
                    btnCol.DefaultCellStyle.SelectionForeColor = Color.FromArgb(207, 216, 220);
                }
            }
    }
    }
}
