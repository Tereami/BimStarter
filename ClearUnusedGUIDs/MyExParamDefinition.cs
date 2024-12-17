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
    public class MyExParamDefinition : IComparable<MyExParamDefinition>
    {
        public ExternalDefinition exDef;
        public MyExParamDefinition(Definition ExDef)
        {
            exDef = ExDef as ExternalDefinition;
        }

        public int CompareTo(MyExParamDefinition other)
        {
            return this.exDef.Name.CompareTo(other.exDef.Name);
        }

        public override string ToString()
        {
            return exDef.Name;
        }
    }
}
