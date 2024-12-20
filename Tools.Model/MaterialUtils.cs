#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.

More about solution / Подробнее: http://weandrevit.ru/plagin-massa-plastin-km/
*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Tools.Model
{
    public class MaterialUtils
    {
        public static double GetMaterialDensity(Document doc, ElementId materialId)
        {
            Material material = doc.GetElement(materialId) as Material;
            if (material.StructuralAssetId == ElementId.InvalidElementId)
            {
                string msg = $"No physical parameters for material / Нет физических параметров у материала {material.Name}";
                System.Windows.Forms.MessageBox.Show(msg);
                throw new Exception(msg);
            }
            PropertySetElement materialStructuralParams = doc.GetElement(material.StructuralAssetId) as PropertySetElement;
            double density = materialStructuralParams.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_STRUCTURAL_DENSITY).AsDouble();
            return density;
        }

        /// <summary>
        /// Проверяет, есть ли в элементе материал с классом "Бетон" и заполнен Мтрл.КодМатериала
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static bool CheckElementIsConcrete(Element elem)
        {
            List<ElementId> materialsIds = elem.GetMaterialIds(false).ToList();

            foreach (ElementId matId in materialsIds)
            {
                Material mat = elem.Document.GetElement(matId) as Material;
                string materialClass = mat.MaterialClass;
                if (materialClass != "Бетон") continue;
                Parameter matCode = mat.get_Parameter(new Guid("b5675d33-fade-46b1-921b-0cab8eec101e")); //Мтрл.КодМатериала
                if (matCode == null) continue;
                if (matCode.AsInteger() == 0) continue;

                double volume = elem.GetMaterialVolume(matId);
                if (volume != 0) return true;
            }

            return false;
        }
    }
}
