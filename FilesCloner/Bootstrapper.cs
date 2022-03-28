using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using FilesCloner.ViewModels;

namespace FilesCloner
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();

            container.PerRequest<ShellViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var settings = new Dictionary<string, object>
            {
            { "SizeToContent", SizeToContent.Manual },
            { "Height" , SystemParameters.PrimaryScreenHeight / 1.1 },
            { "Width"  , SystemParameters.PrimaryScreenWidth / 1.5 },
            };
            DisplayRootViewFor<ShellViewModel>(settings);
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}