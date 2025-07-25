using SamStoreWPFJson_BOs;
using SamStoreWPFJson_Repositories.Implements;
using SamStoreWPFJson_Repositories.Interfaces;
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

namespace SamStoreWPFJson
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IMemberRepo _memberRepo;

        public LoginWindow()
        {
            InitializeComponent();
            _memberRepo = new MemberRepo();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            var member = _memberRepo.GetMemberByLogin(email, password);
            var Roles = member?.RoleID;
            if(Roles == 3)
            {
                MessageBox.Show("You are not allowed to access this function!");
                return;
            }
            if (member != null)
            {
                OrderManagementWindow orderManagementWindow = new OrderManagementWindow(member);
                orderManagementWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("You have no permission to access this function!");
            }
        }
    }
}
