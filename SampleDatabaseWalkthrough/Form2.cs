using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SampleDatabaseWalkthrough
{
    public partial class Form2 : Form
    {
        string str = "";
      
        string connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\SampleDatabaseWalkthrough\SampleDatabaseWalkthrough\Database1.mdf;Integrated Security=True;
Connect Timeout = 30");
        SqlConnection sqlconnect;
    
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)

        {   sqlconnect = new SqlConnection(connectionString);
                sqlconnect.Open();
                string proverka = textBox2.Text.ToString();//получила пароль
                str = ("SELECT Пароль from Пользователи WHERE Имя_пользователя = N'" + textBox1.Text.ToString() + "'");//возвращаю пароль если имя пользователя найдено
                SqlCommand command = new SqlCommand(str, sqlconnect);
                string name = command.ExecuteScalar().ToString();//пароль
            try
            {
             
                if (proverka == name)//сравниваю пароли из бд и текс бокс 2
                {//пароль==изначению логин или логин =админ
                    if (name == textBox1.Text.ToString() || textBox1.Text.ToString() == "admin")
                    {
                        Form1 okno_Admina = new Form1();
                        okno_Admina.Show();
                    }
                    else
                    {

                        // MessageBox.Show("пароль совпадает!авторизуйтесь");
                        okno_menedgera Okno_menedgera = new okno_menedgera();
                        Okno_menedgera.Show();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации! Попробуйте еще раз или обратитесь к администратору!");
                }
                sqlconnect.Close();
                MessageBox.Show("что вернул запрос" + name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("5756576766878787887Ошибка авторизации! Попробуйте еще раз или обратитесь к администратору!");
            }
           
        }
    } 
}
