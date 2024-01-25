//using System;
//using System.IO;

//namespace PlantWebApps.Helper
//{
//    public class FileManagement
//    {
//        private FileSystemObject fs;

//        public bool ViewFile(string filePath)
//        {
//            try
//            {
//                fs = new FileSystemObject();

//                bool viewFile = false;

//                if (!File.Exists(filePath))
//                {
//                    MessageBox.Show("File does not exist");
//                    return false;
//                }

//                string efilename = fs.GetFileName(filePath);

//                DialogResult emsg = MessageBox.Show("Do You Want to Save This File?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

//                if (emsg == DialogResult.Yes)
//                {
//                    SaveFile(filePath);
//                    return true;
//                }
//                else if (emsg == DialogResult.No)
//                {
//                    return ViewFile(filePath);
//                }

//                return viewFile;
//            }
//            catch (Exception ex)
//            {
//                // Handle error
//                Console.WriteLine("An error occurred: " + ex.Message);
//                return false;
//            }
//            finally
//            {
//                fs = null;
//            }
//        }

//        private void SaveFile(string filePath)
//        {
//            try
//            {
//                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

//                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
//                {
//                    string destinationFolder = folderBrowserDialog.SelectedPath;
//                    string fileName = Path.GetFileName(filePath);
//                    string destinationPath = Path.Combine(destinationFolder, fileName);

//                    File.Copy(filePath, destinationPath, true);

//                    MessageBox.Show("Saved at " + destinationPath);
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle error
//                Console.WriteLine("An error occurred during file saving: " + ex.Message);
//            }
//        }
//    }
//}
