using Prism.Events;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApplicationMavinwoo_Test.core.Events
{
    public class SalesOrderUpdateEvent : PubSubEvent<string>
    {
    }
}
