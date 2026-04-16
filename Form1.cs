namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string FormatSize(long size)
        {
            string[] units = { "byte", "KB", "MB", "GB", "TB" };
            double s = size;
            int i = 0;
            while (s >= 1024 && i < units.Length - 1)
            {
                s /= 1024;
                i++;
            }
            return $"{s:0.##} {units[i]}";
        }

        private void PopulateListView(ListView lv, string dir)
        {
            lv.BeginUpdate();
            lv.Items.Clear();

            try
            {
                var dirs = Directory.EnumerateDirectories(dir)
                    .Select(p => new DirectoryInfo(p))
                    .OrderBy(p => p.Name);

                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);
                    item.SubItems.Add("<DIR>");
                    item.SubItems.Add(d.LastWriteTime.ToString("g"));
                    item.Tag = d;
                    lv.Items.Add(item);
                }

                var files = Directory.EnumerateFiles(dir)
                    .Select(p => new FileInfo(p))
                    .OrderBy(p => p.Name);

                foreach (var f in files)
                {
                    var item = new ListViewItem(f.Name);
                    item.SubItems.Add(FormatSize(f.Length));
                    item.SubItems.Add(f.LastWriteTime.ToString("g"));
                    item.Tag = f;
                    lv.Items.Add(item);
                }

                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    lv.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show($"디렉토리를 찾을 수 없습니다: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException e)
            {
                MessageBox.Show($"입출력 오류: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { lv.EndUpdate(); }
        }

        private void FolderDialogButton_Click(TextBox dir, ListView lv)
        {
                using (var dlg = new FolderBrowserDialog())
                {
                    dlg.Description = "폴더를 선택하세요.";
                    if (!string.IsNullOrWhiteSpace(dir.Text) && Directory.Exists(dir.Text))
                    {
                        dlg.SelectedPath = dir.Text;
                    }
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        dir.Text = dlg.SelectedPath;
                        PopulateListView(lv, dlg.SelectedPath);

                        if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text)
                            && !string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                        {
                            CompareListViews();
                        }
                    }
            }
        }

        private void CompareListViews()
        {
            var leftMap = lvwLeftDir.Items.Cast<ListViewItem>().ToDictionary(i => i.Text, StringComparer.OrdinalIgnoreCase);
            var rightMap = lvwRightDir.Items.Cast<ListViewItem>().ToDictionary(i => i.Text, StringComparer.OrdinalIgnoreCase);

            var names = new HashSet<string>(leftMap.Keys, StringComparer.OrdinalIgnoreCase);
            names.UnionWith(rightMap.Keys);

            foreach (var name in names)
            {
                leftMap.TryGetValue(name, out var lvi);
                rightMap.TryGetValue(name, out var rvi);

                if (lvi != null && rvi != null)
                {
                    DateTime ltime = DateTime.MinValue;
                    DateTime rtime = DateTime.MinValue;

                    if (lvi.Tag is FileInfo lf) ltime = lf.LastWriteTime;
                    else if (lvi.Tag is DirectoryInfo ld) ltime = ld.LastWriteTime;

                    if (rvi.Tag is FileInfo rf) rtime = rf.LastWriteTime;
                    else if (rvi.Tag is DirectoryInfo rd) rtime = rd.LastWriteTime;

                    if (ltime == rtime)
                    {
                        lvi.ForeColor = Color.Black;
                        rvi.ForeColor = Color.Black;
                    }
                    else if (ltime > rtime)
                    {
                        lvi.ForeColor = Color.Red;
                        rvi.ForeColor = Color.Gray;
                    }
                    else
                    {
                        lvi.ForeColor = Color.Gray;
                        rvi.ForeColor = Color.Red;
                    }
                }
                else if (lvi != null)
                {
                    lvi.ForeColor = Color.Purple;
                }
                else if (rvi != null)
                {
                    rvi.ForeColor = Color.Purple;
                }
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            FolderDialogButton_Click(txtLeftDir, lvwLeftDir);
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            FolderDialogButton_Click(txtRightDir, lvwRightDir);
        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {

        }
    }
}
