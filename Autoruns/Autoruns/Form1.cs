using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Autoruns
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitListView();
            FillInListView();
        }

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            // In order to make the Reg Keys and Startup Folders stand out across 
            // multipule columns, we need to draw it by ourselves instead of using
            // the default draw funcions.
            listView1.OwnerDraw = true;
            listView1.DrawItem += 
                new DrawListViewItemEventHandler(listView1_DrawItem);
            listView1.DrawSubItem += 
                new DrawListViewSubItemEventHandler(listView1_DrawSubItem);
            listView1.DrawColumnHeader += 
                new DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);

            // Add five header columns
            listView1.Columns.Add("Autorun Entry", 320, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 160, HorizontalAlignment.Left);
            listView1.Columns.Add("Publisher", 240, HorizontalAlignment.Left);
            listView1.Columns.Add("Image Path", 480, HorizontalAlignment.Left);
            listView1.Columns.Add("Timestamp", 140, HorizontalAlignment.Left);
        }

        private void FillInListView()
        {
            string USERPROFILE = Environment.GetEnvironmentVariable("USERPROFILE");
            LoadStartupDir(new DirectoryInfo(USERPROFILE + "\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"));
            LoadRegEntry("HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run");
        }
        private void LoadRegEntry(string keyPath)
        {
            int firstSlash = keyPath.IndexOf('\\');
            string rootKey = keyPath.Substring(0, firstSlash);
            string regEntryName = keyPath.Substring(firstSlash + 1);
            RegistryKey key;
            switch (rootKey)
            {
                case "HKLM":
                    key = Registry.LocalMachine;
                    break;
                case "HKCU":
                    key = Registry.CurrentUser;
                    break;
                default:
                    return;
            }
            RegistryKey subkey = key.OpenSubKey(regEntryName, true);
            
            listView1.BeginUpdate();
            // add reg entry
            AddContainerItem(keyPath, DateTime.Now);
            
            string [] valuenames = subkey.GetValueNames();
            foreach(string valuename in valuenames)
            {
                string value = subkey.GetValue(valuename).ToString();
                AddAutorunItem(value, "", "", "", DateTime.Now);
            }
            

            listView1.EndUpdate();
        }

        private void LoadStartupDir(DirectoryInfo dir)
        {
            listView1.BeginUpdate();

            // add folder directory
            AddContainerItem(dir.FullName, dir.LastWriteTime);

            // Create an image list and add an image.
            ImageList list = new ImageList();
            list.Images.Add(new Bitmap(typeof(Button), "Button.bmp"));
            // SmallImageList must be set when using IndentCount.
            listView1.SmallImageList = list;

            FileInfo[] subFiles = dir.GetFiles();
            foreach (FileInfo f in subFiles)
            {
                string targetFile = f.FullName;
                if (Path.GetExtension(f.FullName).ToLower() == ".lnk")
                {
                    targetFile = GetShortcutTarget(f.FullName);
                }
                else if (Path.GetExtension(f.FullName).ToLower() == ".ini")
                {
                    continue;
                }
                X509Certificate xcert = X509Certificate.CreateFromSignedFile(targetFile);
                string subject = xcert.Subject;
                X509Certificate2 x2 = new X509Certificate2();
                //x2.Verify();
                string x = xcert.ToString(true);
                string y = xcert.ToString(false);
                int start = subject.IndexOf("CN") + 3;
                int end = subject.IndexOf(",", start);
                string CN = "(Verified) " + subject.Substring(start, end - start);

                // Get the file version for the notepad.
                FileVersionInfo myFileVersionInfo =
                    FileVersionInfo.GetVersionInfo(targetFile);

                // Print the file description.
                string desc = myFileVersionInfo.FileDescription;
                
                AddAutorunItem(f.Name, desc, CN, targetFile, f.LastWriteTime);
            }
            listView1.EndUpdate();

        }

        private void AddContainerItem(string entry, DateTime time)
        {
            ListViewItem item = new ListViewItem(entry, 0);

            // prepare 5 subitems to be appended to an item of listview
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(time.ToString("G"));

            // no indent to mark it as a ContainerItem, instead of an AutorunItem
            item.IndentCount = 0;

            // Do actual add
            listView1.Items.Add(item);
        }

        private void AddAutorunItem(string entry, string desc, string publisher, string path, DateTime time)
        {
            ListViewItem item = new ListViewItem(entry, 0);
            
            // prepare 5 subitems to be appended to an item of listview
            item.SubItems.Add(desc);
            item.SubItems.Add(publisher);
            item.SubItems.Add(path);
            item.SubItems.Add(time.ToString("G"));
            
            // indent to mark it as a AutorunItem, instead of a ContainerItem
            item.IndentCount = 1;
            
            // Do actual add
            listView1.Items.Add(item);
        }


        private void hideMicrosoftEntrysToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Draws the backgrounds for entire ListView items.
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.IndentCount != 0)
                e.DrawDefault = true;
        }

        // Draws subitem text and applies content-based formatting.
        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.IndentCount == 0)
            {
                ListView listView = (ListView)sender;

                // Unless the item is selected, draw the standard background 
               
                if (e.ColumnIndex == 0)
                {
                    Rectangle rowBoundsL = e.Item.SubItems[0].Bounds;
                    Rectangle rowBoundsR = e.Item.SubItems[4].Bounds;
                    Rectangle bounds = new Rectangle(rowBoundsL.Left,
                        rowBoundsL.Top,
                        rowBoundsR.Left - rowBoundsL.Left,
                        rowBoundsR.Height);
                    TextRenderer.DrawText(e.Graphics,
                        e.Item.SubItems[0].Text,
                        listView.Font,
                        bounds,
                        Color.Black,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
                }
                else if (e.ColumnIndex == 4)
                {
                    TextRenderer.DrawText(e.Graphics,
                        e.Item.SubItems[4].Text,
                        listView.Font,
                        e.Item.SubItems[4].Bounds,
                        Color.Black,
                        TextFormatFlags.Left);
                }
            }
            else
                e.DrawDefault = true;
        }

        // Draws column headers.
        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }


        private string GetShortcutTarget(string shortcutFilename)
        {
            if (System.IO.File.Exists(shortcutFilename) && Path.GetExtension(shortcutFilename).ToLower() == ".lnk")
            {
                WshShell shell = new WshShell();
                IWshShortcut IWshShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFilename);
                return IWshShortcut.TargetPath;
            }
            else
                return string.Empty;
        }

        private string GetFileDescription(string file)
        {
            if (System.IO.File.Exists(file)) return "File does not exists.";
            
            try
            {
                // Get the file version for the file.
                FileVersionInfo myFileVersionInfo =
                    FileVersionInfo.GetVersionInfo(file);

                // Print the file description.
                return myFileVersionInfo.FileDescription;
            }
            catch
            {
                return "GetFileDescription() ERROR.";
            }
            
        }

    }
}
