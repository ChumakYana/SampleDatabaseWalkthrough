using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SampleDatabaseWalkthrough
{
    public partial class Form4 : Form

    {
        private SqlConnection sqlconnection = null;
        private SqlCommandBuilder SqlCommandBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdd = false;

        string str = "";
        string str_ins = "";
        string connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\SampleDatabaseWalkthrough\SampleDatabaseWalkthrough\Database1.mdf;Integrated Security=True;
Connect Timeout = 30");
        SqlConnection sqlconnect;
        

        public Form4()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            sqlconnect = new SqlConnection(connectionString);
            await sqlconnect.OpenAsync();
            string proverka = textBox1.Text.ToString();//получила логин
            str = ("SELECT Имя_пользователя from Пользователи where Имя_пользователя like N'%" + textBox1.Text.ToString() + "%'");//возвращаю логин если уже есть такой в БД
            str_ins = ("SELECT Имя_пользователя from Пользователи where Имя_пользователя like N'%" + textBox1.Text.ToString() + "%'");
            SqlCommand command = new SqlCommand(str, sqlconnect);
            string name = command.ExecuteScalar().ToString();//имя пользователя

            try
            {
                sqlconnect = new SqlConnection(connectionString);
                sqlconnect.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();//логин
                DataTable table = new DataTable();

                SqlDataAdapter adapter2 = new SqlDataAdapter();//права
                DataTable table2 = new DataTable();

                str = ("SELECT   Имя_пользователя, Пароль from Пользователи  WHERE  Имя_пользователя=N'" + textBox1.Text.ToString() + "' and Пароль=N'" + textBox2.Text.ToString() + "'");

                str_ins = ("SELECT Имя_пользователя, Пароль from Пользователи WHERE Имя_пользователя=N' " + textBox1.Text.ToString() + "' and Пароль=N'" + textBox2.Text.ToString() + "'");
                SqlCommand command1 = new SqlCommand(str, sqlconnect);
                //  string name = command.ExecuteScalar().ToString();
                adapter.SelectCommand = command1;
                adapter.Fill(table);

                if (table.Rows.Count == 1)//проверяем есть ли такой логин в бд
                {
                    str_ins = ("SELECT  Права_пользователя from Пользователи  WHERE  Имя_пользователя=N'" + textBox1.Text.ToString() + "'");
                    SqlCommand command2 = new SqlCommand(str, sqlconnect);
                    string prava = command2.ExecuteScalar().ToString();

                    if (prava == "manager")
                    {
                        MessageBox.Show("Права менеджера");
                    }
                    else { MessageBox.Show("Права админа"); }
                }
                else
                {
                    MessageBox.Show("Логина  не существует! или пароль введен не верно!");
                }
                sqlconnect.Close();
            }    
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации! Попробуйте еще раз или зарегестрируйтесь!");
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            sqlconnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\SampleDatabaseWalkthrough\SampleDatabaseWalkthrough\Database1.mdf;Integrated Security=True;
Connect Timeout = 30");
            sqlconnection.Open();
            loadDate();
        }
        private void loadDate()//вывод информации из бд
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *,N'Удалить' AS [Команда] FROM [Пользователи]", sqlconnection);
                SqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                SqlCommandBuilder.GetDeleteCommand();
                SqlCommandBuilder.GetUpdateCommand();
                SqlCommandBuilder.GetInsertCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Пользователи");
                dataGridView1.DataSource = dataSet.Tables["Пользователи"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[4, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReloeDate()
        {
            try
            {
                dataSet.Tables["Пользователи"].Clear();
                sqlDataAdapter.Fill(dataSet, "Пользователи");
                dataGridView1.DataSource = dataSet.Tables["Пользователи"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            sqlconnect = new SqlConnection(connectionString); //создаем нового пользователя в таблице "Пользователи"
            sqlconnect.Open();
            string trgistr = textBox1.Text.ToString();
            str = ("INSERT INTO Пользователи VALUES('Имя_пользователя', 'Пароль')" + textBox1.Text.ToString() + textBox2.Text.ToString() + "'"); //создаем Имя_пользователя и Пароль в таблице "Пользователи"
            SqlCommand command = new SqlCommand(str, sqlconnect);
            string name = command.ExecuteScalar().ToString();
            try
            {
                if (name == textBox1.Text.ToString() )
                {
                    
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации! Попробуйте еще раз или обратитесь к администратору!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}


