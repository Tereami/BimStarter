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
#region Using
using System;
using Autodesk.Revit.DB;
using RVTDocument = Autodesk.Revit.DB.Document;
using ASDocument = Autodesk.AdvanceSteel.DocumentManagement.Document;
using Autodesk.Revit.DB.Steel;
using Autodesk.AdvanceSteel.DocumentManagement;
using Autodesk.AdvanceSteel.CADAccess;
#endregion

namespace RevitPlatesWeight
{
    public static class AdvanceSteelUtils
    {
		public static FilerObject GetFilerObject(RVTDocument doc, Reference eRef)
		{
			FilerObject filerObject = null;
			ASDocument curDocAS = DocumentManager.GetCurrentDocument();
			if (null != curDocAS)
			{
				OpenDatabase currentDatabase = curDocAS.CurrentDatabase;
				if (null != currentDatabase)
				{
					Guid uid = SteelElementProperties.GetFabricationUniqueID(doc, eRef);
					string asHandle = currentDatabase.getUidDictionary().GetHandle(uid);
					filerObject = FilerObject.GetFilerObjectByHandle(asHandle);
				}
			}
			return filerObject;
		}
	}
}
