using ImapX.Collections;
using ImapX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp11
{
    internal class ImapHelper
    {
        private static ImapClient client { get; set; }


        public static void Initialize(string host)
        {
            client = new ImapClient(host, true);
            if (!client.Connect())
            {
                throw new Exception("Не удалось подключиться!");
            }
        }
        public static bool Login(string u, string p)
        {
            return client.Login(u, p);
        }
        public static void Logout()
        {
            // Выйти из аккаунта, если он авторизирован.  
            if (client.IsAuthenticated)
            {
                client.Logout();
                client.Dispose();
            }
        }
        public static CommonFolderCollection GetFolders()
        {
            client.Folders.Inbox.StartIdling(); // И продолжить слушать входящие дальше.  
            client.Folders.Inbox.OnNewMessagesArrived += Inbox_OnNewMessagesArrived;
            return client.Folders;
        }
        private static void Inbox_OnNewMessagesArrived(object sender, IdleEventArgs e)
        {
            // Показать сообщение  
            MessageBox.Show($"Пришло новое сообщение в папку {e.Folder.Name}.");
        }
        public static MessageCollection GetMessagesForFolder(string name)
        {
            client.Folders[name].Messages.Download(); // Начать скачивание сообщений;  
            return client.Folders[name].Messages;
        }

    }
}
