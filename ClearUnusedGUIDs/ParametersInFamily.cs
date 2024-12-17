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
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
#endregion

namespace ClearUnusedGUIDs
{
    public class ParametersInFamily
    {
        public List<SharedParameterContainer> parameters = new List<SharedParameterContainer>();

        public ParametersInFamily(Document familyDoc)
        {
            FamilyManager fMan = familyDoc.FamilyManager;

            foreach(FamilyParameter fParam in fMan.Parameters)
            {
                if (!fParam.IsShared) continue;

                string name = fParam.Definition.Name;
                bool isInstance = fParam.IsInstance;
                InternalDefinition def = fParam.Definition as InternalDefinition;
                Guid guid = fParam.GUID;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                BuiltInParameterGroup paramGroup = fParam.Definition.ParameterGroup;
#else
                ForgeTypeId paramGroup = fParam.Definition.GetGroupTypeId();
#endif

                SharedParameterContainer paramContainer = new SharedParameterContainer(name, guid, paramGroup, isInstance, def);

                parameters.Add(paramContainer);
            }
        }


        public static bool ParamIsExists(Document famDoc, SharedParameterContainer paramContainer)
        {
            FamilyManager fMan = famDoc.FamilyManager;

            foreach(FamilyParameter fparam in fMan.Parameters)
            {
                if (!fparam.IsShared) continue;
                Guid guid1 = fparam.GUID;
                Guid guid2 = paramContainer.guid;
                bool check = guid1.Equals(guid2);
                if (check) return true;
            }
            return false;
        }
    }
}
