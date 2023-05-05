using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TagPlugin.Data;
using Finkit.ManicTime.Common;
using Finkit.ManicTime.Shared.Logging;
using Finkit.ManicTime.Shared.Plugins.ServiceProviders.PluginCommands;

namespace TagPlugin.CreateDatabase
{
    public class CreateDatabaseCommand : PluginCommand
    {
        public CreateDatabaseCommand(
            IEventHub eventHub)
        {
            InvokeOnUiThread(SetCanExecute);
        }

        public override string Name => "Create database";

        public static void InvokeOnUiThread(Action action)
        {
            var currentApplication = Application.Current;
            if (currentApplication == null || currentApplication.CheckAccess())
                action();
            else
                currentApplication.Dispatcher.Invoke(action);
        }

        private void SetCanExecute()
        {
            CanExecute = true;
        }

        public override void Execute()
        {
            try
            {
                using (TagsPluginContext context = new TagsPluginContext())
                    context.Database.EnsureCreated();
                { }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(ex);
            }
        }


    }
}
