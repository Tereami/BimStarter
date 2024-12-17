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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BatchPrintYay
{
    public class MyDimension
    {
        private int _mms;

        public int Millimeters
        {
            get { return _mms; }
            set
            {
                _mms = value;
                _feets = value / 304.8;
            }
        }

        private double _feets;
        public double Feets
        {
            get { return _feets; }
            set
            {
                _feets = value;
                _mms = (int)Math.Round((value * 304.8), 0);
            }
        }

        public static double GetLengthInMillimeters(double feets)
        {
#if R2017 || R2018 || R2019 || R2020
            double mm = UnitUtils.ConvertFromInternalUnits(feets, DisplayUnitType.DUT_MILLIMETERS);
#else
            double mm = UnitUtils.ConvertFromInternalUnits(feets, UnitTypeId.Millimeters);
#endif
            return mm;
        }
    }
}
