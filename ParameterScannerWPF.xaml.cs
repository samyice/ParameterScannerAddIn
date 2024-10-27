using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ParameterScannerAddIn
{
    /// <summary>
    /// Lógica de interacción para ParameterScannerWPF.xaml
    /// </summary>
    public partial class ParameterScannerWPF : Window
    {

        public UIDocument UiDoc { get; }
        public Document Doc {  get; }

        public ParameterScannerWPF(UIDocument uidoc)
        {
            
            UiDoc = uidoc;
            Doc = UiDoc.Document;
            InitializeComponent();
            Title = "Parameter Scanner";
        }

        //attempts to find all model elements matching the entered criteria
        private ICollection<ElementId> FindMatchingElements(String paramName, String paramValue)
        {
            
            Boolean existsParam = false;

            //retrieves all model elements
            FilteredElementCollector collector = new FilteredElementCollector(Doc);
            ICollection<Element> listElements = collector.WhereElementIsNotElementType().ToElements();
            ICollection<ElementId> listElementIds = new List<ElementId>();

                foreach (Element element in listElements)
                {
                    //attempts to find the paramenter name input for each model element
                    Parameter param = element.LookupParameter(paramName);
                    if (param != null && param.HasValue)
                    {
                        //if found, retrieves its value and converts it to the correct type to find a match with the given value
                        existsParam = true;
                        StorageType st = param.StorageType;
                        string elementParamValue = "";
                        switch (st)
                        {
                            case StorageType.String:
                                elementParamValue = param.AsString();
                                break;
                            //TODO search for enum and boolean
                            case StorageType.Integer:
                                elementParamValue = param.AsInteger().ToString();
                                break;
                            case StorageType.Double:
                                elementParamValue = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.GetUnitTypeId()).ToString();
                                break;
                        }
                        if (paramValue.Equals(elementParamValue))
                        {
                            //if name and value match, it adds the element to the list to perform the requested action
                            listElementIds.Add(element.Id);
                        }
                    }
                }
                if (!existsParam)
                {
                    return null;
                }
            return listElementIds;

        }

        private void IsolateElements(object sender, RoutedEventArgs e)
        {

            String paramName = ParameterName.Text;
            String paramValue = ParameterValue.Text;
            //TODO implement IDataError
            if (!paramName.Equals(String.Empty))
            {
                //validates the enable views to perform the requested action
                String viewtype = Doc.ActiveView.ViewType.ToString();
                if ((!viewtype.Equals("FloorPlan")) && (!viewtype.Equals("CeilingPlan")) && (!viewtype.Equals("ThreeD")))
                {
                    TaskDialog.Show("Warning", "This addin is not enabled for the active view. Please, switch to a Floor Plan, a Ceiling Plan or a 3D View");
                }
                else
                {
                    //if enabled view, attempts to find the matching elements
                    ICollection<ElementId> listElementIds = FindMatchingElements(paramName, paramValue);

                    using (Transaction t = new Transaction(Doc, "Isolate Elements"))
                    {
                        t.Start();

                        if (listElementIds != null)
                        {
                            if (listElementIds.Count > 0)
                            {
                                //isolates in view the list of elements that match the criteria
                                Doc.ActiveView.IsolateElementsTemporary(listElementIds);
                                TaskDialog.Show("Warning", "Number of elements found: " + listElementIds.Count);
                            }
                            else
                            {
                                TaskDialog.Show("Warning", "No element was found with the specified Parameter Value");
                            }
                        }
                        else
                        {
                            TaskDialog.Show("Warning", "No element found matching the specified Parameter Name");
                        }


                        t.Commit();
                    }
                }
            } else
            {
                TaskDialog.Show("Warning", "You must specify a Parameter Name");
            }
            
        }

        private void SelectElements(object sender, RoutedEventArgs e)
        {

            String paramName = ParameterName.Text;
            String paramValue = ParameterValue.Text;
            //TODO implement IDataError
            if (!paramName.Equals(String.Empty))
            {
                //validates the enable views to perform the requested action
                String viewtype = Doc.ActiveView.ViewType.ToString();
                if ((!viewtype.Equals("FloorPlan")) && (!viewtype.Equals("CeilingPlan")) && (!viewtype.Equals("ThreeD")))
                {
                    TaskDialog.Show("Warning", "This addin is not enabled for the active view. Please, switch to a Floor Plan, a Ceiling Plan or a 3D View");
                }
                else
                {
                    //if enabled view, attempts to find the matching elements
                    ICollection<ElementId> listElementIds = FindMatchingElements(paramName, paramValue);

                    using (Transaction t = new Transaction(Doc, "Isolate Elements"))
                    {
                        t.Start();

                        if (listElementIds != null)
                        {
                            if (listElementIds.Count > 0)
                            {
                                //selects in view the list of elements that match the criteria
                                UiDoc.Selection.SetElementIds(listElementIds);
                                TaskDialog.Show("Warning", "Number of elements found: " + listElementIds.Count);
                            }
                            else
                            {
                                TaskDialog.Show("Warning", "No element was found with the specified Parameter Value");
                            }
                        }
                        else
                        {
                            TaskDialog.Show("Warning", "No element found matching the specified Parameter Name");
                        }


                        t.Commit();
                    }
                }
            } else
            {
                TaskDialog.Show("Warning", "You must specify a Parameter Name");
            }
            
        }
    }
}
