using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Threading;

namespace SqlDependencyTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=10.10.13.46; initial catalog=AutoLink_Experiment; User ID=wst;Password=wst~!@#$SQL;";
        SqlDependency dependency = null;

        public MainWindow()
        {
            InitializeComponent();
            SqlDependency.Start(connectionString);
            GetData();
        }

        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e, string vinnum, string drivingKey)
        //private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            MessageBox.Show("call");
            /*
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UP_POSITIONCYCLEREPORTING_SELECT_POSITIONCYCLEREPORTING_VIEW_REALTIME", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter vinnumParam = new SqlParameter("@VINNUM", SqlDbType.VarChar, 34);
                    vinnumParam.Value = vinnum;
                    command.Parameters.Add(vinnumParam);

                    SqlParameter drivingKeyParam = new SqlParameter("@DRIVINGKEY", SqlDbType.VarChar, 50);
                    drivingKeyParam.Value = drivingKey;
                    command.Parameters.Add(drivingKeyParam);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        dataGrid.ItemsSource = dt.DefaultView;
                    }));
                }
            }
            */
            /*
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                GetData();
            }));
            */
            /*
            SqlDependency sqlDependency = sender as SqlDependency;
            sqlDependency.OnChange -= new OnChangeEventHandler(Dependency_OnChange);*/
        }

        private void GetData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UP_POSITIONCYCLEREPORTING_SELECT_POSITIONCYCLEREPORTING_VIEW_REALTIME", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter vinnumParam = new SqlParameter("@VINNUM", SqlDbType.VarChar, 34);
                vinnumParam.Value = "KMHJ3812WKU970466";
                command.Parameters.Add(vinnumParam);

                SqlParameter drivingKeyParam = new SqlParameter("@DRIVINGKEY", SqlDbType.VarChar, 50);
                drivingKeyParam.Value = "20190928222719";
                command.Parameters.Add(drivingKeyParam);

                dependency = new SqlDependency(command);
                dependency.OnChange += (sender, e) => Dependency_OnChange(sender, e, "KMHJ3812WKU970466", "20190928222719");
                //dependency.OnChange += (sender, e) => Dependency_OnChange(sender, e);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SqlDependency.Stop(connectionString);
        }
    }
}
