using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudOperationUsingStoreProcedure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbCommand cmd;
        OleDbConnection conn;

        public int ID { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Telefonbok.accdb");

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        //Student information från databas
        private void StudentRecord()
        {
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from information";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

        }

        //Uppdaterings knapp 

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (txtNumber.Text != string.Empty && txtFirstName.Text != string.Empty && txtLastName.Text != string.Empty
                && ValidEmail(txtEmail.Text) && txtClass.Text != string.Empty && ID != null)
            {
                cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "update information set Förnamn='" + txtFirstName.Text + "',Efternamn='" + txtLastName.Text + "',Klass='" + txtClass.Text + "',Telefonnummer='" + txtNumber.Text + "',Email='" + txtEmail.Text + "' where id=" + ID + " ";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated successfully.", "Record Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
                StudentRecord();
                reset();
            }
            else
            {
                MessageBox.Show("Please enter value in all fields OR select row", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Knapp som tar bort information

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record ? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dialogResult == DialogResult.Yes && ID != null)
            {
                conn.Open();
                cmd.CommandText = "delete from information where ID=" + ID +"";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted successfully.", "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = cmd.CommandText;
                reset();
                conn.Close();
                StudentRecord();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnsave.Enabled = true;
            }
            else if (dialogResult == DialogResult.No)
            {

            }
            else
            {
                MessageBox.Show("Please select row to delete the record", "Invalid Select", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Knapp som sparar nya informationen
        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (txtNumber.Text != string.Empty  && txtFirstName.Text != string.Empty && txtLastName.Text != string.Empty
                && ValidEmail(txtEmail.Text) && txtClass.Text != string.Empty)
            {
                cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "insert into information (Förnamn,Efternamn,Klass,Telefonnummer,Email) values('" + txtFirstName.Text +"','"+ txtLastName.Text + "','"+ txtClass.Text + "','"+ txtNumber.Text + "','"+ txtEmail.Text + "')";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                reset();
                conn.Close();
                StudentRecord();
            }
            else
            {
                MessageBox.Show("Please enter value in all fields", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //startar om textfälten för namn, email, telefonnummer osv
        private void reset()
        {
            txtFirstName.Text = "";
            txtNumber.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtClass.Text = "";
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            StudentRecord();
        }

        //Data Grid View som visar hela telefonbok 
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnsave.Enabled = false;
                btnUpdate.Enabled = true;
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                ID = Convert.ToInt32(row.Cells[0].Value);
                txtFirstName.Text = row.Cells[1].Value.ToString();
                txtLastName.Text = row.Cells[2].Value.ToString();
                txtClass.Text = row.Cells[3].Value.ToString();
                txtNumber.Text = row.Cells[4].Value.ToString();
                txtEmail.Text = row.Cells[5].Value.ToString();
            }
        }

        // Kollar så att email adressen är rätt annars kommer fel meddelande
        bool ValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                MessageBox.Show("Email address in invalid", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                MessageBox.Show("Email address in invalid", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnDelete.Enabled = true;
                btnsave.Enabled = false;
                btnUpdate.Enabled = true;
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                ID = Convert.ToInt32(row.Cells[0].Value);
                txtFirstName.Text = row.Cells[1].Value.ToString();
                txtLastName.Text = row.Cells[2].Value.ToString();
                txtClass.Text = row.Cells[3].Value.ToString();
                txtNumber.Text = row.Cells[4].Value.ToString();
                txtEmail.Text = row.Cells[5].Value.ToString();
            }

        }
    }
}
