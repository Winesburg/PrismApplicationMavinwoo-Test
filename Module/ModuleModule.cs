using Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class ModuleModule : IModule
    {


        private readonly IRegionManager _regionManager;

        public ModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

       
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Data));
        }

        
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
