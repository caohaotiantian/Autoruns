using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TaskScheduler;

namespace Autoruns
{
    class TaskScheduleStartups : Startups
    {
        private TaskSchedulerClass taskScheduler = new TaskSchedulerClass();

        public TaskScheduleStartups(ListView _listView)
        {
            listView = _listView;
            AddHeaderKeyEntry();
            taskScheduler.Connect();
            GetAllTasksUnderTaskFolder(@"\");
        }

        private void GetAllTasksUnderTaskFolder(string folderPath)
        {
            ITaskFolder taskFolder = taskScheduler.GetFolder(folderPath);

            var taskList = taskFolder.GetTasks(0);

            

            foreach (IRegisteredTask task in taskList)
            {
                ParseXML(task.Xml, folderPath + task.Name);
            }

            var subTaskFolders = taskFolder.GetFolders(0);
            foreach(ITaskFolder task in subTaskFolders)
            {
                string folderName = task.Path;
                GetAllTasksUnderTaskFolder(folderName);
            }
        }

        private void ParseXML(string xml, string entryName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if ("Actions" == node.Name.ToString())
                {
                    foreach (XmlNode inode in node.ChildNodes)
                    {
                        string target = inode.InnerText;
                        TryTarget(target, entryName);
                    }

                }
            }
        }

        private void TryTarget(string t, string entryName)
        {
            string target = GetValueContentAsPath(t);
            target = GetFilePathUnderSystemPath(target);
            if (!target.Contains(".exe")) return;
            FileInfo f = new FileInfo(target);
            AddStartupEntry(false,
                    entryName,
                    GetFileDescription(target),
                    GetFilePublisher(target),
                    target,
                    f.LastWriteTime);
        }

        private void AddHeaderKeyEntry()
        {
            StartupEntry localStartupEntry = new StartupEntry(true,
                "Task Scheduler",
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
            localStartupEntry.IsEmpty = false;
            starupEntrys.Add(localStartupEntry);
        }
    }
}
