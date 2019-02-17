using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FilesSearching.Source {
     
    class FileSearcher {
        
        private enum FilesSearchingX { Name, Text, NameAndText }
        public Int32 NumFiles { get; set; } 
        public String Directory { get; set; }
        public String FilePattern { get; set; }
        public String TextPattern { get; set; }
        public TimeSpan Time { get; set; }

        
        public event EventHandler<FileProcessed> NewFileProcessed;
        public event EventHandler<FileFound> NewFileFound;

   
        private CancellationTokenSource cts;
        private DateTime beginning;
        private FilesSearchingX fsm;

        
        public async Task StartSearching() {
            await Task.Run(() => { FindFiles(new DirectoryInfo(Directory)); });
        }

        private void FindFiles(DirectoryInfo dir) {
            beginning = DateTime.Now;
            cts = new CancellationTokenSource();
            SetFileSearchingX();
            ProcessDirectories(dir, cts.Token);
        }

        private void ProcessDirectories(DirectoryInfo dir, CancellationToken token) {
            if (token.IsCancellationRequested) {
                return;
            }
            try {
                DirectoryInfo[] subdirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();
                foreach (var subdir in subdirs) {
                    ProcessDirectories(subdir, token);
                }
                foreach (var file in files) {
                    NumFiles++;
                    Time = DateTime.Now.Subtract(beginning);
                    OnNewFileProcessed(new FileProcessed(file.Name));
                    switch (fsm) {
                        case FilesSearchingX.Name: {
                            FileProcessingNameX(file);
                            break;
                        }
                        case FilesSearchingX.Text: {
                            FileProcessingTextX(file);
                            break;
                        }
                        case FilesSearchingX.NameAndText: {
                            FileProcessingNameAndTextX(file);
                            break;
                        }
                        default: break;
                    }
                }
            } catch (Exception) {
            }
        }

        protected virtual void OnNewFileProcessed(FileProcessed e) {
            RaiseEvent(e, ref NewFileProcessed);
        }

        protected virtual void OnNewFileFound(FileFound e) {
            RaiseEvent(e, ref NewFileFound);
        }

        private void RaiseEvent<TEventArgs>(TEventArgs e, ref EventHandler<TEventArgs> eventDelegate) {
            EventHandler<TEventArgs> temp = Volatile.Read(ref eventDelegate);
            if (temp != null) {
                temp(this, e);
            }
        }

        public void Stop() {
            cts.Cancel();
        }

        private void SetFileSearchingX() {
            if (TextPattern == String.Empty) {
                fsm = FilesSearchingX.Name;
            } else if (FilePattern == String.Empty && TextPattern != String.Empty) {
                fsm = FilesSearchingX.Text;
            } else {
                fsm = FilesSearchingX.NameAndText;
            }
        }

        private void FileProcessingNameX(FileInfo file) {
            if (file.Name.Contains(FilePattern)) {
                OnNewFileFound(new FileFound(file.FullName));
            }
        }

        private void FileProcessingTextX(FileInfo fi) {
            String s = File.ReadAllText(fi.FullName);
            if (s.Contains(TextPattern)) {
                OnNewFileFound(new FileFound(fi.FullName));
            }
        }

        private void FileProcessingNameAndTextX(FileInfo fi) {
            if (fi.Name.Contains(FilePattern)) {
                String s = File.ReadAllText(fi.FullName);
                if (s.Contains(TextPattern)) {
                    OnNewFileFound(new FileFound(fi.FullName));
                }
            }
        }
    }
}