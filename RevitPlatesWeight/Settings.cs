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
#region usings
using System;
#endregion

namespace RevitPlatesWeight
{
    [Serializable]
    public class Settings
    {
        public string ProfileNameValue = MyStrings.ProfileNameValue;
        public int GroupConstParamValue = 10;
        public int ElementTypeParamValue = 6;
        public int ElementWeightTypeValue = 5;
        public bool Rewrite = true;
        public bool writeThickName = true;
        public bool writeThickvalue = true;
        public string platePrefix = "—";
        public bool writePlatesLengthWidth = false;
        public bool enablePlatesNumbering = false;
        public string plateNumberingParamName = MyStrings.PlateNumberingParamName;
        public int plateNumberingStartWith = 1;
        public bool writeBeamLength = false;
        public bool writeColumnLength = false;
        public bool useOnlyVisibleOnCurrentView = true;


        public string GroupConstParamName = MyStrings.ValueGroupConstParamName;
        public string ElementTypeParamName = MyStrings.ValueElementTypeParamName;
        public string ElementWeightTypeParamName = MyStrings.ValueElementWeightTypeParamName;
        public string WeightParamName = MyStrings.ValueWeightParamName;
        public string MaterialNameParam = MyStrings.ValueMaterialNameParam;
        public string VolumeParamName = MyStrings.ValueVolumeParamName;
        public string ProfileNameParamName = MyStrings.ValueProfileNameParamName;
        public string PlateNameParamName = MyStrings.ValuePlateNameParamName;

        public string ThicknessParamName = MyStrings.ValueThicknessParamName;
        public string PlateLengthParamName = MyStrings.ValuePlateLengthParamName;
        public string PlateWidthParamName = MyStrings.ValuePlateWidthParamName;
        public string LengthCorrectedParamName = MyStrings.ValueLengthCorrectedParamName;

    }
}
