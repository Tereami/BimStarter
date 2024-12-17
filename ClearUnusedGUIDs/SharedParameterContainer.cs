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
    public class SharedParameterContainer
    {
        public string name;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
        public BuiltInParameterGroup paramGroup;
#else
        public ForgeTypeId paramGroup;
#endif
        public bool isInstance;
        public bool isDefineByFormula;
        public string formula;
        public InternalDefinition intDefinition;
        public Guid guid;
        public ExternalDefinition exDefinition;

        public SharedParameterContainer(string Name, Guid ParamGuid,
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            BuiltInParameterGroup ParamGroup, 
#else
            ForgeTypeId ParamGroup,
#endif

            bool IsInstance, InternalDefinition ParamDefinition)
        {
            name = Name;
            paramGroup = ParamGroup;
            isInstance = IsInstance;
            intDefinition = ParamDefinition;
            guid = ParamGuid;
        }
    }
}
