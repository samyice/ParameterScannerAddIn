using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ParameterScannerAddIn
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class OpenScanner : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //retrieving the uidocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            try
            {
                ParameterScannerWPF parScan = new ParameterScannerWPF(uidoc);
                //showing wpf window
                parScan.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
