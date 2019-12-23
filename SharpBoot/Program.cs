using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpBoot.Forms;
using SharpBoot.Models;
using SharpBoot.Properties;
using SharpBoot.Utilities;

namespace SharpBoot
{
    public static class Program
    {
        public static string editcode = "";
        public static string fpath = "";

        public static readonly string DirCharStr = Path.DirectorySeparatorChar.ToString();


        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            Utils.ClrTmp(true);

            Utils.CurrentRandom = new Random();

            Settings.Default.PropertyChanged += Default_PropertyChanged;

            if (Settings.Default.AppsXml == "") Settings.Default.AppsXml = Resources.DefaultISOs;
            ISOInfo.RefreshISOs();


            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);

            Application.ApplicationExit += Application_ApplicationExit;
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            W7RUtils.Install();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

            Utils.ClrTmp();
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Utils.HandleUnhandled((Exception) unhandledExceptionEventArgs.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Utils.HandleUnhandled(e.Exception, "Thread exception");
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Utils.ClrTmp();
        }

        private static void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Settings.Default.Save();
            if (e.PropertyName == "Lang")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
                ISOInfo.RefreshISOs();
            }
        }
    }
}