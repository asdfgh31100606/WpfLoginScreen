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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace WpfLoginScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }
        SqlConnection con = new SqlConnection(@"Data Source=localhost\sqlexpress; Initial Catalog=MemberDB; Integrated Security=True");
        
        public void ClearData()
        {
            Name_txt.Clear();
            Age_txt.Clear();
            Sex_txt.Clear();
            City_txt.Clear();
            ID_txt.Clear();
        }
        
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from tblMember", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            DataGrid.ItemsSource = dt.DefaultView;
        }
        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        public bool isValid()
        {
            if (Name_txt.Text == String.Empty)
            {
                MessageBox.Show("Name is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if(Age_txt.Text == String.Empty)
            {
                MessageBox.Show("Age is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if(Sex_txt.Text == String.Empty)
            {
                MessageBox.Show("Sex is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if(City_txt.Text == String.Empty)
            {
                MessageBox.Show("City is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Insert_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tblMember VALUES (@Name,@Age,@Sex,@City) ", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", Name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", Age_txt.Text);
                    cmd.Parameters.AddWithValue("@Sex", Sex_txt.Text);
                    cmd.Parameters.AddWithValue("@City", City_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully Insert", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM tblMember WHERE ID=" + ID_txt.Text +"", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Delete", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearData();
                LoadGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Data is not deleted.",ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblMember SET Name = '" + Name_txt.Text + "', Age ='" + Age_txt.Text + "', Sex ='" + Sex_txt.Text + "', City ='" + City_txt.Text + "' WHERE ID = '" + ID_txt.Text + "' ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Update", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Data is not deleted.", ex.Message);
            }
            finally
            {
                con.Close();
                ClearData();
                LoadGrid();
            }
        }
    }
}
