﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ProComsys
{
    public partial class WebForm15 : System.Web.UI.Page
    {
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Session["Name"].ToString();
            showData();
            showNoData();
        }

        protected string CutString(string str)
        {
            int s1 = str.Length;
            string str2 = "";
            for (int i = 0; i < s1; i++)
            {
                if (str[i] != ' ')
                {
                    str2 = str2 + str[i];
                }
                else
                {
                    break;
                }
            }

            return str2;
        }

        protected void showData()
        {
            string constr = WebConfigurationManager.ConnectionStrings["Db"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("select distinct sh.NOForm as CPE , sh.Form as form , CONVERT(VARCHAR, sh.RequestDate, 103) as Date , sh.StatusRequest as status ,pr.PThaiName as PName , sh.IDRequest as IDRequest" +
            " from ShowAllform sh join Project pr on sh.IDProject = pr.IDProject   "+
            " where sh.SignFName ='" + CutString(id) + "'  and sh.StatusRequest ='รอตอบรับ' ", con);
            SqlDataReader reader2 = cmd.ExecuteReader();
            GridView1.DataSource = reader2;
            GridView1.DataBind();

            reader2.Close();
            con.Close();

        }

        protected void showNoData()
        {
            string constr = WebConfigurationManager.ConnectionStrings["Db"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
           

            string cou = "";
            SqlCommand cmd1 = new SqlCommand("select count(*) " +
            " from ShowAllform sh join Project pr on sh.IDProject = pr.IDProject  " +
            "  where sh.SignFName ='" + CutString(id) + "'  and sh.StatusRequest ='รอตอบรับ' ", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cou = reader[0].ToString();
                }
            }
            reader.Close();
            con.Close();
           
            if (cou == "0")
            {
                NoRequest.Visible = true;
            }
            else
            {
                NoRequest.Visible = false;
            }
        }


        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            string pro = row.Cells[2].Text;
            Session["Project"] = row.Cells[4].Text;
            Session["IDRe"] = row.Cells[1].Text ;
            Session["Role"] = "Edit";
            Response.Redirect("TForm" + pro + ".aspx");

        }

       
    }
   
}