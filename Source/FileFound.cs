using System;

namespace FilesSearching.Source {
    class FileFound : EventArgs {
        private readonly String fullname;
        public FileFound(String s) { 
            fullname = s; 
        }
        public String FullName { 
            get { 
                return fullname; 
            } 
        }
    }
}