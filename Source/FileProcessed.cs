using System;

namespace FilesSearching.Source {
    class FileProcessed : EventArgs {
        private readonly String fname;
        public FileProcessed(String filename) {
            fname = filename;
        }
        public String FileName { 
            get { 
                return fname; 
            } 
        }
    }
}