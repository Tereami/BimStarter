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
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace ArchParametrisation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CmdArchParametrisation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("ArchParametrisation"));

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<Settings> settingsSaver = new Tools.SettingsSaver.Saver<Settings>();
            Settings sets = settingsSaver.Activate("ArchParametrisation");
            Debug.WriteLine($"Settings is loaded");

            List<Room> rooms = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Autodesk.Revit.DB.SpatialElement))
                .OfCategoryId(new ElementId(BuiltInCategory.OST_Rooms))
                .Cast<Room>()
                .Where(r => r.Area > 0)
                .ToList();
            Debug.WriteLine($"Rooms found: {rooms.Count}");

            Dictionary<string, RoomInfo> roomInfosDraft = new Dictionary<string, RoomInfo>();

            foreach (Room r in rooms)
            {
                string roomName = r.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                if (!roomInfosDraft.ContainsKey(r.Name))
                {
                    RoomInfo ri = new RoomInfo(r);
                    ri.Name = roomName;

                    List<RoomInfo> checkHaveDefaultInfo = sets.defaultRoomInfos
                        .Where(i => i.Name == roomName).ToList();
                    if (checkHaveDefaultInfo.Count() > 0)
                        ri = checkHaveDefaultInfo[0];

                    if (!roomInfosDraft.ContainsKey(roomName))
                        roomInfosDraft.Add(roomName, ri);
                }
            }


            foreach (RoomInfo ri in roomInfosDraft.Values)
            {
                List<RoomInfo> inSettingsRoomInfo = sets.RoomInfos
                    .Where(i => i.Name == ri.Name).ToList();
                if (inSettingsRoomInfo.Count == 0)
                {
                    sets.RoomInfos.Add(ri);
                }
            }


            FormArchParametrisation form = new FormArchParametrisation(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                if (form.DialogResult == System.Windows.Forms.DialogResult.Retry)
                {
                    Debug.WriteLine("Reset settings");
                    settingsSaver.Reset();
                }
                else
                {
                    Debug.WriteLine("Cancelled");
                }
                return Result.Cancelled;
            }

            sets = form.curSettings;
            List<string> messages = new List<string>();

            int mirroredCount = 0;
            if (sets.enableMirrored)
            {
                Debug.WriteLine("MIRRORED");
                using (Transaction t = new Transaction(doc))
                {
                    t.Start(MyStrings.TransactionMirrored);
                    List<FamilyInstance> mirroredElems = doc.GetMirroredElements();
                    foreach (FamilyInstance e in mirroredElems)
                    {
                        e.SetValue(sets.mirroredParamName, sets.mirroredText, false);
                        mirroredCount++;
                    }
                    t.Commit();
                }

                Debug.WriteLine($"Mirrored found: {mirroredCount}");
                messages.Add($"{MyStrings.ResultMirroredCount}: {mirroredCount}");
            }

            int openingsCount = 0;
            if (sets.enableOpeningsArea)
            {
                Debug.WriteLine("OPENINGS AREA");
                using (Transaction t = new Transaction(doc))
                {
                    t.Start(MyStrings.TransactionOpeningsArea);
                    Dictionary<int, List<FamilyInstance>> roomIdsAndOpenings = doc.GetOpenings(sets);

                    foreach (Room r in rooms)
                    {
                        int roomId = r.Id.GetElementIdValue();
                        double sumOpeningsArea = 0;
                        if (roomIdsAndOpenings.ContainsKey(roomId))
                        {
                            foreach (FamilyInstance fi in roomIdsAndOpenings[roomId])
                            {
                                double curOpeningArea = fi.GetOpeningArea(sets);
                                if (curOpeningArea > 0)
                                {
                                    sumOpeningsArea += curOpeningArea;
                                    openingsCount++;
                                }
                            }
                        }

                        r.SetValue(sets.openingsAreaParamName, sumOpeningsArea, true);
                    }
                    t.Commit();
                }
                Debug.WriteLine($"Openings found: {openingsCount}");
                messages.Add($"{MyStrings.ResultOpeningsCount}: {openingsCount}");
            }

            int roomsCount = 0;
            if (sets.enableNumbersOfFinishings)
            {
                Debug.WriteLine("ROOMS FINISHING");
                using (Transaction t = new Transaction(doc))
                {
                    t.Start(MyStrings.TransactionFinishing);

                    Dictionary<string, HashSet<string>> floorTypesAndRoomNumbers = new Dictionary<string, HashSet<string>>();
                    Dictionary<string, HashSet<string>> finishesTypesAndRoomNumbers = new Dictionary<string, HashSet<string>>();

                    Dictionary<int, string> roomIdsAndFloorNames = new Dictionary<int, string>();
                    Dictionary<int, string> roomIdsAndFinishingNames = new Dictionary<int, string>();

                    List<BuiltInParameter> floorParameters = new List<BuiltInParameter> {
                        BuiltInParameter.ROOM_FINISH_FLOOR
                    };
                    List<BuiltInParameter> finishingParameters = new List<BuiltInParameter>
                    {
                        BuiltInParameter.ROOM_FINISH_WALL,
                        BuiltInParameter.ROOM_FINISH_CEILING
                    };
                    if (sets.chkbxFloorsIncludeInFinishing)
                        finishingParameters.Add(BuiltInParameter.ROOM_FINISH_FLOOR);

                    foreach (Room r in rooms)
                    {
                        int roomId = r.Id.GetElementIdValue();
                        string roomNumber = GetRoomNumber(r, sets);

                        string floorTypeName = GetFinishingName(r, floorParameters);
                        CollectFinishingTypes(roomNumber, floorTypeName, ref floorTypesAndRoomNumbers);
                        roomIdsAndFloorNames.Add(roomId, floorTypeName);

                        string finishingTypeName = GetFinishingName(r, finishingParameters);
                        CollectFinishingTypes(roomNumber, finishingTypeName, ref finishesTypesAndRoomNumbers);
                        roomIdsAndFinishingNames.Add(roomId, finishingTypeName);
                    }

                    foreach (Room r in rooms)
                    {
                        int roomId = r.Id.GetElementIdValue();

                        string floorTypeName = roomIdsAndFloorNames[roomId];
                        HashSet<string> floorTypeNumbers = floorTypesAndRoomNumbers[floorTypeName];
                        WriteFinishSequence(r, sets.numbersOfFloorTypesParamName, ", ", floorTypeNumbers);

                        string finishingTypename = roomIdsAndFinishingNames[roomId];
                        HashSet<string> finishingTypeNumners = finishesTypesAndRoomNumbers[finishingTypename];
                        WriteFinishSequence(r, sets.numbersOfFinishingParamName, ", ", finishingTypeNumners);

                        roomsCount++;
                    }

                    t.Commit();
                }
                Debug.WriteLine($"Finishing numbers are written for {roomsCount} rooms");
                messages.Add($"{MyStrings.ResultFinishing}: {roomsCount}");
            }

            int wallCount = 0;
            if (sets.enableRoomNumberToFinishing)
            {
                Debug.WriteLine($"3D FINISHING");
                using (Transaction t = new Transaction(doc))
                {
                    t.Start(MyStrings.Transaction3dFinishing);

                    foreach (Room r in rooms)
                    {
                        string roomNumber = GetRoomNumber(r, sets);
                        List<Element> walls = r.GetBoundaryRoomsElements();
                        foreach (Element e in walls)
                        {
                            e.SetValue(sets.roomNumberParamName, roomNumber, false);
                            wallCount++;
                        }
                    }

                    t.Commit();
                }
                Debug.WriteLine($"Finishing numbers are written for {wallCount} walls");
                messages.Add($"{MyStrings.Result3dFinishing}: {wallCount}");
            }

            int flatsCount = 0;
            if (sets.enableFlatography)
            {
                Debug.WriteLine($"FLATOGRAPHY");
                List<string> livingRoomNames = sets.RoomInfos.Where(i => i.IsLiving).Select(i => i.Name).ToList();
                Dictionary<string, double> roomCoefs = new Dictionary<string, double>();
                foreach (RoomInfo ri in sets.RoomInfos)
                {
                    roomCoefs.Add(ri.Name, ri.Coeff);
                }

                Dictionary<string, FlatInfo> flats = new Dictionary<string, FlatInfo>();
                foreach (Room room in rooms)
                {
                    string flatNumber = room.GetValue<string>(sets.flatNumberParamName);
                    if (flatNumber == null)
                        continue;

                    double roomArea = room.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble();
                    string roomName = room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();

                    bool isLiving = false;
                    if (livingRoomNames.Contains(roomName))
                    {
                        isLiving = true;
                    }

                    double coeff = roomCoefs[roomName];
                    double areaWithCoeff = roomArea * coeff;

                    FlatInfo curFlat = null;
                    if (flats.ContainsKey(flatNumber))
                        curFlat = flats[flatNumber];
                    else
                        curFlat = new FlatInfo();

                    curFlat.rooms.Add(room);
                    curFlat.FullArea += roomArea;
                    curFlat.AreaWithCoeff += areaWithCoeff;
                    if (isLiving)
                    {
                        curFlat.LivingArea += roomArea;
                        curFlat.RoomsCount++;
                    }

                    if (!flats.ContainsKey(flatNumber))
                        flats.Add(flatNumber, curFlat);
                }

                using (Transaction t = new Transaction(doc))
                {
                    t.Start(MyStrings.TransactionFlatography);

                    foreach (KeyValuePair<string, FlatInfo> kvp in flats)
                    {
                        FlatInfo flat = kvp.Value;
                        foreach (Room curRoom in flat.rooms)
                        {
                            string roomName = curRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                            double coeff = roomCoefs[roomName];
                            curRoom.SetValue(sets.flatRoomAreaCoeffParamName, coeff, true);
                            curRoom.SetValue(sets.flatRoomsCountParamName, flat.RoomsCount, true);
                            curRoom.SetValue(sets.flatSumAreaParamName, flat.FullArea, true);
                            curRoom.SetValue(sets.flatLivingAreaParamName, flat.LivingArea, true);
                            curRoom.SetValue(sets.flatAreaParamName, flat.AreaWithCoeff, true);

                            int isLiving = 0;
                            if (livingRoomNames.Contains(roomName))
                                isLiving = 1;
                            curRoom.SetValue(sets.isLivingParamName, isLiving, true);

                        }

                        flatsCount++;
                    }

                    t.Commit();
                }
                Debug.WriteLine($"Flats completed: {flatsCount}");
                messages.Add($"{MyStrings.ResultFlatography}: {flatsCount}");
            }

            settingsSaver.Save(sets);

            string msg = string.Join(System.Environment.NewLine, messages);
            Debug.WriteLine(msg);

            BalloonTip.Show(MyStrings.Result, msg);
            return Result.Succeeded;
        }

        private void CollectFinishingTypes(string roomNumber, string finishingTypeName, ref Dictionary<string, HashSet<string>> data)
        {
            if (string.IsNullOrEmpty(finishingTypeName))
                return;

            if (data.ContainsKey(finishingTypeName))
            {
                data[finishingTypeName].Add(roomNumber);
            }
            else
            {
                HashSet<string> roomNumbers = new HashSet<string> { roomNumber };
                data.Add(finishingTypeName, roomNumbers);
            }
        }

        private string GetRoomNumber(Room r, Settings sets)
        {
            string roomNumber = null;
            if (sets.useRoomName)
                roomNumber = r.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
            else
                roomNumber = r.Number;
            return roomNumber;
        }

        private void WriteFinishSequence(Room r, string roomNumbersParamName, string separator, HashSet<string> curTypeRoomNumbers)
        {
            List<string> numbers = curTypeRoomNumbers.ToList();
            numbers.Sort();

            string numbersJoined = string.Join(separator, numbers);

            r.SetValue(roomNumbersParamName, numbersJoined, true);
        }

        private string GetFinishingName(Room r, List<BuiltInParameter> parameters)
        {
            List<string> finishNames = new List<string>();

            foreach (BuiltInParameter param in parameters)
            {
                Parameter finishParam = r.get_Parameter(param);
                if (finishParam == null || !finishParam.HasValue)
                    continue;

                string finishTypeName = finishParam.AsString();
                if (string.IsNullOrEmpty(finishTypeName))
                    continue;
                finishNames.Add(finishTypeName);
            }

            if (finishNames.Count == 0)
                return "-";

            string joinedFinishTypeName = string.Join("_", finishNames);
            return joinedFinishTypeName;
        }
    }
}
