﻿using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using DJ.Resolver;
using NLog;

namespace TestApplication
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string LOREM_IPSUM = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        
        private readonly Logger _Logger = LogManager.GetCurrentClassLogger();
        private readonly Logger _Logger2 = LogManager.GetLogger("Lorem.Ipsum.Foo.Hello.World.Lorem.Ipsum");

        private int _CntMessage;
        private int _CntError;


        public MainWindow()
        {
            Title = $"Testing v{AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName}";
            InitializeComponent();
            DataContext = this;

            NLogViewer1.TimeStampResolver = new FooTimeStampResolver();
            Stopwatch stopwatch = Stopwatch.StartNew();
            Random random = new Random();
           Observable.Interval(TimeSpan.FromMilliseconds(200)).ObserveOnDispatcher().Subscribe(l =>
            {
                if((_CntMessage == 10 || _CntError == 20) && TabControl1.Items.Count > 0)
                    TabControl1.Items.RemoveAt(0);
                switch (random.Next(1,6))
                {
                    case 1:
                        _CntMessage++;
                        _Logger.Trace($"Hello everyone: {_CntMessage}");
                        break;
                    case 2:
                        _CntMessage++;
                        if (stopwatch.Elapsed.Seconds > 5)
                        {
                            _Logger2.Debug($"Hello everyone: {_CntMessage}");
                        }
                        else
                        {
                            _Logger.Debug($"Hello everyone: {_CntMessage}");
                        }
                        break;
                    case 3:
                        _CntMessage++;
                        _Logger.Info($"Hello everyone: {_CntMessage}");
                        break;
                    case 4:
                        _CntError++;
                        _Logger.Warn($"Hello everyone: {_CntError}");
                        break;
                    case 5:
                        _CntError++;

                        try
                        {
                            int a = 0;
                            int b = 1;
                            var c = b / a;
                        }
                        catch (Exception ex)
                        {
                            _Logger.Error(ex, $"There was an error on divison :/ {_CntError}");
                        }
                        break;
                    case 6:
                        _CntError++;
                        _Logger.Fatal($"{_CntError}\n{LOREM_IPSUM}");
                        break;
                }
            });
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TestPopup popup = new TestPopup();
            popup.Show();
        }
    }

    /// <summary>
    /// Reformat the DateTime
    /// </summary>
    public class FooTimeStampResolver : ILogEventInfoResolver
    {
        public string Resolve(LogEventInfo logEventInfo)
        {
            return logEventInfo.TimeStamp.ToUniversalTime().ToString();
        }
    }
}
