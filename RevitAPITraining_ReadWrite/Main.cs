using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAPITraining_ReadWrite
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            string wallInfo = string.Empty;
            var walls = new FilteredElementCollector(doc)
               .OfCategory(BuiltInCategory.OST_Walls)
               .OfType<Wall>()
               .Cast<Wall>()
               .ToList();


            foreach (var wall in walls)
            {
                if (wall is Wall)
                {
                    Parameter Volume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    string wallVolume = Volume.AsDouble().ToString();
                    wallInfo += $"{wall.Name},{wallVolume}{Environment.NewLine}";
                }


            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string csvPath = Path.Combine(desktopPath, "wallInfo.txt");

            File.WriteAllText(csvPath, wallInfo);

            return Result.Succeeded;
        }

    }
}
