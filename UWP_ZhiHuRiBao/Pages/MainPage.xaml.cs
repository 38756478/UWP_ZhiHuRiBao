﻿using LLQ;
using Brook.ZhiHuRiBao.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Common;
using XPHttp;
using Windows.UI.Core;
using Brook.ZhiHuRiBao.Utils;
using Windows.UI.Xaml.Media;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initalize();
            NavigationCacheMode = NavigationCacheMode.Required;
            CommentListView.Loaded += (s, e) =>
            {
                var view = GetScrollViewer(CommentListView);
                view.ViewChanged += (sender, arg) =>
                {
                    if (view.ExtentHeight - view.VerticalOffset - view.ViewportHeight < 300)
                    {
                        CommentList.RequestComments();
                    }
                };
            };
        }

        public static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
                return obj as ScrollViewer;

            for(int i=0;i<VisualTreeHelper.GetChildrenCount(obj);i++)
            {
                var view = GetScrollViewer(VisualTreeHelper.GetChild(obj, i));
                if (view != null)
                    return view;
            }

            return null;
        }

        public ObservableCollection<Story> MainList { get { return GetVM<MainViewModel>().MainList; } }

        public CommentLoadMoreCollection CommentList { get { return GetVM<MainViewModel>().CommentList; } }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var storyId = (e.ClickedItem as Story).id.ToString();
            GetVM<MainViewModel>().RequestMainContent(storyId);
            GetVM<MainViewModel>().RefreshComments(storyId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BlankPage1), true);
        }
    }
}
