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
using System;
using System.Collections.Generic;
#endregion

namespace ArchParametrisation
{
    [Serializable]
    public class Settings
    {
        public bool enableMirrored = false;
        public string mirroredText = MyStrings.ValueMirroredText;
        public string mirroredParamName = MyStrings.ValueCommentsParam;

        public bool enableOpeningsArea = false;
        public string openingsWidthParamName = MyStrings.ValueOpeningWidthParam;
        public string openingsHeightParamName = MyStrings.ValueOpeningHeightParam;
        public string openingsAreaParamName = MyStrings.ValueOpeningAreaParam;
        public bool openingsCalculateInOneRoom = false;

        public bool enableNumbersOfFinishings = false;
        public string numbersOfFloorTypesParamName = MyStrings.ValueRoomNumberByFloorType;
        public string numbersOfFinishingParamName = MyStrings.ValueRoomNumberByFinishingType;
        public bool chkbxFloorsIncludeInFinishing = false;
        public bool useRoomName = false;

        public bool enableRoomNumberToFinishing = false;
        public string roomNumberParamName = MyStrings.ValueRoomNumberParam;

        public bool enableFlatography = false;
        public string flatNumberParamName = MyStrings.ValueFlatNumberParam;
        public string flatAreaParamName = MyStrings.ValueFlatAreaParam;
        public string flatSumAreaParamName = MyStrings.ValueFlatSumAreaParam;
        public string flatLivingAreaParamName = MyStrings.ValueFlatLivingArea;
        public string flatRoomsCountParamName = MyStrings.ValueFlatRoomsCount;
        public string flatRoomAreaCoeffParamName = MyStrings.ValueFlatAreaCoeff;
        public string isLivingParamName = MyStrings.ValueRoomIsLivingParam;

        public List<RoomInfo> defaultRoomInfos = new List<RoomInfo>
        {
            new RoomInfo(MyStrings.RoomBedroom, 1, true),
            new RoomInfo(MyStrings.RoomKitchen, 1, false),
            new RoomInfo(MyStrings.RoomLoggia, 0.5, false),
            new RoomInfo(MyStrings.RoomBalcony, 0.3, false)
        };

        public List<RoomInfo> RoomInfos = new List<RoomInfo>();
    }
}