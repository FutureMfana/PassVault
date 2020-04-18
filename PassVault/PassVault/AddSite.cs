using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassVault
{
    public partial class AddSite : Form
    {

        #region GeneralDeclarations
        BusinessClass bc = new BusinessClass();
        string con;
        string pass;
        #endregion
        public AddSite()
        {
            InitializeComponent();
        }

        #region Form_Load()
        private void AddSite_Load(object sender, EventArgs e)
        {
            try
            {
                chkAutoPassword.Checked = true;
                con = bc.getConnection();
                if (!con.ToLower().Equals("true"))
                {
                    throw new Exception(con);
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Connection error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error );
            }
        }
        #endregion

        #region Button_Click()
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //ensures at least email or pass is provided for site/app login
                if (txtEmail.Text.Trim().Equals("") && txtUsername.Text.Trim().Equals("")) {
                    txtError.Text = "Please provide at least Username or Email address";
                    txtEmail.Focus();
                    return;
                }

                //ensures that passwords are provided
                if (txtPass1.Text.Trim().Equals(""))
                {
                    txtPass1.Focus();
                    txtError.Text = "Please provide password...";
                    return;
                }
                if (txtPass2.Text.Trim().Equals(""))
                {
                    txtPass2.Focus();
                    txtError.Text = "Please provide password...";
                    return;
                }

                //ensures that the password is the same
                if (!txtPass1.Text.Trim().Equals(txtPass2.Text.Trim()))
                {
                    txtError.Text = "Passwords do not match.Please provide matching passwords...";
                    txtPass1.Focus();
                    return;
                }

                //int uid = Convert.ToInt32(txtUID.Text);
                string siteName = txtSiteName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string uname = txtUsername.Text.Trim();
                string comments = txtComments.Text.Trim();

                //ensures that passwords are provided
                if (txtPass1.Text.Trim().Equals(""))
                {
                    txtPass1.Focus();
                    txtError.Text = "Please provide password...";
                    return;
                }

                pass = txtPass1.Text.Trim();

                string addSite = bc.addSite(siteName, pass, 1000, uname, email, comments);
                if (addSite.ToLower().Equals("true"))
                {
                    txtError.Text = "Site added successfully...";
                }
               else
                {
                    throw new Exception(addSite);
                }

            }catch (Exception ex)
            {
                txtError.Text = ex.Message.ToString();
            }
        }
        #endregion

        #region getPassword()
        private void getPassword() {
            try
            {
                bc.generatePassword(out pass);
                txtPass1.Text = pass;
                txtPass2.Text = pass;
            } catch(Exception ex)
            {
                txtError.Text = ex.Message.ToString();
            }
        }
        #endregion

        #region chk_Checked
        private void chkAutoPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPassword.Checked == true)
            { 
                getPassword();
            }
            else
            {
                txtPass1.Text = "";
                txtPass2.Text = "";
            }
        }
        #endregion

        #region Regenerate_Click()
        private void btnRegenerate_Click(object sender, EventArgs e)
        {
            if (chkAutoPassword.Checked == false) { return; }
            getPassword();
        }
        #endregion

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
