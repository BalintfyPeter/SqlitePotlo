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
using Microsoft.Data.Sqlite;
using System.IO;

namespace sqlite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqliteConnection connection;
        string path = "Filename=felvi.csv";

        List<Student> studentList = new List<Student>();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            connection = new SqliteConnection("Filename=data.db");
            connection.Open();

            string cmdTxt = "CREATE TABLE IF NOT EXISTS Students(id INT NOT NULL AUTO_INCREMENT, name VARCHAR(100), gender BOOL, points INT(500), className VARCHAR(50))";

            SqliteCommand cmd = new SqliteCommand(cmdTxt, connection);
            cmd.ExecuteNonQuery();

            foreach (var item in File.ReadAllLines("felvi.csv", Encoding.UTF8).Skip(1)) 
            {
                string[] parts = item.Split(';');

                int id = Convert.ToInt32(parts[0]);
                string  name = parts[1];
                bool gender = Convert.ToBoolean(parts[2]);
                int points = Convert.ToInt32(parts[3]);
                string className = parts[4];

                string insertIntoTxt = $"INSERT INTO data.db(id, name, gender, points, className) VALUES('{id}' ,'{name}', '{gender}', ''{points}, '{className}')";

                cmd = new(insertIntoTxt, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void btnReadIntoGrid_Click(object sender, RoutedEventArgs e)
        {
            connection = new("Filename=data.db");
            connection.Open();

            string queryTxt = "SELECT * FROM data";
            SqliteCommand cmd = new(queryTxt, connection);
            SqliteDataReader reader = cmd.ExecuteReader();

            studentList = new();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                bool gender = Convert.ToBoolean(reader.GetString(2));
                int points = Convert.ToInt32(reader.GetString(3));
                string className = reader.GetString(4);

                Student newItem = new(id ,name, gender, points, className);
                studentList.Add(newItem);
            }

            dgGrid.ItemsSource = studentList;
            reader.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Student selected = dgGrid.SelectedItem as Student;

            string del = $"DELETE FROM data WHERE id={selected.id}";
            SqliteCommand cmd = new(del, connection);
            cmd.ExecuteNonQuery();
            studentList.Remove(selected);
            dgGrid.Items.Refreash();
        }

        private void btnFel7_Click(object sender, RoutedEventArgs e)
        {           
            string infoQuery = "SLELECT name FROM data WHERE className LIKE %informatoka%";
        }
    }
}
