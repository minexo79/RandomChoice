using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RandomChoice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] choice { get; set; }

        public static string[] defaultChoice = new string[] { "打電動", "打手槍", "寫程式", "去買好吃的", "睡覺睡到隔天" };
    }
}
