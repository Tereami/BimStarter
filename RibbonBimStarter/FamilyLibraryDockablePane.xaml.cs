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
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace RibbonBimStarter
{
    /// <summary>
    /// Interaction logic for FamilyLibraryDockablePane.xaml
    /// </summary>
    public partial class FamilyLibraryDockablePane : System.Windows.Controls.Page, IDockablePaneProvider
    {

        private ExternalEvent _exEvent;
        private EventLoadFamily _loadfamEvent;

        public SortedDictionary<string, ObservableCollection<FamilyFileInfo>> familyCollection;

        public FamilyLibraryDockablePane(ExternalEvent exEvent, EventLoadFamily loadFamEvent)
        {
            Debug.WriteLine("Start initializing pane");
            InitializeComponent();

            ToolTipService.ShowDurationProperty
                .OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(int.MaxValue));

            _exEvent = exEvent;
            _loadfamEvent = loadFamEvent;
            Debug.WriteLine("Pane is initialized");
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState = new DockablePaneState();
            data.InitialState.DockPosition = DockPosition.Left;
            data.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
        }


        private void AddFamilies_Click(object sender, RoutedEventArgs e)
        {
            /*string appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string selectedPath = System.IO.Path.Combine(appdataFolder, @"Autodesk\Revit\Addins\20xx\BimStarter\FamiliesLibraryTest");
            if(!System.IO.Directory.Exists(selectedPath))
            {
                Debug.WriteLine("Folder not found");
                return;
            }
            familyCollection = FilesScaner.GetInfo(selectedPath);*/
            WebConnection connect = App.connect;
            ServerResponse sr = connect.Request("familiesapp?revitversion=" + App.revitVersion);
            if(sr.Statuscode >= 400)
            {
                Debug.WriteLine("Error request, status " + sr.Statuscode + " message " + sr.Message);
                MessageBox.Show(sr.Message, "Error" + sr.Statuscode);
                return;
            }

            familyCollection = FamiliesCollection.convertJsonToFamilies(sr.Message);
            if (familyCollection == null || familyCollection.Count == 0)
            {
                Debug.WriteLine("No families found");
                return;
            }
            this.tabcontrol.ItemsSource = familyCollection;
            Debug.WriteLine("Families found: " + familyCollection.Count.ToString());
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Click load family");

            ListViewItem item = (ListViewItem)sender;
            if (!(item.Content is FamilyFileInfo))
            {
                Debug.WriteLine("Familyinfo is incorrect");
            }
            FamilyFileInfo famInfo = (FamilyFileInfo)item.Content;
            if(famInfo == null)
            {
                Debug.WriteLine("Fam info is null");
                return;
            }
           
            Debug.WriteLine("Fam file guid: " + famInfo.Guid);
            EventLoadFamily.familyguid = famInfo.Guid;
            EventLoadFamily.familyname = famInfo.FamilyName;
            _exEvent.Raise();
        }
    }
}
