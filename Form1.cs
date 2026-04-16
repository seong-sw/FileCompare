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
                MessageBox.Show("복사할 항목을 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sel = srcLv.SelectedItems[0];

            if (sel.Tag is FileInfo fi)
            {
                var srcPath = Path.Combine(srcDir, fi.Name);
                var dstPath = Path.Combine(dstDir, fi.Name);

                try
                {
                    if (File.Exists(dstPath))
                    {
                        var dstFile = new FileInfo(dstPath);
                        if (dstFile.LastWriteTime > fi.LastWriteTime)
                        {
                            var result = MessageBox.Show($"대상 파일이 원본보다 최신입니다. 덮어쓰시겠습니까?\n\n원본: {fi.LastWriteTime:g}\n대상: {dstFile.LastWriteTime:g}", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result != DialogResult.Yes) return;
                        }
                    }

                    File.Copy(srcPath, dstPath, true);
                }
                catch (IOException e)
                {
                    MessageBox.Show($"파일 복사 중 오류가 발생했습니다: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (sel.Tag is DirectoryInfo di)
            {
                var srcPath = Path.Combine(srcDir, di.Name);
                var dstPath = Path.Combine(dstDir, di.Name);

                try
                {
                    if (Directory.Exists(dstPath))
                    {
                        var dstDirInfo = new DirectoryInfo(dstPath);
                        if (dstDirInfo.LastWriteTime > di.LastWriteTime)
                        {
                            var result = MessageBox.Show($"대상 폴더가 원본보다 최신입니다. 덮어쓰시겠습니까?\n\n원본: {di.LastWriteTime:g}\n대상: {dstDirInfo.LastWriteTime:g}", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result != DialogResult.Yes) return;
                        }
                    }

                        DirectoryCopy(srcPath, dstPath, true);
                }
                catch (IOException e)
                {
                    MessageBox.Show($"폴더 복사 중 오류가 발생했습니다: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show($"권한 오류: {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            PopulateListView(lvwLeftDir, srcDir);
            PopulateListView(lvwRightDir, dstDir);
            CompareListViews();
        }

        private void DirectoryCopy(string sourceDir, string destDir, bool overwrite)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) throw new DirectoryNotFoundException(sourceDir);

            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            foreach (var file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                bool doCopy = overwrite;
                if (File.Exists(targetFilePath))
                {
                    var dstInfo = new FileInfo(targetFilePath);

                    if (file.LastWriteTime > dstInfo.LastWriteTime) doCopy = true;
                    else doCopy = overwrite;
                }
                if (doCopy)
                {
                    file.CopyTo(targetFilePath, true);
                }
            }

            foreach (var subdir in dir.GetDirectories())
            {
                string newDestDir = Path.Combine(destDir, subdir.Name);
                if (!Directory.Exists(newDestDir)) Directory.CreateDirectory(newDestDir);
                DirectoryCopy(subdir.FullName, newDestDir, overwrite);
            }
        }
    }
}
