using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IntoTheBrain.DataModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IntoTheBrain
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkSpace : Page
    {
        public WorkSpace()
        {
            this.InitializeComponent();
        }

        public bool IsStartRandom { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var taskDataSource = (TaskDataSource)App.Current.Resources["taskDataSource"];
            if (taskDataSource != null)
            {
                DataContext = (taskDataSource.Tasks).First();
            }
            if (e.Parameter is String)
                IsStartRandom = true;
        }

        private void ItemListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          if (e.AddedItems.Count > 0)
            {
                var taskItem = e.AddedItems[0] as TaskItem;
                if (taskItem!=null)
                {
                    var BrushForeGround = new SolidColorBrush(Colors.Gray);
                    comment.Text = String.Empty;
                    numOfTask.Text = taskItem.NumTask.ToString();
                    var oldPanel = StackPanelAnswer.Children.Where(x => x is StackPanel);
                    if(oldPanel.Count()!=0)
                        StackPanelAnswer.Children.RemoveAt(0);
                    
                    var dynamicStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                    string pattern = @"\d+";
                    string answer = taskItem.TrueAnswer;
                    Match match = Regex.Match(answer, pattern);
                    if (match.Success) //Если нашел соответствие
                    {
                        
                        var digitsAtTrueAnswer = Regex.Split(answer, pattern).ToList();
                        if (digitsAtTrueAnswer.First()!="")
                        {
                            var textBlock = new TextBlock
                            {
                                Text = digitsAtTrueAnswer.First(),
                                FontSize = 15,
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Margin = new Thickness(0, 0, 7, 0),
                                Foreground = BrushForeGround
                            };

                            dynamicStackPanel.Children.Add(textBlock);
                            digitsAtTrueAnswer.RemoveAt(0);
                        }
                        foreach (var s in digitsAtTrueAnswer)
                        {
                            if (s!=string.Empty)
                            {
                                var textBox = new TextBox
                                                  {
                                                      Height = 20, 
                                                      Width = 30
                                                  };
                                textBox.KeyDown += notEnterLettersKeyDown;
                                dynamicStackPanel.Children.Add(textBox);
                                var textBlock = new TextBlock
                                                    {
                                                        Text = s,
                                                        FontSize = 15,
                                                        VerticalAlignment = VerticalAlignment.Bottom,
                                                        Margin = new Thickness(7, 0, 7, 0),
                                                        Foreground = BrushForeGround
                                                    };
                                dynamicStackPanel.Children.Add(textBlock);
                            }
                        }
                        if (digitsAtTrueAnswer.Last()==String.Empty)
                        {
                            var textBox = new TextBox { Height = 30, Width = 30 };
                            textBox.KeyDown += notEnterLettersKeyDown;
                            dynamicStackPanel.Children.Add(textBox);
                        }
                    }
                    else
                    {
                        var items = taskItem.ComboBoxTask.Split(';');
                        var comboBox = new ComboBox();
                        foreach (var item in items)
                        {
                            comboBox.Items.Add(item);
                        }
                        comboBox.SelectedIndex = 0;
                        dynamicStackPanel.Children.Add(comboBox);
                    }
                    StackPanelAnswer.Children.Insert(0,dynamicStackPanel);
                }
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            String result = "";
            if (StackPanelAnswer.Children.Count!=0)
            {
                try
                {
                    var stackPanel = StackPanelAnswer.Children.First(x => x is StackPanel) as StackPanel;
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is TextBox)
                        {
                            result += (child as TextBox).Text.Trim(' ');
                            (child as TextBox).Text = (child as TextBox).Text.Trim(' ');
                        }
                        if (child is TextBlock)
                        {
                            result += (child as TextBlock).Text;
                        }
                        if (child is ComboBox)
                        {
                            result += (child as ComboBox).SelectedValue;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Нет стек-панели с ответами");
                }

                var ListRight = new List<String>
                                    {
                                        "Челом бью, Вы правы",
                                        "Сея попытка верная",
                                        "Ващец, верно",
                                        "Правильно, государь",
                                        "Велика мзда ваша, правильно ответил",
                                        "Истину глаголишь"
                                    };
                var ListNotRight = new List<String>
                                    {
                                        "Наветки, барин, наветки, не верно",
                                        "Не верно говоришь, друг",
                                        "Ложь это, неправильно",
                                        "Злочинства делаешь, не то говоришь, государь",
                                        "Зде нету истины",
                                        "Не друг, не пойдет так, не прав ты"
                                    };

                var rnd = new Random();
                if ((ItemListView.SelectedItems.First() as TaskItem).TrueAnswer == result)
                {
                    comment.Foreground = new SolidColorBrush(Colors.Green);
                    comment.Text = ListRight[rnd.Next(0, ListRight.Count - 1)];
                }
                else
                {
                    comment.Foreground = new SolidColorBrush(Colors.Red);
                    comment.Text =  ListNotRight[rnd.Next(0, ListNotRight.Count - 1)];
                }
            }
        }

        private void ToggleSwitch_Toggled_1(object sender, RoutedEventArgs e)
        {
            if (Tips!=null)
            {
                Trips.Visibility = Tips.IsOn ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                var numtask = (numOfTask.Text == String.Empty) ? 1 : Convert.ToInt32(numOfTask.Text);
                if (numtask > ItemListView.Items.Count)
                {
                    numtask = ItemListView.Items.Count;
                    numOfTask.Text = numtask.ToString();
                }
                ItemListView.SelectedIndex = numtask - 1;
                ItemListView.ScrollIntoView(ItemListView.SelectedItem);
            }

            catch(FormatException)
            {
                ItemListView.SelectedIndex = 0;
                numOfTask.Text = (ItemListView.SelectedIndex + 1).ToString();
                ItemListView.ScrollIntoView(ItemListView.SelectedItem);
            }
            catch (Exception)
            {
                throw new Exception("Не возможно переместиться к задаче, так как недействителен номер её");
            }


        }

        private void notEnterLettersKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key < VirtualKey.Number0 || e.Key > VirtualKey.Number9)
            {
                e.Handled = true;
            }            
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (IsStartRandom)
            {
                var random = new Random();
                numOfTask.Text = random.Next(1, ItemListView.Items.Count).ToString();
            }
            else
            {
                numOfTask.Text = 1.ToString();
            }
        }
    }
}
