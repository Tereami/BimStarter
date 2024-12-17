using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace FixSlowFile
{
    public class MyProjectSharedParameter
    {
        public string Name { get; set; }
        public Definition def;
        public List<Category> categories = new List<Category>();
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
        public BuiltInParameterGroup paramGroup;
#else
        public ForgeTypeId paramGroup;
#endif
        public Guid guid;


        public MyProjectSharedParameter(Parameter param, Document doc)
        {
            def = param.Definition;
            Name = def.Name;

            InternalDefinition intDef = def as InternalDefinition;
            if (intDef != null)
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                paramGroup = intDef.ParameterGroup;
#else
                paramGroup = intDef.GetGroupTypeId();
#endif

            guid = param.GUID;


            ElementBinding elemBind = this.GetBindingByParamName(Name, doc);

            foreach (Category cat in elemBind.Categories)
            {
                categories.Add(cat);
            }
        }

        public bool RemoveOrAddFromRebarCategory(Document doc, Element elem, bool addOrDeleteCat)
        {
            Application app = doc.Application;

            ElementBinding elemBind = this.GetBindingByParamName(Name, doc);

            //получаю список категорий
            CategorySet newCatSet = app.Create.NewCategorySet();
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            int rebarcatid = new ElementId(BuiltInCategory.OST_Rebar).IntegerValue;
#else
            int rebarcatid = (int)(new ElementId(BuiltInCategory.OST_Rebar).Value);
#endif
            foreach (Category cat in elemBind.Categories)
            {
                int catId = cat.Id.GetElementId();
                if (catId != rebarcatid)
                {
                    newCatSet.Insert(cat);
                }
            }

            if (addOrDeleteCat)
            {
                Category cat = elem.Category;
                newCatSet.Insert(cat);
            }

            TypeBinding newBind = app.Create.NewTypeBinding(newCatSet);
            if (doc.ParameterBindings.Insert(def, newBind, paramGroup))
            {
                return true;
            }
            else
            {
                if (doc.ParameterBindings.ReInsert(def, newBind, paramGroup))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddToProjectParameters(Document doc, Element elem)
        {
            Application app = doc.Application;
            //string oldSharedParamsFile = app.SharedParametersFilename;

            ExternalDefinition exDef = null;
            string sharedFile = app.SharedParametersFilename;
            DefinitionFile sharedParamFile = app.OpenSharedParameterFile();
            foreach (DefinitionGroup defgroup in sharedParamFile.Groups)
            {
                foreach (Definition def in defgroup.Definitions)
                {
                    if (def.Name == Name)
                    {
                        exDef = def as ExternalDefinition;
                    }
                }
            }
            if (exDef == null) throw new Exception("В файле общих параметров не найден общий параметр " + Name);

            CategorySet catSet = app.Create.NewCategorySet();
            catSet.Insert(elem.Category);
            TypeBinding newBind = app.Create.NewTypeBinding(catSet);

            doc.ParameterBindings.Insert(exDef, newBind, paramGroup);

            //app.SharedParametersFilename = oldSharedParamsFile;

            Parameter testParam = elem.LookupParameter(Name);
            if (testParam == null) throw new Exception("Не удалось добавить обший параметр " + Name);
        }



        private ElementBinding GetBindingByParamName(String paramName, Document doc)
        {
            Application app = doc.Application;
            DefinitionBindingMapIterator iter = doc.ParameterBindings.ForwardIterator();
            while (iter.MoveNext())
            {
                Definition curDef = iter.Key;
                if (!Name.Equals(curDef.Name)) continue;

                def = curDef;
                ElementBinding elemBind = (ElementBinding)iter.Current;
                return elemBind;
            }
            throw new Exception("не найден параметр " + paramName);
        }
    }
}
