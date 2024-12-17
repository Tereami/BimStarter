#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
#endregion

namespace RibbonBimStarter
{
    public static class FamiliesCollection
    {
        public static SortedDictionary<string, ObservableCollection<FamilyFileInfo>> convertJsonToFamilies(string json)
        {
            Dictionary<int[], string> groups = new Dictionary<int[], string>();

            SortedDictionary<string, ObservableCollection<FamilyFileInfo>> dictionary =
                new SortedDictionary<string, ObservableCollection<FamilyFileInfo>>();

            List<FamilyShortInfo> infos = new List<FamilyShortInfo>();

            FamilySuperGroupInfo.Activate();

            try
            {
                infos = JsonConvert.DeserializeObject<List<FamilyShortInfo>>(json);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Не удалось десериализовать: " + json);
            }
            foreach (FamilyShortInfo fsi in infos)
            {
                FamilyFileInfo familyFileInfo = new FamilyFileInfo();
                familyFileInfo.Guid = fsi.guid;
                familyFileInfo.Title = fsi.name;
                familyFileInfo.ImagePath = App.settings.Website + "img/families/" + fsi.guid + "_small.jpg";
                familyFileInfo.ImageBigPath = App.settings.Website + "img/families/" + fsi.guid + ".jpg";

                string clearDesc = fsi.description.Replace("&#34;", "\"");
                familyFileInfo.Description = clearDesc;


                familyFileInfo.HostType = "Тип основы: " + fsi.host;
                familyFileInfo.HostShortName = fsi.hostshortname;
                familyFileInfo.CategoryName = fsi.category;
                familyFileInfo.CategoryTitleAndName = "Категория: " + fsi.category;
                familyFileInfo.CategoryShortName = fsi.catshortname;
                familyFileInfo.DateAdd = "Дата создания: " + fsi.dateadd;
                familyFileInfo.RevitVersion = "Версия Revit: " + fsi.revitversion;

                familyFileInfo.FamilyName = fsi.GetFamilyName();
                familyFileInfo.FolderTitle = fsi.groupname;

                //familyFileInfo.FamilyName = 

                string groupIdText = fsi.groupid;
                string folderTitle = FamilySuperGroupInfo.GetSupergroupByIndex(groupIdText);
                
                if (dictionary.ContainsKey(folderTitle))
                {
                    dictionary[folderTitle].Add(familyFileInfo);
                }
                else
                {
                    dictionary.Add(folderTitle, new ObservableCollection<FamilyFileInfo>
                    {
                        familyFileInfo
                    });
                }
                /*if (dictionary.ContainsKey("000_Все"))
                {
                    dictionary["000_Все"].Add(familyFileInfo);
                }
                else
                {
                    dictionary.Add("000_Все", new ObservableCollection<FamilyFileInfo>
                    {
                        familyFileInfo
                    });
                }*/
            }
            return dictionary;
        }
    }
}
