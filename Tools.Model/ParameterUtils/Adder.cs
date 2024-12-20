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
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using System;
#endregion

namespace Tools.Model.ParameterUtils
{
    public enum BindSharedParamResult
    {
        eAlreadyBound,
        eSuccessfullyBound,
        eSuccesfullyReBound,
        eWrongParamType,
        eWrongBindingType,
        eFailed
    }

    public static class Adder
    {
        /// <summary>
        /// Добавить привязку к категории для параметра проекта
        /// </summary>
        public static BindSharedParamResult BindSharedParam(Document doc, Element elem, string paramName, Definition definition)
        {
            try
            {
                Autodesk.Revit.ApplicationServices.Application app = doc.Application;

                //собираю уже добавленные категории для повторной вставки и добавления новой категории
                CategorySet catSet = app.Create.NewCategorySet();

                // Loop all Binding Definitions
                // IMPORTANT NOTE: Categories.Size is ALWAYS 1 !?
                // For multiple categories, there is really one 
                // pair per each category, even though the 
                // Definitions are the same...

                DefinitionBindingMapIterator iter
                  = doc.ParameterBindings.ForwardIterator();

                while (iter.MoveNext())
                {
                    Definition def = iter.Key;
                    if (!paramName.Equals(def.Name)) continue;

                    ElementBinding elemBind
                      = (ElementBinding)iter.Current;

                    // Check for category match - Size is always 1!


                    // If here, no category match, hence must 
                    // store "other" cats for re-inserting

                    foreach (Category catOld in elemBind.Categories)
                        catSet.Insert(catOld); // 1 only, but no index...

                }

                // If here, there is no Binding Definition for 
                // it, so make sure Param defined and then bind it!


                Category cat = elem.Category;
                catSet.Insert(cat);

                InstanceBinding bind = app.Create.NewInstanceBinding(catSet);

                //используем Insert или ReInsert, что сработает
                if (doc.ParameterBindings.Insert(definition, bind))
                {
                    return BindSharedParamResult.eSuccessfullyBound;
                }
                else
                {
                    if (doc.ParameterBindings.ReInsert(definition, bind))
                    {
                        return BindSharedParamResult.eSuccessfullyBound;
                    }
                    else
                    {
                        return BindSharedParamResult.eFailed;
                    }
                }
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", $"Error in Binding Shared Param {paramName}. {ex.Message}");

                return BindSharedParamResult.eFailed;
            }
        }

        /// <summary>
        /// Проверяет налиxие общего параметра у элемента. Если параметр есть - возвращает его. Иначе добавляет параметр из файла общих параметров.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="app"></param>
        /// <param name="catset"></param>
        /// <param name="ParameterName"></param>
        /// <param name="paramGroup"></param>
        /// <param name="SetVaryByGroups"></param>
        /// <returns></returns>
        public static Parameter CheckAndAddSharedParameter(Element elem, Autodesk.Revit.ApplicationServices.Application app, CategorySet catset, string ParameterName,
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            BuiltInParameterGroup paramGroup,
#else
            ForgeTypeId paramGroup,
#endif
            bool SetVaryByGroups)
        {
            Document doc = elem.Document;
            Parameter param = elem.LookupParameter(ParameterName);
            if (param != null) return param;


            ExternalDefinition exDef = null;
            string sharedFile = app.SharedParametersFilename;
            DefinitionFile sharedParamFile = app.OpenSharedParameterFile();
            foreach (DefinitionGroup defgroup in sharedParamFile.Groups)
            {
                foreach (Definition def in defgroup.Definitions)
                {
                    if (def.Name == ParameterName)
                    {
                        exDef = def as ExternalDefinition;
                    }
                }
            }
            if (exDef == null) throw new Exception("В файл общих параметров не найден общий параметр " + ParameterName);

            bool checkContains = doc.ParameterBindings.Contains(exDef);
            if (checkContains)
            {
                var res = BindSharedParam(doc, elem, ParameterName, exDef);
            }

            InstanceBinding newIB = app.Create.NewInstanceBinding(catset);

            doc.ParameterBindings.Insert(exDef, newIB, paramGroup);

            if (SetVaryByGroups)
            {
                doc.Regenerate();

                SharedParameterElement spe = SharedParameterElement.Lookup(doc, exDef.GUID);
                InternalDefinition intDef = spe.GetDefinition();
                intDef.SetAllowVaryBetweenGroups(doc, true);
            }
            doc.Regenerate();


            param = elem.LookupParameter(ParameterName);
            if (param == null) throw new Exception("Не удалось добавить обший параметр " + ParameterName);

            return param;
        }
    }
}