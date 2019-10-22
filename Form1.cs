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
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        MySqlConnection cnn;
        int codsis = 0,idu=0;
		// nueva linea
		int linea;

        public SqlConnection conn;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connetionString = null;
            
            connetionString = "server=200.87.164.66;database=db_pedidosneo;uid=root;pwd=.rj1m3n3z;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }

        private void OpenBD()
        {
            string connetionString = null;

            connetionString = "server=200.87.164.66;database=db_pedidosneo;uid=root;pwd=.rj1m3n3z;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
                //MessageBox.Show("Connection Open ! ");
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {

            OpenBD();

            
            MySqlDataReader reg;
            int idusr=0,n=0;

            cnn.Open();

            MySqlCommand command = cnn.CreateCommand();

            command.CommandText = "select ursid,cod_sis from sec_users where login ='" + txtUsuario.Text.Trim() + "' and pswd ='"+txtPasswd.Text.Trim()+"'";

            reg = command.ExecuteReader();
            if (reg.Read())
            {
                idusr = (int)reg[0];
                idu = idusr;
                codsis = (int)reg[1];
            }


            cnn.Close();
            //Recuperar Datos

            if (idusr > 0)
            {
                getpedido(idusr);
                getpedidoDetalle(idusr);
            }
            else MessageBox.Show("Error No existe el usuario o password incorrecto ");

          
            
        }

        private void getpedido(int idusr)
        {
            dgv.Rows.Clear();

            MySqlDataReader reg;     

            MySqlCommand comando = cnn.CreateCommand();

            cnn.Open();
            comando.CommandText = "select idpedido,fechaPedido,idcliente,nombre,cliente,factura,cantidad,total,cerrado from vpedido "+
                                  "where cerrado='S' and procesado is null and usrid = " + idusr + " and fechapedido = '" + fechaToMysql(fecha.Text) + "'";

            //"select cantidad,idpedido,fechaPedido,cliente_a,factura_a,total,cerrado from pedido where usrid=" + idusr + " and fechapedido='" + fechaToMysql(fecha.Text) + "'";

            reg = comando.ExecuteReader();

            int i = 0;
      
            while (reg.Read())
            {
                dgv.Rows.Add();
                 this.dgv[0, i].Value =  (int)reg[0];
                 this.dgv[1, i].Value =  reg.GetString("fechaPedido").Substring(0,10);
                 this.dgv[2, i].Value =  (int)reg[2];
                 this.dgv[3, i].Value =  (string)reg[3];
                 this.dgv[4, i].Value = (string)reg[4];
                 this.dgv[5, i].Value =  (string)reg[5];
                 this.dgv[6, i].Value =  (int)reg[6];
                 this.dgv[7, i].Value =  (decimal)reg[7];
                 this.dgv[8, i].Value =  (string)reg[8];
                i++;
            }
            cnn.Close();

            if (dgv.Rows.Count==1) MessageBox.Show("No existe datos en esa fecha");

        }

        private void getpedidoDetalle(int idusr)
        {
            dgv2.Rows.Clear();
            MySqlDataReader reg;

            MySqlCommand comando = cnn.CreateCommand();

            cnn.Open();
            comando.CommandText = "select idpedido,iditem,codigo,cantidad,precio,total from vpedidodetalle " +
                                   "where cantidad >0 and usrid = " + idusr + " and fechapedido = '" + fechaToMysql(fecha.Text) + "'";

            //MessageBox.Show(comando.CommandText);
            reg = comando.ExecuteReader();

            int i = 0;
            while (reg.Read())
            {
                dgv2.Rows.Add();
               
                    this.dgv2[0, i].Value = (int)reg[0];
                    this.dgv2[1, i].Value = (int)reg[1];
                    this.dgv2[2, i].Value = (string)reg[2];
                    this.dgv2[3, i].Value = Convert.ToInt32(reg[3]);
                    this.dgv2[4, i].Value = Convert.ToDecimal(reg[4]);
                    this.dgv2[5, i].Value = Convert.ToDecimal(reg[5]);
                 i++;
            }
            cnn.Close();
        }

        private void Procesado(int usrid)
        {
            cnn.Open();

            MySqlCommand comando = cnn.CreateCommand();

            comando.CommandText = "update Pedido set Procesado='S' " +
                                   "where  usrid = " + usrid + " and fechapedido = '" + fechaToMysql(fecha.Text) + "'";

            comando.ExecuteNonQuery();
            cnn.Close();

        }

        public string fechaToMysql(string fecha)
        {
            DateTime fech = Convert.ToDateTime(fecha);
            string mm, dd;

            if (fech.Month <= 9) mm = "0" + fech.Month.ToString(); else mm = fech.Month.ToString();

            if(fech.Day<=9) dd= "0" + fech.Day.ToString(); else dd = fech.Day.ToString();

            return (fech.Year.ToString() + "-" + mm+ "-" + dd);
        }


        private void OpenBDVentas()
        {
            string MyConString = null;

           // MyConString = "Data Source= localhost;Initial Catalog=Ventas; Persist Security Info=True ;User ID=sa; Password=alefranco";
             MyConString = "Data Source= 192.168.1.35;Initial Catalog=Ventas; Persist Security Info=True ;User ID=sa; Password=.rj1m3n3z";
            MyConString = "Data Source= " +txtServer.Text+";Initial Catalog=Ventas; Persist Security Info=True ;User ID=sa; Password="+txtPassServer.Text;
            //MessageBox.Show(MyConString)

            conn = new SqlConnection(MyConString);
       
            try
            {
                conn.Open();
                //MessageBox.Show("Connection Open ! ");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Insert into  Ventas
            OpenBDVentas();
            int usrid = codsis,cod_ped=0,idp=0;
            conn.Open();

            SqlCommand command = conn.CreateCommand();
            int j = 0,idped=0;
            
            for (int i = 0; i < dgv.RowCount - 1; i++)
            {

                SqlCommand cmd = new SqlCommand("[dbo].[InsertPedido]", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@fecha", Convert.ToDateTime(dgv[1, i].Value.ToString())));
                cmd.Parameters.Add(new SqlParameter("@cod_cliente", dgv[2, i].Value));
                cmd.Parameters.Add(new SqlParameter("@cliente", dgv[4, i].Value));
                cmd.Parameters.Add(new SqlParameter("@factura_a", dgv[5, i].Value));
                cmd.Parameters.Add(new SqlParameter("@id_usuario", usrid));
                cmd.Parameters.Add(new SqlParameter("@total", dgv[7, i].Value));

                cmd.Parameters.Add(new SqlParameter("@codped", SqlDbType.Int));
                cmd.Parameters["@codped"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add(new SqlParameter("@id_", SqlDbType.Int));
                cmd.Parameters["@id_"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                cod_ped = Convert.ToInt32(cmd.Parameters["@codped"].Value);
                idp= Convert.ToInt32(cmd.Parameters["@id_"].Value);

                idped = Convert.ToInt32(dgv[0, i].Value);

                for(j=0; j<dgv2.RowCount-1;j++ )
                {

                    if(idped ==Convert.ToInt32(dgv2[0,j].Value.ToString()))
                    {
                        SqlCommand comd = new SqlCommand("InsertPedidoDetalle", conn);
                        comd.CommandType = CommandType.StoredProcedure;

                        comd.Parameters.Add(new SqlParameter("@idp", idp));
                        comd.Parameters.Add(new SqlParameter("@noped", cod_ped));
                        comd.Parameters.Add(new SqlParameter("@cod_prod", dgv2[2, j].Value));
                        comd.Parameters.Add(new SqlParameter("@pu", dgv2[4, j].Value));
                        comd.Parameters.Add(new SqlParameter("@cantidad", dgv2[3, j].Value));
                        comd.Parameters.Add(new SqlParameter("@total", dgv2[5, j].Value));

                        comd.ExecuteNonQuery();
                    }
                }
                // command.ExecuteNonQuery();
              
            }

            MessageBox.Show("Concluido...");
            conn.Close();
            Procesado(idu);

            dgv.Rows.Clear();

            dgv2.Rows.Clear();
        }

       
    }
}
