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
            CopySelectedFile(true);
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            CopySelectedFile(false);
        }

        private void CopySelectedFile(bool fromLeft)
        {
            var srcLv = fromLeft ? lvwLeftDir : lvwRightDir;
            var dstLv = fromLeft ? lvwRightDir : lvwLeftDir;
            var srcDir = fromLeft ? txtLeftDir.Text : txtRightDir.Text;
            var dstDir = fromLeft ? txtRightDir.Text : txtLeftDir.Text;

            if (string.IsNullOrWhiteSpace(srcDir) || !Directory.Exists(srcDir))
            {
                MessageBox.Show("원본 폴더를 먼저 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(dstDir) || !Directory.Exists(dstDir))
            {
                MessageBox.Show("대상 폴더를 먼저 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (srcLv.SelectedItems.Count == 0)
            {
                MessageBox.Show("복사할 파일을 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sel = srcLv.SelectedItems[0];
            if (!(sel.Tag is FileInfo srcFile))
            {
                MessageBox.Show("파일만 복사할 수 있습니다. 폴더는 지원되지 않습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var srcPath = Path.Combine(srcDir, srcFile.Name);
            var dstPath = Path.Combine(dstDir, srcFile.Name);

            try
            {
                if (File.Exists(dstPath))
                {
                    var dstFile = new FileInfo(dstPath);

                    if (dstFile.LastWriteTime > srcFile.LastWriteTime)
                    {
                        var msg = $"대상 파일이 더 최신입니다. 덮어쓰시겠습니까?\n\n원본: {srcFile.LastWriteTime:G}\n대상: {dstFile.LastWriteTime:G}";
                        var dr = MessageBox.Show(msg, "덮어쓰기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr != DialogResult.Yes) return;
                    }
                }

                File.Copy(srcPath, dstPath, true);

                PopulateListView(dstLv, dstDir);
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text)
                    && !string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                {
                    CompareListViews();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"파일 복사 중 오류가 발생했습니다: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
