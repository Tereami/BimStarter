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
using Autodesk.Revit.DB;
#endregion

namespace SchedulesTools
{
    public enum RevisionParameterType
    {
        Description,
        IssuedBy,
        IssuedTo,
        RevisionDate,
        RevisionNumber,
        SequenceNumber
    };


    public class RevisionParameter
    {
        public static List<RevisionParameter> GetAllRevisionParams()
        {
            List<RevisionParameter> revParams = new List<RevisionParameter>();

            foreach (RevisionParameterType paramType in Enum.GetValues(typeof(RevisionParameterType)))
            {
                RevisionParameter rp = new RevisionParameter(paramType);
                revParams.Add(rp);
            }
            return revParams;
        }

        public RevisionParameterType paramType { get; }

        public RevisionParameter(RevisionParameterType ParamType)
        {
            paramType = ParamType;
        }

        public string GetValueFromRevision(Revision rev)
        {
            string val;

            switch (paramType)
            {
                case RevisionParameterType.Description:
                    val = rev.Description;
                    break;
                case RevisionParameterType.IssuedBy:
                    val = rev.IssuedBy;
                    break;
                case RevisionParameterType.IssuedTo:
                    val = rev.IssuedTo;
                    break;
                case RevisionParameterType.RevisionDate:
                    val = rev.RevisionDate;
                    break;
                case RevisionParameterType.RevisionNumber:
                    val = rev.RevisionNumber;
                    break;
                case RevisionParameterType.SequenceNumber:
                    val = rev.SequenceNumber.ToString();
                    break;
                default:
                    val = "INVALID";
                    break;
            }
            return val;
        }

        public override string ToString()
        {
            switch (paramType)
            {
                case RevisionParameterType.Description:
                    return "Описание текущего изменения";
                case RevisionParameterType.IssuedBy:
                    return "Утвердил";
                case RevisionParameterType.IssuedTo:
                    return "Утверждено для";
                case RevisionParameterType.RevisionDate:
                    return "Дата";
                case RevisionParameterType.RevisionNumber:
                    return "Номер изменения";
                case RevisionParameterType.SequenceNumber:
                    return "Номер редакции";
                default:
                    return "INVALID";
            }
        }
    }
}
