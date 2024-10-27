using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace ParameterScannerAddIn
{
    internal class App : IExternalApplication
    {
        
        public Result OnStartup(UIControlledApplication application)
        {
            //creates the new tab and panel
            String assemName = Assembly.GetExecutingAssembly().Location;
            String path = System.IO.Path.GetDirectoryName(assemName);

            String tabName = "Parameters";
            application.CreateRibbonTab(tabName);

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Parameter Scanner Panel");

            //creates the button on the panel
            PushButtonData btnData = new PushButtonData("Parameters", "Parameter Scanner", assemName, "ParameterScannerAddIn.OpenScanner")
            {
                LargeImage = new BitmapImage(new Uri(path + @"\ParameterScannerIcon_RS.png"))
            };

            panel.AddItem(btnData);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Failed;
        }
    }
}
