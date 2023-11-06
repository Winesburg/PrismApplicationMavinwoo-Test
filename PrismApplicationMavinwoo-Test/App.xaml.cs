using Module;
using Module.ViewModels;
using Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.Views;
using System.Windows;

namespace PrismApplicationMavinwoo_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        public App()
        {
            //Register Syncfusion license
            string key = "Mjc2NTM5N0AzMjMzMmUzMDJlMzBaQmlFeVpadEtyT21yOWJKejQvOGVncjZxZExkSit2R2hONVF3UzBjbENRPQ==";
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDataRepository, DataRepository>();
            containerRegistry.RegisterDialog<AddDialogView, AddDialogViewModel>();
            containerRegistry.RegisterDialog<InventoryDialogView, InventoryDialogViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleModule>();
        }

    }
}