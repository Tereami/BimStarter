﻿#region License
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
#endregion

namespace RebarParametrisation
{
    public enum ProcessedLinkFiles { NoLinks, OnlyLibs, AllKrFiles }
    public class RebarParametrisationSettings
    {
        public bool UseHostId = true;
        public bool UseUniqueHostId = true;
        public bool UseHostMark = true;
        public bool UseHostThickness = true;

        public bool UseRebarWeight = true;
        public bool UseRebarLength = true;
        public bool UseRebarDiameter = true;

        public double DefaultConcreteClass = 25;

        public ProcessedLinkFiles LinkFilesSetting = ProcessedLinkFiles.NoLinks;
    }
}
