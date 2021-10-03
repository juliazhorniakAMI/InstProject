using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InstProject.BLL.Services;

namespace InstProject
{
    /// <summary>
    /// Interaction logic for PersonsList.xaml
    /// </summary>
    public partial class PersonsList : Window
    {
        private UserService userService;
        public PersonsList(List<string> list)
        {

            
            InitializeComponent();
            userService = new UserService();
            if (list != null && list.Count > 0)
            {
                foreach (var el in list)
                {
                    People.Items.Add(el);
                }
            }
        }
        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PersonPage info = new PersonPage(userService.GetUserByNick(People.SelectedItem.ToString()).Id);
            info.Show();

        }
    }
}
